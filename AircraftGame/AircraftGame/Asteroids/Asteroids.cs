using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameSpace
{
    public class Asteroids
    {
        private SpaceGame game;
        public LinkedList<Asteroid> asteroids;
        Random random = new Random();

        public Particle3D.ParticleSystem explosionParticleSystem;
        public Particle3D.ParticleSystem explosionSmokeParticleSystem;
        public Particle3D.ParticleSystem damageParticleSystem;
        
        public Asteroids(SpaceGame game)
        {
            this.game = game;
            asteroids = new LinkedList<Asteroid>();

            damageParticleSystem = new Particle3D.ExplosionParticleSystem(game, game.Content, 4, Color.White);
            explosionParticleSystem = new Particle3D.ExplosionParticleSystem(game, game.Content, 8, Color.White);
            explosionSmokeParticleSystem = new Particle3D.ExplosionSmokeParticleSystem(game, game.Content, 5, Color.White);
            game.Components.Add(damageParticleSystem);
            game.Components.Add(explosionParticleSystem);
            game.Components.Add(explosionSmokeParticleSystem);
        }

        public void AddBackgroudAsteroids(int number, int range, int depth, int centerX, int centerY)
        {
            for (int i = 0; i < number; i++)
            {
                AddAsteroid(new Vector3(centerX, centerY, depth), 0, range, Vector3.Zero, random.Next(10, 100), 1);
            }
        }

        public void AddAsteroids(int number, int innerRange, int outerRange, int centerX, int centerY)
        {
            for (int i = 0; i < number; i++)
            {
                AddAsteroid(new Vector3(centerX, centerY, 0), innerRange, outerRange, Vector3.Zero, random.Next(10, 50), 1);
            }
        }

        public void AddAsteroid(Vector3 position, int innerRange, int outerRange, Vector3 velocity, float hitPoint, float armor)
        {
            Asteroid newAsteroid = new Asteroid(game);
            newAsteroid.Initialize(
                game.utilities.RandomRingArea(innerRange, outerRange) + position,
                velocity,
                hitPoint,
                armor);

            asteroids.AddLast(newAsteroid);
            
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids.ElementAt(i).Update(gameTime);
            }
        }

        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids.ElementAt(i).Draw(graphics, gameTime);
            }

            Matrix View = game.uIManager.GlobalCamera.View;
            Matrix Projection = game.uIManager.GlobalCamera.Projection;
            damageParticleSystem.SetCamera(View, Projection);
            explosionParticleSystem.SetCamera(View, Projection);
            explosionSmokeParticleSystem.SetCamera(View, Projection);
        }

        public void Scatter(Asteroid asteroid)
        {
            asteroids.Remove(asteroid);

            if (asteroid.MaxHitpoint > 5)
            {
                int num = random.Next(4, 7);
                for (int i = 0; i < num; i++)
                {
                    AddAsteroid(asteroid.Position, 0, 5, 
                        //Vector3.Zero,
                        asteroid.RandomVector(-20, 20) + asteroid.CurrentVelocity / (asteroid.MaxHitpoint * 5) + asteroid.CurrentVelocity * 1.5f,
                        (asteroid.MaxHitpoint / 2 + asteroid.RandomNumber(-4, 4)) / (i+1),
                        1);
                }
            }

        }
    }
}
