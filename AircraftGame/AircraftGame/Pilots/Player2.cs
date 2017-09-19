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
    public class Player2 : Pilot
    {
        public Player2(SpaceGame game)
            : base(game)
        {
        }

        public override void Initialize(TeamRole teamRole, int[] teamMember)
        {
        }

        public override void AddAircraft(Aircraft aircraft, Vector3 location, RelationEnum relation, Pilots pilots)
        {
        }

        public override void Update(GameTime gameTime, bool isPaussed, bool isInEquip)
        {
        }
    
    }
}
