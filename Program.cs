using MySql.Data.MySqlClient;

using game;
using player;
using utilities;

public class Program
{
    static String dbCredentials = @"server=localhost;userid=connect4;password=connect4;database=connect4";
    static MySqlConnection databaseConnection = new MySqlConnection(dbCredentials);

    public static Player? Login(String name, String password, ConsoleColor colour) {
        MySqlCommand cmd = new MySqlCommand($"SELECT * FROM users WHERE name = @name",
                databaseConnection);
        int id = 1;
        try {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            MySqlDataReader result = cmd.ExecuteReader();
            if (result.Read() == false) {
                Printing.PrintColouredText("No user with such name.\n", ConsoleColor.Red);
                id = 0;
            }
            else {
                if (result.GetString("password") == password)
                    id = result.GetInt32("id");
                else {
                    Printing.PrintColouredText("Wrong password.\n", ConsoleColor.Red);
                    id = 0;
                }
            }
            result.Close();
        }
        catch(Exception) {
            Printing.PrintColouredText("Error while attempting to login.\n", ConsoleColor.Red);
            id = 0;
        }
        if (id != 0) {
            Printing.PrintColouredText($"Welcome back, {name}!\n", ConsoleColor.Green);
            return new Human(id, name, colour);
        }

        return null;
    }

    private static Player? Register(String name, String password, ConsoleColor colour) {
        int id = 1;
        MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE name = @name",
                databaseConnection);
        try {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Prepare();
            MySqlDataReader result = cmd.ExecuteReader();
            if (result.Read()) {
                Printing.PrintColouredText("There is already an user with such name.\n", ConsoleColor.Red);
                id = 0;
            }
            result.Close();
        }
        catch(Exception) {
            Printing.PrintColouredText("Error while attempting to create a new user.\n", ConsoleColor.Red);
            return null;
        }
        if (id == 0)
            return null;
        cmd = new MySqlCommand("INSERT INTO users(name, password) VALUES(@name, @password)",
                databaseConnection);
        try {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        catch(Exception) {
            Printing.PrintColouredText("Error while attempting to create a new user.\n", ConsoleColor.Red);
            return null;
        } 
        cmd = new MySqlCommand("SELECT MAX(id) FROM users",
            databaseConnection);
        MySqlDataReader queryResult = cmd.ExecuteReader();
        queryResult.Read();
        id = queryResult.GetInt32(0);
        queryResult.Close();
        Printing.PrintColouredText("Registration completed.\n", ConsoleColor.Green);

        return new Human(id, name, colour);
    }
    
    private static Player? Connect(ConsoleColor colour) {
        // Authenfication process
        Console.Write("Do you have an account? [Y/n] ");
        String name, password;
        while (true) {
            String prompt = Input.GetInput(true);
            if (prompt == "" || prompt == "y" || prompt == "yes") {
                Printing.PrintColouredText("Login\n", ConsoleColor.Yellow);
                Console.Write("Your name: ");
                Console.ForegroundColor = ConsoleColor.White;
                name = Input.GetInput();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Your password: ");
                Console.ForegroundColor = ConsoleColor.White;
                password = Input.GetInput();
                Console.ForegroundColor = ConsoleColor.Gray;
                return Login(name, password, colour);
            }
            else if (prompt == "" || prompt == "n" || prompt == "no") {
                Printing.PrintColouredText("Register\n", ConsoleColor.Yellow);
                Console.Write("Your name: ");
                Console.ForegroundColor = ConsoleColor.White;
                name = Input.GetInput();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Your password: ");
                Console.ForegroundColor = ConsoleColor.White;
                password = Input.GetInput();
                Console.ForegroundColor = ConsoleColor.Gray;
                return Register(name, password, colour);
            }
            else {
                Printing.PrintColouredText("Invalid value. Please answer yes or no. [Y/n] ", ConsoleColor.Red);
            }
        }   
    }

    private static void Help() {
        Printing.PrintColouredText("help", ConsoleColor.White);
        Console.WriteLine(": lists all possible actions");
        Printing.PrintColouredText("new", ConsoleColor.White);
        Console.WriteLine(": creates a new game");
        Printing.PrintColouredText("delete", ConsoleColor.White);
        Console.WriteLine(": deletes your account");
        Printing.PrintColouredText("quit", ConsoleColor.White);
        Console.WriteLine(": quits the game");
    }

