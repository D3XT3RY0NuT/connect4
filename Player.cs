using board;
using utilities;

namespace player
{
    public abstract class Player
    {
        private int id;
        private String name;
        private ConsoleColor colour;

        public Player(int id, String name, ConsoleColor colour) {
            if (name == "")
                Printing.PrintColouredText("WARNING: Creating a player without a name.\n", ConsoleColor.Yellow);
            if (id < 0)
                Printing.PrintColouredText("WARNING: Creating a player with negative ID value.\n", ConsoleColor.Yellow);
            if (colour != ConsoleColor.Blue && colour != ConsoleColor.Red)
                Printing.PrintColouredText("WARNING: Custom colours for players are not implemented yet.\n", ConsoleColor.Yellow);

            this.name = name;
            this.id = id;
            this.colour = colour;
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

        public ConsoleColor Colour {
            get {
                return colour;
            }
            set {
                colour = value;
            }
        }

        public abstract int NextTurn(Board board);
    }
}
