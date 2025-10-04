
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Dice_Game.Scripts
{
    // this class creates a random number when the player rolls a dice
    internal class Die
    {
        private int sides;
        private Random random;


        public Die(int sides)
        {
            this.sides = sides;
            random = new Random();
        }

        public int Roll()
        {
            /// this returns the a random number to the caller
            return random.Next(1, sides + 1);
        }
        // Return the number of sides if asked
        public int GetSides()
        {
            return sides;
        }


    }








    internal class Player
    {
        // this gets the name of the player and assigns their most current rolll and points
        public string Name { get; set; }
        public int CurrentRoll { get; set; }
        public int Points { get; set; }
        public Player(string name)
        {
            Name = name;
            CurrentRoll = 0;
            Points = 0;
        }
        // Roll a die and stores the result
        public void TakeTurn(Die die)
        {
            CurrentRoll = die.Roll();
        }

        // Increase points when the player wins
        public void AddPoint()
        {
            Points++;
        }

        // Show current score
        public string ShowScore()
        {
            return $"{Name}: {Points} points";
        }
        // Show roll result
        public void ShowRoll(Die die)
        {
            Console.WriteLine($"\n{Name} rolled a {CurrentRoll} on a d{die.GetSides()}.");
        }
    }




    internal class GameManager
    {
        private Player player;
        private Player computer;
        private Random random = new Random();
        private bool playerGoseFirst;
        private List<int> playerBag;
        private List<int> cpuBag;
        private List<int> newbag;
        // this playgame function calls all the functions in everything so 
        public void playgame()
        {

            intro();
            SetupPlayer();

            playerBag = new List<int> { 4, 6, 8, 12, 20 };
            cpuBag = new List<int> { 4, 6, 8, 12, 20 };
            newbag = new List<int> { 20 };

            // this loop runs the game untill one of the players gets 5 points
            Boolean keeplaying = true;
            while (keeplaying)
            {
                DecideTurnOrder();
                PlayRound();
                ComparePlayeres();

                // Check win condition first
                if (playerBag.Count <= 0)
                {
                    Console.WriteLine($"\n{player.Name} you ran out of dice ");
                    keeplaying = false;
                    continue; // skip asking for another round
                }
                else if (cpuBag.Count <= 0)
                {
                    Console.WriteLine($"\n{computer.Name} has no dice left ");
                    keeplaying = false;
                    continue; // skip asking for another round
                }


                // if there is no die in the bag the game is over

                // Ask if player wants another round (validated input)
                string response = "";
                while (response != "y" && response != "yes" && response != "n" && response != "no")
                {
                    Console.Write("\nDo you want to play another round? (y/n): ");
                    response = Console.ReadLine().Trim().ToLower();

                    if (response != "y" && response != "yes" && response != "n" && response != "no")
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                    }
                }

                // Exit if they said no
                if (response == "n" || response == "no")
                {
                    keeplaying = false;
                }
            }
            Outro();
        }







        /// intro to the game and states the rules
        private void intro()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("\t Welcome to Dice Roller Game!");
            Console.WriteLine($"\t Today’s date: {DateTime.Now.ToShortDateString()}");
            Console.WriteLine("==================================================\n");
            Console.WriteLine("--The Rules--:\nYou would go against Baray for each round,");
            Console.WriteLine("the player with the higest roll gets a point.");
            Console.WriteLine("The game goes on untill both players do not have any die to roll");
            Console.WriteLine("At the end the player with the most point Wins..");
        }
        /// asking for players name and getting them ready for the game
        private void SetupPlayer()
        {
            Console.Write("\nEnter your name: ");
            string name = Console.ReadLine();
            player = new Player(name);
            computer = new Player("Baray");
            Console.WriteLine($"\nWelcome {player.Name}, you’ll be facing {computer.Name}!");


        }
        // this function flips a coin to tell who goes first
        private void DecideTurnOrder()
        {
            Console.WriteLine("\nFlipping a coin to decide who goes first.....");
            int coinflip = random.Next(2);

            if (coinflip == 0)
            {
                Console.WriteLine("\nHeads");
                Console.WriteLine($"{player.Name} goes first!\n");
                playerGoseFirst = true;
            }
            else if (coinflip == 1)
            {
                Console.WriteLine("\nTails");
                Console.WriteLine($"{computer.Name} goes first!\n");
                playerGoseFirst = false;
            }

        }


        // this choses who goes first and calls the playturn to roll the dice
        private void PlayRound()
        {
            if (playerGoseFirst)
            {
                PlayerTurn();
                ComputerTurn();
            }
            else
            {
                ComputerTurn();
                PlayerTurn();

            }
        }


        private void validation()
        {
            // Players turn
            Console.WriteLine($"\n{player.Name}, choose a die to roll from your Bag:");
            Console.WriteLine($"{player.Name}'s Bag: " + string.Join(", ", playerBag.Select(d => $"d{d}"))); // displays the dice the bag and converts the int to a string and adds a d infront
            // takes the users input as a string d4, d6, .....
            string input = Console.ReadLine().ToLower().Trim();// if they player inputs an upercase letter it would 
            // Remove 'd' and convert to int
            int sides;
            if (input.StartsWith("d") && int.TryParse(input.Substring(1), out sides))
            {

                if (playerBag.Contains(sides))
                {
                    Die playerDie = new Die(sides);
                    player.TakeTurn(playerDie);
                    player.ShowRoll(playerDie);

                    // removes die from the the bag after being used 
                    playerBag.Remove(sides);
                }
                else
                {
                    Console.WriteLine("That die is not availlable. Choose another die.  ");
                    validation();
                }

            }
            else
            {
                Console.WriteLine("Invalid input. Try again. ");
                validation();
            }
        }
        private void PlayerTurn()
        {

            validation();

        }

        private void ComputerTurn()
        {
            Console.WriteLine($"{computer.Name}'s Bag: " + string.Join(", ", cpuBag.Select(d => $"d{d}")));
            //computer turn
            if (cpuBag.Count == 0)
            {
                Console.WriteLine($"{computer.Name} has no dice left to roll.");
                return;

            }

            int index = random.Next(cpuBag.Count); // picks a random index
            int compuSides = cpuBag[index];

            Die compDie = new Die(compuSides);
            computer.TakeTurn(compDie);
            computer.ShowRoll(compDie);

            cpuBag.Remove(compuSides);

        }

        private void ShowRoundResults()
        {
            Console.WriteLine($"\n--- Round Results ---");
            Console.WriteLine($"{player.Name} rolled: {player.CurrentRoll}");
            Console.WriteLine($"{computer.Name} rolled: {computer.CurrentRoll}");
        }

        private void ComparePlayeres()
        {
            // shows the what each player has rolled before comparing to check who is the winner 

            ShowRoundResults();

            if (player.CurrentRoll > computer.CurrentRoll)
            {
                Console.WriteLine($"\n{player.Name} wins this round!");
                player.AddPoint();

            }
            else if (computer.CurrentRoll > player.CurrentRoll)
            {
                Console.WriteLine($"\n{computer.Name} wins this round!");
                computer.AddPoint();
            }
            else
            {
                Console.WriteLine("\nIt’s a tie! No points awarded.");
            }

            //Shows the scoreboard
            Console.WriteLine("\n--- Scoreboard ---");
            Console.WriteLine(player.ShowScore());
            Console.WriteLine(computer.ShowScore());
        }

        private void Outro()
        {
            if (player.Points > computer.Points)
            {
                Console.WriteLine($"\n{player.Name} Won the game.");
            }
            else if (computer.Points > player.Points)
            {
                Console.WriteLine($"\n{computer.Name} Won the game.");
            }

            else if (player.Points == computer.Points || computer.Points == player.Points)
            {
                Console.WriteLine("\nTie Game");
                Console.WriteLine("One more round, in this round ");
                Overtime();
            }
            Console.WriteLine("\nThanks for playing Dice Roller!");
            Console.WriteLine("\nGame Over.");
        }

        private void Overtime()
        {
            // this function determins the winner if there is a tie after comparing the total points of both players
            Console.WriteLine("This is overtime, you and the Computer would roll a D20 to determind the Winner ");
            Console.WriteLine("\nAvailable Die: " + string.Join(", ", newbag.Select(d => $"d{d}")));
            Console.WriteLine($"\n{player.Name}, Press 'r' to roll the die :");

            string lastroll = Console.ReadLine().ToLower().Trim();
            int d20;

            if (lastroll == "r" && int.TryParse(lastroll.Substring(1), out d20))
            {
                Die playerDie = new Die(d20);
                player.TakeTurn(playerDie);
                player.ShowRoll(playerDie);

            }
            else
            {
                Console.WriteLine("Invalid Input ");
                Overtime();

            }

            Console.WriteLine($"{computer.Name}'s Turn ");


            int index = random.Next(newbag.Count); // picks a random index
            int cpulastroll = cpuBag[index];

            Die compDie = new Die(cpulastroll);
            computer.TakeTurn(compDie);
            computer.ShowRoll(compDie);

            ComparePlayeres();
            Outro();


        }
    }

}
