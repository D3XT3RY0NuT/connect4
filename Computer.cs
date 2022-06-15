using board;
using utilities;

namespace player
{
    public class Computer : Player
    {
        Random random = new Random(DateTime.Now.Millisecond);
        public Computer(ConsoleColor colour) : base(0, "Computer", colour) {

        }

        private int ExploreSolutionTree(int depth, Board board) {
            if (depth == 0 || board.IsGameOver) {
                int coef = board.PlayerToMove == Cell.Player1 ? 1 : -1;
                if (board.WinningPlayer == Cell.Player1)
                    return Constants.Infinity * coef;
                else if (board.WinningPlayer == Cell.Empty)
                    return 0;
                else
                    return -Constants.Infinity * coef;
            }
            int bestMoveValue = -Constants.Infinity;
            bool[] possibleMoves = board.GetPossibleMoves();
            for (int j = 0; j < Constants.MaxColumn; j++) {
                if (possibleMoves[j]) {
                    board.PlayMove(j + 1);
                    int moveValue = -ExploreSolutionTree(depth - 1, board);
                    board.UndoMove();
                    if (moveValue > bestMoveValue || moveValue == bestMoveValue && random.Next() % 2 == 0)
                        bestMoveValue = moveValue;
                }
            }

            return bestMoveValue;
        }

        private int CalculateNextMove(int depth, Board board) {
            if (depth <= 0)
                throw new Exception("Invalid depth value.");
            int bestMove = 0;
            int bestMoveValue = -Constants.Infinity - 1;
            int moveValue;
            bool[] possibleMoves = board.GetPossibleMoves();
            for (int j = 0; j < Constants.MaxColumn; j++) {
                if (possibleMoves[j]) {
                    board.PlayMove(j + 1);
                    moveValue = -ExploreSolutionTree(depth - 1, board);
                    board.UndoMove();
                    if (moveValue > bestMoveValue || moveValue == bestMoveValue && random.Next() % 2 == 0) {
                        bestMove = j + 1;
                        bestMoveValue = moveValue;
                    }
                    if (bestMoveValue == Constants.Infinity) {
                        Console.WriteLine(bestMove);
                        return bestMove;
                    }   
                }
            }

            return bestMove;
        }

        public override int NextTurn(Board board) {
            int move = CalculateNextMove(7, board);
            Printing.PrintColouredText($"{this.Name}'s ", this.Colour);
            Console.Write("turn: ");
            Printing.PrintColouredText($"{move}\n", ConsoleColor.White);
            return move;
        }
    }
}
