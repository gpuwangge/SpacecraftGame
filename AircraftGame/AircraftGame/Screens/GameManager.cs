using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameSpace
{
    public class GameManager
    {
        public SpaceGame game;

        public Texture2D backgroundTex2d;
        public bool isPaused;
        public string commandLine;
        public bool showCommand = false;

        public GameManager(SpaceGame game)
        {
            this.game = game;
        }

        public virtual void Initialize() { }
        public virtual void LoadContent(ContentManager content) { }
        public virtual void ArrangeControl() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GraphicsDeviceManager graphics, GameTime gameTime) { }
        public virtual void DrawSprites(GameTime gametime, SpriteBatch spriteBatch) { }
        public virtual void DrawCommandSprites(GameTime gametime, SpriteBatch spriteBatch) { }
        
    }
}
