namespace HolaHousing_BE.Services.Utils
{
    public class common
    {
        public string GetJwtToken()
        {
            //// Retrieve the JWT token from the Authorization header
            //var bearerToken = HttpContext.Request.Headers["Authorization"].ToString();

            //if (bearerToken.StartsWith("Bearer "))
            //{
            //    var token = bearerToken.Substring("Bearer ".Length).Trim();

            //    // Now you have the JWT token, you can process it if needed
            //    return Ok(new { Token = token });
            //}
            return "";
        }
    }
}
