using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameSpace
{
    public class UIManager
    {
        public SpaceGame game;
        public GraphicsDeviceManager graphics;

        public Camera GlobalCamera;

        public UIManager(SpaceGame game, GraphicsDeviceManager graphics)
        {
            this.game = game;
            this.graphics = graphics;

            GlobalCamera = new Camera(game, graphics);
        }

        public virtual void Initialize()
        {
            GlobalCamera.Initialize();

            ArrangeControl();
        }
        public virtual void LoadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void ArrangeControl()
        {
        }

    
        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {

        }
    }
}
