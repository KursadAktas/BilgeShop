using BilgeShop.Data.Enums;
using System.Security.Claims;

namespace BilgeShop.WebUI.Extensions
{
    // Cookie'de tutulan verilerin kontrollerini yapmak için yazılacak metotları bu class içerisinde topluyorum.
    public static class ClaimsPrincipalExtensions
    {
        // this -> Bu sayede metot artık sondan çağırılır.
        // User.IsLogged() tarzında
        public static bool IsLogged(this ClaimsPrincipal user)
        {
            if (user.Claims.FirstOrDefault(x => x.Type == "id") != null)
                return true;
            else
                return false;

        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            if (user.Claims.FirstOrDefault(x => x.Type == "userType")?.Value == UserTypeEnum.Admin.ToString())
                return true;
            else
                return false;
        }
       
        public static string GetUserFirstName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "firstName")?.Value;
        }

        public static string GetUserLastName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "lastName")?.Value;
        }

    }

    

    
}
