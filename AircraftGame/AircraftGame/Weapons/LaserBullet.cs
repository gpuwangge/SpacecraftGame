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
    public class LaserBullet : Bullet
    {
        //public Particle3D.ParticleSystem machineBulletParticleSystem;

        public LaserBullet(SpaceGame game): base(game)
        {
            //machineBulletParticleSystem = new Particle3D.MachineBulletParticleSystem(game, game.Content, 2);
            //game.Components.Add(machineBulletParticleSystem);

        }

        public void Initialize(float Power, float Lifetime)
        {
            LifeTime = Lifetime;         
            CurrentPower = Power;
            weaponType = weaponType;
            ModelScale = 0.15f;
            modelIndex = (int)ModelType.LAZERBULLET;

            objectType = ObjectType.BULLET;

            base.Initial();
        }

        public override void LoadContent(ContentManager content){
            
        }

        public override void Update(float delta)
        {
            Position += CurrentVelocity * delta;
        }
        

        //public override void Draw(GraphicsDeviceManager graphics, Model model, Effect effect)
        //{
            //base.Draw(graphics, model, effect);

            //Matrix View = game.uiCombat.GlobalCamera.View;
            //Matrix Projection = game.uiCombat.GlobalCamera.Projection;
            //machineBulletParticleSystem.SetCamera(View, Projection);

            //SetEffect(model, Matrix.CreateScale(0.1f) * Orientation * Matrix.CreateTranslation(Position),
                        //null, "Technique1", effect, Matrix.Identity);
        //}

        //public override void HitSomething(string pilotName, string aircraftName, float maxHitpoint, float hitpoint,
        //    float maxShieldPoint, float shieldPoint,
        //    float maxEnergyPoint, float energyPoint)
        //{
        //    if (OperatorName == "")//the player owns me
        //    {
        //        game.uiCombat.Player1HitIt(pilotName, aircraftName, maxHitpoint, hitpoint, maxShieldPoint, shieldPoint, maxEnergyPoint, energyPoint);
        //    }
        //}

       // private void SetEffect(Model model, Matrix world, Texture2D normalFile, string tech, Effect p, Matrix rotationMatrix)
        //{
            //Matrix[] transforms = new Matrix[model.Bones.Count];
            //model.CopyAbsoluteBoneTransformsTo(transforms);

            //p.CurrentTechnique = p.Techniques[tech];
            //p.Begin();
            //foreach (EffectPass pass in p.CurrentTechnique.Passes)
            //{
            //    // Begin current pass
            //    pass.Begin();

            //    foreach (ModelMesh mesh in model.Meshes)
            //    {
            //        foreach (ModelMeshPart part in mesh.MeshParts)
            //        {

            //            BasicEffect bEffect = part.Effect as BasicEffect;
            //            if (bEffect != null && bEffect.Texture != null)
            //            {
            //                p.Parameters["Texture"].SetValue(bEffect.Texture);
            //                //  p.Parameters["ColorMap"].SetValue(bEffect.Texture);
            //            }


            //            p.Parameters["LightAmbient"].SetValue(game.scenario1.space1.LightAmbient);
            //            p.Parameters["Light1Location"].SetValue(game.scenario1.space1.light1Location);
            //            p.Parameters["Light1Color"].SetValue(game.scenario1.space1.light1Color);
            //            p.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * world);
            //            Matrix View = game.uiCombat.GlobalCamera.View;
            //            Matrix Projection = game.uiCombat.GlobalCamera.Projection;
            //            p.Parameters["View"].SetValue(View);
            //            p.Parameters["Projection"].SetValue(Projection);
            //            p.Parameters["matShadeScale"].SetValue(Matrix.CreateScale(3));

            //            game.graphics.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
            //            game.graphics.GraphicsDevice.Indices = mesh.IndexBuffer;
            //            game.graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
            //                                                          part.BaseVertex, 0, part.NumVertices,
            //                                                          part.StartIndex, part.PrimitiveCount);
            //        }


            //    }

            //    pass.End();
            //}

            //p.End();


        //}
    }

}
