using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public class AircraftFederalObserver : Aircraft
    {
        public AircraftFederalObserver(SpaceGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            ObjectName = "Fed. Observer";

            base.MaxHitpoint = 120;
            base.MaxShieldPoint = 50;
            base.MaxEnergyPoint = 100;
            base.MaxArmor = 1;

            //base.MaxPitchRate = 60;
            //base.MaxRollRate = 60;
            //base.MaxTurnRate = 60;

            base.MaxThrust = 200;

            //base.Weight = 965;
            //base.DragCoff = Weight / 10; // a factor of the air

            base.MaxEnergyRegeneration = 3;
            base.MaxShieldRegeneration = 10;
            base.MaxShieldCoolDown = 3;

            base.Size = 1.5f;
            base.ModelScale = 2.0f;

            base.engineSoundName = "engine_2";
            base.SmokePos1 = new Vector3(0, 0, -10.0f);

            //base.Power = 20;// collision damage when speed is low

            base.DisplayModelScale = 4;

            //base.Elevation = 0;
            //base.Azimuth = 0;

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            /*Thrust*/
            float acceleration = CurrentThrust * MaxThrust - Drag * Speed;
            Speed += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            System.Diagnostics.Trace.WriteLine(Speed.ToString());

            //Matrix transform = Matrix.CreateRotationX(Elevation) * Matrix.CreateRotationY(Azimuth);

            //Vector3 directedThrust = Vector3.TransformNormal(new Vector3(0, 0, 1), transform);

            //Position += directedThrust * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            /*Turn*/
            //Azimuth += CurrentTurnRate * MaxTurnRate * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }


        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            //Matrix transform = Matrix.CreateRotationX(Elevation) *
            //   Matrix.CreateRotationY(Azimuth) *
            //   Matrix.CreateTranslation(Position);

            //base.game.DrawModel(graphics, model, Matrix.CreateScale(this.ModelScale) * transform);
            base.Draw(graphics, gameTime);
        }

 
    }
}
