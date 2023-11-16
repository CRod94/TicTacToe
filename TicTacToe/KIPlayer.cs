using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TicTacToe
{
    internal class KIPlayer
    {
        private static Random rnd = new Random();
        private readonly MainGame game;
        private readonly bool ownPlayer;
        private readonly MainGame.GameState PlayerWaitState;

        public MainGame.GameState PlayerWinState { get; }

        private readonly System.Threading.Timer timer;

        public KIPlayer(MainGame game, bool ownPlayer)
        {
            this.game = game;
            this.ownPlayer = ownPlayer;
            PlayerWaitState = ownPlayer ? MainGame.GameState.WaitPlayer2 : MainGame.GameState.WaitPlayer1;
            PlayerWinState = ownPlayer ? MainGame.GameState.WinPlayer2 : MainGame.GameState.WinPlayer1;
            timer = new System.Threading.Timer(Timer_Tick, null, 1000, 1000);
        }

        bool lockTick = false;
        private void Timer_Tick(object state)
        {
            if (lockTick) return;
            try
            {
                lockTick = true;
                if (game.State != MainGame.GameState.WaitPlayer1 && game.State != MainGame.GameState.WaitPlayer2)
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                if (game.State != PlayerWaitState) return;

                var i = FindBestMove(game);
                game.PlayerTurn(i);
            }
            finally
            {
                lockTick = false;
            }
        }

        public int FindBestMove(MainGame game, int depth = 0)
        {
            if (depth > 9) return -1;

            int[] bestMove = { -1, -1 };
            int bestValue = int.MinValue;
            var field = game.GetField();


            foreach (var sp in new bool[] { ownPlayer, !ownPlayer })
            {
                if (field[4] == null && ((field[0] == sp || field[2] == sp || field[6] == sp || field[8] == sp))) return 4;


                if (field[2] == null && ((field[0] == sp && field[0] == field[1]) || (field[5] == sp && field[5] == field[8]) || (field[4] == sp && field[4] == field[6]))) return 2;
                if (field[1] == null && ((field[0] == sp && field[0] == field[2]) || (field[4] == sp && field[4] == field[7]))) return 1;
                if (field[0] == null && ((field[1] == sp && field[1] == field[2]) || (field[3] == sp && field[3] == field[6]) || (field[4] == sp && field[4] == field[8]))) return 0;
                if (field[3] == null && ((field[4] == sp && field[4] == field[5]) || (field[0] == sp && field[0] == field[6]))) return 3;
                if (field[4] == null && ((field[3] == sp && field[3] == field[5]) || (field[1] == sp && field[1] == field[7]) || (field[2] == sp && field[2] == field[6]) || (field[0] == sp && field[0] == field[8]))) return 4;
                if (field[5] == null && ((field[3] == sp && field[3] == field[4]) || (field[2] == sp && field[2] == field[8]))) return 5;
                if (field[6] == null && ((field[0] == sp && field[0] == field[3]) || (field[7] == sp && field[7] == field[8]) || (field[4] == sp && field[4] == field[2]))) return 6;
                if (field[7] == null && ((field[6] == sp && field[6] == field[8]) || (field[4] == sp && field[4] == field[1]))) return 7;
                if (field[8] == null && ((field[5] == sp && field[5] == field[2]) || (field[7] == sp && field[7] == field[6]) || (field[4] == sp && field[4] == field[0]))) return 8;
            }
/*
            var zuege = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (field[i] == null)
                {
                    var gameCopy = game.Clone();
                    gameCopy.PlayerTurn(i); //spielzug simulieren
                    if (gameCopy.State == PlayerWinState) return i;

                    if (!gameCopy.GameRunning) continue;

                    var gameOther = gameCopy.Clone(!(gameCopy.CurrentPlayer ?? false));
                    gameCopy.PlayerTurn(FindBestMove(gameOther, depth++));

                    if (!gameCopy.GameRunning) continue;

                    gameCopy.PlayerTurn(FindBestMove(gameCopy, depth++));

                    if (gameCopy.State == PlayerWaitState) zuege.Add(i);

                    if (!gameCopy.GameRunning) continue;
                }
            }
            
            int[] centerAndCorners = { 0, 2, 4, 6, 8 };
            var availableCenterAndCorners = centerAndCorners.Where(pos => field[pos] == null).ToList();

            if (availableCenterAndCorners.Any())
            {
                return availableCenterAndCorners[rnd.Next(availableCenterAndCorners.Count)];
            }*/
            //if (zuege.Any()) return zuege[rnd.Next(zuege.Count)];

            return rnd.Next(9);
        }

        /*
        private int MinimaxValue(MainGame game, int depth)
        {
            MainGame.GameState gameState = game.State;
            if (gameState == MainGame.GameState.Tie) return 0;
            if (gameState == PlayerWinState) return 1;
            if (gameState == MainGame.GameState.WinPlayer2 || (gameState == MainGame.GameState.WinPlayer1)) return 0;

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
        }*/
    }

}
