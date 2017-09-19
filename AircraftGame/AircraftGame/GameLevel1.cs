using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*Created by Xiaojun Wang*/

namespace GameSpace
{
    public class GameLevel1: GameManager
    {
        /*Neutral Objects*/
        public Asteroids asteroids;
        public Asteroids backgroundAsteroids;
        public Planets backgroundPlanets;
        public Bullets bullets;
        //public Aircrafts aircrafts;

        /*Players and AIs*/
        public Pilots pilots;
        public Player1 player1;
        public Player2 player2;

        public bool isInEquip = false;

        Level1Quests questStage;
        bool startQuest;
        float StartQuestTime;
        int destroyedAsteroids;
        int destroyedEnemyAircraft;

        public enum Level1Quests {
            START,
            WEAPON,
            MAXTHRUST, MINTHRUST,
            ZOOMIN, ZOOMOUT,
            ASTEROID,
            ENEMYFOCUS, ENEMY,
            ENEMYTEAMFOCUS, ENEMYTEAM,
            COMPLETE }

        public GameLevel1(SpaceGame game): base(game)
        {
            /*Neutral Objects*/
            asteroids = new Asteroids(game);
            backgroundAsteroids = new Asteroids(game);
            backgroundPlanets = new Planets(game);
            //aircrafts = new Aircrafts(game);
            bullets = new Bullets(game);

            /*Players*/
            player1 = new Player1(game);
            player2 = new Player2(game);
            pilots = new Pilots(game);

           // questStage = Level1Quests.START;
            questStage = Level1Quests.ENEMYFOCUS;
            startQuest = false;
            StartQuestTime = 0;

            destroyedAsteroids = 0;
            destroyedEnemyAircraft = 0;
        }

        public override void Initialize() {
            player1.Initialize(TeamRole.LEADER, new int[0] { });

            /*Player1*/
            player1.AddAircraft(new AircraftXwing(game),new Vector3(0,0,0), RelationEnum.PLAYER, pilots);

            /*Player2*/
            // player2.AddAircraft(new AircraftXwing(game), new Vector3(0, 0, 0), RelationEnum.PLAYER, pilots);
            //pilots.AddPilot(player2);

            commandLine = ">>";
            //commandLine = "ADD ENEMYXWINGTEAM 6 1000,0";
            //commandLine = "ADD ASTEROID 100 0,0 100,1000";
            game.IsMouseVisible = true;

            /*Add asteroids*/
            backgroundAsteroids.AddBackgroudAsteroids(300, 5000, 3000, 0, 0 );
            backgroundPlanets.AddBackgroudPlanets();
        }

        public override void LoadContent(ContentManager content)
        {
            //backgroundTex2d = content.Load<Texture2D>("combine3");
        }

