using Newtonsoft.Json;
using System.Buffers;
using System.Globalization;
using System.IO.Pipelines;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using WaffarXPartnerApi.Application.Common.DTOs;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Domain.Models.SharedModels;

namespace PartnerWebApi.Infrastructure;

#nullable disable
public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiAuthenticator _apiAuthenticator;
    private readonly IServiceProvider _serviceProvider;
    //private readonly IClientService _clientService;
    public AuthenticationMiddleware(RequestDelegate next, ApiAuthenticator ApiAuthenticator, IServiceProvider serviceProvider )
    {
        _next = next;
        _apiAuthenticator = ApiAuthenticator;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        bool IsValidSignature = false;
        var request = context.Request;
        try
        {
            string strInfoBody = string.Empty;
            bool infoBody = request.ContentLength > 0;
            if (infoBody)
            {
                request.EnableBuffering();
                request.Body.Position = 0;
                List<string> tmp = await GetListOfStringFromPipe(request.BodyReader);
                request.Body.Position = 0;
                strInfoBody = tmp.FirstOrDefault();
            }
            var actionName = context.Request.Path;
            var Method = context.Request.Method;
            var Headers = GetHeaderData(context);
            if (Headers != null && !string.IsNullOrEmpty(Headers.ClientId))
            {
                // CheckIsValidUser
                ApiClientDto user = new ApiClientDto();
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _service = scope.ServiceProvider.GetRequiredService<IClientService>();
                    user = await _service.GetUser(Headers.ClientId);
                }

                if (user != null)
                {
                    IsValidSignature = await _apiAuthenticator.CheckSignature(strInfoBody, Method, actionName, user, Headers);
                }
            }
            if (!IsValidSignature)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                GenericResponse<int> Res = new GenericResponse<int>()
                {
                    Data = 0,                    
                    Errors = new List<string>(),
                    Status = StaticValues.Error,
                };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(Res));
                return;
            }
            await _next.Invoke(context);

        }
        catch (Exception)
        {
            throw;
        }
    }
    private async Task<List<string>> GetListOfStringFromPipe(PipeReader reader)
    {
        List<string> results = new List<string>();

        while (true)
        {
            ReadResult readResult = await reader.ReadAsync();
            var buffer = readResult.Buffer;

            SequencePosition? position = null;

            do
            {
                // Look for a EOL in the buffer
                position = buffer.PositionOf((byte)'\n');

                if (position != null)
                {
                    var readOnlySequence = buffer.Slice(0, position.Value);
                    AddStringToList(results, in readOnlySequence);

                    // Skip the line + the \n character (basically position)
                    buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                }
            }
            while (position != null);


            if (readResult.IsCompleted && buffer.Length > 0)
            {
                AddStringToList(results, in buffer);
            }

            reader.AdvanceTo(buffer.Start, buffer.End);

            // At this point, buffer will be updated to point one byte after the last
            // \n character.
            if (readResult.IsCompleted)
            {
                break;
            }
        }

        return results;
    }
    private static void AddStringToList(List<string> results, in ReadOnlySequence<byte> readOnlySequence)
    {
        // Separate method because Span/ReadOnlySpan cannot be used in async methods
        ReadOnlySpan<byte> span = readOnlySequence.IsSingleSegment ? readOnlySequence.First.Span : readOnlySequence.ToArray().AsSpan();
        results.Add(Encoding.UTF8.GetString(span));
    }
    private HeaderData GetHeaderData(HttpContext context)
    {
        //Return
        HeaderData Data = new HeaderData();
        var headers = context.Request.Headers;

        if (headers != null)
        {
            foreach (var header in headers)
            {
                string key = header.Key;
                List<string> valueList = header.Value.ToList();
                if (key == "wxc-id")
                {
                    Data.ClientId = valueList.FirstOrDefault();
                }
                if (key.ToLower() == "authorization")
                {
                    Data.Signature = valueList.FirstOrDefault();
                }
                if (key == "wxc-date")
                {
                    Data.Date = valueList.FirstOrDefault();
                }
                //if (key == "wxu-token")
                //{
                //    Data.UserToken = valueList.FirstOrDefault();
                //}
            }
        }
        return Data;
    }
}

public class ApiAuthenticator
{
    public async Task<bool> CheckSignature(string payload, string method, string actionName, ApiClientDto client, HeaderData headerData)
    {
        bool isValidSignature = false;
        try
        {
            // Sign Request  
            DateTime datetime = DateTime.Parse(headerData.Date,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal);

            var serializedData = payload.Trim().ToLower();
            byte[] hashedPayload = SigningHelper.Sha256(serializedData);
            string hashedPayloadString = SigningHelper.ToHex(hashedPayload);
            string stringToSign = "";

            if (string.IsNullOrEmpty(payload))
            {
                stringToSign = SigningHelper.BuildStringToSignWithNoPayload(client.Clientkey, datetime, method.ToLower(), actionName.ToLower());
            }
            else
            {
               stringToSign = SigningHelper.BuildStringToSign(client.Clientkey, datetime, hashedPayloadString, method.ToLower(), actionName.ToLower());
            }

            byte[] secretKey = Encoding.UTF8.GetBytes(client.Secret ?? "");
            byte[] payloadHmac = await Task.Run(() => SigningHelper.HmacSha256(stringToSign, secretKey));

            string signedPayload = SigningHelper.ToHex(payloadHmac);

            if (signedPayload == headerData.Signature)
            {
                isValidSignature = true;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return isValidSignature;
    }
}
public static class SigningHelper
{
    public const string ISO8601BasicDateTimeFormat = "yyyyMMddTHHmmssZ";

    public static byte[] Sha256(string data)
    {
        using (var sha256 = SHA256.Create()) // Use the parameterless Create method for SHA256  
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
    }

    public static byte[] HmacSha256(string stringtosign, byte[] key)
    {
        using (var algorithm = new HMACSHA256(key))
        {
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(stringtosign));
        }
    }

    public static string BuildStringToSign(string ClientKey, DateTime date, string hashedpayload, string Method, string ActionName)
    {


        var format = "{0}-{1}-{2}-{3}-{4}";
        var dateString = date.ToString(ISO8601BasicDateTimeFormat);

        return String.Format(
                format, Method, ActionName, ClientKey, dateString, hashedpayload);
    }
    public static string BuildStringToSignWithNoPayload(string ClientKey, DateTime date, string Method, string ActionName)
    {


        var format = "{0}-{1}-{2}-{3}";
        var dateString = date.ToString(ISO8601BasicDateTimeFormat);

        return String.Format(
                format, Method, ActionName, ClientKey, dateString);
    }
    public static string ToHex(byte[] data)
    {
        var builder = new StringBuilder();

        for (var i = 0; i < data.Length; i++)
            builder.Append(data[i].ToString("x2"));

        return builder.ToString();
    }

}
