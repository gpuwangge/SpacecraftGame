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
    public class MyObject
    {
        /*Properties*/
        /*Offense*/
        public float MaxPower;
        public float CurrentPower;
        /*Defence*/
        public float MaxHitpoint;
        public float CurrentHitpoint;
        public float MaxShieldPoint;
        public float CurrentShieldPoint;
        public float MaxArmor;
        public float CurrentArmor;
        /*Energy*/
        public float MaxEnergyPoint;
        public float CurrentEnergyPoint;
        /*Physics*/
        public float ObjectWeight;
        public float CurrentWeight;
        public Vector3 MaxVelocity;
        public Vector3 CurrentVelocity;
        float speed;        // How fast we are going (cm/sec)
        public float Speed { get { return speed; } set { speed = value; } }
        Quaternion orientation;
        public Quaternion Orientation { get { return orientation; } set { orientation = value; } }
        public Quaternion bankOrientation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), 0);/*Bank*/
        public Vector3 Axis;
        public float MaxTurnRate;
        public float CurrentTurnRate;
        public Vector3 SpeedDir;

        /*Type Properties*/
        public string ObjectName;
        public RelationEnum Relation;
        public SourceGroup sourceGroup;
        public int modelIndex;
        public float ModelScale;
        public float DisplayModelScale;
        public ObjectType objectType;
     
        /*Game Properties*/
        public float LifeTime;
        public float MaxLifeTime;
        public float Size;
        public Vector3 Position;
        public int ObjectIndex;

        /*Common Properties*/
        public SpaceGame game;
        public float PI = 3.1415926f;
        public bool inCollision = false;
        public bool Visible = true;
        public bool Destroyed = false;
        public bool Invulnarable = false;/*not used yet*/
        public bool NonTargetable = false;/*not used yet*/
        public bool Dodged = false;
        public float CollisionCooldown = 0;
        public float CollisionMaxCooldown = 0.5f;
        public bool CanCollision = true;

        private static Random random = new Random();
        public Vector3 RandomVector(float min, float max)
        {
            return new Vector3(
                RandomNumber(min, max),
                RandomNumber(min, max),
                0);
        }
        
        public float RandomNumber(float min, float max)
        {
           
            return (float)(min + (random.NextDouble() * (max - min)));
        }

        public int RandomNumber(int min, int max)
        {

            return (int)(min + (random.NextDouble() * (max - min)) + 0.5f);
        }

        /*Hit Function*/
        public virtual void DamageHitpointAnimation(Vector3 weaponPos, Vector3 weaponVel, ObjectType objectType) { }
        public virtual void DamageShieldAnimation(Vector3 weaponPos, Vector3 weaponVel, ObjectType objectType) { }
        public virtual void DestroyAnimation() { }
        public virtual void HitSomething(MyObject myObject) { }



        public MyObject(SpaceGame game)
        {
            this.game = game;
            Vector3 axis = new Vector3(0, 0, 0);
            int dir = RandomNumber(1, 3);
            switch (dir)
            {
                case 1:
                    axis.X = 1;
                    break;
                case 2:
                    axis.Y = 1;
                    break;
                case 3:
                    axis.Z = 1;
                    break;
            }
            this.Orientation = Quaternion.CreateFromAxisAngle(axis, RandomNumber(-PI, PI));
        }

        public Matrix Transform
        {
            get
            {
                return Matrix.CreateScale(this.ModelScale) *
                    Matrix.CreateFromQuaternion(Orientation) *
                    Matrix.CreateFromQuaternion(bankOrientation) *
                    Matrix.CreateTranslation(Position);
            }
        }


        /*Public Render*/
        public virtual void DrawModel(GraphicsDeviceManager graphics, Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

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
