using System;
using Project0;

namespace Project0
{
	public class Hider
	{
		public string HidePassword()
		{
            ConsoleKeyInfo key;
            string code = "";
            do
            {
                key = Console.ReadKey(true);

                if (Char.IsNumber(key.KeyChar) || Char.IsLetter(key.KeyChar))
                {
                    Console.Write("*");
                    code += key.KeyChar.ToString();
                }
                
            } while (key.Key != ConsoleKey.Enter);
            
           int pwdLength = code.Length / 2;
            code.Remove(0, pwdLength).TrimEnd();
            return code;
        }
    }
}
