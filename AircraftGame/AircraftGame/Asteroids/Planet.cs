using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public class Planet : MyObject
    {
        public Planet(SpaceGame game)
            : base(game)
        {
        }

        public void Initialize(Vector3 position, int modelIndex)
        {
            /*Model*/
            base.modelIndex = modelIndex;
            base.Relation = RelationEnum.NEUTRAL;

            /*Physics*/
            base.Position = position;
            base.ModelScale = 2.5f;
            base.LifeTime = -1;//infinite
            base.Orientation = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), 0);
        }

        public void Remove()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            base.DrawModel(graphics, game.modelManager.GetModel(modelIndex), Transform);
        }

    }
}
