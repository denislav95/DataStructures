using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Wintellect.PowerCollections;

public class ScoreboardSlowMain
{
    static void Main()
    {
        var commandExecutor = new ScoreboardCommandSlowExecutor();
        while (true)
        {
            string command = Console.ReadLine();
            if (command == "End")
            {
                break;
            }
            if (command != "")
            {
                string commandResult = commandExecutor.ProcessCommand(command);
                Console.WriteLine(commandResult);
            }
        }
    }
}

public class ScoreboardCommandSlowExecutor
{
    private ScoreboardSlow scoreboard = new ScoreboardSlow();

    public string ProcessCommand(string commandLine)
    {
        var tokens = commandLine.Split(new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries);
        var command = tokens[0];
        switch (command)
        {
            case "RegisterUser":
                return RegisterUser(tokens[1], tokens[2]);
            case "RegisterGame":
                return RegisterGame(tokens[1], tokens[2]);
            case "AddScore":
                return AddScore(tokens[1], tokens[2], tokens[3], tokens[4],
                    int.Parse(tokens[5]));
            case "ShowScoreboard":
                return ShowScoreboard(tokens[1]);
            case "DeleteGame":
                return DeleteGame(tokens[1], tokens[2]);
            case "ListGamesByPrefix":
                return ListGamesByPrefix(tokens[1]);
            default:
                return "Incorrect command";
        }
    }

    private string RegisterUser(string username, string userPassword)
    {
        if (this.scoreboard.RegisterUser(username, userPassword))
        {
            return "User registered";
        }

        return "Duplicated user";
    }

    private string RegisterGame(string gameName, string gamePassword)
    {
        if (this.scoreboard.RegisterGame(gameName, gamePassword))
        {
            return "Game registered";
        }

        return "Duplicated game";
    }

    private string AddScore(string username, string userPassword,
        string gameName, string gamePassword, int score)
    {
        if (this.scoreboard.AddScore(username, userPassword,
            gameName, gamePassword, score))
        {
            return "Score added";
        }

        return "Cannot add score";
    }

    private string ShowScoreboard(string gameName)
    {
        var scoreboardEntries = this.scoreboard.ShowScoreboard(gameName);
        if (scoreboardEntries == null)
        {
            return "Game not found";
        }

        if (scoreboardEntries.Any())
        {
            var result = new StringBuilder();
            int counter = 0;
            foreach (var entry in scoreboardEntries)
            {
                counter++;
                result.AppendFormat("#{0} {1} {2}",
                    counter, entry.Username, entry.Score);
                result.AppendLine();
            }
            result.Length -= Environment.NewLine.Length;
            return result.ToString();
        }

        return "No score";
    }

    private string DeleteGame(string gameName, string gamePassword)
    {
        if (this.scoreboard.DeleteGame(gameName, gamePassword))
        {
            return "Game deleted";
        }

        return "Cannot delete game";
    }

    private string ListGamesByPrefix(string namePrefix)
    {
        var matchedGames = this.scoreboard.ListGamesByPrefix(namePrefix);
        if (matchedGames.Any())
        {
            return string.Join(", ", matchedGames);
        }

        return "No matches";
    }
}

public class ScoreboardSlow
{
    private Dictionary<string, string> usersPasswords =
        new Dictionary<string, string>();

    private Dictionary<string, string> gamesPasswords =
        new Dictionary<string, string>();

    private OrderedDictionary<string, OrderedBag<ScoreboardEntry>> scoreboardEntries =
        new OrderedDictionary<string, OrderedBag<ScoreboardEntry>>
            ((g1, g2) => StringComparer.Ordinal.Compare(g1, g2));

    public bool RegisterUser(string username, string password)
    {
        if (this.usersPasswords.ContainsKey(username))
        {
            return false;
        }

        this.usersPasswords.Add(username, password);

        return true;
    }

    public bool RegisterGame(string game, string password)
    {
        if (this.gamesPasswords.ContainsKey(game))
        {
            return false;
        }

        this.gamesPasswords.Add(game, password);
        this.scoreboardEntries[game] = new OrderedBag<ScoreboardEntry>();

        return true;
    }

    public bool AddScore(string username, string userPassword, 
        string game, string gamePassword, int score)
    {
        if (!this.usersPasswords.ContainsKey(username)
            || this.usersPasswords[username] != userPassword)
        {
            return false;
        }

        if (!this.gamesPasswords.ContainsKey(game)
            || this.gamesPasswords[game] != gamePassword)
        {
            return false;
        }


        this.scoreboardEntries[game].Add(new ScoreboardEntry(score, username));

        return true;
    }

    public IEnumerable<ScoreboardEntry> ShowScoreboard(string game)
    {
        if (!this.gamesPasswords.ContainsKey(game))
        {
            return null;
        }

        var score = this.scoreboardEntries[game].Take(10);

        return score;
    }

    public bool DeleteGame(string game, string gamePassword)
    {
        if (!this.gamesPasswords.ContainsKey(game) ||
            this.gamesPasswords[game] != gamePassword)
        {
            return false;
        }

        this.gamesPasswords.Remove(game);
        this.scoreboardEntries.Remove(game);

        return true;
    }

    public IEnumerable<string> ListGamesByPrefix(string gameNamePrefix)
    {
        var upperBound = gameNamePrefix + char.MaxValue;
        var gameWithPrefix = this.scoreboardEntries.Range(gameNamePrefix, true, upperBound, false);

        return gameWithPrefix.Select(p => p.Key).Take(10);
    }
}

public class ScoreboardEntry : IComparable<ScoreboardEntry>
{
    public ScoreboardEntry(int score, string userName)
    {
        this.Score = score;
        this.Username = userName;
    }

    public int Score { get; set; }

    public string Username { get; set; }

    public int CompareTo(ScoreboardEntry other)
    {
        if (this.Score == other.Score)
        {
            return this.Username.CompareTo(other.Username);
        }

        return other.Score.CompareTo(this.Score);
    }
}
