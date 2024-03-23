using System.Security.Cryptography;

namespace WGMansion.Api.Services
{
    public static class EncryptionService
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HashingIterationsCount = 10101;

        public static string HashPassword(string password)
        {
            var salt = GenerateSalt();
            var hash = ComputeHash(password, salt);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        private static byte[] ComputeHash(string password, byte[] salt, int iterations = HashingIterationsCount, int hashByteSize = HashByteSize)
        {
            var hashGenerator = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512);
            return hashGenerator.GetBytes(hashByteSize);
        }

        private static byte[] GenerateSalt(int saltByteSize = SaltByteSize)
        {
            var saltGenerator = new RNGCryptoServiceProvider();
            byte[] salt = new byte[saltByteSize];
            saltGenerator.GetBytes(salt);
            return salt;
        }

        public static bool VerifyPassword(string password, string passwordHash)
        {
            char[] delimiter = [':'];
            var split = passwordHash.Split(delimiter);
            var salt = Convert.FromBase64String(split[0]);
            var hash = Convert.FromBase64String(split[1]);
            byte[] computedHash = ComputeHash(password, salt);
            return AreHashesEqual(computedHash, hash);
        }

        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            int minHashLenght = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < minHashLenght; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
    }
}
