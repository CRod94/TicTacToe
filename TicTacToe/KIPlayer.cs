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
        private readonly MainGame game; //speicher instanz des Maingames, repräsentiert das aktuelle spiel
        private readonly bool ownPlayer; // speichert den aktuellen spieler, bool gibt an welcher Spieler an der reihe ist
        private readonly MainGame.GameState PlayerWaitState; //Speichert zustand GameState ab, welcher ausdrückt wann die KI ziehen muss
        public KiMode Mode { get; set; } //speichert und ruft den schwierigkeitsgrad des Spiels ab

        public MainGame.GameState PlayerWinState { get; } //speichert den zustand ab in dem die KI gewinnt

        private readonly System.Threading.Timer timer; //initalisierung des Timers, wird benötigt für ereignis timer_tick

        public KIPlayer(MainGame game, bool ownPlayer, KiMode mode) //konstruktor der KIPlayer klasse, übernimmt eine instanz des spiels, eine angabe welcher spieler am zug ist und eine angabe welcher Schwiergkeitsgrad aktulle ausgewählt wurde
        {
            this.game = game; //speicher den übergebenen zustand in die instanzveriable
            this.ownPlayer = ownPlayer; //weißt der instanzveriable ownplayer den übergebenen wert zu
            this.Mode = mode; //weißt mode die übergebene eigenschaft zu
            PlayerWaitState = ownPlayer ? MainGame.GameState.WaitPlayer2 : MainGame.GameState.WaitPlayer1; //weißt der instanzvariable den zustand zu indem die KI auf den zug des spielers waret
            PlayerWinState = ownPlayer ? MainGame.GameState.WinPlayer2 : MainGame.GameState.WinPlayer1; //weißt der instanzveriable den zustand zu indem der spieler auf den zug der KI wareten
            timer = new System.Threading.Timer(Timer_Tick, null, 1000, 1000); //erstellt eine neues instanz des Timers, mit der methode Timer_Tick und einem intervall von 1000ms
        }

        bool lockTick = false; //sicherheitsmechanismus um zu verhindern dass mehrere threads auf einmal aufgerufen werden ---> verhindert den loop
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

            if (Mode > KiMode.Random)
            {

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

                    if (Mode > KiMode.Normal) if (field[4] == null && ((field[0] == sp || field[2] == sp || field[6] == sp || field[8] == sp))) return 4;
                }
            }

            return game.FreeFields.OrderBy(o => Guid.NewGuid()).FirstOrDefault();
        }

    }

    public enum KiMode
    {
        Random,
        Normal,
        Hardcore
    }
}
