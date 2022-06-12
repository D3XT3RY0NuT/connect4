using utilities;

namespace player
{
    public abstract class Player
    {
        private int id;
        private String name;

        public Player(int id, String name) {
            if (name == "")
                Printing.PrintColouredText("WARNING: Creating a player without a name.\n", ConsoleColor.Yellow);

            if (id < 0)
                Printing.PrintColouredText("WARNING: Creating a player with negative ID value.\n", ConsoleColor.Yellow);

            this.name = name;
            this.id = id;
        }

        public String Name {
            get {
                return name;
            }
        }

        public int Id {
            get {
                return id;
            }
        }

        //public virtual nextTurn();
    }
}
