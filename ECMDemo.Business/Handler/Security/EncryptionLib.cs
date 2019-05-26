using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.IdentityModel;  
using System.Security;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ECMDemo.Data;

namespace ECMDemo.Common
{
    public class EncryptionLib
    {
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 2, 9, 0, 1, 1, 9, 9, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 2, 9, 0, 1, 1, 9, 9, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.None;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static string EncryptText(string input, string password = "E6t187^D43%F")
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public static string DecryptText(string input, string password = "E6t187^D43%F")
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        public static class KeyGenerator
        {
            public static string GetUniqueKey(int maxSize = 15)
            {
                char[] chars = new char[62];
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
                byte[] data = new byte[1];
                using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetNonZeroBytes(data);
                    data = new byte[maxSize];
                    crypto.GetNonZeroBytes(data);
                }
                StringBuilder result = new StringBuilder(maxSize);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length)]);
                }
                return result.ToString();
            }
        }
    }
    public class JWTEncryptionLib
    {
        // Define const Key this should be private secret key  stored in some safe place
        static string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

        // Create Security key  using private key above:
        // not that latest version of JWT using Microsoft namespace instead of System
        static SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        static SigningCredentials credentials = new SigningCredentials
                             (securityKey, SecurityAlgorithms.HmacSha256Signature);
        static JwtHeader header = new JwtHeader(credentials);
        static JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        public static string GenerateToken(string UserName,string SessionCode,int expireHours = 1)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Authentication, SessionCode));
            claims.Add(new Claim(ClaimTypes.Name, UserName));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(expireHours),
                SigningCredentials = credentials
            };

            var stoken = handler.CreateToken(tokenDescriptor);
            var token = handler.WriteToken(stoken);
            return token;
        }
        private static ClaimsPrincipal GetPrincipal(string tokenString)
        {        
            ClaimsPrincipal principal;

            try
            {
                var jwtToken = handler.ReadToken(tokenString) as JwtSecurityToken;
              
                if (jwtToken == null)
                {
                    principal = null;
                }
                else
                {
                    var validationParameters = new TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = securityKey
                    };

                    SecurityToken securityToken;
                    principal = handler.ValidateToken(tokenString, validationParameters, out securityToken);
                }
            }
            catch (Exception ex)
            {
                principal = null;
            }

            return principal;
        }
        public static bool ValidateToken(string token,User user,out string message)
        {
            message = "Authorization has been denied for this request.";
            var simplePrinciple = GetPrincipal(token);

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Authentication);
            string tmp = usernameClaim?.Value;

            if (string.IsNullOrEmpty(tmp))
                return false;
            if (!tmp.Equals(user.SessionLoginCode.ToString())) {
                message = "Tài khoản của bạn đã được đăng nhập trên một máy khác vào lúc: " + user.LastLoginOnDate.Value.ToString("hh:mm:ss, dd-MM-yyyy") +".";
                return false;
                
            } 

            // More validate to check whether username exists in system

            return true;
        }

    }
}