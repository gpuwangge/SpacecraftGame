using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameSpace
{
    public class UINone: UIManager
    {
        public UINone(SpaceGame game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
        }
    }
}
