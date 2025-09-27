
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Dice_Game.Scripts
{
// this class creates a random number when the player rolls a dice
    internal class  Die
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
            Console.WriteLine($"{Name} rolled a {CurrentRoll} on a d{die.GetSides()}.");
        }
    }




    internal class GameManager
    {
        private Player player;
        private Player computer;
        private Random random = new Random();
        private bool playerGoseFirst;

        // this playgame function calls all the functions in everything so 
        public void playgame()
        {
            intro();
            SetupPlayer();
            DecideTurnOrder();
            PlayRound();
            ComparePlayeres();
            Outro();

        }
        /// intro to the game
        private void intro()
        {
            Console.WriteLine("=================================");
            Console.WriteLine(" Welcome to Dice Roller Game!");
            Console.WriteLine($" Today’s date: {DateTime.Now.ToShortDateString()}");
            Console.WriteLine("=================================\n");
        }
        /// asking for players name and getting them ready for the game
        private void SetupPlayer()
        {
            Console.Write(" Enter your name: ");
            string name = Console.ReadLine();
            player = new Player(name);
            computer = new Player("Baray");
            Console.WriteLine($"Welcome {player.Name}, you’ll be facing {computer.Name}!"); 
            

        }
        // this function flips a coin to tell who goes first
        private void DecideTurnOrder()
        {
            Console.WriteLine("Flipping a coin to decide who goes first.....");
            int coinflip = random.Next(2);

            if (coinflip == 0)
            {
                Console.WriteLine($"{player.Name} goes first!\n");
                playerGoseFirst = true;
            }
            else if (coinflip == 1)
            {
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
            Console.WriteLine($"{player.Name}, choose a die to roll (d4, d6, d8, d12, d20): ");
            // takes the users input as a string d4, d6, .....
            string input = Console.ReadLine().ToLower().Trim();// if they player inputs an upercase letter it would 
            // Remove 'd' and convert to int
            int sides;
            if (input.StartsWith("d") && int.TryParse(input.Substring(1), out sides))
            {
                Die playerDie = new Die(sides);
                if (input == "d4" || input == "d6" || input == "d8" || input == "d12" || input == "d20" )
                {
                    player.TakeTurn(playerDie);
                    player.ShowRoll(playerDie);
                }
                else
                {
                    Console.WriteLine("Invalid input ");
                    validation();
                }
               
            }
            else
            {
                Console.WriteLine("Invalid input ");
                validation();
            }
        }
        private void PlayerTurn()
        {
          
            validation();
            
        }

        private void ComputerTurn()
        {
            //computer turn
            int compuSides = 0;
            int choice = random.Next(1, 6); // So the computer choos a die randomly

            switch (choice)
            {
                case 1: compuSides = 4; break;
                case 2: compuSides = 6; break;
                case 3: compuSides = 8; break;
                case 4: compuSides = 12; break;
                case 5: compuSides = 20; break;

            }
            Die compDie = new Die(compuSides);
            computer.TakeTurn(compDie);
            computer.ShowRoll(compDie);

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

            if (player.CurrentRoll > computer.CurrentRoll )
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
            Console.WriteLine("\nThanks for playing Dice Roller!");
            Console.WriteLine("Game Over.");
        }

    }

}
 