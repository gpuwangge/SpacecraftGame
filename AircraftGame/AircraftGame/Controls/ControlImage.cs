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
    public class ControlImage
    {
        public SpaceGame game;

        private Rectangle renderRect;
        public Rectangle RenderRect { get { return renderRect; } set { renderRect = value; } }

        protected Color color;
        public Color Color{
            get { return color; }
            set { color = value; }
        }

        bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        private Texture2D tex2d;
        public Texture2D Texture2D { get { return tex2d; } set { tex2d = value; } }

        public string text;
        protected SpriteFont font;
        public Color textColor;
        public Color textColorHighlight;
        public Color imageColor;
        public enum TextAlign
        {
            MiddleLeft,
            MiddleCenter
        }
        protected TextAlign textAlign = TextAlign.MiddleCenter;


        protected static readonly Vector2 DefaultButtonSize = new Vector2(116, 38);

        protected static readonly Rectangle DefaultRectSource = new Rectangle(0, 0, 64, 24);
        protected static readonly Rectangle DefaultRectHighlight = new Rectangle(64, 0, 64, 24);
        protected static readonly Rectangle DefaulRectClicked = new Rectangle(64, 24, 64, 24);

        protected static readonly Color WhiteTextColor = Color.White;
        protected static readonly Color WhiteTextColorHighlight = Color.White;

        protected static readonly Color BlackTextColor = Color.Black;

        public bool isMouseInside;
        protected bool lastIsMouseInside;

        public event EventHandler MouseEnter;
        internal void OnMouseEnter() { isMouseInside = true; OnMouseEnter(EventArgs.Empty); }
        protected virtual void OnMouseEnter(EventArgs e){
            if (MouseEnter != null)
                MouseEnter(this, e);
        }

        public event EventHandler MouseLeave;
        protected virtual void OnMouseLeave(EventArgs e){
            isMouseInside = false;
            if (MouseLeave != null)
                MouseLeave(this, e);
        }

        internal void OnMouseLeave() { OnMouseLeave(EventArgs.Empty); }

        public ControlImage(SpaceGame game) {
            this.game = game;
        }
        public virtual void Initialize() { }

        public virtual bool CheckButton(Point p)
        {
            float x1 = renderRect.X;
            float x2 = renderRect.X + renderRect.Width;
            float y1 = renderRect.Y;
            float y2 = renderRect.Y + renderRect.Height;

            if ((x1 < p.X) && (p.X < x2) && (y1 < p.Y) && (p.Y < y2))
                return true;

            return false;
        }


        public virtual void Update(GameTime gameTime, Point mousePos) { }
        public virtual void LoadContent(string texName, SpriteFont font, string text, 
            Color textColor, Color textHightColor, Color imageColor) {
                this.font = font;
                this.text = text;
                this.textColor = textColor;
                this.textColorHighlight = textHightColor;
                this.imageColor = imageColor;
            }
        public virtual void DrawSprites(GameTime gametime, SpriteBatch spriteBatch) { }
    }
}
