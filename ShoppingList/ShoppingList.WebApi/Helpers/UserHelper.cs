using System.IdentityModel.Tokens.Jwt;

namespace ShoppingList.WebApi.Helpers
{
    public class UserHelper
    {
        public static async Task<Guid> GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var userId = Guid.Parse(jwtToken.Subject);
            return userId;
        }
    }
}
