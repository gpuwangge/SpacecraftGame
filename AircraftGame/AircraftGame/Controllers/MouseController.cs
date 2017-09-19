using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameSpace
{
    public class MouseController
    {
        SpaceGame game;
        public MouseState lastMouseState;
        public float PI = 3.1415926f;
        int lastScrollValue;

        private bool mousePitchYaw;
        public bool MousePitchYaw { get { return mousePitchYaw; } set { mousePitchYaw = value; } }

        private bool mousePanTilt;
        public bool MousePanTilt { get { return mousePanTilt; } set { mousePanTilt = value; } }

        //public int userAction;
        public bool firedWeapon;

        public MouseController(SpaceGame game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            lastMouseState = Mouse.GetState();
            mousePitchYaw = false;
            mousePanTilt = false;

            //userAction = -1;/*No Action*/
            firedWeapon = false;
        }

        public void Update(GameTime gameTime, int aircraftIndex, ActionManager actionManager)
        {
            MouseState mouseState = Mouse.GetState();
            int userAction = -1;/*No Action*/
            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                userAction = 0; /*Left Mouse Action*/
                firedWeapon = true;
            }
            if(mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Pressed)
                userAction = 1; /*Right Mouse Action*/

            RotatePlayerShip(GetMouseAndCenterAngle(), aircraftIndex);

            Aircraft myAircraft = game.gameLevel1.pilots.GetAircraft(aircraftIndex);

            for (int i = 0; i < actionManager.GetActionNum(); i++){
                if (i != userAction) continue;/*User action not matchs what this action slot has*/
                ActionType[] currentActions = actionManager.GetAction(i);
                for (int j = 0; j < currentActions.Count(); j++){
                    if (currentActions[j] >= ActionType.FIREWEAPON1 && currentActions[j] <= ActionType.FIREWEAPON8)
                        myAircraft.FireWeapon((int)currentActions[j]);
                    if (currentActions[j] == ActionType.DODGE)
                        myAircraft.Dodge();
                }
            }

            int scrollValue = mouseState.ScrollWheelValue;
            //if (scrollValue > lastScrollValue)  myAircraft.IncreaseThrust();
            //else if (scrollValue < lastScrollValue) myAircraft.DecreaseThrust();

            if (scrollValue > lastScrollValue) game.uICombat.GlobalCamera.DecreaseCameralLevel();
            else if (scrollValue < lastScrollValue) game.uICombat.GlobalCamera.IncreaseCameraLevel();

            /*Audio*/
            /*Audio works, but choose not to use for now*/
            //if (myAircraft.Acc > 0) myAircraft.PlayEngineSound();
            //else myAircraft.StopEngineSound();

            lastScrollValue = scrollValue;



            if (mousePitchYaw && mouseState.LeftButton == ButtonState.Pressed &&
                lastMouseState.LeftButton == ButtonState.Pressed)
            {
                float changeY = mouseState.Y - lastMouseState.Y;
                float changeX = mouseState.X - lastMouseState.X;
                game.uIManager.GlobalCamera.Pitch(-changeY * 0.005f);
                game.uIManager.GlobalCamera.Yaw(-changeX * 0.005f);
            }

            if (MousePanTilt && mouseState.RightButton == ButtonState.Pressed &&
                     lastMouseState.RightButton == ButtonState.Pressed)
            {
                float changeY = mouseState.Y - lastMouseState.Y;
                float changeX = mouseState.X - lastMouseState.X;
                game.uIManager.GlobalCamera.Tilt(changeY * 3);
                game.uIManager.GlobalCamera.Pan(-changeX * 3);
            }

            lastMouseState = mouseState;

            //userAction = -1;
        }

        private void RotatePlayerShip(float MouseAngle, int aircraftIndex)
        {
            Quaternion mouseOrientation;
            mouseOrientation = Quaternion.Identity;
            mouseOrientation *= Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -PI / 2);
            mouseOrientation.Normalize();
            mouseOrientation *= Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), MouseAngle); //0 ~ PI
            mouseOrientation.Normalize();

            //float thresh = 0.000001f;
            float shipAngle = game.utilities.ComputeTheAngleFromQuaternion(game.gameLevel1.pilots.GetAircraft(aircraftIndex).Orientation);
            float angleDiff = Math.Abs(shipAngle - MouseAngle);

            float MaxTurnRate = game.gameLevel1.pilots.GetAircraft(aircraftIndex).MaxTurnRate;
            //float MaxTurnAcc = game.gameLevel1.aircrafts.GetAircraft(aircraftIndex).MaxTurnAcc;

            if (shipAngle <= MouseAngle)
                if (angleDiff <= PI)
                    game.gameLevel1.pilots.GetAircraft(aircraftIndex).CurrentTurnRate = MaxTurnRate;//turn left
                else
                    game.gameLevel1.pilots.GetAircraft(aircraftIndex).CurrentTurnRate = -MaxTurnRate;//turn right
            else
                if (angleDiff <= PI)
                    game.gameLevel1.pilots.GetAircraft(aircraftIndex).CurrentTurnRate = -MaxTurnRate;//turn right
                else
                    game.gameLevel1.pilots.GetAircraft(aircraftIndex).CurrentTurnRate = MaxTurnRate;//turn left

            //System.Diagnostics.Debug.WriteLine(game.aircrafts.GetAircraft(0).CurrentTurnRate.ToString());/*Shake below and above +-*/

        }

        

        private float GetMouseAndCenterAngle()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

            Vector2 Diff = new Vector2(game.ScreenCenter.X - mousePos.X, game.ScreenCenter.Y - mousePos.Y);

            Vector2 gameWorldDiff = new Vector2(
                Diff.X/2.0f,
                Diff.Y/2.0f);

            float z = gameWorldDiff.X * gameWorldDiff.X;
            float y = gameWorldDiff.Y * gameWorldDiff.Y;
            float x = z + y;//x=y^2+z^2

            float angle = 0.0f;
            if (x != 0)
            {
                angle = (float)Math.Acos((x + y - z) / (2 * Math.Sqrt(x) * Math.Sqrt(y))); //always 0 ~ PI / 2
                if (gameWorldDiff.Y <= 0)
                    angle = PI - angle;

                if (gameWorldDiff.X <= 0)
                    angle = 2 * PI - angle;
            }

            return game.utilities.LimitAngle(angle);
        }

        
    }
}
