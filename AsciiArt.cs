namespace CybersecurityChatbott
{
    public static class AsciiArt
    {
        public static void Display()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(@"
========================================
   CYBERSECURITY AWARENESS BOT
========================================

        🔒   🔒   🔒
      [  STAY SAFE ONLINE  ]
        🔒   🔒   🔒

");

            Console.ResetColor();
        }
    }
}
