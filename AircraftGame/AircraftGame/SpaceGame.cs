using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


/*Created by Xiaojun Wang*/
namespace GameSpace
{
    
    public class SpaceGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        /*Common Object*/
        CollisionManager collisionManager;
        public Utilities utilities;
        public Vector2 ScreenCenter;
        public GameSetting gameSetting;

        /*Game Manager*/
        public GameScreens currentGameScreen;
        public GameManager gameManager;
        public LoadingScreen loadingScreen;
        public MenuScreen menuScreen;
        public GameLevel1 gameLevel1;
        public OptionScreen optionScreen;

        /*UI Manager*/
        public UIScreens currentUIScreen;
        public UIManager uIManager;
        public UICombat uICombat;
        public UINone uINone;
        public UIEquipment uIEquipment;

        /*Model Manager*/
        public ModelManager modelManager;

        private bool loaded = false;
        private int loadCount = 0;

        public ResolutionType resolutionType;

        AudioEngine audioEngine;
        WaveBank waveBank; //have to instanlize this to avoid error in audioEngine
        public SoundBank soundBank;

        public SpaceGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameSetting = new GameSetting(this);
            utilities = new Utilities();
            collisionManager = new CollisionManager(this);

            /*Game Manager*/
            gameManager = new GameManager(this);
            loadingScreen = new LoadingScreen(this);
            menuScreen = new MenuScreen(this);
            gameLevel1 = new GameLevel1(this);
            optionScreen = new OptionScreen(this);
            
            /*UI Manager*/
            uIManager = new UIManager(this, graphics);
            uICombat = new UICombat(this, graphics);
            uINone = new UINone(this, graphics);
            uIEquipment = new UIEquipment(this, graphics);

            /*Model Manager*/
            modelManager = new ModelManager(this);
        }

        protected override void Initialize()
        {
            resolutionType = ResolutionType.res1600X900;
            ApplyResolution();

            /*Game Manager*/
            gameManager.Initialize();
            loadingScreen.Initialize();
            menuScreen.Initialize();
            gameLevel1.Initialize();
            optionScreen.Initialize();

            /*UI Manager*/
            uIManager.Initialize();
            uICombat.Initialize();
            uIEquipment.Initialize();

            /*Model Manager*/
            modelManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            /*Load content here only run once*/
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loadingScreen.LoadContent(Content);
            utilities.LoadContent(Content);

            /*Initial Game Manager*/
            //gameManager = startScreen; //jump start screen for now
            //currentGameScreen = GameScreens.START;
            // gameManager = menuScreen;
            //currentGameScreen = GameScreens.MENU;
            SetGameManager(GameScreens.LOADING);
            //modelManager.LoadContent();
           // SetGameManager(GameScreens.MENU);

            uIManager = uINone;
            currentUIScreen = UIScreens.NONE;
            
            /*Audio*/
            audioEngine = new AudioEngine("Content\\SpaceGameAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Sound Bank.xsb");
        }

        protected override void UnloadContent() { }

        public void SetGameManager(GameScreens newScreen)
        {
            currentGameScreen = newScreen;

            switch (newScreen)
            {
                case GameScreens.LOADING:
                    gameManager = loadingScreen;
                    break;
                case GameScreens.GAMELEVEL1:
                    SetUIManager(UIScreens.COMBAT);
                    gameManager = gameLevel1;
                    gameManager.isPaused = false;
                    break;
                case GameScreens.MENU:
                    gameManager = menuScreen;
                    break;
                case GameScreens.OPTION:
                    gameManager = optionScreen;
                    break;
                case GameScreens.NONE:
                    break;
            }

            gameManager.ArrangeControl();
        }

        public void SetUIManager(UIScreens newUI)
        {
            currentUIScreen = newUI;

            switch (newUI)
            {
                case UIScreens.COMBAT:
                    uIManager = uICombat;
                    break;
                case UIScreens.EQIPMENT:
                    uIManager = uIEquipment;
                    break;
                case UIScreens.NONE:
                    uIManager = uINone;
                    break;
            }

            uIManager.ArrangeControl();
        }

        protected override void Update(GameTime gameTime)
        {
            gameManager.Update(gameTime);
            uIManager.Update(gameTime);
            collisionManager.Update();

            audioEngine.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            gameManager.Draw(graphics, gameTime);
            uIManager.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            uIManager.DrawSprites(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            gameManager.DrawSprites(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            gameManager.DrawCommandSprites(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);

            if (loaded == false)
            {
                loadCount++;
                if (loadCount == 2)
                {
                    loaded = true;
                    
                    menuScreen.LoadContent(Content);   
                    gameLevel1.LoadContent(Content);/*Cost time here*/
                    optionScreen.LoadContent(Content);

                    uICombat.LoadContent();
                    uIEquipment.LoadContent();
                }
            }

        }

        public void ApplyResolution()
        {
            switch (resolutionType)
            {
                case ResolutionType.res800X600:
                    graphics.PreferredBackBufferWidth = 800;
                    graphics.PreferredBackBufferHeight = 600;
                    if (graphics.IsFullScreen)
                        graphics.ToggleFullScreen();
                    break;
                case ResolutionType.res1024X768:
                    graphics.PreferredBackBufferWidth = 1024;
                    graphics.PreferredBackBufferHeight = 768;
                    if (graphics.IsFullScreen)
                        graphics.ToggleFullScreen();
                    break;
                case ResolutionType.res1600X900:
                    graphics.PreferredBackBufferWidth = 1600;
                    graphics.PreferredBackBufferHeight = 900;
                    if (graphics.IsFullScreen)
                        graphics.ToggleFullScreen();
                    break;
                case ResolutionType.resFullScreen:
                    Vector2 MaxRes = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                    graphics.PreferredBackBufferWidth = (int)MaxRes.X;
                    graphics.PreferredBackBufferHeight = (int)MaxRes.Y;
                    if (!graphics.IsFullScreen)
                        graphics.ToggleFullScreen();
                    break;
            }

            gameSetting.PreferredWindowHeight = graphics.PreferredBackBufferHeight;
            gameSetting.PreferredWindowWidth = graphics.PreferredBackBufferWidth;

            graphics.ApplyChanges();

            ScreenCenter = new Vector2(gameSetting.PreferredWindowWidth / 2, gameSetting.PreferredWindowHeight / 2);

            gameManager.ArrangeControl();
            uIManager.ArrangeControl();
        }

        public void QuitGame()
        {
            this.Exit();
        }

       

    }
}
