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
        private int occupiedCells = 0;
        private Cell winningPlayer = Cell.Empty;
        private bool isGameOver = false;

        public Board() {
            for (int i = 0; i < Constants.MaxRow; i++) {
                for (int j = 0; j < Constants.MaxColumn; j++)
                    board[i, j] = Cell.Empty;
            }
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

        private bool ValidCoords(int x, int y) {
            return x >= 0 && x < Constants.MaxRow && y >= 0 && y < Constants.MaxColumn;
        }

        private int CheckDirection(int x, int y, int dx, int dy) {
            int res = 1;
            for (int i = 1; i <= 3 && ValidCoords(x + i * dx, y + i * dy); i++) {
                if (this.board[x, y] == this.board[x + i * dx, y + i * dy])
                    res++;
            }

            return res;
        }

        private bool CheckCell(int x, int y) {
            return CheckDirection(x, y, 1, -1) == 4 || CheckDirection(x, y, 1, 0) == 4 || CheckDirection(x, y, 1, 1) == 4
                || CheckDirection(x, y, 0, 1) == 4;
        }

        private void CheckBoard() {
            for (int i = 0; i < Constants.MaxRow; i++) {
                for (int j = 0; j < Constants.MaxColumn; j++) {
                    if (this.board[i, j] != Cell.Empty && CheckCell(i, j)) {
                        if (board[i, j] == Cell.Player1)
                            this.winningPlayer = Cell.Player1;
                        else
                            this.winningPlayer = Cell.Player2;
                        this.isGameOver = true;
                    }
                }
            }
            if (this.occupiedCells == Constants.MaxRow * Constants.MaxColumn)
                this.isGameOver = true;
        }

        public void PlayMove(int column, Cell player) {
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
            board[row, column] = player;
            this.occupiedCells++;
            CheckBoard();
        }
    }
}
