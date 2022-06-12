using MySql.Data.MySqlClient;

using utilities;
using player;

public class Program
{
    static String credentials = @"server=localhost;userid=connect4;password=connect4;database=connect4";
    static MySqlConnection databaseConnection = new MySqlConnection(credentials);

    public static Player? Login(String name, String password) {
        bool ok = true;
        MySqlCommand cmd = new MySqlCommand($"SELECT * FROM users WHERE name = @name",
                databaseConnection);
        int id = 0;
        try {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            MySqlDataReader result = cmd.ExecuteReader();
            if (result.Read() == false) {
                Printing.PrintColouredText("No user with such name.\n", ConsoleColor.Red);
                ok = false;
            }
            else {
                if (result.GetString("password") == password)
                    id = result.GetInt32("id");
                else {
                    Printing.PrintColouredText("Wrong password.\n", ConsoleColor.Red);
                    ok = false;
                }
            }
            result.Close();
        }
        catch(Exception) {
            Printing.PrintColouredText("Error while attempting to login.\n", ConsoleColor.Red);
            ok = false;
        }
        if (ok)
            return new Human(name);
        return null;
    }

    public static Player? Register(String name, String password) {
        bool ok = true;
        MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE name = @name",
                databaseConnection);
        try {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Prepare();
            MySqlDataReader result = cmd.ExecuteReader();
            if (result.Read()) {
                Printing.PrintColouredText("There is already an user with such name.\n", ConsoleColor.Red);
                ok = false;
            }
            result.Close();
        }
        catch(Exception e) {
            Printing.PrintColouredText(e.Message + "\n", ConsoleColor.Red);
            Printing.PrintColouredText("Error while attempting to create a new user.\n", ConsoleColor.Red);
            return null;
        }
        if (!ok)
            return null;
        cmd = new MySqlCommand("INSERT INTO users(name, password) VALUES(@name, @password)",
                databaseConnection);
        try {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        catch(Exception e) {
            Printing.PrintColouredText(e.Message + "\n", ConsoleColor.Red);
            Printing.PrintColouredText("Error while attempting to create a new user.\n", ConsoleColor.Red);
            return null;
        } 

        return new Human(name);
    }
    
    public static Player? Connect() {
        // Authenfication process
        Console.Write("Do you have an account? [Y/n] ");
        String name, password;
        while (true) {
            String prompt = Console.ReadLine()?.ToLower() ?? "";
            if (prompt == "" || prompt == "y" || prompt == "yes") {
                Printing.PrintColouredText("Login\n", ConsoleColor.Yellow);
                Console.Write("Your name: ");
                Console.ForegroundColor = ConsoleColor.White;
                name = Console.ReadLine() ?? "";
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Your password: ");
                Console.ForegroundColor = ConsoleColor.White;
                password = Console.ReadLine() ?? "";
                Console.ForegroundColor = ConsoleColor.Gray;
                return Login(name, password);
            }
            else if (prompt == "" || prompt == "n" || prompt == "no") {
                Printing.PrintColouredText("Register\n", ConsoleColor.Yellow);
                Console.Write("Your name: ");
                Console.ForegroundColor = ConsoleColor.White;
                name = Console.ReadLine() ?? "";
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Your password: ");
                Console.ForegroundColor = ConsoleColor.White;
                password = Console.ReadLine() ?? "";
                Console.ForegroundColor = ConsoleColor.Gray;
                return Register(name, password);
            }
            else {
                Console.Write("Invalid value. Please answer yes or no. [Y/n] ");
            }
        }   
    }
    public static void Main(String[] args)
    {
        // Connecting to the database
        databaseConnection.Open();
        MySqlCommand cmd = new MySqlCommand("SELECT VERSION()", databaseConnection);
        String version = cmd.ExecuteScalar().ToString() ?? "Not found";
        Console.WriteLine($"Using MySQL version ${version}");
        if (args.Length == 1 && args[0] == "--help") {
            Console.WriteLine("Builtin help not implemented yet.");
            return;
        }
        // Getting the user's credentials
        Player? player1 = Connect();
        while(player1 == null) {
            Printing.PrintColouredText("Connection failed.\n", ConsoleColor.Red);
            player1 = Connect();
        }
        Console.WriteLine("Hello " + player1.Name);
        // Closing the connection to the database
        databaseConnection.Close();
    }
}
