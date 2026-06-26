using System;
using System.Threading;
using System.Media;

namespace CybersecurityChatbott
{
    public static class UI
    {
        public static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================================================");
            Console.WriteLine("      🔐 CYBERSECURITY AWARENESS BOT 🔐");
            Console.WriteLine("==================================================");

            Console.WriteLine(@"
   _____                 _                 
  / ____|               | |                
 | |     _ __ ___  _   _| | ___  ___       
 | |    | '__/ _ \| | | | |/ _ \/ __|      
 | |____| | | (_) | |_| | |  __/\__ \      
  \_____|_|  \___/ \__,_|_|\___||___/      
        ");

            Console.WriteLine("==================================================");
            Console.ResetColor();

            // Play a short system sound to notify the user (works without external files)
            try
            {
                SystemSounds.Asterisk.Play();
            }
            catch
            {
                // Ignore any errors when attempting to play sound (platform may not support it)
            }
        }

        public static void ShowWelcome(string name)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            TypeText($"\nHello, {name}! Welcome to the Cybersecurity Awareness Bot.");

            // small notification sound for welcome
            try
            {
                SystemSounds.Exclamation.Play();
            }
            catch
            {
            }
        }

        public static void TypeText(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }
            Console.WriteLine();
        }
    }
}