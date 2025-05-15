using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLibrary.Models;

namespace BattleshipApp
{
    internal class Program
    {
        public static List<PlayerModel> players = new List<PlayerModel>();
        public static PlayerModel whoIsWinner = null;

        static void Main(string[] args)
        {
            Run();
            string restartApp = Console.ReadLine().ToLower();
            if (restartApp == "yes")
            {
               Console.Clear();
               Console.WriteLine("Go Again: ");
               Run();
            }
        }

        public static void Run()
        {
            Console.WriteLine("Welcome to the Battleship App!");
            Console.WriteLine("You will need to enter positions from A1-E5.");
            int playerCount = HowManyPlayers();
            AddPlayersToList(playerCount);
            PlayerModel playerTurn = WhichPlayerGoesFirst(players);
            Console.Clear();
            PlayerTurns(playerTurn);
            Console.WriteLine($"Winner! Player {whoIsWinner.PlayerNumber}!");
            Console.WriteLine($"Number of shots take: {whoIsWinner.Stats}");
            Console.WriteLine();
        }

        private static void PlayerTurns(PlayerModel playerTurn)
        {
            do
            {
                foreach (PlayerModel player in players)
                {
                    if (playerTurn.PlayerNumber == player.PlayerNumber)
                    {
                        BuildOppenantGrid(playerTurn);
                        Console.WriteLine();
                        Console.Write($"Player {playerTurn.PlayerNumber}, take your shot: ");
                        PlayerModel targetPlayer = TargetPlayer(playerTurn);
                        CheckIfHitOrMiss(playerTurn, targetPlayer);
                        playerTurn = targetPlayer;
                    }
                    if (whoIsWinner != null)
                    {
                        break;
                    }
                }
            } while (whoIsWinner == null);
        }

        private static void CheckIfHitOrMiss(PlayerModel playerTurn, PlayerModel targetPlayer)
        {
            string fireShot = GetPosition(playerTurn).ToUpper();
            string targetHit = targetPlayer.ShipLocation.Find(s => s == fireShot);
            int targetPosition = playerTurn.OpponentGrid.IndexOf(fireShot);
            if (targetHit != null)
            {
                Console.WriteLine("Hit!");
                playerTurn.ShotsTaken.Add(fireShot);
                targetPosition = playerTurn.OpponentGrid.IndexOf(fireShot);
                playerTurn.OpponentGrid[targetPosition] = "O";
                targetPlayer.ShipLocation.Remove(fireShot);
                playerTurn.Stats += 1;
                if (targetPlayer.ShipLocation.Count == 0)
                {
                    whoIsWinner = playerTurn;
                }
            }
            else
            {
                Console.WriteLine("Miss!");
                targetPosition = playerTurn.OpponentGrid.IndexOf(fireShot);
                playerTurn.OpponentGrid[targetPosition] = "X";
                playerTurn.Stats += 1;
            }
        }

        private static void BuildOppenantGrid(PlayerModel playerTurn)
        {
            for (int j = 0; j < playerTurn.OpponentGrid.Count; j++)
            {
                if (j == 4 || j == 9 || j == 14 || j == 19)
                {
                    Console.WriteLine(playerTurn.OpponentGrid[j]);
                }
                else
                {
                    Console.Write(playerTurn.OpponentGrid[j] + " ");
                }
            }
        }
        private static bool VerifyIfShotAlreadyTaken(string shot, PlayerModel player)
        {
            if(player.ShotsTaken.Contains(shot) != false)
            {
                return false;
            }
            return true;
        }

        private static PlayerModel TargetPlayer(PlayerModel playerTurn)
        {
            if (players.Count <= 2)
            {
                foreach (PlayerModel player in players)
                {
                    if (playerTurn.PlayerNumber != player.PlayerNumber)
                    {
                        return player;
                    }
                }
            }
            else
            {
                Console.WriteLine("Choose Target: ");
                int targetPlayer = int.Parse(Console.ReadLine());
                foreach (PlayerModel player in players)
                {
                    if (playerTurn.PlayerNumber == targetPlayer)
                    {
                        return player;
                    }
                }
            }
            return null;
        }

