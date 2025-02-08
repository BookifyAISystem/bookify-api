using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Helper
{
	public class EncryptionHelper
	{
		public static string Encrypt(string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes("DNSoft@gmail.com");
			byte[] bytes2 = Encoding.UTF8.GetBytes(text);
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes("@DNSolution.com", null);
			byte[] bytes3 = passwordDeriveBytes.GetBytes(32);
			ICryptoTransform transform = new RijndaelManaged
			{
				Mode = CipherMode.CBC
			}.CreateEncryptor(bytes3, bytes);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(bytes2, 0, bytes2.Length);
			cryptoStream.FlushFinalBlock();
			byte[] inArray = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(inArray);
		}
		public static string Decrypt(string EncryptedText)
		{
			byte[] bytes = Encoding.ASCII.GetBytes("DNSoft@gmail.com");
			byte[] array = Convert.FromBase64String(EncryptedText);
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes("@DNSolution.com", null);
			byte[] bytes2 = passwordDeriveBytes.GetBytes(32);
			ICryptoTransform transform = new RijndaelManaged
			{
				Mode = CipherMode.CBC
			}.CreateDecryptor(bytes2, bytes);
			MemoryStream memoryStream = new MemoryStream(array);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array2 = new byte[array.Length];
			int count = cryptoStream.Read(array2, 0, array2.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(array2, 0, count);
		}
		public static T Decrypt<T>(string EncryptedText)
		{
			return JsonConvert.DeserializeObject<T>(Decrypt(EncryptedText));
		}
	}
}
