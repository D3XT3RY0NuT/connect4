using utilities;

namespace player
{
    public abstract class Player
    {
        String name;

        public Player(String name) {
            if (name == "") {
                Printing.PrintColouredText("WARNING: Creating a player without a name.\n", ConsoleColor.Yellow);
            }
            this.name = name;
        }

        public String Name {
            get {
                return name;
            }
        }

        //public virtual nextTurn();
    }
}
