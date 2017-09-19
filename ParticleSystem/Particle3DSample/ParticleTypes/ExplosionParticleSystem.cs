using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Particle3D
{
    public class ExplosionParticleSystem : ParticleSystem
    {
        
        public ExplosionParticleSystem(Game game, ContentManager content, float size0, Color color0)
            : base(game, content)
        {
            Size = size0;
            Color = color0;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "explosion";
            settings.MaxParticles = 100;
            settings.Duration = TimeSpan.FromSeconds(2.0f);
            settings.DurationRandomness = 1;

            settings.BlendState = BlendState.AlphaBlend;
 
            settings.MinXVelocity = -1;
            settings.MaxXVelocity = 1;

            settings.MinYVelocity = -1;
            settings.MaxYVelocity = 1;

            settings.MinZVelocity = -1;
            settings.MaxZVelocity = 1;

            settings.Gravity = new Vector3(0, 0, 0);

            Color.A = 0;
            settings.MinColor = Color;
            settings.MaxColor = Color;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = Size;
            settings.MaxStartSize = Size;

            settings.MinEndSize = Size / 10;
            settings.MaxEndSize = Size / 10;

            settings.EndVelocity = 0;

            // Use additive blending.
          //  settings.SourceBlend = Blend.SourceAlpha;
          //  settings.DestinationBlend = Blend.One;
        }
        
    }
}
