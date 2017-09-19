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
    public class Pilot
    {
        public SpaceGame game;

        public int PilotIndex;

        string operaterName;
        public String OperaterName { get { return operaterName; } set { operaterName = value; } }

        public SourceGroup sourceGroup;

        public float PI = 3.1415926f;
        public Random random = new Random();

        public TeamRole teamRole = TeamRole.LEADER;
        public int[] teamMember = { };/*For leader, teamMember is his followers; For follower, teamMember is leader*/

        public Aircraft aircraft;

        /*AI*/
        public int TargetIndex = -1;/*No target aimed*/
        //public MoveState moveState = MoveState.DONE;
        //public CommandState commandState = CommandState.SENDCOMMAND;
        public List<float> hatred = new List<float>();
        public Formation formation = Formation.FREE;
        public AIState aiState = AIState.FORMING;
        public Vector3 DestinationPos = Vector3.Zero;/*Leader commands his ship to this point*/
        public Vector3 DestinationVel = Vector3.Zero;

        public Vector3 targetPos = Vector3.Zero;

        public bool inFollowState = false;

        public Pilot(SpaceGame game)
        {
            this.game = game;
        }

        public virtual void Initialize(TeamRole teamRole, int[] teamMember) { }
        public virtual void AddAircraft(Aircraft aircraft, Vector3 location, RelationEnum relation, Pilots pilots) {}
        public virtual void Update(GameTime gameTime, bool isPaused, bool isInEquip) {
            if (!isPaused && !isInEquip) aircraft.Update(gameTime);
        }

        public virtual void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            if (aircraft != null)
                aircraft.Draw(graphics, gameTime);
        }

        public virtual void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
        }

    }
}
