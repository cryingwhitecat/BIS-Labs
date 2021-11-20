using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BisLab2.Core.KeyTransformers
{
    public class KeyTransformer
    {
        public byte[] GetKey(string password, int keyLength)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hash;
            using (var md5 = MD5.Create())
            {
                hash = md5.ComputeHash(passwordBytes);
            }
            if(hash.Length > keyLength)
            {
                return hash.Take(keyLength).ToArray();
            }
            else
            {
                var result = hash.AppendToEnd(hash.Take(keyLength - hash.Length)).ToArray();
                return result;
            }
          
        }
    }
}
