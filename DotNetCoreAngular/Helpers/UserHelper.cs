using System.Security.Cryptography;
using System.Text;

namespace DotNetCoreAngular.Helpers
{
    public class UserHelper
    {
        public static string CreateUsername(string email)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(email));
                var res = Convert.ToBase64String(hash);
                return res?.ToLower();
            }
        }
    }
}
