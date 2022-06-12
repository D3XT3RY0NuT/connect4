using board;
using exceptions;
using player;
using utilities;

namespace game
{
    public class Game
    {
        private Player player1;
        private Player player2;
        private Cell currentPlayer;
        private Board board;
        private bool running = true;

        public Game(Player player1, Player player2) {
            this.player1 = player1;
            this.player2 = player2;
            this.currentPlayer = Cell.Player1;
            this.board = new Board();
            this.Start();
        }

        private int NextTurn() {
            if (this.currentPlayer == Cell.Player1)
                return this.player1.NextTurn();
            else
                return this.player2.NextTurn();
        }

        private void ChangeCurrentPlayer() {
            if (this.currentPlayer == Cell.Player1)
                this.currentPlayer = Cell.Player2;
            else
                this.currentPlayer = Cell.Player1;
        }

        private void Start() {
            Printing.PrintColouredText("The game has been successfully created.\n", ConsoleColor.Green);
            Printing.PrintColouredText($"{player1.Name}", ConsoleColor.Blue);
            Printing.PrintColouredText($" vs ", ConsoleColor.Yellow);
            Printing.PrintColouredText($"{player2.Name}\n\n", ConsoleColor.Red);
            int move = 0;
            while (this.running && !this.board.IsGameOver) {
                this.board.Display();
                move = this.NextTurn();
                if (move == 0)
                    this.running = false;
                else {
                    try {
                        this.board.PlayMove(move, this.currentPlayer);
                        ChangeCurrentPlayer();
                    }
                    catch(BoardException) {
                        Printing.PrintColouredText("Invalid choice. Please choose again.\n", ConsoleColor.Red);
                    }
                    catch(Exception) {
                        Printing.PrintColouredText("Internal error occured. Please try again.\n", ConsoleColor.Red);
                    }
                }
            }
            this.board.Display();
            if (this.board.WinningPlayer == Cell.Player1)
                Printing.PrintColouredText($"{player1.Name} has won! Congratulations!\n", player1.Colour);
            else if (this.board.WinningPlayer == Cell.Player2)
                Printing.PrintColouredText($"{player2.Name} has won! Congratulations!\n", player2.Colour);
            else
                Printing.PrintColouredText($"Draw! This was a though game!\n", ConsoleColor.White);
            Printing.PrintColouredText("Game ended.\n", ConsoleColor.Gray);
        }
    }
}
