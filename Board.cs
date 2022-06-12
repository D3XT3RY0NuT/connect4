using utilities;

namespace board
{
    public enum Cell {
        Empty,
        Player1,
        Player2
    }

    public class Board
    {
        Cell[,] board = new Cell[6, 7];

        public Board() {
            for (int i = 0; i < 6; i++) {
                for (int j = 0; j < 7; j++)
                    board[i, j] = Cell.Empty;
            }
        }

        public void Display() {
            for (int i = 0; i < 6; i++) {
                Console.Write(" ");
                for (int j = 0; j < 7; j++) {
                    if (board[i, j] == Cell.Empty)
                        Console.Write("_ ");
                    else if (board[i, j] == Cell.Player1)
                        Printing.PrintColouredText("O ", ConsoleColor.Blue);
                    else
                        Printing.PrintColouredText("O ", ConsoleColor.Red);
                }
                Console.WriteLine($"{(char)('A' + i)}");
            }
            Console.Write(" ");
            for (int j = 0; j < 7; j++) 
                Console.Write($"{j + 1} ");
            Console.WriteLine(); 
        }
    }
}
