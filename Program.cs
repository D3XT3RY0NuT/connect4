using MySql.Data.MySqlClient;

using board;
using game;
using player;
using utilities;

public class Program
{
    static String dbCredentials = @"server=localhost;userid=connect4;password=connect4;database=connect4";
    static MySqlConnection databaseConnection = new MySqlConnection(dbCredentials);

    public static Player? Login(ConsoleColor colour) {
        Printing.PrintColouredText("Login\n", ConsoleColor.Yellow);
        Console.Write("Your name: ");
        String name = Input.GetInput();
        Console.Write("Your password: ");
        String password = Input.GetSecureInput();
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

    private static Player? Register(ConsoleColor colour) {
        String name = "", password = "", passwordConfirmation = "";
        Printing.PrintColouredText("Register\n", ConsoleColor.Yellow);
        Console.Write("Your name: ");
        name = Input.GetInput();
        Console.Write("Your password: ");
        password = Input.GetSecureInput();
        while (password.Length < 6 || password.Length > 50) {
            if (password.Length < 6)
                Printing.PrintColouredText("Your password must be at least 6 characters long. Please try again: ",
                        ConsoleColor.Red);
            else 
                Printing.PrintColouredText("Your password must be less than 50 characters long. Please try again: ",
                        ConsoleColor.Red);
            password = Input.GetSecureInput();
        }
        Console.Write("Confirm your password: ");
        passwordConfirmation = Input.GetSecureInput();
        while (passwordConfirmation != password) {
            Printing.PrintColouredText("Passwords don't match. Please try again: ", ConsoleColor.Red);
            passwordConfirmation = Input.GetSecureInput();
        }
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
        while (true) {
            String prompt = Input.GetInput(true);
            if (prompt == "" || prompt == "y" || prompt == "yes") {
                return Login(colour);
            }
            else if (prompt == "" || prompt == "n" || prompt == "no")
                return Register(colour);
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
        Printing.PrintColouredText("stats", ConsoleColor.White);
        Console.WriteLine(": shows your stats");
        Printing.PrintColouredText("delete", ConsoleColor.White);
        Console.WriteLine(": deletes your account");
        Printing.PrintColouredText("quit", ConsoleColor.White);
        Console.WriteLine(": quits the game");
    }

    private static void New(Player player1) {
        bool choosing = true;
        bool reversedOrder = false; // If true, the creator of the game will start second
        String input = "";
        while (choosing) {
            Console.Write("Would you like to start first? [Y/n] ");
            input = Input.GetInput(true);
            if (input == "y" || input == "yes" || input == "" || input == "n" || input == "no")
                choosing = false;
            else
                Printing.PrintColouredText("Invalid input. Please answer \"yes\" or \"no\"", ConsoleColor.Red);
        }
        if (input == "quit")
            return;
        if (input == "n" || input == "no")
            reversedOrder = true;
        choosing = true;
        while (choosing) {
            Console.Write("Would you like to play against a human player or the computer? [Human/computer] ");
            input = Input.GetInput(true);
            if (input == "h" || input == "human" || input == "" || input == "c" || input == "computer")
                choosing = false;
            else
                Printing.PrintColouredText("Invalid input. Please type either \"human\" or \"computer\".\n", ConsoleColor.Red);
        }
        if (input == "quit")
            return;
        Player? player2 = null;
        if (input == "h" || input == "human" || input == "") {
            while(player2 == null)
                player2 = Login(ConsoleColor.Red);
        }
        else
            player2 = new Computer(ConsoleColor.Red);
        if (player1.Id == player2.Id) {
            Printing.PrintColouredText("Impossible to play a game against yourself.\n", ConsoleColor.Red);
            return;
        }
        Game game;
        if (reversedOrder) {
            Player aux = player1;
            player1 = player2;
            player2 = aux;
        }
        game = new Game(player1, player2);
        Cell gameResult = game.Start(); 
        MySqlCommand cmd;
        if (gameResult == Cell.Empty) {
            try {
                cmd = new MySqlCommand("UPDATE users SET games_played = games_played + 1 WHERE id = @id",
                        databaseConnection);
                cmd.Parameters.AddWithValue("@id", player1.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("UPDATE users SET games_played = games_played + 1 WHERE id = @id", 
                        databaseConnection);
                cmd.Parameters.AddWithValue("@id", player2.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch(Exception) {
                Printing.PrintColouredText("An internal error occured. The result of the game has not been uploaded to the server.\n",
                        ConsoleColor.Red);
                return;
            }
        }
        else {
            try {
                int winningId = gameResult == Cell.Player1 ? player1.Id : player2.Id;
                int losingId = gameResult == Cell.Player1 ? player2.Id : player1.Id;
                if (winningId != 0) {
                    cmd = new MySqlCommand("UPDATE users SET games_played = games_played + 1, games_won = games_won + 1 WHERE id = @id",
                            databaseConnection);
                    cmd.Parameters.AddWithValue("@id", winningId);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                if (losingId != 0) {
                    cmd = new MySqlCommand("UPDATE users SET games_played = games_played + 1, games_lost = games_lost + 1 WHERE id = @id",
                            databaseConnection);
                    cmd.Parameters.AddWithValue("@id", losingId);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception) {
                Printing.PrintColouredText("An internal error occured. The result of the game has not been uploaded to the server.\n",
                        ConsoleColor.Red);
                return;
            }
        }
    }

    private static void Stats(int id) {
        MySqlCommand cmd = new MySqlCommand("SELECT games_played, games_won, games_lost FROM users WHERE id = @id",
                databaseConnection);
        MySqlDataReader queryResult;
        try {
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            queryResult = cmd.ExecuteReader();
        }
        catch(Exception) {
            Printing.PrintColouredText("Error while trying to communicate with the server.\n", ConsoleColor.Red);
            return;
        }
        if (queryResult.Read()) {
            int games_played = queryResult.GetInt32("games_played");
            int games_won = queryResult.GetInt32("games_won");
            int games_lost = queryResult.GetInt32("games_lost");
            Printing.PrintColouredText("Games played: " + games_played + "\n", ConsoleColor.White);
            Printing.PrintColouredText("Games won: ", ConsoleColor.White);
            Printing.PrintColouredText(games_won + "\n", ConsoleColor.Green);
            Printing.PrintColouredText("Draws: ", ConsoleColor.White);
            Printing.PrintColouredText((games_played - games_won - games_lost) + "\n", ConsoleColor.Yellow);
            Printing.PrintColouredText("Games lost: ", ConsoleColor.White);
            Printing.PrintColouredText(games_lost + "\n", ConsoleColor.Red);
        }
        queryResult.Close();
    }

    private static bool Delete(int id) {
        while(true) {
            Printing.PrintColouredText("Are you sure you want to permanently delete your account? [y/N] ", ConsoleColor.DarkRed);
            String input = Input.GetInput(true);
            if (input == "y" || input == "yes") {
                Printing.PrintColouredText("Please enter your password to confirm. If you changed your mind, enter \"quit\". ",
                        ConsoleColor.DarkRed);
                input = Input.GetSecureInput();
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
            else if (input == "stats")
                Stats(player1.Id);
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
