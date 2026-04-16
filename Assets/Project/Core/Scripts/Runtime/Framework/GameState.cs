using System;

namespace Project.Core.Runtime.Framework
{
    public enum GameState
    {
        None = 0,
        Init = 1,
        Title = 2,
        Exploration = 3,
        Inspection = 4,
        VisualNovel = 5,
        Pause = 6,
        GameOver = 7,
        Victory = 8
    }
}
