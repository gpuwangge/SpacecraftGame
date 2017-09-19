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
    public class ControlInformation : ControlImage
    {
        float currentTextX = 10;
        float currentTextY = 10;

        float textHit = 20;
        int maxLines = 9; /*(200-20)/20=9*/

        List<string> drawLines = new List<string>();
        List<Color> drawColors = new List<Color>();

        public float totalTime = 0;

        public ControlInformation(SpaceGame game)
            : base(game)
        {
            this.IsVisible = true;
            this.textAlign = TextAlign.MiddleLeft;
        }

        public override void LoadContent(string texName, SpriteFont font, string text,
            Color textColor, Color textHightColor, Color imageColor)
        {
            Texture2D = game.Content.Load<Texture2D>(texName);
            base.LoadContent(texName, font, text, textColor, textHightColor, imageColor);
        }

        public void AddText(string newText, Color color)
        {
            text = "[" + totalTime.ToString("G3") + "]: " +  newText;
            textColorHighlight = color;
        }

        public override void Update(GameTime gameTime, Point mousePos)
        {
            //count++;           
            //if (count == 50)
            //{
            //    AddText("new text mmmmmmmmmmmmmmmmmmxxxxxxxxxxxxxxxxxxxxxyyyyyyyyyyyyyyyyyyyyy");
            //    count = 0;
            //}

            totalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
            if (!base.IsVisible) return;

            /*Draw Background*/
            if (Texture2D != null) spriteBatch.Draw(Texture2D, RenderRect, null, imageColor, 0, new Vector2(0, 0), 0, 0);

            if (text == null || font == null) return;

            /*Decode the incomming text into new lines and oldLines*/
            int drawLineCount = drawLines.Count;
            while (text != "")
            {
                int textCount = 0;
                drawLines.Add("");
                drawColors.Add(textColorHighlight);

                while ((font.MeasureString(drawLines[drawLineCount]).X < RenderRect.Width - 40) && (textCount < text.Length))
                {
                    drawLines[drawLineCount] += text[textCount++];
                }
                text = text.Remove(0, textCount);

                drawLineCount++;
            }

            /*Print the lines*/
            int startIndex = drawLineCount >= maxLines ? drawLineCount - maxLines : 0;

            for (int i = startIndex; i < drawLineCount; i++)
            {
                spriteBatch.DrawString(font, drawLines[i],
                    new Vector2(currentTextX + RenderRect.X, currentTextY + textHit * (i - startIndex) + RenderRect.Y),
                    drawColors[i], 0, new Vector2(0, 0), 1.0f, 0, 0);
            }

        }

    }
}
