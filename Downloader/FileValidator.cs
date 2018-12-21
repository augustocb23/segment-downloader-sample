using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Downloader
{
	class FileValidator
	{
		public static bool Validate(string path, string hash)
		{
			using (SHA256 SHA256 = SHA256.Create())
			{
				FileStream fileStream = File.OpenRead(path);
				return hash == ComputeHash(fileStream);
			}
		}

		static string ComputeHash(Stream obj)
		{
			// Create a SHA256   
			using (SHA256 sha256Hash = SHA256.Create())
			{
				// ComputeHash - returns byte array  
				byte[] bytes = sha256Hash.ComputeHash(obj);

				// Convert byte array to a string   
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString().ToUpper();
			}
		}
	}
}
