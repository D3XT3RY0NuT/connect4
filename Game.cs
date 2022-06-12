using board;
using player;
using utilities;

namespace game
{
    public class Game
    {
        private Player player1;
        private Player player2;
        private Board board;
        private bool running = true;

        public Game(Player player1, Player player2) {
            this.player1 = player1;
            this.player2 = player2;
            this.board = new Board();
            this.Start();
        }

        private int AskForInput(Cell currentPlayerTurn) {
            int move = 0;
            String input = "";
            while ((move < 1 || move > 7) && this.running) {
                if (currentPlayerTurn == Cell.Player1)
                    Printing.PrintColouredText("BLUE ", ConsoleColor.Blue);
                else 
                    Printing.PrintColouredText("RED ", ConsoleColor.Red);
                Console.Write("player turn: ");
                input = Input.GetInput(true); 
                if (input == "help" || input == "h")
                    Printing.PrintColouredText("Help during the game not implemented yet.\n", ConsoleColor.Yellow);
                else if (input == "undo" || input == "u")
                    Printing.PrintColouredText("Undo not implemented yet.\n", ConsoleColor.Yellow);
                else if (input == "quit" || input == "q")
                    this.running = false;
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

        private Cell ChangeCurrentPlayer(Cell currentPlayer) {
            if (currentPlayer == Cell.Player1)
                return Cell.Player2;
            
            return Cell.Player1;
        }

        private void Start() {
            Cell currentPlayerTurn = Cell.Player1;
            Printing.PrintColouredText("The game has been successfully created.\n", ConsoleColor.Green);
            Printing.PrintColouredText($"{player1.Name}", ConsoleColor.Blue);
            Printing.PrintColouredText($" vs ", ConsoleColor.Yellow);
            Printing.PrintColouredText($"{player2.Name}\n\n", ConsoleColor.Red);
            while (this.running) {
                board.Display();
                AskForInput(currentPlayerTurn);
                currentPlayerTurn = ChangeCurrentPlayer(currentPlayerTurn);
            }
            Printing.PrintColouredText("Game ended.\n", ConsoleColor.Gray);
        }
    }
}
