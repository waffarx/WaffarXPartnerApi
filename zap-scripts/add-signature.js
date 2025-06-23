function sendingRequest(msg, initiator, helper) {
    // 1) Compute current timestamp in ISO 8601 (no milliseconds) and formatted string
    var now = new Date();
    var iso = now.toISOString().split('.')[0] + 'Z';                // e.g. "2025-06-24T14:23:45Z"
    var formattedDateTime = iso.replace(/-/g, '').replace(/:/g, '');  // e.g. "20250624T142345Z" without separators

    // 2) Build method and path
    var method = msg.getRequestHeader().getMethod().toLowerCase();    // e.g. "get", "post"
    var path = msg.getRequestHeader().getURI().getPath().toLowerCase(); // e.g. "/api/orders"

    // 3) Read and normalize body
    var body = msg.getRequestBody().toString();
    var data = body ? body.toLowerCase().trim() : "";

    // 4) Compute SHA-256 of payload (hex)
    var hashedPayloadString = data
        ? org.apache.commons.codec.digest.DigestUtils.sha256Hex(data)
        : "";

    // 5) Your client key (as in Postman)
    var clientKey = "Bv0fMtFoUVeRm4OElR57CVCGS4dtbNgJAVdZfwc+ct4kmcY5PzqswCbOm9gH3yi59IaiZPfT4V7LC3xOaDk+ZQ==";

    // 6) Construct the string to sign
    var stringToSign = method + "-" + path + "-" + clientKey + "-" + formattedDateTime
        + (data ? "-" + hashedPayloadString : "");

    // 7) Your secret key (UTF-8 string)
    var secretKey = "n6rntfyk0amdu7fjzj5u02xpp8adgv4c9dfngiw01z93v7mi31lq5qg8nzf7yjbopb4wnz5vyyzj82s9ld9evbtg4hrvm3z3sw9it41e3sgfj1k3dn7rrgx6vggd3pxkby86khxkjdcz1kjwz5r4yzvnnxeivsf9th6g5k1ou0vjck1jbcus50hfh7gp3utqy99oh1a9k2nz0xajbpr7rhz2mc3cdqjxskpyo93lmh69vraej4d5dd9h6cic8pi8e!@";

    // 8) Compute HMAC-SHA256 over the stringToSign
    var mac = javax.crypto.Mac.getInstance("HmacSHA256");
    var keySpec = new javax.crypto.spec.SecretKeySpec(
        secretKey.getBytes(java.nio.charset.StandardCharsets.UTF_8),
        "HmacSHA256"
    );
    mac.init(keySpec);
    var rawHmac = mac.doFinal(
        stringToSign.getBytes(java.nio.charset.StandardCharsets.UTF_8)
    );
    var SignedPayload = org.apache.commons.codec.binary.Hex.encodeHexString(rawHmac);

    // 9) Inject headers into the request
    msg.getRequestHeader().setHeader("wxc-date", iso);
    msg.getRequestHeader().setHeader("wxc-auth", SignedPayload);
}

function responseReceived(msg, initiator, helper) {
    // no-op
}
