using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public enum ModelType
    {
        ASTEROID1,
        ASTEROID2,
        ASTEROID3,
        ASTEROID4,
        XWING,
        OBSERVER,
        LAZERBULLET,
        Planet1,
        Planet2
    };

    public class ModelManager
    {
        public SpaceGame game;

        public Model[] models;
        public string[] modelNames;

        public int modelSize = 9;

        public ModelManager(SpaceGame game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            models = new Model[modelSize];
            modelNames = new string[modelSize];
            modelNames[(int)ModelType.ASTEROID1] = "Asteroid1";
            modelNames[(int)ModelType.ASTEROID2] = "Asteroid2";
            modelNames[(int)ModelType.ASTEROID3] = "Asteroid3";
            modelNames[(int)ModelType.ASTEROID4] = "Asteroid4";
            modelNames[(int)ModelType.XWING] = "xwing";
            modelNames[(int)ModelType.OBSERVER] = "FederalObserver01";
            modelNames[(int)ModelType.LAZERBULLET] = "LaserBlast";
            modelNames[(int)ModelType.Planet1] = "Planet01";
            modelNames[(int)ModelType.Planet2] = "Planet02";
        }

        public void LoadContent()
        {
            for (int i = 0; i < modelSize; i++)
            {
                models[i] = game.Content.Load<Model>(modelNames[i]);
            }
        }

        public Model GetModel(int i)
        {
            return models[i];
        }
    }
}
