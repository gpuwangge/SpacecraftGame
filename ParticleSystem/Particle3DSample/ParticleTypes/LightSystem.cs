using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Particle3D
{
    /*Not used anymore*/
    public class LightSystem : ParticleSystem
    {
        public LightSystem(Game game, ContentManager content, float size0, Color color0, float duration0)
            : base(game, content)
        {
            Size = size0;
            Color = color0;
            Duration = duration0;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "sphere";
            settings.MaxParticles = 200;
            settings.Duration = TimeSpan.FromSeconds(Duration);

            settings.BlendState = BlendState.AlphaBlend;

            settings.MinXVelocity = -0;
            settings.MaxXVelocity = 0;

            settings.MinYVelocity = -0;
            settings.MaxYVelocity = 0;

            settings.MinZVelocity = -0;
            settings.MaxZVelocity = 0;

            settings.Gravity = new Vector3(0, 0, 0);

            Color.A = 0;
            settings.MinColor = Color;
            settings.MaxColor = Color;

            settings.EndVelocity = 0;

            settings.MinRotateSpeed = -0;
            settings.MaxRotateSpeed = 0;

            settings.MinStartSize = Size;
            settings.MaxStartSize = Size;

            settings.MinEndSize = Size / 10;
            settings.MaxEndSize = Size / 10;
        }
    }


}
