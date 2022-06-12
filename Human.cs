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
            while ((move < Constants.MinColumn || move > Constants.MaxColumn) && choosing) {
                Printing.PrintColouredText($"{this.Name}'s ", this.Colour);
                Console.Write("turn: ");
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
