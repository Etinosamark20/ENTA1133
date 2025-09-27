using Dice_Game.Scripts;

namespace Dice_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager game = new GameManager();
            game.playgame(); 
        }
    }
}
