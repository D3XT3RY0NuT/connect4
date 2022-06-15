using board;
using utilities;

namespace player
{
    public class Human : Player
    {
        public Human(int id, String name, ConsoleColor colour) : base(id, name, colour) {
                
        }

        public override int NextTurn(Board board) {
            int move = 0;
            String input = "";
            bool choosing = true;
            while ((move < Constants.MinColumn || move > Constants.MaxColumn) && choosing) {
                Printing.PrintColouredText($"{this.Name}'s ", this.Colour);
                Console.Write("turn: ");
                input = Input.GetInput(true); 
                if (input == "help" || input == "h")
                    Printing.PrintColouredText("Help during the game not implemented yet.\n", ConsoleColor.Yellow);
                else if (input == "undo" || input == "u")
                    return -1;
                else if (input == "quit" || input == "q") {
                    Printing.PrintColouredText("Are you sure you want to abort the game? This will result in a defeat. [y/N] ",
                            ConsoleColor.Yellow);
                    input = Input.GetInput(true);
                    while(input != "yes" && input != "y" && input != "no" && input != "n" && input != "") {
                        Printing.PrintColouredText("Please answer yes or no. [y/N] ", ConsoleColor.Red);
                        input = Input.GetInput(true);
                    }
                    if (input == "yes" || input == "y")
                        choosing = false;
                }
                else {
                    try {
                        move = Convert.ToInt32(input);
                        if (move < Constants.MinColumn || move > Constants.MaxColumn)
                            Printing.PrintColouredText("Invalid input. Please try again.\n", ConsoleColor.Red);
                    }
                    catch(Exception) {
                        Printing.PrintColouredText("Invalid input. Please try again.\n", ConsoleColor.Red);
                    }
                }
            }
            
            return move;
        }
    }
}
