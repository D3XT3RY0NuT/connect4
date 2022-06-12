using utilities;

namespace player
{
    public class Computer : Player
    {
        Random random = new Random(DateTime.Now.Millisecond);
        public Computer(ConsoleColor colour) : base(0, "Computer", colour) {

        }

        public override int NextTurn() {
            int move = random.Next(Constants.MinColumn, Constants.MaxColumn + 1) ;
            Printing.PrintColouredText($"{this.Name}'s ", this.Colour);
            Console.Write("turn: ");
            Printing.PrintColouredText($"{move}\n", ConsoleColor.White);
            return move;
        }
    }
}
