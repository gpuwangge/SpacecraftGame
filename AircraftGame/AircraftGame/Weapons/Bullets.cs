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
    public class Bullets
    {
        private LinkedList<Bullet> m_bullets = new LinkedList<Bullet>();
        public LinkedList<Bullet> m_Bullets { get { return m_bullets; } }

        private SpaceGame game;

        //private Effect BlastEffect1;

        public Bullets(SpaceGame game)
        {
            this.game = game;
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (LinkedListNode<Bullet> bulletNode = m_Bullets.First; bulletNode != null; )
            {
                LinkedListNode<Bullet> nextBlast = bulletNode.Next;
                Bullet bullet = bulletNode.Value;

                bullet.LifeTime -= delta;
                if (bullet.LifeTime <= 0)
                    bullet.Remove();
                else
                    bullet.Update(delta);

                bulletNode = nextBlast;
            }
        }

        


        public void Draw(GraphicsDeviceManager graphics)
        {
            foreach (Bullet bullet in m_Bullets)
            {
                //blast.Draw(graphics, model, BlastEffect1);

                bullet.DrawModel(graphics, game.modelManager.GetModel( bullet.modelIndex), bullet.Transform);
            }
        }

        //private void DrawModel(GraphicsDeviceManager graphics, Model model, Matrix world)
        //{
        //    Matrix[] transforms = new Matrix[model.Bones.Count];


        //    model.CopyAbsoluteBoneTransformsTo(transforms);

        //    for (int i = 0; i < model.Bones.Count; i++)
        //    {
        //        transforms[i] = Matrix.CreateScale(0.1f) * transforms[i]; 
        //    }

        //    foreach (ModelMesh mesh in model.Meshes)
        //    {
        //        foreach (BasicEffect effect in mesh.Effects)
        //        {
        //            effect.EnableDefaultLighting();
        //            effect.World = transforms[mesh.ParentBone.Index] * world;
        //            effect.View = game.GlobalCamera.View;
        //            effect.Projection = game.GlobalCamera.Projection;
        //        }

        //        mesh.Draw();
        //    }
        //}

    }
}
