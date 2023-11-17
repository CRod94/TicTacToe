using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TicTacToe
{
    internal class KIPlayer
    {
        private static Random rnd = new Random();
        private readonly MainGame game;
        private readonly bool ownPlayer;
        private readonly MainGame.GameState PlayerWaitState;
        public int Mode { get; set; }

        public MainGame.GameState PlayerWinState { get; }

        private readonly System.Threading.Timer timer;

        public KIPlayer(MainGame game, bool ownPlayer, int mode)
        {
            this.game = game;
            this.ownPlayer = ownPlayer;
            this.Mode = mode;
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

            if (Mode > 0)
            {
                if (depth > 9) return -1;

                int[] bestMove = { -1, -1 };
                int bestValue = int.MinValue;
                var field = game.GetField();


                foreach (var sp in new bool[] { ownPlayer, !ownPlayer })
                {

                    if (field[2] == null && ((field[0] == sp && field[0] == field[1]) || (field[5] == sp && field[5] == field[8]) || (field[4] == sp && field[4] == field[6]))) return 2;
                    if (field[1] == null && ((field[0] == sp && field[0] == field[2]) || (field[4] == sp && field[4] == field[7]))) return 1;
                    if (field[0] == null && ((field[1] == sp && field[1] == field[2]) || (field[3] == sp && field[3] == field[6]) || (field[4] == sp && field[4] == field[8]))) return 0;
                    if (field[3] == null && ((field[4] == sp && field[4] == field[5]) || (field[0] == sp && field[0] == field[6]))) return 3;
                    if (field[4] == null && ((field[3] == sp && field[3] == field[5]) || (field[1] == sp && field[1] == field[7]) || (field[2] == sp && field[2] == field[6]) || (field[0] == sp && field[0] == field[8]))) return 4;
                    if (field[5] == null && ((field[3] == sp && field[3] == field[4]) || (field[2] == sp && field[2] == field[8]))) return 5;
                    if (field[6] == null && ((field[0] == sp && field[0] == field[3]) || (field[7] == sp && field[7] == field[8]) || (field[4] == sp && field[4] == field[2]))) return 6;
                    if (field[7] == null && ((field[6] == sp && field[6] == field[8]) || (field[4] == sp && field[4] == field[1]))) return 7;
                    if (field[8] == null && ((field[5] == sp && field[5] == field[2]) || (field[7] == sp && field[7] == field[6]) || (field[4] == sp && field[4] == field[0]))) return 8;

                    if (Mode > 1) if (field[4] == null && ((field[0] == sp || field[2] == sp || field[6] == sp || field[8] == sp))) return 4;
                }
            }

            return rnd.Next(9);
        }

    }
}
