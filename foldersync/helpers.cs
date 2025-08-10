using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Security.Cryptography;

namespace foldersync
{
    public class CryptoWrappers
    {
        public static byte[] ComputeHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            using (var fileStream = File.OpenRead(filePath))
            return sha256.ComputeHash(fileStream);
        }
    }
}
