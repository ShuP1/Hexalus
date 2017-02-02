using System;
namespace Server
{
	  public static class StringManager
	  {
			private static readonly Random _random = new Random();
			public static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopkrqtuvwxyz-0123456789";

			public static string RandomString(int size)
			{
				  char[] buffer = new char[size];

				  for(int i = 0; i < size; i++)
				  {
						buffer[i] = chars[_random.Next(chars.Length)];
				  }
				  return new string(buffer);
			}

			public static bool IsCorrect(string text)
			{
				  if(text == string.Empty || text == null || text == "")
						return false;

				  for(int i = 0; i < text.Length; i++)
				  {
						if(!chars.Contains(text.Substring(i, 1)))
							  return false;
				  }
				  return true;
			}
	  }
}
