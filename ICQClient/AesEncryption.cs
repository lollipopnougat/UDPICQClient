using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ICQClient
{
    class AesEncryption
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt">要加密的字符串</param>
        /// <param name="key">加密使用的密钥，字符串类型</param>
        /// <param name="iv">加密使用的偏移量，字符串类型</param>
        /// <returns>返回结果，以字符串形式</returns>
        public static string EncryptToBase64(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = StringToBytes(key, 16);
            byte[] ivArray = StringToBytes(iv, 16);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt">要加密的字符串</param>
        /// <param name="keyArray">加密使用的密钥，字节码形式</param>
        /// <param name="ivArray">加密使用的偏移量，字节码形式</param>
        /// <returns>返回结果，以字符串形式</returns>
        public static string EncryptToBase64(string toEncrypt, byte[] keyArray, byte[] ivArray)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt">要加密的字符串</param>
        /// <param name="key">加密使用的密钥，字符串类型</param>
        /// <param name="iv">加密使用的偏移量，字符串类型</param>
        /// <returns>返回结果，以字节码形式</returns>
        public static byte[] Encrypt(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = StringToBytes(key, 16);//UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = StringToBytes(iv, 16);//UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toEncryptArray">以字节码形式给出密文</param>
        /// <param name="key">加密使用的密钥，字符串类型</param>
        /// <param name="iv">加密使用的偏移量，字符串类型</param>
        /// <returns>解密结果，字符串类型</returns>
        public static string DecryptFromBytes(byte[] toEncryptArray, string key, string iv)
        {
            byte[] keyArray = StringToBytes(key, 16);//UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = StringToBytes(iv, 16);//UTF8Encoding.UTF8.GetBytes(iv);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt">以Base64字符串形式给出密文</param>
        /// <param name="key">加密使用的密钥，字符串类型</param>
        /// <param name="iv">加密使用的偏移量，字符串类型</param>
        /// <returns>解密结果，字符串类型</returns>
        public static string DecryptFromBase64(string toDecrypt, string key, string iv)
        {
            byte[] keyArray = StringToBytes(key, 16);//UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = StringToBytes(iv, 16);//UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt">以Base64字符串形式给出密文</param>
        /// <param name="keyArray">加密使用的密钥，字节码类型</param>
        /// <param name="ivArray">加密使用的偏移量，字节码类型</param>
        /// <returns>解密结果，字符串类型</returns>
        public static string DecryptFromBase64(string toDecrypt, byte[] keyArray, byte[] ivArray)
        {
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 字符串转字节码(UTF-8)，自动填充
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="length">结果长度</param>
        /// <returns>经过转换后的字节码</returns>
        public static byte[] StringToBytes(string text, int length)
        {
            Byte[] ByteArray = new Byte[length];
            Array.Copy(Encoding.UTF8.GetBytes(text.PadRight(ByteArray.Length)), ByteArray, ByteArray.Length);
            return ByteArray;
        }

        /// <summary>
        /// 使用了非自带库Aes实现的加密
        /// </summary>
        /// <param name="toEncrypt">待加密的字符串</param>
        /// <param name="key">密钥，字符串给出</param>
        /// <param name="iv">偏移量，字符串给出</param>
        /// <returns>返回加密后的Base64格式编码</returns>
        public static string MyEncryptToBase64(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = StringToBytes(key, 16);
            byte[] ivArray = StringToBytes(iv, 16);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            byte[] resultArray = MyAes.Encrypt(toEncryptArray, keyArray, MyAes.Mode.CBC, ivArray, MyAes.Padding.PKCS7);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 使用了非自带库Aes实现的解密
        /// </summary>
        /// <param name="toDecrypt">待解密的Base64字符串</param>
        /// <param name="key">密钥，字符串形式</param>
        /// <param name="iv">偏移量，字符串形式</param>
        /// <returns></returns>
        public static string MyDecryptFromBase64(string toDecrypt, string key, string iv)
        {
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            byte[] keyArray = StringToBytes(key, 16);
            byte[] ivArray = StringToBytes(iv, 16);
            byte[] resultArray = MyAes.Decrypt(toEncryptArray, keyArray, MyAes.Mode.CBC, ivArray, MyAes.Padding.PKCS7);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }



    }
}
