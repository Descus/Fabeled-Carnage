using System;
using System.Security.Authentication.ExtendedProtection;

namespace Rewrite.Scoreboards
{
    [Serializable]
    public class GameSave
    {
        public string score = "0";
        public string name = "";

        public void SetName(string name)
        {
            this.name = name;
        }
        public void SetScore(string score)
        {
            this.score = score;
        }
    }
}