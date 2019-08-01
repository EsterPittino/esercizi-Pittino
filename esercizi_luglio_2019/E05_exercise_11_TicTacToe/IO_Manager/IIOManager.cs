namespace E05_exercise_11_TicTacToe.IO_Manager
{
    interface IIOManager
    {
        #region Output
        void Refresh();

        void Notify(string message);
        void HighNotify(string message);
        void LowNotify(string message);

        void LittleBreak();
        void MediumBreak();
        void LargeBreak();
        #endregion

        #region Attesa
        void Wait();
        void Wait(string message);
        #endregion

        #region Input
        string AcceptString(string message, string defaultValue);
        bool AcceptBool(string message, bool defaultValue);
        int AcceptInt(string message, int defaultValue);
        #endregion
    }
}
