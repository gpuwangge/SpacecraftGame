using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GameSpace
{

    public class WeaponLaserGun : Weapon
    {
        public WeaponLaserGun(SpaceGame game): base(game){}

        public override void Initialize(){
            weaponName = "Laser Gun";
            //Level = 0;
            maxCooldown = 0.5f;
            Power = 35;
            EnergyCost = 0.5f;
            MaxSpeed = 500;
            Range = 700;
            //BasicLevelUpCost = 100;
            PositionModify = 20;
            base.Initialize();
        }

        //public void SetLevel(int l)
        //{
        //    while (Level < l)
        //    {
        //        LevelUp();
        //    }
        //}

        //public override void LevelUp()
        //{
        //    base.LevelUp();
        //    switch (Level)
        //    {
        //        case 1:
        //        case 2:
        //        case 3:
        //            maxCooldown -= 0.02f;
        //            Power += 1;
        //            EnergyCost += 0.5f;
        //            MaxSpeed += 15;
        //            break;
        //    }
        //}

        //public override void SetPosition(Vector3 position, float angle)
        //{
        //    base.SetPosition(position, angle);
        //}

        //public override void LoadContent(ContentManager content)
        //{
        //    base.LoadContent(content);
        //}

        public override bool Fire(Vector3 shipPosition, Quaternion shipOrientation, Vector3 shipVelocity, Vector3 position, float angle){
           return base.Fire(shipPosition, shipOrientation, shipVelocity, position, angle);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        //public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        //{
            //base.Draw(graphics, gameTime);
        //}




    }
}
