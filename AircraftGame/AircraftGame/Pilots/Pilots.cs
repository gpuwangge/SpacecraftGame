using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameSpace
{
    public class Pilots
    {
        private LinkedList<Pilot> m_pilots = new LinkedList<Pilot>();
        public LinkedList<Pilot> m_Pilots { get { return m_pilots; } }

        //private int pilotsSize = 0;
        //public int PilotsSize { get { return pilotsSize; } set { pilotsSize = value; } }

        private SpaceGame game;

        public Pilots(SpaceGame game)
        {
            this.game = game;
        }

        //public void Initilize()
        //{
        //    foreach (Pilot pilot in m_pilots)
        //    {
        //        pilot.Initialize();
        //    }
        //}

        public void AddPilot(Pilot p)
        {
            m_pilots.AddLast(p);
            p.PilotIndex = m_pilots.Count - 1; //0 should be the player
        }


        public void Update(GameTime gameTime, bool isPaused, bool isInEquip)
        {
            foreach (Pilot pilot in m_pilots)
            {
                pilot.Update(gameTime, isPaused, isInEquip);
            }
        }

        public int Count()
        {
            return m_Pilots.Count;
        }

        public Pilot GetPilot(int index)
        {
            return m_Pilots.ElementAt(index);
        }

        public Aircraft GetAircraft(int index)
        {
            return m_Pilots.ElementAt(index).aircraft;
        }

        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            foreach (Pilot pilot in m_Pilots)
            {
                pilot.Draw(graphics, gameTime);
            }
        }

        public void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
            foreach (Pilot pilot in m_Pilots)
            {
                pilot.DrawSprites(gametime, spriteBatch);
            }
        }
    }
}
