using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public class MenuScreen : GameManager
    {
        public ControlButton btnSingleCampaign;
        public ControlButton btnOption;
        public ControlButton btnMultiplayer;
        public ControlButton btnExit;
       
        private MouseState mouseState;
        private KeyboardState keyboardState;
        private MouseState lastMousedState;
        private KeyboardState lastKeyboardState;

        public MenuScreen(SpaceGame game)
            : base(game)
        {
            btnSingleCampaign = new ControlButton(game);
            btnMultiplayer = new ControlButton(game);
            btnOption = new ControlButton(game);
            btnExit = new ControlButton(game);
        }

        public override void Initialize()
        {
            game.IsMouseVisible = true;
            game.Window.Title = "Menu";
        }

        public override void LoadContent(ContentManager content)
        {
            btnSingleCampaign.LoadContent(null, game.utilities.ui_button_large, "New Game",
                new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 170));//the first null means not use texture
            btnSingleCampaign.Initialize();//Vector2.Zero means use the texture/fond size

            btnMultiplayer.LoadContent(null, game.utilities.ui_button_large, "Load Game",
                new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 170));
            btnMultiplayer.Initialize();

            btnOption.LoadContent(null, game.utilities.ui_button_large, "Option",
            new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 170));
            btnOption.Initialize();

            btnExit.LoadContent(null, game.utilities.ui_button_large, "Exit",
                new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 170));
            btnExit.Initialize();

            backgroundTex2d = game.Content.Load<Texture2D>("Background01");

            ArrangeControl();
        }

        public override void ArrangeControl(){
            int buttonWid = 250;
            int buttonHit = 50;

            float render_x = game.ScreenCenter.X - buttonWid / 2;
            float render_y = game.gameSetting.PreferredWindowHeight / 3 * 2;
            btnSingleCampaign.RenderRect = new Rectangle((int)render_x, (int)render_y, buttonWid, buttonHit);//Vector2.Zero means use the texture/fond size

            render_x = game.ScreenCenter.X - buttonWid / 2;
            render_y = game.gameSetting.PreferredWindowHeight / 3 * 2 + 50;
            btnMultiplayer.RenderRect = new Rectangle((int)render_x, (int)render_y, buttonWid, buttonHit);

            render_x = game.ScreenCenter.X - buttonWid / 2;
            render_y = game.gameSetting.PreferredWindowHeight / 3 * 2 + 100;
            btnOption.RenderRect = new Rectangle((int)render_x, (int)render_y, buttonWid, buttonHit);

            render_x = game.ScreenCenter.X - buttonWid / 2;
            render_y = game.gameSetting.PreferredWindowHeight / 3 * 2 + 150;
            btnExit.RenderRect = new Rectangle((int)render_x, (int)render_y, buttonWid, buttonHit);
        }

        public void ApplyResolution()
        {
            ArrangeControl();
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            Point mouseLPos = new Point(mouseState.X, mouseState.Y);

            if (lastMousedState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed){
                game.soundBank.PlayCue("Click");
                if (btnSingleCampaign.CheckButton(mouseLPos))
                    game.SetGameManager(GameScreens.GAMELEVEL1);
                if (btnOption.CheckButton(mouseLPos))
                    game.SetGameManager(GameScreens.OPTION);
                if (btnExit.CheckButton(mouseLPos))
                    game.QuitGame();
            }


            btnSingleCampaign.Update(gameTime, mouseLPos);//check leave and enter sound and effect
            btnMultiplayer.Update(gameTime, mouseLPos);
            btnOption.Update(gameTime, mouseLPos);
            btnExit.Update(gameTime, mouseLPos);

            lastMousedState = mouseState;
            lastKeyboardState = keyboardState;

        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
        }
        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //int wid = game.GraphicsDevice.Viewport.Width;
            //int hit = game.GraphicsDevice.Viewport.Height;

            //spriteBatch.Draw(backgroundTex2d,
            //    new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
            //    Color.White);

            game.GraphicsDevice.Clear(Color.Black);

            btnSingleCampaign.DrawSprites(gameTime, spriteBatch);
            btnMultiplayer.DrawSprites(gameTime, spriteBatch);
            btnOption.DrawSprites(gameTime, spriteBatch);
            btnExit.DrawSprites(gameTime, spriteBatch);

            base.DrawSprites(gameTime, spriteBatch);
        }


    }
}
