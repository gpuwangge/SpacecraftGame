using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public class Planets
    {
        private SpaceGame game;
        public LinkedList<Planet> planets;
        Random random = new Random();

        public Planets(SpaceGame game)
        {
            this.game = game;
            planets = new LinkedList<Planet>();
        }

        public void AddBackgroudPlanets()
        {
           AddPlanet(new Vector3(0, 0, 60000), (int)ModelType.Planet1);
           AddPlanet(new Vector3(10000, 10000, 50000), (int)ModelType.Planet2);
        }

        public void AddPlanet(Vector3 position, int modelIndex)
        {
            Planet newPlanet = new Planet(game);
            newPlanet.Initialize(position, modelIndex);
            planets.AddLast(newPlanet);

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            for (int i = 0; i < planets.Count; i++)
            {
                planets.ElementAt(i).Draw(graphics, gameTime);
            }
        }
        
    }
}
