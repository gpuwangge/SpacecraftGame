using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameSpace
{
    public class KeyboardController
    {
        SpaceGame game;

        private KeyboardState lastKeyboardState;

        int[] lockKey;

        string lastCommand = "";

        /*For surface, first key stroke has no effect*/

        public KeyboardController(SpaceGame game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            lastKeyboardState = Keyboard.GetState();
            lockKey = new int[10];
            for (int i = 0; i < 10; i++) lockKey[i] = 0;
        }

        private bool CommandInput(KeyboardState keyboardState)
        {
            if (game.gameLevel1.showCommand)
            {
                for (int i = 8; i <= 189; i++) /*All number and digits*/
                {
                    if (keyboardState.IsKeyDown((Keys)i))//&& isKeyReleased == true
                    {
                        bool sameKey = false;
                        for (int j = 0; j < 10; j++)
                            if (lockKey[j] == i) { sameKey = true; break; }

                        if (sameKey) continue;

                        if ((Keys)i == Keys.Enter)
                        {
                            continue;
                        }
                        else if ((Keys)i == Keys.Space)
                        {
                            game.gameLevel1.commandLine += " ";
                        }
                        else if ((Keys)i == Keys.OemComma)
                        {
                            game.gameLevel1.commandLine += ",";
                        }
                        else if ((Keys)i == Keys.OemMinus)
                        {
                            game.gameLevel1.commandLine += "-";
                        }
                        else if ((Keys)i == Keys.Up)
                        {
                            game.gameLevel1.commandLine = lastCommand;
                        }
                        else if (i <= 57 && i >= 48)
                        {
                            game.gameLevel1.commandLine += (i - 48).ToString();
                        }
                        else if ((Keys)i == Keys.Back)
                        {
                            int c = game.gameLevel1.commandLine.Count();
                            if (c > 0)
                                game.gameLevel1.commandLine = game.gameLevel1.commandLine.Remove(c - 1, 1);
                        }
                        else
                        {
                            game.gameLevel1.commandLine += (Keys)(i);
                        }

                        for (int j = 0; j < 10; j++) if (lockKey[j] == 0) { lockKey[j] = i; break; }
                    }

                    for (int j = 0; j < 10; j++)
                    {
                        if (keyboardState.IsKeyUp((Keys)i) && lockKey[j] == i)
                        {
                            lockKey[j] = 0;
                            break;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime, int aircraftIndex)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            /*Command*/
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (game.gameLevel1.commandLine != "")
                    lastCommand = game.gameLevel1.commandLine;
                game.gameLevel1.isPaused = false;
                game.gameLevel1.showCommand = false;
            }

            if (keyboardState.IsKeyDown(Keys.OemTilde))
            {
                game.gameLevel1.isPaused = true;
                game.gameLevel1.showCommand = true;
            }
            bool inCommand = CommandInput(keyboardState);
            if (inCommand) return;

            /*Camera*/
            //if (keyboardState.IsKeyDown(Keys.PageUp)   && lastKeyboardState.IsKeyUp(Keys.PageUp))
            //game.uICombat.GlobalCamera.IncreaseCameraLevel();
            //if (keyboardState.IsKeyDown(Keys.PageDown) && lastKeyboardState.IsKeyUp(Keys.PageDown))
            //game.uICombat.GlobalCamera.DecreaseCameralLevel();


            //
            //myAircraft.DecreaseThrust();

            /*General*/
            Aircraft myAircraft = game.gameLevel1.pilots.GetAircraft(aircraftIndex);
            if (keyboardState.IsKeyDown(Keys.LeftControl) && lastKeyboardState.IsKeyUp(Keys.LeftControl))
                myAircraft.IncreaseThrust();
            if (keyboardState.IsKeyDown(Keys.LeftAlt) && lastKeyboardState.IsKeyUp(Keys.LeftAlt))
                myAircraft.DecreaseThrust();

            if (keyboardState.IsKeyDown(Keys.Home) && lastKeyboardState.IsKeyUp(Keys.Home))
                game.gameLevel1.isPaused = !game.gameLevel1.isPaused;

            if (keyboardState.IsKeyDown(Keys.Escape) && lastKeyboardState.IsKeyUp(Keys.Escape))
                game.SetGameManager(GameScreens.OPTION);

            if (keyboardState.IsKeyDown(Keys.C) && lastKeyboardState.IsKeyUp(Keys.C))
            {
                if (game.currentUIScreen == UIScreens.COMBAT)
                {
                    game.gameLevel1.isInEquip = true;
                    game.SetUIManager(UIScreens.EQIPMENT);
                }
                else if (game.currentUIScreen == UIScreens.EQIPMENT)
                {
                    game.SetUIManager(UIScreens.COMBAT);
                    game.gameLevel1.isInEquip = false;
                }
                
            }
  
            if (keyboardState.IsKeyDown(Keys.H) && lastKeyboardState.IsKeyUp(Keys.H))
            {
                game.uICombat.HelpStringVisible = !game.uICombat.HelpStringVisible;
            }

            /*Avator Action*/
            if (!game.gameLevel1.isPaused)
            {
                //if (keyboardState.IsKeyDown(Keys.LeftControl) && lastKeyboardState.IsKeyUp(Keys.LeftControl))
                //{
                //    game.gameLevel1.aircrafts.GetAircraft(aircraftIndex).Bool_thrust =
                //        !game.gameLevel1.aircrafts.GetAircraft(aircraftIndex).Bool_thrust;
                //}

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    game.gameLevel1.pilots.GetAircraft(aircraftIndex).CurrentTurnRate = game.gameLevel1.pilots.GetAircraft(aircraftIndex).MaxTurnRate;
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    game.gameLevel1.pilots.GetAircraft(aircraftIndex).CurrentTurnRate = -game.gameLevel1.pilots.GetAircraft(aircraftIndex).MaxTurnRate;
                }
                //else
                //{
                //    game.gameLevel1.aircrafts.GetAircraft(aircraftIndex).CurrentTurnRate = 0;
                //}

                lastKeyboardState = keyboardState;
            }
        }
        

    }
}