        private static string GetPosition(PlayerModel player)
        {
            bool isValid = false;
            string shot = "";
            do
            {
                shot = Console.ReadLine().ToUpper();
                isValid = VerifyCharacterCount(shot);
                if (isValid != true)
                {
                    Console.WriteLine("Please ensure character count is 2.");
                    continue;
                }
                isValid = VerifyShotLetterAndNumber(shot);
                if (isValid != true)
                {
                    Console.WriteLine("Please ensure you're entering correct values.");
                    continue;
                }

                isValid = VerifyIfShotAlreadyTaken(shot, player);
                if (isValid != true)
                {
                    Console.WriteLine("Shot already taken. Please try again");
                    continue;
                }
            }
            while (isValid != true);
            return shot;
        }

        private static bool VerifyCharacterCount(string shot)
        {
            if (shot.Length > 2)
            {
                return false;
            }
            return true;
        }

        private static bool VerifyShotLetterAndNumber(string shot)
        {
            if (shot.Length == 1)
            {
                return false;
            }
            string letter = shot.Substring(0, 1).ToLower();
            string number = shot.Substring(1, 1);
            if (letter != "a" && letter != "b" && letter != "c" &&  letter != "d" && letter != "e") 
            {
                return false;
            }
            if (number != "1" && number != "2" && number != "3" && number != "4" && number != "5")
            {
                return false;
            }
            return true;
        }

        private static void AddPlayersToList(int playerCount)
        {
            for (int i = 0; i < playerCount; i++)
            {
                players.Add(CreatePlayer(i));
            }
        }

        private static PlayerModel WhichPlayerGoesFirst(List<PlayerModel> players)
        {
            Console.Write("Which player should go first: ");
            bool isValid = int.TryParse(Console.ReadLine(), out int result);
            while (isValid != true)
            {
                Console.Write("Please enter a valid number");
                isValid = int.TryParse(Console.ReadLine(), out result);
            }
            foreach (PlayerModel player in players) 
            { 
                if (player.PlayerNumber == result)
                {
                    return player;
                }
            }
            return null;
        }



        private static PlayerModel CreatePlayer(int playerNumber)
        {
            PlayerModel player = new PlayerModel();
            player.PlayerNumber = playerNumber + 1;
            Console.Write($"Please enter your name player {player.PlayerNumber}: ");
            player.FirstName = Console.ReadLine();
            player = GetShipPositions(player);
            player.OpponentGrid = CreateGrid();
            return player;
        }

        private static int HowManyPlayers()
        {
            Console.WriteLine("How many players are there: ");
            bool isValid = int.TryParse(Console.ReadLine(), out int result);
            while (isValid != true)
            {
                Console.WriteLine("Please enter a valid number.");
                isValid = int.TryParse(Console.ReadLine(), out result);
            }
            return result;
        }

        private static List<string> CreateGrid()
        {
            return new List<string> { "A1", "A2", "A3", "A4", "A5",
                                      "B1", "B2", "B3", "B4", "B5",
                                      "C1", "C2", "C3", "C4", "C5",
                                      "D1", "D2", "D3", "D4", "D5",
                                      "E1", "E2", "E3", "E4", "E5" };
        }

        private static PlayerModel GetShipPositions(PlayerModel player)
        {
            Console.Write("Please enter ship positions: ");
            do
            {
                string shipPosition = GetPosition(player);
                if (player.ShipLocation.Contains(shipPosition) && player.ShipLocation != null)
                {
                    Console.WriteLine("Position already taken. Please try again:");
                    continue;
                }
                player.ShipLocation.Add(shipPosition);

            }
            while (player.ShipLocation.Count < 5);
            return player;
        }
    }
}
