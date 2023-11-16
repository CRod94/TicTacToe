using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using static TicTacToe.Form1;

namespace TicTacToe
{

    internal class MainGame : INotifyPropertyChanged  //refreshed aktuells game sobald event ausgelöst wird
    {
        public bool? CurrentPlayer { get; private set; } = false;  //anzeige des Aktuelles spielers, ist false by default -> player1 = false, player2 = true.
        private bool?[] Field = new bool?[9];  //Die verschiedenen Felder des TicTacToe bretts als bool dargestellt.
        private GameState state; //Wieso grün?

        public event PropertyChangedEventHandler PropertyChanged; //deklaration des PropertyChangedEventHandlers names PropertyChanged
        public bool GameRunning => state == GameState.WaitPlayer1 || state == GameState.WaitPlayer2;
        public bool?[] GetField() => Field.ToList().ToArray();  //gibt array von nullable bools zurück source: daniel

        public IEnumerable<int> FreeFields => Enumerable.Range(0, 9).Where(O => Field[0] == null); 
        public GameState State { get => state; private set { state = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State))); } }
        //name of gibt exakten namen zurück. Frägt und aktuallisiert den status des spiels jedes mal wenn PropertyChanged ausgelöst oder der status verändert wurde.
        public void PlayerTurn(int fieldNr) //öffentliche methode um zu bestimmen welcher spieler an der reihe ist - öffentlich damit man von Form1 aus drauf zugreifen kann
        {
            if (State != GameState.WaitPlayer1 && State != GameState.WaitPlayer2)
            { CurrentPlayer = null; return; }//prüft ob überhaupt ein spieler an der reihe ist
            if (fieldNr < 0 || fieldNr > Field.Length) return; //prüft ob überhaupt noch felder vorhanden sind || prüft ob das gesuchte feld überhaupt teil des arreys ist
            if (Field[fieldNr] != null) return;  //prüft ob das gewählte feld noch frei ist also null, falls dies nicht der falls ist -> return
            Field[fieldNr] = CurrentPlayer; //setzt das im Arrey Field durch den input "fieldNR" ausgewählten index auf den zustand/namen des aktuellen spieler (true, false)
            CurrentPlayer = !CurrentPlayer; //Wechselt die spieler indem das bool druch den operator "!" auf falsch bzw true gesetzt wird (umgedreht)
            State = CheckGame(); //initiiert die nächste "Phase" des spiels in der der spielstand überprüft wird
            if (State != GameState.WaitPlayer1 && State != GameState.WaitPlayer2) CurrentPlayer = null; //überprüft ob nach CheckGame wieder ein spieler am zug ist bzw ob ein aktueller "Waitplayer" status aktiv ist
        }

        public GameState CheckGame() //funktion names "CheckGame" die einen "GameState" zurück gibt 
        {
            if ((Field[0] != null || Field[4] != null || Field[8] != null) && Field.Count(a => a != null) >= 5) // Prüfent ob überhaupt ein Gewinner möglich ist, diese Felder müssen belegt sein damit überhaupt jemand gewinnen kann
            {

                bool? winner = null; //Deklaration eines Nullable bools names winner und dessen status wurde auf Null gesetzt
                if (Field[4] != null) //prüft ob überhaupt ein gewinner durch die Kreuzstruktur möglich ist, die wird geprüft bevor die einzelnen reihen geprüft werden
                {
                    if (Field[4] == Field[1] && Field[4] == Field[7]) winner = Field[4];//prüft ob die die felder der mittleren vertikalen reihe aus 1,4,7 alle dem gleichen spieler gehören
                    else if (Field[4] == Field[0] && Field[4] == Field[8]) winner = Field[4]; // prüft ob die diagonalen felder aus 0,4,8 alle dem gleichen spieler gehören
                    else if (Field[4] == Field[2] && Field[4] == Field[6]) winner = Field[4]; // prüft ob die anderen diagonalen felder aus 2,4,6, dem gleichen spieler gehören
                    else if (Field[4] == Field[3] && Field[4] == Field[5]) winner = Field[4];  //prüft ob die felder der mittleren horizontalen Felder 3,4,5 alle dem gleichen spieler gehören   
                } //Falls eine dieser Bedingungen zutrifft wird der bool Winner auf den selben status wie die gewinner gesetzt, also auf den namen des Gewinners
                if (winner == null && Field[0] != null) //prüft ob es bereits einen Gewinner gibt und ob das feld 0 einem spieler gehört
                {
                    if (Field[0] == Field[1] && Field[0] == Field[2]) winner = Field[0]; //prüft ob die horizontalen felder 0,1,2 der ersten reihe alle dem gleichen spieler gehören
                    else if (Field[0] == Field[3] && Field[0] == Field[6]) winner = Field[0];// prüft ob die vertikalen felder 0,3,6 der linken spalte alle dem gleichen spieler gehören
                } //falls eine Bedingung zutrifft wird der bool Winner auf den Status der 3 gleichen felder gesetzt (true, false -> steht für player1, player2)
                if (winner == null && Field[8] != null) //prüft ob bereits ein Winner vorhanden ist und ob das feld mit dem index 8 einen wert/status besitzt bzw einem spieler gehört
                {
                    if (Field[8] == Field[7] && Field[8] == Field[6]) winner = Field[8]; // prüft ob die horizontalen felder 6,7,8 der untersten reihe alle dem gleichen spieler gehören
                    else if (Field[8] == Field[2] && Field[8] == Field[5]) winner = Field[8];// prüft ob die vertikalen felder 2,5,8 der rechten spalte alle dem gleichen spieler gehören
                } //falls eine Bedingung zutrifft wird der bool Winner auf den Status der 3 gleichen felder gesetzt (true, false -> steht für player1, player2)
                if (winner != null) //überprüft ob es einen Gewinner gibt, falls es einen gewinner gibt wird der code in den geschweiften klammern ausgeführt
                {
                    return winner.Value ? GameState.WinPlayer2 : GameState.WinPlayer1; //gibt einen Wert zurück basierend auf dem Value von winner -> ist Winner true wird WinPlayer2 zurück gegeben ist Winner false wird Winplayer1 zurückgegeben
                }                                                                       //dies macht man da Spieler 1 by default false ist und Spieler 2 by default true
            }
            if ((State == GameState.WaitPlayer1 || State == GameState.WaitPlayer2) && !Field.Any(a => a == null)) return GameState.Tie; //Prüft ob das spiel weiter geht und einer der beiden "WarteStates" aktiv ist && prüft jedes element im arrey Field ob eines davon (any) noch frei(null) ist. any = linq methode (a => a == null) ist eine Lambda-ausrducksfunktion. Falls beides zutrifft hat niemand gewonnen und es sind keine Felder übrig, somit ein Tie
            return CurrentPlayer.HasValue ? (CurrentPlayer.Value ? GameState.WaitPlayer2 : GameState.WaitPlayer1) : State;
        } //prüft ob CurrentPlayer einen wert besitzt, falls es einen wert besitzt wird der Gamestate bei true auf WaitPlayer2 gesetzt und bei false auf WaitPlayer1, falls Currentplayer keinen wert besitzt wird der aktuelle State des spiels zurück gegeben
          //Wechsel des spielers



        public enum GameState //deklariert einen satz fester konstanten die unter dem Namen Gamestate.Object aufgerufen werden können
        {
            WaitPlayer1,
            WaitPlayer2,
            WinPlayer1,
            WinPlayer2,
            Tie
        }

        internal MainGame Clone()
        {
            return Clone(CurrentPlayer.Value);
        }
        internal MainGame Clone(bool player)
        {
            return new MainGame() { State = State, CurrentPlayer = player, Field = GetField() };
        }
    }
}
