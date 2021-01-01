using System;

namespace con_rogue
{

    class Program
    {
        public static bool gameOn = true;
        static void Main(string[] args)
        {
            GameSession _gameSession = new GameSession();

            _gameSession.Update(GameSession.Input);
            while (gameOn)
            {
                // Temporary set cursor position fix
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                GameSession.Input = Console.ReadKey(true);
                _gameSession.Update(GameSession.Input);
            }

        }
    }
}