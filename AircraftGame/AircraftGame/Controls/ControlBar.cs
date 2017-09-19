using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameSpace{
    public class Bar : ControlImage{
        public float value;
        public float maxValue;
        float widthPerValue;

        public int MaxWidth;

        public byte alpha1 = 100;
        public byte alpha2 = 50;

        public enum BarStyle{
            LEFT,
            TWOSIDES
        }

        public BarStyle Style = BarStyle.LEFT;

        public Bar(SpaceGame game) : base(game){
            this.IsVisible = true;
            this.text = null;
            this.textAlign = TextAlign.MiddleLeft;

            value = game.utilities.minusInf;
            MaxWidth = 0;

            color = Color.White;
        }
            
        public void LoadGreenContent(string text){
            LoadContent("ButtonGreen", game.utilities.ui_screen_medium, text,
                game.utilities.BarTextColor, game.utilities.BarHighColor, game.utilities.BarImageColor);
            Style = Bar.BarStyle.TWOSIDES;
        }

        public override void LoadContent(string texName, SpriteFont font, string text, 
                Color textColor, Color textHightColor, Color imageColor){//load first
            if (text != null){
                Texture2D = game.Content.Load<Texture2D>(texName);
                Vector2 RenderSize = new Vector2(font.MeasureString(text).X, font.MeasureString(text).Y);
                RenderRect = new Rectangle(0, 0, (int)RenderSize.X, (int)RenderSize.Y);
            }
            else Texture2D = game.Content.Load<Texture2D>(texName);

            base.LoadContent(texName, font, text, textColor, textHightColor, imageColor);
        }

        public void SetMaxValue(float newMaxValue){
            widthPerValue = RenderRect.Width / newMaxValue;
            value = newMaxValue;
            maxValue = newMaxValue;
        }

        public void SetMaxValue(float newMaxValue, int maxWidth, Rectangle renderRect){
            this.MaxWidth = maxWidth;
            this.RenderRect = renderRect;
            SetMaxValue(newMaxValue);
        }

        public void UpdateValue(float newValue){
            value = newValue;
        }

        public override void Initialize(){}

        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
            if (base.IsVisible){
                imageColor.A = alpha1;
                Rectangle currentRect = new Rectangle(RenderRect.X, RenderRect.Y, (int)(value * widthPerValue), RenderRect.Height);
                spriteBatch.Draw(Texture2D, currentRect,
                       null, imageColor, 0, new Vector2(0, 0), 0, 0);

                imageColor.A = alpha2;
                Rectangle maxRect = new Rectangle(RenderRect.X, RenderRect.Y, MaxWidth, RenderRect.Height);
                spriteBatch.Draw(Texture2D, maxRect,
                       null, imageColor, 0, new Vector2(0, 0), 0, 0);


                if (Style == BarStyle.LEFT){
                    string showing = ((int)value).ToString();

                    spriteBatch.DrawString(font, showing, new Vector2(RenderRect.X, RenderRect.Y - 5),
                        textColor, 0, new Vector2(-5, -7), 1.0f, 0, 0);
                }
                else if (Style == BarStyle.TWOSIDES){
                    string showing = " " + text + ": "+((int)value).ToString() + "/" + ((int)maxValue).ToString();

                    //spriteBatch.DrawString(font, showing, new Vector2(RenderRect.X + RenderRect.Width + 10, RenderRect.Y),
                        //textColor, 0, new Vector2(0, 0), 1.0f, 0, 0);
                    spriteBatch.DrawString(font, showing, new Vector2(RenderRect.X, RenderRect.Y - 5),
                        textColor, 0, new Vector2(-5, -7), 1.0f, 0, 0);

                    //spriteBatch.DrawString(font, Title, new Vector2(RenderRect.X - 60, RenderRect.Y),
                        //textColor, 0, new Vector2(0, 0), 1.0f, 0, 0);
                }
            }
        }

    }
}
