using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Particle3D
{
    public class ExplosionSmokeParticleSystem : ParticleSystem
    {
       
        public ExplosionSmokeParticleSystem(Game game, ContentManager content, float size0, Color color0)
            : base(game, content)
        {
            Size = size0;
            Color = color0;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "smoke";
            settings.MaxParticles = 100;
            settings.Duration = TimeSpan.FromSeconds(4);
            settings.DurationRandomness = 1;

            settings.MinXVelocity = -30;
            settings.MaxXVelocity = 30;

            settings.MinYVelocity = -30;
            settings.MaxYVelocity = 30;

            settings.MinZVelocity = -30;
            settings.MaxZVelocity = 30;

            settings.Gravity = new Vector3(0, 0, 0);

            settings.EndVelocity = 0;

            settings.MinColor = Color;// Color.DarkGray;
            settings.MaxColor = Color;// Color.Gray;

            settings.MinRotateSpeed = -2;
            settings.MaxRotateSpeed = 2;

            settings.MinStartSize = Size;
            settings.MaxStartSize = Size;

            settings.MinEndSize = Size * 10;
            settings.MaxEndSize = Size * 20;

            // Use additive blending.
           // settings.SourceBlend = Blend.SourceAlpha;
          //  settings.DestinationBlend = Blend.One;
        }
    }
}
