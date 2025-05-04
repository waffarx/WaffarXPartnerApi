using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;

namespace PartnerWebApi.Infrastructure;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IResourceService _resourceService;

    // Services are injected via constructor
    public ValidationFilter(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            if (errors.Count > 0)
            {
                var resourcesValues = await _resourceService.GetResourcesList(errors);
                errors = resourcesValues.Select(x => x.Value).ToList();
            }
            context.Result = new BadRequestObjectResult(new GenericResponse<object>() { Status = StaticValues.Error, Errors = errors });
            return;
        }

        await next();
    }
}

