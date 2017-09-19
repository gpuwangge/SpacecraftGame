using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSpace
{
    public class GameSetting
    {
        public bool PreferredFullScreen;
        public int PreferredWindowWidth;
        public int PreferredWindowHeight;
        public bool EnableVsync;

        private SpaceGame game;

        public GameSetting(SpaceGame game)
        {
            this.game = game;
        }

    }
}
