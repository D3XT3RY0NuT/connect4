using board;
using utilities;

namespace player
{
    public class Computer : Player
    {
        Random random = new Random(DateTime.Now.Millisecond);
        public Computer(ConsoleColor colour) : base(0, "Computer", colour) {

        }

        private int ExploreSolutionTree(Cell currentPlayer, int depth, Board board) {
            if (depth == 0 || board.IsGameOver) {
                if (board.WinningPlayer == Cell.Player1)
                    return -1000;
                else if (board.WinningPlayer == Cell.Empty)
                    return 0;
                else
                    return 1000;
            }
            int bestMoveValue = -1000;
            bool[] possibleMoves = board.GetPossibleMoves();
            for (int j = 0; j < Constants.MaxColumn; j++) {
                if (possibleMoves[j]) {
                    board.PlayMove(j + 1, currentPlayer);
                    if (currentPlayer == Cell.Player1)
                        currentPlayer = Cell.Player2;
                    else
                        currentPlayer = Cell.Player1;
                    int moveValue = -ExploreSolutionTree(currentPlayer, depth - 1, board);
                    board.UndoMove();
                    if (moveValue > bestMoveValue)
                        bestMoveValue = moveValue;
                }
            }

            return bestMoveValue;
        }

        private int CalculateNextMove(Cell playerToMove, int depth, Board board) {
            int bestMove = 0;
            int bestMoveValue = -1000;
            int moveValue;
            bool[] possibleMoves = board.GetPossibleMoves();
            for (int j = 0; j < Constants.MaxColumn; j++) {
                if (possibleMoves[j]) {
                    board.PlayMove(j + 1, playerToMove);
                    Cell currentPlayer = playerToMove == Cell.Player1 ? Cell.Player2 : Cell.Player1;
                    moveValue = -ExploreSolutionTree(currentPlayer, depth - 1, board);
                    board.UndoMove();
                    if (moveValue > bestMoveValue) {
                        bestMove = j + 1;
                        bestMoveValue = moveValue;
                    }
                    if (bestMoveValue == 1000) {
                        Console.WriteLine(bestMove);
                        return bestMove;
                    }   
                }
            }

            return bestMove;
        }

        public override int NextTurn(Cell playerToMove, int depth, Board board) {
            int move = CalculateNextMove(playerToMove, depth, board);
            Printing.PrintColouredText($"{this.Name}'s ", this.Colour);
            Console.Write("turn: ");
            Printing.PrintColouredText($"{move}\n", ConsoleColor.White);
            return move;
        }
    }
}
