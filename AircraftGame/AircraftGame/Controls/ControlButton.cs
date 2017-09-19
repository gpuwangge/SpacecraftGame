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
    public class ControlButton : ControlImage
    {
        private Rectangle rectClicked;
        protected Rectangle RectClicked{
            get { return rectClicked; }
            set { rectClicked = value; }
        }
        Texture2D HighTex2D;

        public ControlButton(SpaceGame game) : base(game){
            this.IsVisible = true;

            this.text = null;
            this.rectClicked = DefaultRectSource;
        }

        public void LoadContent(string contentName){
            LoadContent(contentName, game.utilities.ui_screen_medium, "",
                game.utilities.whiteBtnTextColor, game.utilities.whiteBtnTextHighColor, game.utilities.whiteBtnTextImageColor);
            Initialize();
        }   

        public override void LoadContent(string texName, SpriteFont font, string text, 
                Color textColor, Color textHightColor, Color imageColor){//load first
            HighTex2D = game.Content.Load<Texture2D>("ButtonBlue3");

            if (text != null) if (texName != null) Texture2D = game.Content.Load<Texture2D>(texName);
            else if (texName != null) Texture2D = game.Content.Load<Texture2D>(texName);
            
            base.LoadContent(texName, font, text, textColor, textHightColor, imageColor);
        }

        public override void Initialize(){}

        public override void Update(GameTime gameTime, Point mousePos){
            if (CheckButton(mousePos))  isMouseInside = true;
            else isMouseInside = false;

            if (isMouseInside && !lastIsMouseInside)
                game.soundBank.PlayCue("Highlight");
            
            lastIsMouseInside = isMouseInside;
        }

        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch){
            if (base.IsVisible){
                if (Texture2D != null){
                    if (!isMouseInside){
                        spriteBatch.Draw(Texture2D, RenderRect,
                                null, imageColor, 0, new Vector2(0, 0), 0, 0);
                    }
                }
                if (isMouseInside){
                    spriteBatch.Draw(HighTex2D, RenderRect,
                            null, imageColor, 0, new Vector2(0, 0), 0, 0);
                }

                if (text != null && font != null){
                    Color textColor0 = isMouseInside ? textColorHighlight : textColor;
                    if (textAlign == TextAlign.MiddleCenter)
                        spriteBatch.DrawString(font, text, new Vector2(RenderRect.X + RenderRect.Width / 2 - font.MeasureString(text).X / 2, RenderRect.Y + RenderRect.Height / 2 - font.MeasureString(text).Y / 2),
                            textColor0, 0, new Vector2(0, 0), 1.0f, 0, 0);
                    else if (textAlign == TextAlign.MiddleLeft)
                        spriteBatch.DrawString(font, text, new Vector2(RenderRect.X + RenderRect.Width / 2 - font.MeasureString(text).Y / 2),
                            textColor0, 0, new Vector2(0, 0), 1.0f, 0, 0);
                }
            }
        }
    }



}
