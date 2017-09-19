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
    public class Weapon
    {
        public SpaceGame game;

        public string weaponName;

        public float cooldown;
        public float maxCooldown;

        private bool activity;
        public bool Activity { get { return activity; } set { activity = value; } }

        public RelationEnum relation;
        public SourceGroup sourceGroup;

        public float EnergyCost;
        public float Power;

        public float Range;

        //public Vector3 Position;
        //public float WeaponAngle;

        public float MaxSpeed;

        //public int Index;

        //public int Level;
        //public int BasicLevelUpCost;
        //public float LevelUpCost;

        public float PositionModify = 0;

        //public WeaponType weaponType;

        public Weapon(SpaceGame game)
        {
            this.game = game;
        }

        public virtual void Initialize()
        {
            //this.Index = index;
            activity = true;
            cooldown = 0;//ready to fire
            //LevelUpCost = BasicLevelUpCost;
            //weaponType = type;
        }

        //public virtual void LevelUp()
        //{
        //    Level++;
        //    LevelUpCost *= 2;
        //}

        //public virtual void SetPosition(Vector3 position, float angle)
        //{
        //    Position = position;
        //    WeaponAngle = angle;
        //}

        //public virtual void LoadContent(ContentManager content)
        //{
        //}

        public virtual bool Fire(Vector3 shipPosition, Quaternion shipOrientation, Vector3 shipVelocity,
            Vector3 Position, float WeaponAngle) //float shipSpeed
        {
            if (cooldown <= 0)
            {
                cooldown = maxCooldown;

                //game.SoundBank.PlayCue("tx0_fire1");

                LaserBullet bullet = new LaserBullet(game);

                bullet.Position = Vector3.TransformNormal(Position, 
                    Matrix.CreateFromQuaternion(shipOrientation)) + shipPosition;

                Matrix bulletOrientationMatrix = 
                    Matrix.CreateFromQuaternion(shipOrientation) *
                    Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), WeaponAngle);

                bullet.Orientation = shipOrientation * 
                    Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0),  -WeaponAngle);

                bullet.CurrentVelocity = Vector3.TransformNormal(new Vector3(0, 0, 1) * MaxSpeed, bulletOrientationMatrix) + shipVelocity;

                bullet.Position += Vector3.Transform(new Vector3(0, 0, PositionModify), bullet.Orientation);

                bullet.Relation = relation;
                bullet.sourceGroup = sourceGroup;
                //bullet.playerOwner = playerOwner;
                
                bullet.Initialize(Power, Range/MaxSpeed);

                bullet.LoadContent(game.Content);

                game.gameLevel1.bullets.m_Bullets.AddLast(bullet);

                game.soundBank.PlayCue("FireLaser1");

                return true;
            }
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (cooldown > 0) cooldown -= delta;
        }

    }
}
