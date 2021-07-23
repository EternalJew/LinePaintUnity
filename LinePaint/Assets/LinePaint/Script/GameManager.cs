using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    public static class GameManager
    {
        public static GameStatus gameStatus = GameStatus.Playing;
        public static int currentLevel = 0;
        public static int totalDiamonds;
    }
    public enum GameStatus
    {
        Playing,
        Complete
    }
}
