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
}
