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
    public class Utilities
    {
        public float PI = 3.1415926f;

        public SpriteFont ui_button_large;
        public SpriteFont ui_screen_large;
        public SpriteFont ui_screen_medium;
        public SpriteFont ui_button_medium;
        public SpriteFont ui_button_small;
        public SpriteFont ui_screen_small;

        public float inf = 999999999;
        public float minusInf = -999999999;

        public Color whiteBtnTextColor =  new Color(0, 0, 0, 255);
        public Color whiteBtnTextHighColor = new Color(0, 0, 0, 255);
        public Color whiteBtnTextImageColor = new Color(200, 200, 200, 0);

        public Color BarTextColor = Color.Black;
        public Color BarHighColor = Color.Black;
        public Color BarImageColor = new Color(255, 255, 255, 10);

        public struct TwinVecter3
        {
            public Vector3 v;
            public Vector3 p;
            public TwinVecter3(Vector3 x1, Vector3 px)
            {
                v = x1;
                p = px;
            }
        };

        public float LimitAngle(float angle)
        {
            while (angle > 2 * PI)
            {
                angle -= 2 * PI;
            }

            while (angle < -2 * PI)
            {
                angle += 2 * PI;
            }

            return angle;
        }

        public float ComputeTheAngleFromQuaternion(Quaternion Orientation)
        {
            /*Old Code*/
            //Matrix transform = Matrix.CreateFromQuaternion(Orientation);
            //float Angle = (float)Math.Acos(transform.M11);
            //if ((transform.M11 + 1) < 0.000001f)
            //    Angle = PI;
            //if ((float)Math.Acos(transform.M12) < PI / 2)
            //   Angle = 2 * PI - Angle;

            float w = Orientation.W;
            float x = Orientation.X;
            float y = Orientation.Y;
            float z = Orientation.Z;

            //float alpha = (float)Math.Atan2((double)(2 * (w * x + y * z)), (double)(1 - 2 * (x * x + y * y)));
            //float beta = (float)Math.Asin(2 * (w * y - z * x));
            float gamma = (float)Math.Atan2((double)(2 * (w * z + x * y)), (double)(1 - 2 * (y * y + z * z)));

            return LimitAngle(2 * PI - gamma);
        }

        public string[] resolutionTypeName;

        public Utilities()
        {
            resolutionTypeName = new string[4];
            resolutionTypeName[0] = "800 X 600";
            resolutionTypeName[1] = "1024 X 768";
            resolutionTypeName[2] = "1600 X 900";
            resolutionTypeName[3] = "Full Screen";
        }

        public bool CheckAgainst(RelationEnum r1, RelationEnum r2)
        {
            if (r1 == RelationEnum.PLAYER && r2 == RelationEnum.ENEMY1) return true;
            if (r1 == RelationEnum.PLAYER && r2 == RelationEnum.ENEMY2) return true;
            if (r1 == RelationEnum.PLAYER && r2 == RelationEnum.NEUTRAL) return true;
            if (r1 == RelationEnum.ALLY && r2 == RelationEnum.ENEMY1) return true;
            if (r1 == RelationEnum.ALLY && r2 == RelationEnum.ENEMY2) return true;
            if (r1 == RelationEnum.ALLY && r2 == RelationEnum.NEUTRAL) return true;
            if (r1 == RelationEnum.NEUTRAL && r2 == RelationEnum.ENEMY1) return true;
            if (r1 == RelationEnum.NEUTRAL && r2 == RelationEnum.ENEMY2) return true;
            if (r1 == RelationEnum.NEUTRAL && r2 == RelationEnum.PLAYER) return true;
            if (r1 == RelationEnum.NEUTRAL && r2 == RelationEnum.ALLY) return true;
            if (r1 == RelationEnum.ENEMY1 && r2 == RelationEnum.PLAYER) return true;
            if (r1 == RelationEnum.ENEMY1 && r2 == RelationEnum.ALLY) return true;
            if (r1 == RelationEnum.ENEMY1 && r2 == RelationEnum.NEUTRAL) return true;
            if (r1 == RelationEnum.ENEMY2 && r2 == RelationEnum.PLAYER) return true;
            if (r1 == RelationEnum.ENEMY2 && r2 == RelationEnum.ALLY) return true;
            if (r1 == RelationEnum.ENEMY2 && r2 == RelationEnum.NEUTRAL) return true;

            return false;
        }

        public void LoadContent(ContentManager content)
        {
            ui_button_large = content.Load<SpriteFont>("ui_button_Large");
            ui_screen_large = content.Load<SpriteFont>("ui_screen_Large");
            ui_button_medium = content.Load<SpriteFont>("ui_button_Medium");
            ui_screen_medium = content.Load<SpriteFont>("ui_screen_Medium");
            ui_button_small = content.Load<SpriteFont>("ui_button_Small");
            ui_screen_small = content.Load<SpriteFont>("ui_screen_Small");
        }

        private static Random random = new Random();

        public float RandomNumber(float min, float max)
        {

            return (float)(min + (random.NextDouble() * (max - min)));
        }

        public Vector3 RandomRingArea(float innerRange, float outerRange)
        {
            float x = RandomNumber(-outerRange, outerRange);
            float y;
            do
            {
                y = RandomNumber(-outerRange, outerRange);
            } while ((x * x + y * y) < innerRange * innerRange);

            return new Vector3(x, y, 0);
        }

        public Color GetMarkerColor(RelationEnum relation)
        {
            if (relation == RelationEnum.ENEMY1 || relation == RelationEnum.ENEMY2)
            {
                return new Color(120, 0, 0);
            }
            else if (relation == RelationEnum.PLAYER)
            {
                return new Color(0, 120, 0);
            }
            else if (relation == RelationEnum.ALLY)
            {
                return new Color(60, 60, 0);
            }
            //else if (relation == RelationEnum.NEUTRAL)
            //{

            return Color.Yellow;
            //}

        }

    }

    public enum GameScreens {NONE, GAMELEVEL1, GAMELEVEL2, LOADING, MENU, OPTION, PLAYERINFO }
    public enum UIScreens { NONE,COMBAT, EQIPMENT }
    public enum SourceGroup { PLAYER1, PLAYER2, AI }
    public enum TeamRole { LEADER, FOLLOWER, INDEPENDENT}
    public enum Formation { FREE, LINE, TRIANGLE }
    public enum AIState { APPROACH, AWAY, FOCUSTARGET, WAIT, FOLLOWLEADER, FORMING, FREE }
    public enum ObjectType { BULLET, AIRCRAFT }
    //public enum MoveState {MOVE, DONE }
    //public enum CommandState {SENDCOMMAND, NONE }
    public enum ActionType
    {
        FIREWEAPON1,
        FIREWEAPON2,
        FIREWEAPON3,
        FIREWEAPON4,
        FIREWEAPON5,
        FIREWEAPON6,
        FIREWEAPON7,
        FIREWEAPON8,
        DODGE,
        NONE
    }
    public enum WeaponType
    {
        NONE,
        GUN,
        TORPEDO,
        MISSILE,
        UTILITY
    }
    public enum WeaponCode{LASER}
    public enum ResolutionType
    {
        res800X600,
        res1024X768,
        res1600X900,
        resFullScreen
    }
    public enum CameraType {NONE, CHASE, GLOBAL }
    public enum AircraftStatus
    {
        NONE,
        INFLAME,
        SLOW,
        STUN,
        CONFUSE,
    }
    public enum RelationEnum
    {
        NONE,
        PLAYER,
        ALLY,
        NEUTRAL,
        ENEMY1,
        ENEMY2,
        INVULNERABLE
    }

    
}
