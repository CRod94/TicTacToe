using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class KIPlayer
    {
        public static int[] FindBestMove(MainGame game)
        {
            int[] bestMove = { -1, -1 };
            int bestValue = int.MinValue;

            for (int i = 0; i < 9; i++)
            {
                if (game.GetField()[i] == null)
                {
                    game.PlayerTurn(i);

                    int moveValue = MinimaxValue(game, 0, false);

                    game.GetField()[i] = null;

                    if (moveValue > bestValue)
                    {
                        bestMove[0] = i / 3;
                        bestMove[1] = i % 3;
                        bestValue = moveValue;
                    }
                }
            }
            return bestMove;
        }

        private static int MinimaxValue(MainGame game, int depth, bool isMaximizing)
        {
            MainGame.GameState gameState = game.CheckGame();

            if (gameState == MainGame.GameState.WinPlayer1)
            {
                return -1;
            }
            else if (gameState == MainGame.GameState.WinPlayer2)
            {
                return 1;
            }
            else if (gameState == MainGame.GameState.Tie)
            {
                return 0;
            }

            if (isMaximizing)
            {
                int bestValue = int.MinValue;

                for (int i = 0; i < 9; i++)
                {
                    if (game.GetField()[i] == null)
                    {
                        SimulateMove(game, i);
                        int moveValue = MinimaxValue(game, depth + 1, false);
                        UndoMove(game, i);

                        bestValue = Math.Max(bestValue, moveValue);
                    }
                }
                return bestValue;
            }
            else
            {
                int bestValue = int.MaxValue;

                for (int i = 0; i < 9; i++)
                {
                    if (game.GetField()[i] == null)
                    {
                        SimulateMove(game, i);
                        int moveValue = MinimaxValue(game, depth + 1, true);
                        UndoMove(game, i);

                        bestValue = Math.Min(bestValue, moveValue);
                    }
                }
                return bestValue;
            }
        }


        private static void SimulateMove(MainGame game, int index)
        {
            game.PlayerTurn(index);
        }

        private static void UndoMove(MainGame game, int index)
        {
            game.GetField()[index] = null;
        }

    }
}
