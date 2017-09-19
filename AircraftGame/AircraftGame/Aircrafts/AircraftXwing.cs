using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public class AircraftXwing: Aircraft
    {
        private int wing1;          // Index to the wing 1 bone
        private int wing2;          // Index to the wing 2 bone
        private float wingAngle = 0.2f;

        public AircraftXwing(SpaceGame game): base(game)
        {
            weaponLaserGun = new WeaponLaserGun(game);
            Initialize();
        }

        public override void Initialize()
        {
            base.ObjectName = "Xwing";
            base.objectType = ObjectType.AIRCRAFT;

            base.MaxHitpoint = 185;
            CurrentHitpoint = MaxHitpoint;
            base.MaxShieldPoint = 50;
            CurrentShieldPoint = MaxShieldPoint;
            base.MaxEnergyPoint = 100;
            CurrentEnergyPoint = MaxEnergyPoint;
            base.MaxArmor = 1;
            CurrentArmor = MaxArmor;

            base.MaxTurnRate = 4.0f;
            CurrentTurnRate = 0;
            base.MaxThrust = 10000;
            CurrentThrust = MaxThrust;
            UserThrust = 0;
            base.MaxVelocity = new Vector3(1000,1000,1000);
            CurrentVelocity = Vector3.Zero;

            //Bool_thrust = false;

            base.MaxPower = 20;// collision damage when speed is low
            base.CurrentPower = MaxPower;

            base.ObjectWeight = 965;
            base.CurrentWeight = ObjectWeight;
            base.DragCoff = CurrentWeight / 10; // a factor of the air

            base.MaxEnergyRegeneration = 2;
            base.CurrentEnergyRegeneration = MaxEnergyRegeneration;
            base.MaxShieldRegeneration = 10;
            base.CurrentShieldRegeneration = MaxShieldRegeneration;
            base.MaxShieldCoolDown = 3;
            base.CurrentShieldCoolDown = MaxShieldCoolDown;

            base.engineSoundName = "engine_2";
            base.SmokePos1 = new Vector3(0, 0, -10.0f);

            base.Size = 12.0f;/*used in collision*/
            base.ModelScale = 0.02f;
            base.DisplayModelScale = 4;
            base.modelIndex = (int)ModelType.XWING;

            base.dodgeDuration = 2.0f;
            base.dodgeMaxCooldown = 12.0f;

            Speed = 0;
            SpeedDir = Vector3.Zero;
            Drag = 0;
            Acc = 0;
            Deacc = 0;

            /*Particle System*/
            base.SmokePos1 = new Vector3(0, 0, -10.0f);

            base.Initialize();/*Weapon Slots must be initializd first*/

            /*Initialize Weapon Slot*/
            /*For xwing, there are four slots in each of the positon*/
            maxWeaponNumber = 9;
            weaponSlot.Initializa(maxWeaponNumber);/*4 including positions and how many weapon in that position*/
            weaponSlot.SetSlot(0, WeaponType.GUN, new Vector3(0, 0, 10), 0);
            weaponSlot.SetSlot(1, WeaponType.GUN, new Vector3(5, 0, 10), 0);
            weaponSlot.SetSlot(2, WeaponType.TORPEDO, new Vector3(0, 0, 10), -PI / 4);           
            weaponSlot.SetSlot(3, WeaponType.MISSILE, new Vector3(0, 0, 10), PI / 4);
            weaponSlot.SetSlot(4, WeaponType.UTILITY, new Vector3(0, 0, 10), PI);
            weaponSlot.SetSlot(5, WeaponType.UTILITY, Vector3.Zero, 0);
            weaponSlot.SetSlot(6, WeaponType.UTILITY, Vector3.Zero, 0);
            weaponSlot.SetSlot(7, WeaponType.UTILITY, Vector3.Zero, 0);
            weaponSlot.SetSlot(8, WeaponType.NONE, Vector3.Zero, 0);

            
        }

        public override void Update(GameTime gameTime)
        {
            //if (Destroyed == false)
            //{
            /*Wing*/
            //float wingDeployTime = 2.0f;        // Seconds

            //if (deployed && wingAngle < 0.20f)
            //{
            //    wingAngle += (float)(0.20 * gameTime.ElapsedGameTime.TotalSeconds / wingDeployTime);
            //    if (wingAngle > 0.20f)
            //        wingAngle = 0.20f;
            //}
            //else if (!deployed && wingAngle > 0)
            //{
            //    wingAngle -= (float)(0.20 * gameTime.ElapsedGameTime.TotalSeconds / wingDeployTime);
            //    if (wingAngle < 0)
            //        wingAngle = 0;
            //}


            //}

            base.Update(gameTime);
        }


        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            if (Destroyed == false)
            {
                //base.DrawParticleSystem();
                ////DrawModel(graphics, game.modelManager.GetModel(modelIndex), Transform);
                base.Draw(graphics, gameTime);
            }
        }

        public override void DrawModel(GraphicsDeviceManager graphics, Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            wing1 = model.Bones.IndexOf(model.Bones["Wing1"]);
            wing2 = model.Bones.IndexOf(model.Bones["Wing2"]);

            transforms[wing1] = Matrix.CreateRotationY(wingAngle) * transforms[wing1];
            transforms[wing2] = Matrix.CreateRotationY(-wingAngle) * transforms[wing2];

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = game.uIManager.GlobalCamera.View;
                    effect.Projection = game.uIManager.GlobalCamera.Projection;
                }
                mesh.Draw();
            }
        }

    }   
}
