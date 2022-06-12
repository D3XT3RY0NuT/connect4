using board;
using utilities;

namespace player
{
    public class Human : Player
    {
        public Human(int id, String name, ConsoleColor colour) : base(id, name, colour) {
                
        }

        public override int NextTurn() {
            int move = 0;
            String input = "";
            bool choosing = true;
            while ((move < 1 || move > 7) && choosing) {
                if (this.Colour == ConsoleColor.Blue)
                    Printing.PrintColouredText("BLUE ", ConsoleColor.Blue);
                else if (this.Colour == ConsoleColor.Red)
                    Printing.PrintColouredText("RED ", ConsoleColor.Red);
                else {
                    Printing.PrintColouredText("Support for custom colour not implemented yet. Using red as a colour.\n",
                            ConsoleColor.Red);
                    Printing.PrintColouredText("RED ", ConsoleColor.Red);
                }
                Console.Write("player turn: ");
                input = Input.GetInput(true); 
                if (input == "help" || input == "h")
                    Printing.PrintColouredText("Help during the game not implemented yet.\n", ConsoleColor.Yellow);
                else if (input == "undo" || input == "u")
                    Printing.PrintColouredText("Undo not implemented yet.\n", ConsoleColor.Yellow);
                else if (input == "quit" || input == "q")
                    choosing = false;
                else {
                    try {
                        move = Convert.ToInt32(input);
                    }
                    catch(Exception) {
                        Printing.PrintColouredText("Invalid input. Please try again.\n", ConsoleColor.Red);
                    }
                    if (move < 1 || move > 7)
                        Printing.PrintColouredText("Invalid input. Please try again.\n", ConsoleColor.Red);
                }
            }
            
            return move;
        }
    }
}
