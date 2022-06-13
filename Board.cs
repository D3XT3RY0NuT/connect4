using exceptions;
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
        private Cell[,] board = new Cell[Constants.MaxRow, Constants.MaxColumn];
        private int[] moves = new int[Constants.MaxRow * Constants.MaxColumn];
        private int occupiedCells = 0;
        private Cell winningPlayer = Cell.Empty;
        private bool isGameOver = false;
        private ConsoleColor player1Colour = ConsoleColor.Blue;
        private ConsoleColor player2Colour = ConsoleColor.Red;

        public Board(ConsoleColor player1Colour, ConsoleColor player2Colour) {
            for (int i = 0; i < Constants.MaxRow; i++) {
                for (int j = 0; j < Constants.MaxColumn; j++)
                    board[i, j] = Cell.Empty;
            }
            this.player1Colour = player1Colour;
            this.player2Colour = player2Colour;
        }

        public bool IsGameOver {
            get {
                return isGameOver;
            }
        }

        public Cell WinningPlayer {
            get {
                return winningPlayer;
            }
        }

        public void Display() {
            for (int i = 0; i < Constants.MaxRow; i++) {
                Console.Write(" ");
                for (int j = 0; j < Constants.MaxColumn; j++) {
                    if (board[i, j] == Cell.Empty)
                        Console.Write("_ ");
                    else if (board[i, j] == Cell.Player1)
                        Printing.PrintColouredText("O ", player1Colour);
                    else
                        Printing.PrintColouredText("O ", player2Colour);
                }
                Console.WriteLine($"{(char)('A' + i)}");
            }
            Console.Write(" ");
            for (int j = 0; j < 7; j++) 
                Console.Write($"{j + 1} ");
            Console.WriteLine(); 
        }

        private bool ValidCoords(int x, int y) {
            return x >= 0 && x < Constants.MaxRow && y >= 0 && y < Constants.MaxColumn;
        }

        private int CheckDirection(int x, int y, int dx, int dy) {
            int res = 1;
            for (int i = 1; i <= 3 && ValidCoords(x + i * dx, y + i * dy); i++) {
                if (this.board[x, y] == this.board[x + i * dx, y + i * dy])
                    res++;
                else
                    break;
            }

            return res;
        }

        private bool CheckMove(int x, int y) {
            return (CheckDirection(x, y, 1, -1) + CheckDirection(x, y, -1, 1) >= 5)
                || CheckDirection(x, y, 1, 0) == 4
                || (CheckDirection(x, y, 1, 1) + CheckDirection(x, y, -1, -1) >= 5)
                || (CheckDirection(x, y, 0, 1) + CheckDirection(x, y, 0, -1) >= 5);
        }

        public bool[] GetPossibleMoves() {
            bool[] possibleMoves = new bool[Constants.MaxColumn];
            for (int j = 0; j < Constants.MaxColumn; j++) {
                if (this.board[0, j] == Cell.Empty)
                    possibleMoves[j] = true;
                else
                    possibleMoves[j] = false;
            }

            return possibleMoves;
        }

        public void PlayMove(int column, Cell player) {
            if (this.occupiedCells == Constants.MaxRow * Constants.MaxColumn)
                throw new BoardException("The board is full");
            column--; // Indexing starts from 0, not 1
            if (player == Cell.Empty)
                throw new InvalidPlayerException();
            if (column < Constants.MinColumn - 1 || column >= Constants.MaxColumn)
                throw new InvalidColumnException();
            int row = Constants.MaxRow - 1;
            while(row > -1 && board[row, column] != Cell.Empty)
                row--;
            if (row == -1)
                throw new InvalidColumnException();
            this.board[row, column] = player;
            this.moves[this.occupiedCells++] = column;
            if (CheckMove(row, column)) {
                winningPlayer = player;
                isGameOver = true;
            }
            if (this.occupiedCells == Constants.MaxRow * Constants.MaxColumn)
                isGameOver = true;
        }

        public void UndoMove() {
            if (this.occupiedCells == 0)
                throw new BoardException("No move has been played to be undone.");
            int column = moves[--this.occupiedCells];
            if (this.isGameOver) {
                this.isGameOver = false;
                this.winningPlayer = Cell.Empty;
            }
            int row = 0;
            while(this.board[row, column] == Cell.Empty)
                row++;
            this.board[row, column] = Cell.Empty;
        }
    }
}
