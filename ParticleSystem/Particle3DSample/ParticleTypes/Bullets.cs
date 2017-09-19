using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Particle3D
{
    class Bullets
    {
        const int numExplosionParticles = 30;
        const int numExplosionSmokeParticles = 50;
        const float projectileLifespan = 1.5f;
        const float sidewaysVelocityRange = 30;
        const float verticalVelocityRange = 30;

        ParticleSystem explosionParticleSystem;
        ParticleSystem explosionSmokeParticleSystem;

        Vector3 position;
        Vector3 velocity;
        float age;

        static Random random = new Random();

        public Bullets(ParticleSystem explosionParticleSystem,
                                    ParticleSystem explosionSmokeParticleSystem)
        {
            this.explosionParticleSystem = explosionParticleSystem;
            this.explosionSmokeParticleSystem = explosionSmokeParticleSystem;

            position = Vector3.Zero;

            velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
            velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
        }

        /// <summary>
        /// Updates the projectile.
        /// </summary>
        public bool Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Simple projectile physics.
            position += velocity * elapsedTime;
            velocity.Y -= elapsedTime;
            age += elapsedTime;

            // Update the particle emitter, which will create our particle trail.
            //trailEmitter.Update(gameTime, position);

            // If enough time has passed, explode! Note how we pass our velocity
            // in to the AddParticle method: this lets the explosion be influenced
            // by the speed and direction of the projectile which created it.
            if (age > projectileLifespan)
            {
                for (int i = 0; i < numExplosionParticles; i++)
                    explosionParticleSystem.AddParticle(position, velocity);

                for (int i = 0; i < numExplosionSmokeParticles; i++)
                    explosionSmokeParticleSystem.AddParticle(position, velocity);

                return false;
            }

            return true;
        }
    }

}
