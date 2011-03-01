using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Security.Cryptography;

namespace Cryptography
{
    public class Cryptography
    {

        #region Singleton Constructor

        public static Cryptography Instance
        {
            get
            {
                return CryptographyCreator.CreatorInstance;
            }
        }

        private sealed class CryptographyCreator
        {
            //-- Retrieve a single instance of a Cryptography
            private static readonly Cryptography _instance = new Cryptography();

            //-- Return an instance of the class
            public static Cryptography CreatorInstance
            {
                get { return _instance; }
            }
        }

        #endregion

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string Password)
        {
            byte[] cipherData = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 110, 0x20, 0x4d, 0x65, 100, 0x76, 0x65, 100, 0x65, 0x76 });
            byte[] buffer2 = Decrypt(cipherData, bytes.GetBytes(0x20), bytes.GetBytes(0x10));
            return Encoding.Unicode.GetString(buffer2);
        }
        /// <summary>
        /// 解密Byte数组
        /// </summary>
        /// <param name="cipherData"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] cipherData, string Password)
        {
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 110, 0x20, 0x4d, 0x65, 100, 0x76, 0x65, 100, 0x65, 0x76 });
            return Decrypt(cipherData, bytes.GetBytes(0x20), bytes.GetBytes(0x10));
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="fileIn"></param>
        /// <param name="fileOut"></param>
        /// <param name="Password"></param>
        public static void Decrypt(string fileIn, string fileOut, string Password)
        {
            int num2;
            FileStream stream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
            FileStream stream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 110, 0x20, 0x4d, 0x65, 100, 0x76, 0x65, 100, 0x65, 0x76 });
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = bytes.GetBytes(0x20);
            rijndael.IV = bytes.GetBytes(0x10);
            CryptoStream stream3 = new CryptoStream(stream2, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
            int count = 0x1000;
            byte[] buffer = new byte[count];
            do
            {
                num2 = stream.Read(buffer, 0, count);
                stream3.Write(buffer, 0, num2);
            }
            while (num2 != 0);
            stream3.Close();
            stream.Close();
        }
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="cipherData"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream stream = new MemoryStream();
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = Key;
            rijndael.IV = IV;
            CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
            try
            {
                stream2.Write(cipherData, 0, cipherData.Length);
            }
            catch (Exception exception)
            {
                throw new Exception("Error while writing encrypted data to the stream: \n" + exception.Message);
            }
            stream2.Close();
            return stream.ToArray();
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="clearText"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText, string Password)
        {
            byte[] clearData = Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 110, 0x20, 0x4d, 0x65, 100, 0x76, 0x65, 100, 0x65, 0x76 });
            return Convert.ToBase64String(Encrypt(clearData, bytes.GetBytes(0x20), bytes.GetBytes(0x10)));
        }
        /// <summary>
        /// 加密byte数组
        /// </summary>
        /// <param name="clearData"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] clearData, string Password)
        {
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 110, 0x20, 0x4d, 0x65, 100, 0x76, 0x65, 100, 0x65, 0x76 });
            return Encrypt(clearData, bytes.GetBytes(0x20), bytes.GetBytes(0x10));
        }
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="fileIn"></param>
        /// <param name="fileOut"></param>
        /// <param name="Password"></param>
        public static void Encrypt(string fileIn, string fileOut, string Password)
        {
            int num2;
            FileStream stream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
            FileStream stream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 110, 0x20, 0x4d, 0x65, 100, 0x76, 0x65, 100, 0x65, 0x76 });
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = bytes.GetBytes(0x20);
            rijndael.IV = bytes.GetBytes(0x10);
            CryptoStream stream3 = new CryptoStream(stream2, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            int count = 0x1000;
            byte[] buffer = new byte[count];
            do
            {
                num2 = stream.Read(buffer, 0, count);
                stream3.Write(buffer, 0, num2);
            }
            while (num2 != 0);
            stream3.Close();
            stream.Close();
        }

        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream stream = new MemoryStream();
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = Key;
            rijndael.IV = IV;
            CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                stream2.Write(clearData, 0, clearData.Length);
            }
            catch (Exception exception)
            {
                throw new Exception("Error while writing encrypted data to the stream: \n" + exception.Message);
            }
            stream2.Close();
            return stream.ToArray();
        }
    }
}