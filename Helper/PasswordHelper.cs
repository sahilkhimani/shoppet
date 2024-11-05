using System.Security.Cryptography;
using System;
using System.Text;


namespace shoppetApi.Helper
{
    public class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        public static byte[] HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            byte[] hash = HashPasswordWithSalt(password, salt);

            byte[] hashBytes = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, KeySize);

            return hashBytes;
        }

        public static bool VerifyPassword(string password, byte[] storedHashBytes)
        {
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(storedHashBytes, 0, salt, 0, SaltSize);

            byte[] storedHash = new byte[KeySize];
            Buffer.BlockCopy(storedHashBytes, SaltSize, storedHash, 0, KeySize);

            byte[] hash = HashPasswordWithSalt(password, salt);

            return CryptographicOperations.FixedTimeEquals(storedHash, hash);
        }

        private static byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(KeySize);
            }
        }
    }
    }
