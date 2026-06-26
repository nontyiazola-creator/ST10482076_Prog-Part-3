using System;

namespace CybersecurityChatbott
{
    public static class User
    {
        public static string GetName()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n👤 Enter your name: ");
            return Console.ReadLine();
        }
    }
}


