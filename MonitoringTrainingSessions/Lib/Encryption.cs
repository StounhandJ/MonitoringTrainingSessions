using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MonitoringTrainingSessions.Lib;

public class Encryption
{
    public static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }

    public static string encoding(string ishText, string pass,
        string sol = "doberman", string cryptographicAlgorithm = "SHA1",
        int passIter = 2, string initVec = "a8doSuDitOz1hZe#",
        int keySize = 256)
    {
        if (string.IsNullOrEmpty(ishText))
            return "";

        byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
        byte[] solB = Encoding.ASCII.GetBytes(sol);
        byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);

        PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
        byte[] keyBytes = derivPass.GetBytes(keySize / 8);
        RijndaelManaged symmK = new RijndaelManaged();
        symmK.Mode = CipherMode.CBC;

        byte[] cipherTextBytes = null;

        using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memStream.ToArray();
                    memStream.Close();
                    cryptoStream.Close();
                }
            }
        }

        symmK.Clear();
        return Convert.ToBase64String(cipherTextBytes);
    }

    //метод дешифрования строки
    public static string dencoding(string ciphText, string pass,
        string sol = "doberman", string cryptographicAlgorithm = "SHA1",
        int passIter = 2, string initVec = "a8doSuDitOz1hZe#",
        int keySize = 256)
    {
        if (string.IsNullOrEmpty(ciphText))
            return "";

        byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
        byte[] solB = Encoding.ASCII.GetBytes(sol);
        byte[] cipherTextBytes = Convert.FromBase64String(ciphText);

        PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
        byte[] keyBytes = derivPass.GetBytes(keySize / 8);

        RijndaelManaged symmK = new RijndaelManaged();
        symmK.Mode = CipherMode.CBC;

        byte[] plainTextBytes = new byte[cipherTextBytes.Length];
        int byteCount = 0;

        using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
        {
            using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                {
                    byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    mSt.Close();
                    cryptoStream.Close();
                }
            }
        }

        symmK.Clear();
        return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
    }
}