        int updateCommand()
        {
            int rval = -1;

            /*Command*/
            if (showCommand == false)
            {
                if (commandLine != "")
                {
                    string[] commands = commandLine.Split(' ');
                    int CommandCount = commands.Count();
                    /*act tar num x,y innerRange,outerRange*/
                    if (CommandCount >= 2 && CommandCount <= 5)
                    {
                        string act = commands[0];
                        string tar = commands[1];

                        int num = 1;
                        int centerX = 0;
                        int centerY = 0;
                        int innerRange = 0;
                        int outerRange = 200;

                        if (CommandCount >= 3 && commands[2] != "")
                            num = Int32.Parse(commands[2]);
                        if (CommandCount >= 4 && commands[3] != "")
                        {
                            string[] center = commands[3].Split(',');
                            centerX = Int32.Parse(center[0]);
                            centerY = Int32.Parse(center[1]);
                        }
                        if (CommandCount >= 5 && commands[4] != "")
                        {
                            string[] range = commands[4].Split(',');
                            if (range.Count() == 1)
                                outerRange = Int32.Parse(commands[4]);
                            else
                            {
                                innerRange = Int32.Parse(range[0]);
                                outerRange = Int32.Parse(range[1]);
                            }
                        }

                        switch (act)
                        {
                            case "ADD":
                                if (tar == "ENEMYXWING")
                                    rval = AddBasicAI(num, innerRange, outerRange, centerX, centerY, RelationEnum.ENEMY1, TeamRole.LEADER, new int[0] { });
                                else if (tar == "ALLYXWING")
                                    AddBasicAI(num, innerRange, outerRange, centerX, centerY, RelationEnum.ALLY, TeamRole.LEADER, new int[0] { });
                                else if (tar == "ENEMYXWINGTEAM")
                                    rval = AddXwingTeam(num, innerRange, outerRange, centerX, centerY, RelationEnum.ENEMY1);
                                else if (tar == "ALLYXWINGTEAM")
                                    AddXwingTeam(num, innerRange, outerRange, centerX, centerY, RelationEnum.ALLY);
                                else if (tar == "ASTEROID")
                                    asteroids.AddAsteroids(num, innerRange, outerRange, centerX, centerY);
                                break;
                        }
                    }

                    commandLine = "";
                }
            }
            /*Command End*/
            return rval;
        }
        public override void Update(GameTime gameTime)
        {
            switch (questStage)
            {
                case Level1Quests.START:
                    if (!startQuest)
                    {
                        game.uICombat.controlInformation.AddText("Welcome to the 3D XNA demo!", Color.White);
                        startQuest = true;
                    }
                    if (startQuest && game.uICombat.controlInformation.totalTime > 3)
                    {
                        questStage = Level1Quests.WEAPON;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.WEAPON:
                    if (!startQuest)
                    {
                        game.uICombat.controlInformation.AddText("Laser Test: Use your left mouse to shoot a laser.", Color.White);
                        startQuest = true;
                        player1.mouseController.firedWeapon = false;
                    }
                    if (startQuest && player1.mouseController.firedWeapon)
                    {
                        game.uICombat.controlInformation.AddText("Good! Our weapon looks in good condition!", Color.White);
                        questStage = Level1Quests.MAXTHRUST;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.MAXTHRUST:
                    if (!startQuest)
                    {
                        game.uICombat.controlInformation.AddText("Engine Test: Press left ctrl multiple times to increase thrust to maximum.", Color.White);
                        startQuest = true;
                    }
                    if (startQuest && player1.aircraft.UserThrust == player1.aircraft.CurrentThrust)
                    {
                        game.uICombat.controlInformation.AddText("Exellent! Our ship is running in maximum thrust!", Color.White);
                        questStage = Level1Quests.MINTHRUST;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.MINTHRUST:
                    if (!startQuest)
                    {
                        game.uICombat.controlInformation.AddText("Engine Test: Use left alt to completely shutdown the engine.", Color.White);
                        startQuest = true;
                    }
                    if (startQuest && player1.aircraft.UserThrust == -player1.aircraft.CurrentThrust)
                    {
                        game.uICombat.controlInformation.AddText("Great! We successfully docked our ship.", Color.White);
                        questStage = Level1Quests.ZOOMIN;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.ZOOMIN:
                    if (!startQuest)
                    {
                        game.uICombat.controlInformation.AddText("Use the mouse scroll wheel to zoom in.", Color.White);
                        startQuest = true;
                    }
                    if (startQuest && game.uICombat.GlobalCamera.cameraLevel == game.uICombat.GlobalCamera.MinCameraLevel)
                    {
                        game.uICombat.controlInformation.AddText("Good!", Color.White);
                        questStage = Level1Quests.ZOOMOUT;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.ZOOMOUT:
                    if (!startQuest)
                    {
                        game.uICombat.controlInformation.AddText("Use the mouse scroll wheel to zoom out.", Color.White);
                        startQuest = true;
                    }
                    if (startQuest && game.uICombat.GlobalCamera.cameraLevel == game.uICombat.GlobalCamera.MaxCameraLevel)
                    {
                        game.uICombat.controlInformation.AddText("Great!", Color.White);
                        questStage = Level1Quests.ASTEROID;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.ASTEROID:
                    if (!startQuest)
                    {
                        startQuest = true;
                        game.uICombat.controlInformation.AddText("Commander, we have detected many asteroids around us. Destroy them so we can move to a safe zone.", Color.White);

                        int x = (int)player1.aircraft.Position.X;
                        int y = (int)player1.aircraft.Position.Y;
                        commandLine = "ADD ASTEROID 400 " + x.ToString() + "," + y.ToString() + " 100,5000";
                        updateCommand();
                    }
                    if (startQuest && destroyedAsteroids >= 10)
                    {
                        game.uICombat.controlInformation.AddText("We have destroyed enough asteroids so we can escape!", Color.White);
                        questStage = Level1Quests.ENEMYFOCUS;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.ENEMYFOCUS:
                    if (!startQuest)
                    {
                        StartQuestTime = game.uICombat.controlInformation.totalTime;
                        game.uICombat.controlInformation.AddText("Warning! We detected an enemy spacecraft coming for us. Prepare for battle!", Color.White);
                        startQuest = true;
                        int x = (int)player1.aircraft.Position.X - 2000;
                        int y = (int)player1.aircraft.Position.Y;
                        commandLine = "ADD ENEMYXWING 1 " + x.ToString() + "," + y.ToString();
                        int enemyPilotIndex = updateCommand();
                        game.uICombat.GlobalCamera.Focus = enemyPilotIndex;
                        game.uICombat.GlobalCamera.UpdateCameralLevel(3);
                    }
                    if (startQuest && game.uICombat.controlInformation.totalTime > StartQuestTime + 10)
                    {
                        questStage = Level1Quests.ENEMY;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.ENEMY:
                    if (!startQuest)
                    {
                        startQuest = true;
                        game.uICombat.GlobalCamera.Focus = 0;
                    }
                    if (startQuest && destroyedEnemyAircraft >= 1)
                    {
                        game.uICombat.controlInformation.AddText("We destroyed the enemy spacecraft!", Color.White);
                        questStage = Level1Quests.ENEMYTEAMFOCUS;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.ENEMYTEAMFOCUS:
                    if (!startQuest)
                    {
                        StartQuestTime = game.uICombat.controlInformation.totalTime;
                        game.uICombat.controlInformation.AddText("Warning! We detected an enemy squad nearby. We should evade them and wait for reinforcement.", Color.White);
                        startQuest = true;
                        int x = (int)player1.aircraft.Position.X - 2000;
                        int y = (int)player1.aircraft.Position.Y;
                        commandLine = "ADD ENEMYXWINGTEAM 10 " + x.ToString() + "," + y.ToString();
                        int enemyPilotIndex = updateCommand();
                        game.uICombat.GlobalCamera.Focus = enemyPilotIndex;
                        game.uICombat.GlobalCamera.UpdateCameralLevel(4);
                    }
                    if (startQuest && game.uICombat.controlInformation.totalTime > StartQuestTime + 60)
                    {
                        questStage = Level1Quests.ENEMYTEAM;
                        startQuest = false;
                    }
                    break;
                case Level1Quests.ENEMYTEAM:
                    if (!startQuest)
                    {
                        startQuest = true;
                        game.uICombat.GlobalCamera.Focus = 0;
                    }
                    break;
                case Level1Quests.COMPLETE:

                    break;
                default:
                    break;
            }

            if (!isPaused && !isInEquip)
            {
                asteroids.Update(gameTime);
                bullets.Update(gameTime);
            }
            pilots.Update(gameTime, isPaused, isInEquip);
            //if (!isPaused && !isInEquip)
            //{
            //    aircrafts.Update(gameTime);
            //}
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime) {

            Color color = new Color(80, 40, 40);
            graphics.GraphicsDevice.Clear(color);

            backgroundPlanets.Draw(graphics, gameTime);
            backgroundAsteroids.Draw(graphics, gameTime);

            asteroids.Draw(graphics, gameTime);
            bullets.Draw(graphics);

            //aircrafts.Draw(graphics, gameTime);
            pilots.Draw(graphics, gameTime);
        }

        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch) { 
            //Vector3 center = aircrafts.m_Aircrafts.ElementAt(player1.AircraftIndex).Position;
            //int centerX = -(int)center.X;
            //int centerY = -(int)center.Y;

            //int graphicWidth = (int)(game.GraphicsDevice.Viewport.Width );
            //int graphicHeight = (int)(game.GraphicsDevice.Viewport.Height );

            //int backWidth = backgroundTex2d.Width;
            //int backHeight = backgroundTex2d.Height;

            /*Area*/
            /*-graphicWidth,          -graphicHeight*/
            /*backWidth-graphicWidth, backHeight-graphicHeight*/

            //spriteBatch.Draw(backgroundTex2d,/*Texture file*/
            //new Rectangle(0, 0, graphicWidth, graphicHeight),/* where to draw, should be all window*/
            //new Rectangle(centerX, centerY, graphicWidth, graphicHeight),/* where to draw, should be all window*/
            //Color.White);

            //pilots.DrawSprites(gametime, spriteBatch);
        }

        public override void DrawCommandSprites(GameTime gametime, SpriteBatch spriteBatch){
            if (showCommand)
            {
                Color alphaTextColor = new Color(255, 255, 255, 255);
                SpriteFont font = game.utilities.ui_button_medium;
                float render_x = 20;
                float render_y = game.gameSetting.PreferredWindowHeight - 40;
                int Wid = 250;
                int Hit = 50;
                Rectangle RenderRect = new Rectangle((int)render_x, (int)render_y, Wid, Hit);
                spriteBatch.DrawString(font, ">>"+ commandLine + "_", new Vector2(RenderRect.X, RenderRect.Y),
                                alphaTextColor, 0, new Vector2(0, 0), 1.0f, 0, 0);
            }
        }

        /*Conditional*/
        public int AddBasicAI(int num, int innerRange, int outerRange, int centerX, int centerY, RelationEnum relation, TeamRole teamRole, int[] teamMember)
        {
            int rval = -1;//return the first pilot index

            Random random = new Random();
            for (int i = 0; i < num; i++)
            {
                Vector3 location = game.utilities.RandomRingArea(innerRange, outerRange) + new Vector3(centerX, centerY, 0);
                Pilot newAI = new AIBasic1(game);
                newAI.Initialize(teamRole, teamMember);
                newAI.AddAircraft(new AircraftXwing(game), location, relation, pilots);
                if (rval == -1) rval = newAI.PilotIndex;
            }

            //game.uICombat.controlInformation.AddText("Basic AI added!", Color.White);
            return rval;
        }

        public int AddXwingTeam(int num, int innerRange, int outerRange, int centerX, int centerY, RelationEnum relation)
        {
            int rval = -1;//return the leader pilot

            int leaderIndex = pilots.Count();
            int[] followersIndex = new int[num - 1];
            for (int i = 0; i < num - 1; i++) followersIndex[i] = leaderIndex + i + 1;
            rval = AddBasicAI(1, innerRange, outerRange, centerX, centerY, relation, TeamRole.LEADER, followersIndex);
            AddBasicAI(num - 1, innerRange, outerRange, centerX, centerY, relation, TeamRole.FOLLOWER, new int[1] { leaderIndex });

            //game.uICombat.controlInformation.AddText("Xwing Team added!", Color.White);
            return rval;
        }

        public void AsteroidDestroy()
        {
            destroyedAsteroids++;
            if (questStage == Level1Quests.ASTEROID)
               game.uICombat.controlInformation.AddText(destroyedAsteroids.ToString() + "/10 asteroids destroyed.", Color.White);
            
        }
        public void EnemyAircraftDestroy()
        {
            destroyedEnemyAircraft++;
        }
    }
}
