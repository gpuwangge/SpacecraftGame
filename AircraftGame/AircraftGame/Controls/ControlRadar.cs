using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameSpace
{
    public class ControlRadar : ControlImage
    {
        public LinkedList<PointStruct> Points = new LinkedList<PointStruct>();

        private Texture2D PointTexture;
        public Vector3 RadarCenterPos;

        public float scale;

        public float pointerSizeFlight = 10;
        public float pointerSizeAsteroid = 1;

        public struct PointStruct
        {
            public Vector3 Pos;
            public Vector2 Size;
            public RelationEnum Relation;
            public PointStruct(Vector3 pos, Vector2 size, RelationEnum relation)
            {
                Pos = pos;
                Size = size;
                Relation = relation;
            }
        };

        public ControlRadar(SpaceGame game)
            : base(game)
        {
            this.IsVisible = true;
            this.text = null;
            this.textColor = WhiteTextColor;
            this.textAlign = TextAlign.MiddleLeft;
            color = Color.White;
        }

        public override void LoadContent(string texName, SpriteFont font, string text, Color textColor, 
            Color textHightColor, Color imageColor)//load first
        {
            if (text != null)
            {
                Texture2D = game.Content.Load<Texture2D>(texName);
                Vector2 RenderSize = new Vector2(font.MeasureString(text).X, font.MeasureString(text).Y);
                RenderRect = new Rectangle(0, 0, (int)RenderSize.X, (int)RenderSize.Y);

            }
            else
            {
                Texture2D = game.Content.Load<Texture2D>(texName);
           }

            PointTexture = game.Content.Load<Texture2D>("Point");

            base.LoadContent(texName, font, text, textColor, textHightColor, imageColor);
        }



        public override void Initialize()//init later
        {
            UpdateScale();
        }

        public void UpdateScale()
        {
            scale = -(game.uIManager.GlobalCamera.DesiredEye.Z / 20);
        }

        public override void Update(GameTime gameTime, Point mousePos)
        {
            UpdateScale();

            Points.Clear();
            Vector3 Player1Pos = ((Aircraft)(game.gameLevel1.pilots.GetAircraft(0))).Position;
            foreach (Pilot pilot in game.gameLevel1.pilots.m_Pilots)
            {
                Aircraft aircraft = pilot.aircraft;
                if (aircraft.Visible == true && aircraft.Destroyed == false)
                {
                    Vector3 RenderPos = (Player1Pos - aircraft.Position) / scale;
                    if (RenderPos.Length() < RenderRect.Width / 2)
                    {
                        Points.AddLast(new PointStruct(
                            RadarCenterPos + RenderPos,
                            new Vector2(pointerSizeFlight, pointerSizeFlight),
                            aircraft.Relation));
                    }
                }
            }
            foreach (Asteroid asteroid in game.gameLevel1.asteroids.asteroids)
            {
                if (asteroid.Visible == true && asteroid.Destroyed == false)
                {
                    Vector3 RenderPos = (Player1Pos - asteroid.Position) / scale;
                    if (RenderPos.Length() < RenderRect.Width / 2)
                    {
                        Points.AddLast(new PointStruct(
                               RadarCenterPos + RenderPos,
                                new Vector2(pointerSizeAsteroid, pointerSizeAsteroid),
                                asteroid.Relation));
                    }
                }
            }
        }


        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
            if (base.IsVisible)
            {
                Color colorAlpha = color;

                colorAlpha.A = 100;
                spriteBatch.Draw(Texture2D, RenderRect,
                       null, colorAlpha, 0, new Vector2(0, 0), 0, 0);

                foreach (PointStruct point in Points)
                {
                    colorAlpha = game.utilities.GetMarkerColor(point.Relation);
                    colorAlpha.A = 0;
                    spriteBatch.Draw(PointTexture, new Rectangle((int)point.Pos.X, (int)point.Pos.Y, (int)point.Size.X, (int)point.Size.Y),
                           null, colorAlpha, 0, new Vector2(0, 0), 0, 0);

                }
            }

        }

    }
}
