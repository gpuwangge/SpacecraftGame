using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameSpace
{
    public class OptionScreen : GameManager
    {
        ControlButton btnExitToMenu;
        ControlButton btnResolution;
        ControlButton btnSaveAndApply;
        ControlButton btnBackToGame;

        private MouseState mouseState;
        private KeyboardState keyboardState;
        private MouseState lastMousedState;
        private KeyboardState lastKeyboardState;

        public OptionScreen(SpaceGame game)
            : base(game)
        {
            btnExitToMenu = new ControlButton(game);
            btnResolution = new ControlButton(game);
            btnSaveAndApply = new ControlButton(game);
            btnBackToGame = new ControlButton(game);

            // gameSettings = new GameSettings(game);
            //gameSettings = SettingsManager.Read();
        }

        public override void Initialize()
        {
            game.IsMouseVisible = true;
            //game.Window.Title = "Option";
        }

        public override void LoadContent(ContentManager content)
        {
            btnExitToMenu.LoadContent(null, game.utilities.ui_button_large, "Exit to Menu",
                new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 170));
            btnExitToMenu.Initialize();

            btnResolution.LoadContent(null, game.utilities.ui_button_large, game.utilities.resolutionTypeName[(int)game.resolutionType],
                new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 170));
            btnResolution.Initialize();

            btnSaveAndApply.LoadContent(null, game.utilities.ui_button_large, "Save and Apply",
                new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 170));
            btnSaveAndApply.Initialize();

            btnBackToGame.LoadContent(null, game.utilities.ui_button_large, "Back to Game",
                new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 170));
            btnBackToGame.Initialize();

            ArrangeControl();
        }

        public override void Update(GameTime gameTime)
        {
            game.gameLevel1.isPaused = true;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            Point mouseLPos = new Point(mouseState.X, mouseState.Y);

            if (lastMousedState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (btnBackToGame.CheckButton(mouseLPos))
                {
                    game.SetGameManager(GameScreens.GAMELEVEL1);
                    game.gameLevel1.isPaused = false;
               
                    //game.gameLevel1.player1.mouseController.lastMouseState;
                }
                if (btnResolution.CheckButton(mouseLPos))
                {
                    game.resolutionType++;
                    if ((int)game.resolutionType > 3) game.resolutionType = ResolutionType.res800X600;
                    btnResolution.text = game.utilities.resolutionTypeName[(int)game.resolutionType];
                }
                if (btnSaveAndApply.CheckButton(mouseLPos))
                {
                    game.ApplyResolution();
                }
                if (btnExitToMenu.CheckButton(mouseLPos))
                {
                    game.SetGameManager(GameScreens.MENU);
                    game.SetUIManager(UIScreens.NONE);
                }
            }

            btnExitToMenu.Update(gameTime, mouseLPos);//check leave and enter sound and effect
            btnResolution.Update(gameTime, mouseLPos);
            btnSaveAndApply.Update(gameTime, mouseLPos);
            btnBackToGame.Update(gameTime, mouseLPos);

            lastMousedState = mouseState;
            lastKeyboardState = keyboardState;
        }

        public override void ArrangeControl()
        {
            int buttonWid = 300;
            int buttonHit = 50;

            float render_x = game.ScreenCenter.X - buttonWid / 2;
            float render_y = game.gameSetting.PreferredWindowHeight / 3 * 2 - 50;
            btnBackToGame.RenderRect = new Rectangle((int)render_x, (int)render_y, buttonWid, buttonHit);

            render_x = game.ScreenCenter.X - buttonWid / 2;
            render_y = game.gameSetting.PreferredWindowHeight / 3 * 2 + 0;
            btnResolution.RenderRect = new Rectangle((int)render_x, (int)render_y, buttonWid, buttonHit);

            render_x = game.ScreenCenter.X - buttonWid / 2;
            render_y = game.gameSetting.PreferredWindowHeight / 3 * 2 + 50;
            btnSaveAndApply.RenderRect = new Rectangle((int)render_x, (int)render_y, buttonWid, buttonHit);

            render_x = game.ScreenCenter.X - buttonWid / 2;
            render_y = game.gameSetting.PreferredWindowHeight / 3 * 2 + 100;
            btnExitToMenu.RenderRect = new Rectangle((int)render_x, (int)render_y, buttonWid, buttonHit);//Vector2.Zero means use the texture/fond size
        }

        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(backgroundTex2d,
            //    new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
            //    Color.White);

            game.GraphicsDevice.Clear(Color.Transparent);

            btnExitToMenu.DrawSprites(gameTime, spriteBatch);
            btnResolution.DrawSprites(gameTime, spriteBatch);
            btnSaveAndApply.DrawSprites(gameTime, spriteBatch);
            btnBackToGame.DrawSprites(gameTime, spriteBatch);

            base.DrawSprites(gameTime, spriteBatch);
        }    

        //public void HandleInput()
        //{
        //    ////也可以用键盘的UP或DOWN键选择菜单项
        //    //if (Input.KeyboardUpJustPressed)
        //    //    UiManager.ChangeFocus(-1);
        //    //if (Input.KeyboardDownJustPressed)
        //    //    UiManager.ChangeFocus(1);
        //}


        //点击“保存”按钮将设置保存到GameSettings.xml文件中
        //void btnSave_MouseClick(object sender, MouseEventArgs e)
        //{
        //    //if (e.Button == MouseButtons.Left)
        //    //{
        //    //    gameSettings.PreferredFullScreen = Convert.ToBoolean (isFullScreen[currentIsFullScreen]);
        //    //    gameSettings.PreferredWindowWidth = Convert .ToInt32 (resolutionX[currentResolution]);
        //    //    gameSettings.PreferredWindowHeight = Convert.ToInt32(resolutionY[currentResolution]);
        //    //    SettingsManager.Save(gameSettings);
        //    //    engine.applyDeviceChanges = true;
        //    //}
        //}

    }
}
