namespace utilities
{
    public static class Printing
    {
        public static void PrintColouredText(String text, ConsoleColor colour) {
            Console.ForegroundColor = colour;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    public static class Input
    {
        public static String GetInput(bool toLower = false) {
            Console.ForegroundColor = ConsoleColor.White;
            String input = Console.ReadLine() ?? ""; 
            if (toLower)
                input = input.ToLower();
            Console.ForegroundColor = ConsoleColor.Gray;

            return input;
        }
    }

    public static class Constants
    {
        public const int MinColumn = 1;
        public const int MaxColumn = 7;
        public const int MinRow = 1;
        public const int MaxRow = 6;
    }
}
