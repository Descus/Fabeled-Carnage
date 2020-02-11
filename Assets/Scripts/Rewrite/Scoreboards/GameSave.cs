using System;
using System.Security.Authentication.ExtendedProtection;

namespace Rewrite.Scoreboards
{
    [Serializable]
    public struct GameSave
    {
        public string score;
        public string name;

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