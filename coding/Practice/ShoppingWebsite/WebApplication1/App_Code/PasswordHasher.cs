using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace LoginAuth
{
    public class PasswordHasher
    {
        private byte[] CreateSalt()
        {
            byte[] salt = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            return salt;
        }




    }
}