    private static void New(Player player1) {
        bool choosing = true;
        String input = "";
        while (choosing) {
            Console.Write("Would you like to play against a human player or the computer? [Human/computer] ");
            input = Input.GetInput(true);
            if (input == "h" || input == "human" || input == "" || input == "c" || input == "computer")
                choosing = false;
            else
                Printing.PrintColouredText("Invalid input. Please type either \"human\" or \"computer\".\n", ConsoleColor.Red);
        }
        Player? player2 = null;
        if (input == "h" || input == "human" || input == "") {
            while(player2 == null)
                player2 = Connect(ConsoleColor.Red);
        }
        else
            player2 = new Computer(ConsoleColor.Red);
        if (player1.Id == player2.Id) {
            Printing.PrintColouredText("Impossible to play a game against yourself.\n", ConsoleColor.Red);
            return;
        }
        Game game = new Game(player1, player2);
    }

    private static bool Delete(int id) {
        while(true) {
            Printing.PrintColouredText("Are you sure you want to permanently delete your account? [y/N] ", ConsoleColor.DarkRed);
            String input = Input.GetInput(true);
            if (input == "y" || input == "yes") {
                Printing.PrintColouredText("Please enter your password to confirm. If you changed your mind, enter \"quit\". ",
                        ConsoleColor.DarkRed);
                input = Input.GetInput();
                if (input == "quit") {
                    Printing.PrintColouredText("Deletion process was aborted\n", ConsoleColor.Yellow);
                    return true;
                }
                MySqlCommand cmd = new MySqlCommand("DELETE FROM users WHERE id = @id AND password = @input",
                        databaseConnection); 
                try {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@input", input);
                    cmd.Prepare();
                    if (cmd.ExecuteNonQuery() == -1) {
                        Printing.PrintColouredText("Wrong password.\n", ConsoleColor.Red);
                        return true;
                    }
                }
                catch(Exception e) {
                    Console.WriteLine(e.Message);
                    Printing.PrintColouredText("An error occured while trying to delete the account. The process was aborted.\n",
                            ConsoleColor.Red);
                    return true;
                }
                Printing.PrintColouredText("The account has been succesfully deleted.\n", ConsoleColor.Green);

                return false;
            }
            else if (input == "n" || input == "no" || input == "")
                return true;
            else
                Printing.PrintColouredText("Invalid input.\n", ConsoleColor.Red);
        }
    }

    private static bool Quit() {
        while(true) {
            Printing.PrintColouredText("Are you sure you want to quit? [y/N] ", ConsoleColor.Yellow);
            String input = Input.GetInput(true);
            if (input == "y" || input == "yes")
                return false;
            else if (input == "n" || input == "no" || input == "")
                return true;
            else 
                Printing.PrintColouredText("Invalid input.\n", ConsoleColor.Red);
        }
    }

    public static void Main(String[] args)
    {
        // Connecting to the database
        try {
            databaseConnection.Open();
        }
        catch(Exception) {
            Printing.PrintColouredText("Unable to connect to the database server.\n", ConsoleColor.Red);
            return;
        }
        if (args.Length == 1) {
            if (args[0] == "--help") {
                Console.WriteLine("Builtin help not implemented yet.");
                return;
            }
            else if (args[0] == "--version") {
                Console.WriteLine("Connect4 v0.1");
                return;
            }
            else
                Printing.PrintColouredText($"The given argument \"{args[0]}\" not recognised. Ignoring it.\n", ConsoleColor.Yellow);
        }
        else if (args.Length > 1) 
            Printing.PrintColouredText("The given arguments are not supported and ignored.\n", ConsoleColor.Yellow);
        // Displaying the logo of the game
        Console.WriteLine(@"  #####                                           #       
 #     #  ####  #    # #    # ######  ####  ##### #    #  
 #       #    # ##   # ##   # #      #    #   #   #    #  
 #       #    # # #  # # #  # #####  #        #   #    #  
 #       #    # #  # # #  # # #      #        #   ####### 
 #     # #    # #   ## #   ## #      #    #   #        #  
  #####   ####  #    # #    # ######  ####    #        #  
                                                          ");
        // Getting the user's credentials
        Player? player1 = Connect(ConsoleColor.Blue);
        while(player1 == null) {
            Printing.PrintColouredText("Connection failed.\n", ConsoleColor.Red);
            player1 = Connect(ConsoleColor.Blue);
        }
        Console.WriteLine("Type \"help\" for a list of all possible actions");
        bool connected = true;
        String input = "";
        while(connected) {
            Console.Write("What would you like to do? ");
            input = Input.GetInput(true);
            if (input == "help")
                Help();
            else if (input == "new") 
                New(player1);
            else if (input == "delete")
                connected = Delete(player1.Id);
            else if (input == "quit") 
                connected = Quit();
            else
                Printing.PrintColouredText("Invalid input\n", ConsoleColor.Red);
            Console.WriteLine("");
        }
        // Closing the connection to the database
        databaseConnection.Close();
    }
}
