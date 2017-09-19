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
    public class AIBasic1 : AIPilot
    {
        public AIBasic1(SpaceGame game)
            : base(game)
        {
        }

        public override void Initialize(TeamRole teamRole, int[] teamMember)
        {
            OperaterName = "AI Basic 1";
            this.sourceGroup = SourceGroup.AI;
            this.teamRole = teamRole;
            if (teamRole == TeamRole.LEADER) aiState = AIState.FREE;
            if (teamRole == TeamRole.FOLLOWER) aiState = AIState.FREE;
            this.teamMember = teamMember;
        }
        public override void AddAircraft(Aircraft aircraft, Vector3 location, RelationEnum relation, Pilots pilots)
        {
            formation = Formation.TRIANGLE;

            aircraft.pilotIndex = PilotIndex;
            aircraft.sourceGroup = sourceGroup;
            
            aircraft.Position = location;
            aircraft.Orientation = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -PI / 2);

            aircraft.Relation = relation;     
            pilots.AddPilot(this);

            aircraft.AddSmoke();

            /*Add Initial Weapon(s)*/
            aircraft.AddWeapon(0, WeaponCode.LASER, 0);

            this.aircraft = aircraft;
        }
        public override void Update(GameTime gameTime, bool isPaused, bool isInEquip)
        {
            if (aircraft.Destroyed) 
                return;

            GetThisInfo();

            ComputeFormationSlot();
            
            TargetIndex = GetTargetInfo(AILookingForTarget());/*Looking for the nearest target*/

            AutoDodge();

            switch (teamRole)
            {
                case TeamRole.LEADER:
                    if (TargetIndex < 0)
                    {
                        CommandTeamFormation();
                        aircraft.UserThrust = 0;
                        aircraft.CurrentTurnRate = 0;
                        base.Update(gameTime, isPaused, isInEquip);
                        return;
                    }

                    FireWeapon(0);

                    switch (aiState)
                    {
                        case AIState.FORMING:/* > mediate400 */
                            float maxSpeed = aircraft.MaxThrust / aircraft.DragCoff;
                            bool allFollowed = CheckTeamFollowed();
                            if (allFollowed)
                                AIControlThrust(maxSpeed / 2.0f);
                            else
                                AIControlThrust(maxSpeed / 10.0f);
                            AITurnToTarget(targetAndThisAngle);
                            CommandTeamFormation();
                            if ((targetLen <= mediateDistance) || (allFollowed && targetLen <= longDistance))
                                ChangeState(AIState.APPROACH);
                            break;
                        case AIState.AWAY:/* <long600 */
                            aircraft.UserThrust = aircraft.CurrentThrust;
                            AwayStrategy();
                            CommandTeamFollow(false);
                            if (targetLen > longDistance || stateTime > 20) ChangeState(AIState.FORMING);
                            break;
                        case AIState.APPROACH: /* > collision50*/
                            aircraft.UserThrust = aircraft.CurrentThrust;
                            AITurnToTarget(targetAndThisAngle);
                            CommandTeamAttack();
                            if (targetLen <= shortDistance) ChangeState(AIState.AWAY);
                            break;
                        case AIState.FREE:
                            if (targetLen <= shortDistance) ChangeState(AIState.AWAY);
                            else if (targetLen <= mediateDistance) ChangeState(AIState.APPROACH);
                            else ChangeState(AIState.FORMING);
                            break;
                    }
                    break;
                case TeamRole.FOLLOWER:
                    GetLeaderInfo(teamMember[0]);
                    if (leaderAircraft.Destroyed) { ChangeState(AIState.FREE); ChangeRole(TeamRole.INDEPENDENT); }
                    if (leaderTargetIndex >= 0) FireWeapon(0);                
                    switch (aiState)
                    {
                        case AIState.FORMING:
                            AIMoveTo(DestinationPos, DestinationVel, true);
                            break;
                        case AIState.FOCUSTARGET:

                            targetDisplacement = AIComputeTargetDisplacement(leaderTargetPos);
                            targetAndThisAngle = AIComputeObjectAngle(targetDisplacement);

                            aircraft.UserThrust = aircraft.CurrentThrust;
                            AITurnToTarget(targetAndThisAngle);

                            break;
                        case AIState.FOLLOWLEADER:
                            AIMoveTo(DestinationPos, DestinationVel, false);
                            break;
                    }
                    break;
                case TeamRole.INDEPENDENT:
                    FireWeapon(0);
                    switch (aiState)
                    {
                        case AIState.AWAY:
                            aircraft.UserThrust = aircraft.CurrentThrust;
                            AwayStrategy();
                            if (targetLen > mediateDistance) ChangeState(AIState.APPROACH);
                            break;
                        case AIState.APPROACH:
                            aircraft.UserThrust = aircraft.CurrentThrust;
                            AITurnToTarget(targetAndThisAngle);
                            if (targetLen <= collisionDistance * 2) ChangeState(AIState.AWAY);
                            break;
                        case AIState.FREE:
                            if (targetLen <= collisionDistance * 2) ChangeState(AIState.AWAY);
                            else ChangeState(AIState.APPROACH);
                            break;
                    }
                    break;
            }


            stateTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //System.Diagnostics.Trace.WriteLine(stateTime.ToString());

            base.Update(gameTime, isPaused, isInEquip);
        }

        public int AILookingForTarget()
        {
            //Aircrafts air = game.gameLevel1.aircrafts;
            Pilots pilots = game.gameLevel1.pilots;
            Aircraft thisAir = this.aircraft;// game.gameLevel1.aircrafts.m_Aircrafts.ElementAt(AircraftIndex);

            //while (hatred.Count < air.m_Aircrafts.Count)
            while (hatred.Count < pilots.Count()) hatred.Add(-1);

            float maxValue = 0;
            int maxIndex = -1;

            //for (int i = 0; i < air.m_Aircrafts.Count; i++)
            for (int i = 0; i < pilots.Count(); i++)
            {
                Aircraft thatAir = pilots.GetAircraft(i);

                if (game.utilities.CheckAgainst(thisAir.Relation, thatAir.Relation) && thatAir.Destroyed == false)
                {
                    Vector2 targetDisplacement = AIComputeTargetDisplacement(thatAir.Position);
                    float targetLen = targetDisplacement.Length();
                    if (targetLen <= 4000)
                        hatred[i] = 2000.0f / targetLen;
                    else
                        hatred[i] = -1;

                    if (hatred.ElementAt(i) > maxValue)
                    {
                        maxValue = hatred.ElementAt(i);
                        maxIndex = i;
                    }

                }
            }

            return maxIndex;
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            base.Draw(graphics, gameTime);
        }
        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
        }

        public void AwayStrategy()
        {
            /*If short distance,diviate direction pi/2*/
            /*If mid distance, diviate direction pi*/
            Vector3 Dir = targetPos - thisPos;
            Dir.Normalize();

            if (targetLen < collisionDistance) /*Short distance*/
            {
                Vector2 DirOrtho = new Vector2(0 * Dir.X - 1 * Dir.Y, 1 * Dir.X + 0);

                float dis2 = (-Dir.X * targetVel.X - Dir.Y * targetVel.Y);/*tarVel dot dir*/
                Vector3 orthogonalVec2 = thisVel - Dir * dis2;

                float dis3 = DirOrtho.X * orthogonalVec2.X + DirOrtho.Y * orthogonalVec2.Y; /*target go left or right*/

                int sign;
                if (dis3 > 0) sign = -1;
                else sign = 1;

                float modifiedTargetAngle = PI / (targetLen / 30.0f);
                if (modifiedTargetAngle > PI) modifiedTargetAngle = PI;
                modifiedTargetAngle *= sign;
                modifiedTargetAngle += targetAndThisAngle;

                if (AIIsAimedTarget(modifiedTargetAngle))
                {
                    aircraft.CurrentTurnRate = 0;
                }
                else
                {
                    AITurnToTarget(modifiedTargetAngle);
                }
            }
            else/*Long distance*/
            {
                float oppositeAngle = PI + targetAndThisAngle;
                if (oppositeAngle > 2 * PI) oppositeAngle -= 2 * PI;
                AITurnToTarget(oppositeAngle);
            }
        }

        public void FireWeapon(int weaponIndex)
        {
            Weapon weapon = aircraft.weaponSlot.slots.ElementAt(weaponIndex).weapon;
            if (AIIsAimedTarget(targetAndThisAngle) && targetLen <= weapon.Range * 0.8f) aircraft.FireWeapon(0);
        }
    }
}
