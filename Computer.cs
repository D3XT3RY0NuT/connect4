namespace player
{
    public class Computer : Player
    {
        Random random = new Random(DateTime.Now.Millisecond);
        public Computer(ConsoleColor colour) : base(0, "Computer", colour) {

        }

        public override int NextTurn() {
            return random.Next(1, 8);
        }
    }
}
