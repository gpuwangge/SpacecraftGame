using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public class LoadingScreen : GameManager
    {
        private double time;
        private float fadeTime;
        private float transparent;
        private Texture2D splash;

        float splashTime = 1.5f;

        string text;

        private bool beginGame = false;

        

        public LoadingScreen(SpaceGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {
           text = "Loading...";
           game.Window.Title = "Welcome";
           game.IsMouseVisible = false;
           time = 0;
           transparent = 0;
        }

        public override void LoadContent(ContentManager content)
        {
            splash = game.Content.Load<Texture2D>("Background15");
        }

        public override void Update(GameTime gameTime)
        {
            if (time > 0) /*To make sure loading is compeleted*/
            {
                /*Model Manager*/
                game.modelManager.LoadContent();

                beginGame = true;
            }

            if (beginGame)
            {
                text = "";
                fadeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                transparent = Math.Min(fadeTime / splashTime, 1);
                if (fadeTime > splashTime) game.SetGameManager(GameScreens.MENU);
                
            }

            time += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
            int wid = game.GraphicsDevice.Viewport.Width;
            int hit = game.GraphicsDevice.Viewport.Height;

            //Rectangle rect = new Rectangle(0, (int)(hit * 0.15), wid, (int)(hit * 0.7));
            Rectangle rect = new Rectangle(0, 0, wid, hit);
            spriteBatch.Draw(splash, rect, Color.Lerp(Color.White, Color.Black, transparent));

            
            byte alpha = 170;
            Color alphaTextColor = new Color(255, 255, 255, alpha);
            SpriteFont font = game.utilities.ui_button_medium;
            float render_x = game.ScreenCenter.X - 100;
            float render_y = game.gameSetting.PreferredWindowHeight / 3 * 2;
            int Wid = 250;
            int Hit = 50;
            Rectangle RenderRect = new Rectangle((int)render_x, (int)render_y, Wid, Hit);
            spriteBatch.DrawString(font, text, new Vector2(RenderRect.X + RenderRect.Width / 2 - font.MeasureString(text).X / 2, RenderRect.Y + RenderRect.Height / 2 - font.MeasureString(text).Y / 2),
                            alphaTextColor, 0, new Vector2(0, 0), 1.0f, 0, 0);

        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {

        }
    }
}
