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
    public class UICombat : UIManager
    {
        public int player1HP;
        public int player1MaxHp;
        public Bar player1HPBar;

        public int player1Shield;
        public int player1MaxShield;
        public Bar player1ShieldBar;

        public int player1Energy;
        public int player1MaxEnergy;
        public Bar player1EnergyBar;

        public MyObject target;
        public Bar player1TargetHPBar;
        public Bar player1TargetShieldBar;
        public Bar player1TargetEnergyBar;

        public int player1AircraftIndex;

        static float elapsedTimeThisFrameInMs = 0.001f;
        static float totalTimeMs = 0;
        static float startTimeThisSecond = 0;
        static int
             frameCountThisSecond = 0,
             totalFrameCount = 0,
             fpsLastSecond = 60;
        static float fpsInterpolated = 100.0f;

        ControlRadar controlRadar;
        public ControlInformation controlInformation;

        string HelpString;
        Vector2 HelpStringPos;
        public bool HelpStringVisible;

        int BarMaxWidth = 400;
        int BarMaxHeight = 25;

        public UICombat(SpaceGame game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {

            player1HPBar = new Bar(this.game);
            player1ShieldBar = new Bar(this.game);
            player1EnergyBar = new Bar(this.game);

            player1TargetHPBar = new Bar(this.game);
            player1TargetShieldBar = new Bar(this.game);
            player1TargetEnergyBar = new Bar(this.game);

            controlRadar = new ControlRadar(game);
            controlInformation = new ControlInformation(game);
        }

        public override void Initialize()
        {
            player1HPBar.Initialize();
            player1HPBar.RenderRect = new Rectangle(10, 10, BarMaxWidth, BarMaxHeight);
            player1HPBar.MaxWidth = BarMaxWidth;
            player1ShieldBar.Initialize();
            player1ShieldBar.RenderRect = new Rectangle(10, 10 + BarMaxHeight+2, BarMaxWidth, BarMaxHeight);
            player1ShieldBar.MaxWidth = BarMaxWidth;
            player1EnergyBar.Initialize();
            player1EnergyBar.RenderRect = new Rectangle(10, 10 + BarMaxHeight*2+4, BarMaxWidth, BarMaxHeight);
            player1EnergyBar.MaxWidth = BarMaxWidth;

            player1TargetHPBar.Initialize();
            player1TargetShieldBar.Initialize();
            player1TargetEnergyBar.Initialize();
            player1TargetHPBar.IsVisible = false;
            player1TargetShieldBar.IsVisible = false;
            player1TargetEnergyBar.IsVisible = false;

            player1AircraftIndex = 0;// ((Player1)game.gameLevel1.pilots.m_Pilots.ElementAt(0)).AircraftIndex;

            controlRadar.Initialize();
            controlRadar.RenderRect = new Rectangle(10, 95, 300, 300);
            controlRadar.RadarCenterPos = new Vector3((int)(controlRadar.RenderRect.Width / 2) + 2, (int)controlRadar.RenderRect.Y - 8 + (int)(controlRadar.RenderRect.Height / 2), 0);

            HelpString = "TIPS:\n" +
                "Press 'Left Ctrl' to increase thrust;\n" +
                "Press 'Left Alt' to decrease thrust;\n" +
                "Press 'C' for charactor page;\n" +
                "Use mouse scroll wheel for zoom in/out;\n" +
                "Press 'ESC' for game option;\n" +
                "Press 'H' for hiding this information.";
            HelpStringVisible = true;

            GlobalCamera.cameraType = CameraType.GLOBAL;
            GlobalCamera.Initialize();

            ArrangeControl();
        }
        public override void LoadContent()
        {
            player1HPBar.LoadContent("ButtonGreen", game.utilities.ui_screen_medium, null, 
                game.utilities.BarTextColor, game.utilities.BarHighColor, game.utilities.BarImageColor);
            player1ShieldBar.LoadContent("ButtonBlue", game.utilities.ui_screen_medium, null,
                game.utilities.BarTextColor, game.utilities.BarHighColor, game.utilities.BarImageColor);
            player1EnergyBar.LoadContent("ButtonPurple2", game.utilities.ui_screen_medium, null,
                game.utilities.BarTextColor, game.utilities.BarHighColor, game.utilities.BarImageColor);
            player1TargetHPBar.LoadContent("ButtonGreen", game.utilities.ui_screen_medium, null,
                game.utilities.BarTextColor, game.utilities.BarHighColor, game.utilities.BarImageColor);
            player1TargetShieldBar.LoadContent("ButtonBlue", game.utilities.ui_screen_medium, null,
                game.utilities.BarTextColor, game.utilities.BarHighColor, game.utilities.BarImageColor);
            player1TargetEnergyBar.LoadContent("ButtonPurple2", game.utilities.ui_screen_medium, null,
                game.utilities.BarTextColor, game.utilities.BarHighColor, game.utilities.BarImageColor);
            controlRadar.LoadContent("Radar", game.utilities.ui_screen_medium, null,
                Color.Black, Color.Black, Color.White);
            controlInformation.LoadContent("white3", game.utilities.ui_screen_medium, null,
                Color.White, Color.Black, new Color(30, 30, 30, 0));

        }

        public void Player1HitIt(MyObject myObject)
        {
            target = myObject;
        }

        public override void ArrangeControl()
        {
            player1TargetHPBar.RenderRect = new Rectangle(game.gameSetting.PreferredWindowWidth - BarMaxWidth-10, 10, BarMaxWidth, BarMaxHeight);
            player1TargetHPBar.MaxWidth = BarMaxWidth;
            player1TargetShieldBar.RenderRect = new Rectangle(game.gameSetting.PreferredWindowWidth - BarMaxWidth-10, 10 + BarMaxHeight + 2, BarMaxWidth, BarMaxHeight);
            player1TargetShieldBar.MaxWidth = BarMaxWidth;
            player1TargetEnergyBar.RenderRect = new Rectangle(game.gameSetting.PreferredWindowWidth - BarMaxWidth-10, 10 + BarMaxHeight * 2 + 4, BarMaxWidth, BarMaxHeight);
            player1TargetEnergyBar.MaxWidth = BarMaxWidth;

            HelpStringPos = new Vector2(game.gameSetting.PreferredWindowWidth - BarMaxWidth - 10, game.gameSetting.PreferredWindowHeight - 200);

            controlInformation.RenderRect = new Rectangle(10, game.gameSetting.PreferredWindowHeight - 210, 400, 200);

            GlobalCamera.UpdateCameralLevel(2);
        }

        public override void Update(GameTime gameTime)
        {
            if (player1HPBar.value == game.utilities.minusInf)
            {
                player1HPBar.SetMaxValue(((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).MaxHitpoint);
                player1ShieldBar.SetMaxValue(((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).MaxShieldPoint);
                player1EnergyBar.SetMaxValue(((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).MaxEnergyPoint);
            }
            else
            {
                player1HPBar.UpdateValue(((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).CurrentHitpoint);
                player1ShieldBar.UpdateValue(((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).CurrentShieldPoint);
                player1EnergyBar.UpdateValue(((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).CurrentEnergyPoint);
            }
            

            /*Target*/
            if (target != null)
            {
                player1TargetHPBar.IsVisible = true;
                player1TargetHPBar.SetMaxValue(target.MaxHitpoint);
                player1TargetHPBar.UpdateValue(target.CurrentHitpoint);
                if (target.MaxShieldPoint > 0)
                {
                    player1TargetShieldBar.IsVisible = true;
                    player1TargetShieldBar.SetMaxValue(target.MaxShieldPoint);
                    player1TargetShieldBar.UpdateValue(target.CurrentShieldPoint);
                }
                else
                {
                    player1TargetShieldBar.IsVisible = false;
                }

                if (target.MaxEnergyPoint > 0)
                {
                    player1TargetEnergyBar.IsVisible = true;
                    player1TargetEnergyBar.SetMaxValue(target.MaxEnergyPoint);
                    player1TargetEnergyBar.UpdateValue(target.CurrentEnergyPoint);
                }
                else
                {
                    player1TargetEnergyBar.IsVisible = false;
                }
            }

            controlRadar.Update(gameTime, Point.Zero);
            controlInformation.Update(gameTime, Point.Zero);

            GlobalCamera.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

        }

        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {

            player1HPBar.DrawSprites(gameTime, spriteBatch);
            player1ShieldBar.DrawSprites(gameTime, spriteBatch);
            player1EnergyBar.DrawSprites(gameTime, spriteBatch);
            player1TargetHPBar.DrawSprites(gameTime, spriteBatch);
            player1TargetShieldBar.DrawSprites(gameTime, spriteBatch);
            player1TargetEnergyBar.DrawSprites(gameTime, spriteBatch);

            controlRadar.DrawSprites(gameTime, spriteBatch);
            controlInformation.DrawSprites(gameTime, spriteBatch);

            if (target != null)
            {
                string targetNameString = String.Format(
                     target.ObjectName);
                    //+ "\n"
                    //+ target.OperatorName);
                spriteBatch.DrawString(game.utilities.ui_screen_medium, targetNameString,
                    new Vector2(game.gameSetting.PreferredWindowWidth - BarMaxWidth - 20 - game.utilities.ui_screen_medium.MeasureString(targetNameString).X, 6), Color.White);
            }

            string player1NameString = String.Format(
              ((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).ObjectName
                + "\n"
                + ((Player1)game.gameLevel1.pilots.m_Pilots.ElementAt(0)).OperaterName);
            spriteBatch.DrawString(game.utilities.ui_screen_medium, player1NameString, new Vector2(BarMaxWidth + 20, 6), Color.White);

            /*Ship Info*/
            Vector2 DataPosition = new Vector2(20, controlRadar.RenderRect.Top + controlRadar.RenderRect.Height + 20);
            int displayCoff = 10;

            string thrustString = String.Format("Acceleration: {0:0} m/s^2", ((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).Acc * displayCoff);
            spriteBatch.DrawString(game.utilities.ui_screen_medium, thrustString, DataPosition + new Vector2(0, 0), Color.White);

            string dragString = String.Format("De-Acceleration: {0:0} m/s^2", ((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).Deacc * displayCoff);
            spriteBatch.DrawString(game.utilities.ui_screen_medium, dragString, DataPosition + new Vector2(0, 20), Color.White);

            string speedString = String.Format("Speed: {0:0} Knots", ((Aircraft)game.gameLevel1.pilots.GetAircraft(player1AircraftIndex)).Speed * 0.15f * displayCoff);// * 5 / 18/1.852
            spriteBatch.DrawString(game.utilities.ui_screen_medium, speedString, DataPosition + new Vector2(0, 40), Color.White);


            /*Position and FPS are correct but decide not to show*/
            //Vector3 aircraftPos = game.gameLevel1.pilots.GetAircraft(player1AircraftIndex).Position;
            //string curserPos = String.Format("Coordinate: {0:0},{1:0}", aircraftPos.X, aircraftPos.Y);
            //spriteBatch.DrawString(game.utilities.ui_screen_medium, curserPos, DataPosition + new Vector2(0, 60), Color.White);

            //ShowFPS(gameTime, spriteBatch, DataPosition + new Vector2(0, 80));

            /*Help Info*/
            if (HelpStringVisible)
                spriteBatch.DrawString(game.utilities.ui_screen_medium, HelpString,
                    HelpStringPos, Color.White);
        }

        private void ShowFPS(GameTime gameTime, SpriteBatch spriteBatch, Vector2 pos)
        {
            elapsedTimeThisFrameInMs = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            totalTimeMs += elapsedTimeThisFrameInMs;


            if (elapsedTimeThisFrameInMs <= 0)
                elapsedTimeThisFrameInMs = 0.001f;


            frameCountThisSecond++;
            totalFrameCount++;


            if (totalTimeMs - startTimeThisSecond > 1000.0f)
            {

                fpsLastSecond = (int)(frameCountThisSecond * 1000.0f /
                    (totalTimeMs - startTimeThisSecond));

                startTimeThisSecond = totalTimeMs;
                frameCountThisSecond = 0;

                fpsInterpolated = MathHelper.Lerp(fpsInterpolated, fpsLastSecond, 0.1f);
            }

            spriteBatch.DrawString(game.utilities.ui_screen_medium, "FPS: "+fpsLastSecond.ToString(), pos, Color.White);

        }
    }
}
