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
    public class AIPilot : Pilot
    {
        public float teamDist = 100;
        public Vector3[] formationSlot = new Vector3[9]; /*Max 9 members and 1 leader*/

        public float minDis = 1.0f;/*aircraft stops if it stays inside*/
        public float minSpeed = 0.1f;/*aircraft stops if it has this speed*/

        public float stateTime = 0;

        public float collisionDistance = 50;
        public float shortDistance = 200;
        public float mediateDistance = 400;
        public float longDistance = 600;      

        public AIPilot(SpaceGame game)
            : base(game)
        {
        }

        public override void Initialize(TeamRole teamRole, int[] teamMember) {
            base.Initialize(teamRole, teamMember);
        }
        public override void AddAircraft(Aircraft aircraft, Vector3 location, RelationEnum relation, Pilots pilots) {
            base.AddAircraft(aircraft, location, relation, pilots);
        }
        public override void Update(GameTime gameTime, bool isPaused, bool isInEquip) {
            base.Update(gameTime, isPaused, isInEquip);
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            base.Draw(graphics, gameTime);
        }

        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
        }

        public Vector3 RotateOrientation(Vector3 p)
        {
            Matrix m = Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), -game.utilities.ComputeTheAngleFromQuaternion(aircraft.Orientation));
            return Vector3.Transform(p, m);
        }

        public void ComputeFormationSlot()
        {
            switch (formation)
            {
                case Formation.LINE:
                    formationSlot[0] = RotateOrientation(new Vector3(0, 50, 0));
                    formationSlot[1] = RotateOrientation(new Vector3(20, 50, 0));
                    formationSlot[2] = RotateOrientation(new Vector3(-20, 50, 0));
                    formationSlot[3] = RotateOrientation(new Vector3(40, 50, 0));
                    formationSlot[4] = RotateOrientation(new Vector3(-40, 50, 0));
                    formationSlot[5] = RotateOrientation(new Vector3(60, 50, 0));
                    formationSlot[6] = RotateOrientation(new Vector3(-60, 50, 0));
                    formationSlot[7] = RotateOrientation(new Vector3(80, 50, 0));
                    formationSlot[8] = RotateOrientation(new Vector3(-80, 50, 0));
                    break;
                case Formation.TRIANGLE:
                    formationSlot[0] = RotateOrientation(new Vector3(0, 80, 0));
                    formationSlot[1] = RotateOrientation(new Vector3(20, 60, 0));
                    formationSlot[2] = RotateOrientation(new Vector3(-20, 60, 0));
                    formationSlot[3] = RotateOrientation(new Vector3(40, 40, 0));
                    formationSlot[4] = RotateOrientation(new Vector3(-40, 40, 0));
                    formationSlot[5] = RotateOrientation(new Vector3(60, 20, 0));
                    formationSlot[6] = RotateOrientation(new Vector3(-60, 20, 0));
                    formationSlot[7] = RotateOrientation(new Vector3(80, 0, 0));
                    formationSlot[8] = RotateOrientation(new Vector3(-80, 0, 0));
                    break;
            }
        }


        public bool AIMoveTo(Vector3 pos, Vector3 vel, bool useStrict)
        {
            /*Get Info*/
            Vector2 disp = AIComputeTargetDisplacement(pos);
            float len = disp.Length();
            float angle = AIComputeObjectAngle(disp);

            /*Turn to destination location*/
            AITurnToTarget(angle);

            /*Get Info 2*/
            float maxSpeed = aircraft.MaxThrust / aircraft.DragCoff;          
            int longDis = 200;/*Control Thrust*/
            int midDis = 50;
            int shortDis = 20;/*Control Speed*/
            float scale = len / (float)longDis;
            if (scale < 0.05f) scale = 0.05f;

            if (inFollowState)
            {
                if (len > midDis)
                {
                    inFollowState = false;
                }
                else
                {
                    if (useStrict)
                    {
                        Vector3 DisToSlot = DestinationPos - thisPos;
                        if (DisToSlot != Vector3.Zero) DisToSlot.Normalize();
                        AIControlVelocity(vel + DisToSlot * maxSpeed / 10.0f);
                        AITurnToTarget(game.utilities.ComputeTheAngleFromQuaternion(leaderAircraft.Orientation));
                    }
                    else
                        AIControlThrust(scale * maxSpeed);
                }

                return true;
            }
            
            /*Control Speed and Thrust*/
            if (len >= longDis)
            {
                AIControlThrust(maxSpeed);
            } 
            else if (len >= midDis && len < longDis)//50~200
            {
                AIControlThrust(scale * maxSpeed);
            }
            else if (len >= shortDis && len < midDis)
            {
                if (useStrict)
                {
                    AIControlSpeed(0);
                    AIControlSpeed(scale * maxSpeed);
                    aircraft.CurrentVelocity += vel;
                }
                else
                    AIControlThrust(scale * maxSpeed);
            }
            else if (len > minDis && len < shortDis)
            {
                if (useStrict)
                {
                    AIControlSpeed(0);
                    AIControlSpeed(scale * maxSpeed);
                    aircraft.CurrentVelocity += vel;
                }
                else
                    AIControlThrust(scale * maxSpeed);
            }else if (len <= minDis){
                inFollowState = true;
            }
            
            return false;

        }

        public bool WithIn(float value, float coff)
        {
            if (Math.Abs(value) < coff)
                return true;
            return false;
        }

        public bool AIControlSpeed(float destinationSpeed)
        {
            float scale = 0;
            if (aircraft.Speed > 1)
            {
                scale = destinationSpeed / aircraft.Speed;
                aircraft.CurrentVelocity.Normalize();
                aircraft.CurrentVelocity *= scale;
            }
            else
            {
                Vector3 directedThrust = Vector3.TransformNormal(new Vector3(0, 0, 1), Matrix.CreateFromQuaternion(aircraft.Orientation));
                aircraft.CurrentVelocity = directedThrust * destinationSpeed;
            }
            aircraft.UserThrust = 0;
            aircraft.Speed = aircraft.CurrentVelocity.Length();
            return true;
        }

        public bool AIControlVelocity(Vector3 destinationVelocity)
        {
            aircraft.CurrentVelocity = destinationVelocity;
            aircraft.UserThrust = 0;
            return true;
        }

        public bool AIControlThrust(float destinationSpeed)
        {
            float speed = aircraft.Speed;

            if (WithIn(speed - destinationSpeed, minSpeed))
                return true;

            if (speed < destinationSpeed)
            {
                //while (speed < (destinationSpeed))
                //{
                //    if (aircraft.IncreaseThrust()) break;
                //}
                aircraft.IncreaseThrust();
            }
            else if (speed > destinationSpeed)
            {
                while (speed > (destinationSpeed))
                {
                    if (aircraft.DecreaseThrust()) break;
                }
            }

            return false;

        }

        public void AITurnToTarget(float targetAngle)
        {
            float angdiff = Math.Abs(thisAngle - targetAngle);
            if (angdiff < aircraft.MaxTurnRate / 1000.0f) //AIIsAimedTarget
                aircraft.CurrentTurnRate = 0;
            if ((thisAngle < targetAngle && angdiff < PI) || (thisAngle > targetAngle && angdiff > PI))
            {
                aircraft.CurrentTurnRate = aircraft.MaxTurnRate / 2.0f;//turn left
            }
            else
            {
                aircraft.CurrentTurnRate = -aircraft.MaxTurnRate / 2.0f;//turn right
            }
        }

        /*GetThisInfo*/
        public float thisAngle = 0;
        public Vector3 thisPos = Vector3.Zero;
        public Vector3 thisVel = Vector3.Zero;
        public void GetThisInfo()
        {
            thisAngle = game.utilities.ComputeTheAngleFromQuaternion(aircraft.Orientation);
            thisPos = aircraft.Position;
            thisVel = aircraft.CurrentVelocity;
        }

        /*GetTargetInfo*/
        public Aircraft targetAircraft = null;
        public Vector3 targetVel = Vector3.Zero;
        public Vector2 targetDisplacement = Vector2.Zero;
        public float targetLen = 0;
        public float targetAndThisAngle = 0;
        public float targetSize = 0;
        public int GetTargetInfo(int targetIndex)
        {
            if (targetIndex == -1) return -1;

            targetAircraft = game.gameLevel1.pilots.GetAircraft(targetIndex);
            targetPos = targetAircraft.Position;
            targetVel = targetAircraft.CurrentVelocity;
            targetDisplacement = AIComputeTargetDisplacement(targetPos);
            targetLen = targetDisplacement.Length();
            targetAndThisAngle = AIComputeObjectAngle(targetDisplacement);
            targetSize = targetAircraft.Size;

            return targetIndex;
        }

        /*GetTargetInfo*/
        public Aircraft leaderAircraft = null;
        public Vector3 leaderPos = Vector3.Zero;
        public Vector3 leaderVel = Vector3.Zero;
        public Vector2 leaderDisplacement = Vector2.Zero;
        public float leaderLen = 0;
        public float leaderAndThisAngle = 0;
        public Vector3 leaderTargetPos = Vector3.Zero;
        public int leaderTargetIndex = -1;
        //float leaderSize = 0;
        public void GetLeaderInfo(int leaderIndex)
        {
            leaderAircraft = game.gameLevel1.pilots.GetAircraft(leaderIndex);

            leaderPos = leaderAircraft.Position;
            leaderVel = leaderAircraft.CurrentVelocity;
            leaderDisplacement = AIComputeTargetDisplacement(leaderPos);
            leaderLen = leaderDisplacement.Length();
            leaderAndThisAngle = AIComputeObjectAngle(leaderDisplacement);

            leaderTargetPos = game.gameLevel1.pilots.GetPilot(leaderIndex).targetPos;
            leaderTargetIndex = game.gameLevel1.pilots.GetPilot(leaderIndex).TargetIndex;
        }

        public Vector2 AIComputeTargetDisplacement(Vector3 targetPos)
        {
            return new Vector2(
                targetPos.X - aircraft.Position.X,
                targetPos.Y - aircraft.Position.Y);
        }

        public float AIComputeObjectAngle(Vector2 disp)
        {
            ///Compute the angle of the target
            float z = disp.X * disp.X;
            float y = disp.Y * disp.Y;//ScreenPos.X * ScreenPos.X 
            float x = z + y;//+ (ScreenPos.Y + 100) * (ScreenPos.Y + 100);

            float result;
            if (x != 0)
            {
                result = (float)Math.Acos((x + y - z) / (2 * Math.Sqrt(x) * Math.Sqrt(y))); //always 0 ~ PI / 2
                if (disp.Y <= 0)
                    result = PI - result;

                if (disp.X <= 0)
                    result = 2 * PI - result;
            }
            else
                result = 0.0f;
            return result;
        }

        public bool AIIsAimedTarget(float targetAngle)
        {
            float angleThreshHold = aircraft.MaxTurnRate / 1000.0f;//AITurnToTarget
            float angdiff = Math.Abs(thisAngle - targetAngle);
            if (angdiff < angleThreshHold)
                return true;
            else
                return false;
        }

        public void CommandTeamFormation()
        {
            //switch (formation)
            //{
            //    case Formation.LINE:
            int j = 0;
            for (int i = 0; i < teamMember.Count(); i++)
            {
                if (game.gameLevel1.pilots.GetAircraft(teamMember[i]).Destroyed) continue;
                game.gameLevel1.pilots.GetPilot(teamMember[i]).DestinationPos = formationSlot[j++] + thisPos;
                game.gameLevel1.pilots.GetPilot(teamMember[i]).DestinationVel = thisVel;
                game.gameLevel1.pilots.GetPilot(teamMember[i]).aiState = AIState.FORMING;
            }
            //        break;
            //    case Formation.TRIANGLE:
            //        break;
            //}
        }

        public void CommandTeamFollow(bool isForward)
        {
            for (int i = 0; i < teamMember.Count(); i++)
            {
                if(isForward)
                    game.gameLevel1.pilots.GetPilot(teamMember[i]).DestinationPos = thisPos + thisVel * 5;
                else
                    game.gameLevel1.pilots.GetPilot(teamMember[i]).DestinationPos = thisPos + thisVel;
                game.gameLevel1.pilots.GetPilot(teamMember[i]).DestinationVel = thisVel;
                game.gameLevel1.pilots.GetPilot(teamMember[i]).aiState = AIState.FOLLOWLEADER;
            }
        }

        public void CommandTeamAttack()
        {
            for (int i = 0; i < teamMember.Count(); i++)
            {
                game.gameLevel1.pilots.GetPilot(teamMember[i]).aiState = AIState.FOCUSTARGET;
            }
        }

        public bool CheckTeamFollowed()
        {
            for (int i = 0; i < teamMember.Count(); i++)
            {
                if (game.gameLevel1.pilots.GetAircraft(teamMember[i]).Destroyed) continue;
                if (!game.gameLevel1.pilots.GetPilot(teamMember[i]).inFollowState)
                    return false;
            }
            return true;
        }

        public void AutoDodge()
        {
            /*If about to colision, dodge*/
            foreach (Pilot pilot in game.gameLevel1.pilots.m_Pilots)
            {
                MyObject Object1 = pilot.aircraft;
                if (Object1.Destroyed) continue;
                if (game.utilities.CheckAgainst(Object1.Relation, aircraft.Relation))
                {
                    if (targetLen < aircraft.Size + Object1.Size + 20) aircraft.Dodge();
                }
            }
        }

        public void ChangeRole(TeamRole role)
        {
            teamRole = role;
        }

        public void ChangeState(AIState state)
        {
            aiState = state;
            stateTime = 0;
        }

    }
}
