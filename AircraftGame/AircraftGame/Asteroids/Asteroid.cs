using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public class Asteroid: MyObject
    {
        public Asteroid(SpaceGame game): base(game)
        {
        }

        public void Initialize(Vector3 position, Vector3 velocity, float hitPoint, float armor)
        {
            base.ObjectName = "Asteroid";
            base.objectType = ObjectType.AIRCRAFT;

            /*Model*/
            modelIndex = RandomNumber((int)ModelType.ASTEROID1, (int)ModelType.ASTEROID4);
            base.Relation = RelationEnum.NEUTRAL;

            /*Perameter*/
            base.MaxHitpoint = hitPoint;
            base.CurrentHitpoint = hitPoint;
            base.MaxShieldPoint = 0;
            base.CurrentShieldPoint = 0;
            base.MaxEnergyPoint = 0;
            base.CurrentEnergyPoint = 0;
            base.MaxArmor = armor;
            base.CurrentArmor = armor;
            base.MaxPower = hitPoint / 2.0f;
            base.CurrentPower = MaxPower;

            /*Physics*/
            base.Position = position;
            base.CurrentVelocity = velocity;
            base.ObjectWeight = hitPoint * 100;
            base.CurrentWeight = hitPoint * 100;
            base.LifeTime = -1;//infinite
            if (MaxHitpoint <= 4)//kill too small asteroid
            {
                LifeTime = (int)(MaxHitpoint * 200);
            }

            base.ModelScale = hitPoint / 100.0f;
            //BoundingSphere bs = models[modelIndex].Meshes[0].BoundingSphere;
            //bs = bs.Transform( models[modelIndex].Bones[0].Transform);
            //Size = ModelScale * bs.Radius * 0.6f;
            Size = ModelScale*50;

            /*Rotation*/
            base.MaxTurnRate = RandomNumber(-10, 10) / 10.0f;
            CurrentTurnRate = MaxTurnRate;
            //if(!isBackgroud)
            //base.Orientation = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -PI / 2);
            float x = RandomNumber(1, 100) / 100.0f;
            float y = RandomNumber(1, 100) / 100.0f;
            float z = RandomNumber(1, 100) / 100.0f;
            base.Axis = new Vector3(x, y, z);
            Axis.Normalize();
        }

        public void Remove()
        {
            game.gameLevel1.asteroids.asteroids.Remove(this);
        }

        public void Update(GameTime gameTime)
        {
            /*Update Self Rotation*/
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Orientation *= Quaternion.CreateFromAxisAngle(Axis, CurrentTurnRate * delta);
            Orientation.Normalize();

            /*Update Position*/
            Position += CurrentVelocity * delta;
            Position.Z = 0;
 
            LifeTime--;
            if (LifeTime == 0)
            {
                Remove();
            }
        }

        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime){
            base.DrawModel(graphics, game.modelManager.GetModel(modelIndex), Transform);
        }



        public override void DamageHitpointAnimation(Vector3 weaponPos, Vector3 weaponVel, ObjectType objectType){
            if (objectType == ObjectType.BULLET)
                DamageAnimation(weaponPos, CurrentVelocity + weaponVel);
            Push(weaponVel);
        }

        public void DamageAnimation(Vector3 pos, Vector3 vel){
            game.soundBank.PlayCue("DamageSound1");
            for (int i = 0; i < 15; i++){
                //game.asteroids.smokeParticleSystem.AddParticle(pos + new Vector3(0, 0, -10),
                //       new Vector3(0, 0, 0) + vel / 50);
                game.gameLevel1.asteroids.damageParticleSystem.AddParticle(pos + new Vector3(0, 0, -30),
                    new Vector3(0, 0, 0) + vel / 50);
            }
        }

        public override void DamageShieldAnimation(Vector3 weaponPos, Vector3 weaponVel, ObjectType objectType)
        {
        }

        public override void DestroyAnimation(){
            ExpodeAnimation(Position, CurrentVelocity);
            Destroyed = true;
            game.gameLevel1.asteroids.Scatter(this);
            game.gameLevel1.AsteroidDestroy();
        }

        public void ExpodeAnimation(Vector3 pos, Vector3 vel){
            game.soundBank.PlayCue("ExplosionSound1");
            for (int i = 0; i < 15; i++){
                game.gameLevel1.asteroids.explosionParticleSystem.AddParticle(pos + new Vector3(0, 0, -30),
                    new Vector3(RandomNumber(-3, 3), RandomNumber(-3, 3), 0));
                for (int j = 0; j < 3; j++)
                    game.gameLevel1.asteroids.explosionSmokeParticleSystem.AddParticle(pos + new Vector3(0, 0, -15),
                       new Vector3(RandomNumber(-5, 5), RandomNumber(-5, 5), 0) + vel);
            }

        }


        public void Push(Vector3 weaponVel)
        {
            CurrentVelocity += weaponVel / (MaxHitpoint * 10);
        }



    }
}
