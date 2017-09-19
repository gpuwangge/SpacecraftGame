using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

/*Created by Xiaojun Wang*/

namespace GameSpace
{
    class CollisionManager
    {
        SpaceGame game;

        public CollisionManager(SpaceGame game)
        {
            this.game = game;

        }

        public void Update()
        {
            if (game.gameLevel1.isPaused || game.gameLevel1.isInEquip) return;

            /*Bullets and Asteroids*/
            foreach (Bullet Object1 in game.gameLevel1.bullets.m_Bullets)
            {
                if (Object1.Destroyed) continue;

                MyObject Object2 = null;
                foreach (MyObject aObject in game.gameLevel1.asteroids.asteroids)
                {
                    Object2 = TestForCollision(
                        Object1.Position,
                        Object1.CurrentVelocity,
                        Object1.Relation,
                        Object1.Size, aObject);
                    if (Object2 != null) break;
                }

                if (Object2 != null && Object2.Destroyed == false && Object2.Dodged == false)/*Asteroid Hit by Bullet*/
                {
                    Object2.inCollision = true;

                    HitByObject(Object1,
                        Object2, Object2.CurrentVelocity, Object2.Position, Object2.CurrentWeight, Object2.CurrentPower, true);

                    Object1.LifeTime = 0;//Process the weapon, remove the bullet
                    Object2.inCollision = true;
                }
            }



            /*aircraft and asteroid*/
            foreach (Pilot pilot in game.gameLevel1.pilots.m_Pilots)
            {
                pilot.aircraft.inCollision = false;
            }
            foreach (Pilot pilot in game.gameLevel1.pilots.m_Pilots)
            {
                MyObject Object1 = pilot.aircraft;

                if (Object1.Destroyed) continue;
                if (Object1.inCollision) continue;

                MyObject Object2 = null;
                foreach (MyObject aObject in game.gameLevel1.asteroids.asteroids)
                {
                    Object2 = TestForCollision(
                        Object1.Position,
                        Object1.CurrentVelocity,
                        Object1.Relation,
                        Object1.Size, aObject);
                    if (Object2 != null) break;
                }

                if (Object2 != null && Object2.Destroyed == false && Object1.Dodged == false &&
                    Object1.CanCollision == true)///hit
                {
                    HitByObject(Object1, Object2, Object2.CurrentVelocity, Object2.Position, Object2.CurrentWeight, Object2.CurrentPower, true);
                    
                    Object1.inCollision = true;
                    Object2.inCollision = true;

                    Object1.CollisionCooldown = 0;
                }

            }

            /*Aircraft and Aircraft*/
            foreach (Pilot pilot in game.gameLevel1.pilots.m_Pilots)
            {
                pilot.aircraft.inCollision = false;
            }
            foreach (Pilot pilot in game.gameLevel1.pilots.m_Pilots)
            {
                MyObject Object1 = pilot.aircraft;
                if (Object1.Destroyed) continue;
                if (Object1.inCollision) continue;

                MyObject Object2 = null;
                foreach (Pilot aPilot in game.gameLevel1.pilots.m_Pilots)
                {
                    MyObject aObject = aPilot.aircraft;
                    Object2 = TestForCollision(
                        Object1.Position,
                        Object1.CurrentVelocity,
                        Object1.Relation,
                        Object1.Size,
                        aObject);
                    if (Object2 != null) break;
                }

                if (Object2 != null && Object2.Destroyed == false && Object2.Dodged == false && Object1.Dodged == false &&
                    Object1.CanCollision == true && Object2.CanCollision == true)///hit
                {
                    //Object1.HitByAircraft((Aircraft)Object2);///Process the hitted target
                    HitByObject(Object1, Object2, Object2.CurrentVelocity, Object2.Position, Object2.CurrentWeight, Object2.CurrentPower, true);

                    Object1.inCollision = true;
                    Object2.inCollision = true;

                    Object1.CollisionCooldown = 0;
                    Object2.CollisionCooldown = 0;

                }

            }

            /*Bullets and Aircraft*/
            foreach (Bullet Object1 in game.gameLevel1.bullets.m_Bullets)
            {
                if (Object1.Destroyed) continue;

                if (Object1.inCollision == false)
                {
                    MyObject Object2 = null;
                    foreach (Pilot pilot in game.gameLevel1.pilots.m_Pilots)
                    {
                        MyObject aObject = pilot.aircraft;
                        Object2 = TestForCollision(
                            Object1.Position,
                            Object1.CurrentVelocity,
                            Object1.Relation,
                            Object1.Size,
                            aObject);
                        if (Object2 != null) break;
                    }

                    if (Object2 != null && Object2.Destroyed == false && Object1.Dodged == false && Object2.Dodged == false)///hit
                    {
                        HitByObject(Object1, Object2, Object2.CurrentVelocity, Object2.Position, Object2.CurrentWeight, Object2.CurrentPower, true);
                        Object1.LifeTime = 0;//Process the weapon
                        Object2.inCollision = true;
                    }
                }
            }

        }

        public MyObject TestForCollision(Vector3 position, Vector3 velocity, RelationEnum relation, float size, MyObject aObject)
        {
            if (game.utilities.CheckAgainst(aObject.Relation, relation))
            {
                float Radius = aObject.Size + size;
                Vector3 Center = aObject.Position;

                if ((position - Center).LengthSquared() < Radius * Radius)
                {
                    return aObject;
                }
            }

            return null;
        }


        public void HitByObject(MyObject object1, MyObject object2, Vector3 V2, Vector3 P2, float W2, float D2, bool isSource)
        {
            /*First process one side*/
            float W1 = object1.CurrentWeight;
            Vector3 P1 = object1.Position;
            Vector3 V1 = object1.CurrentVelocity;
            float D1 = object1.CurrentPower;

            float damage = D2;

            if (object1.CurrentShieldPoint > 0)
            {
                object1.CurrentShieldPoint -= damage;

                if (object1.CurrentShieldPoint < 0)
                {
                    damage = -object1.CurrentShieldPoint;
                    object1.CurrentShieldPoint = 0;
                }
                else
                {
                    damage = 0;
                }

                object1.DamageShieldAnimation(P2, V2, object2.objectType);/*Call object function*/
            }

            if (damage > 0)
            {
                float damageAfterArmor = damage - object1.CurrentArmor;
                if (damageAfterArmor < 0.1f)
                {
                    damageAfterArmor = 0.1f;
                }

                object1.CurrentHitpoint -= damageAfterArmor;
                if (object1.CurrentHitpoint > 0)
                    object1.DamageHitpointAnimation(P2, V2, object2.objectType);/*Call object function*/
                else
                    object1.DestroyAnimation();/*Call object function*/
            }

            TwinVecter3 result = ComputeCollisionConservation(P1, V1, W1, P2, V2, W2);/*Compute velocitie after collision*/
            object1.CurrentVelocity += result.v1;

            if (isSource)
                HitByObject(object2, object1, V1, P1, W1, D1, false);/*Process the other side*/
            else
                object2.HitSomething(object1);/*Call the owner of object2 and let it know*/


        }

        public struct TwinVecter3
        {
            public Vector3 v1;
            public Vector3 v2;
            public Vector3 p;
            public TwinVecter3(Vector3 x1, Vector3 x2, Vector3 px)
            {
                v1 = x1;
                v2 = x2;
                p = px;
            }
        };

        public TwinVecter3 ComputeCollisionConservation(Vector3 p1, Vector3 vec1, float m1, Vector3 p2, Vector3 vec2, float m2)
        {
            Vector3 collisionDir = p2 - p1;
            Vector3 collision = collisionDir;
            collisionDir.Normalize();

            float v1 = vec1.X * collisionDir.X + vec1.Y * collisionDir.Y + vec1.Z * collisionDir.Z;
            float v2 = vec2.X * collisionDir.X + vec2.Y * collisionDir.Y + vec2.Z * collisionDir.Z;

            float v11 = (v1 * (m1 - m2) + 2 * m2 * v2) / (m1 + m2);
            float v21 = (v2 * (m2 - m1) + 2 * m1 * v1) / (m1 + m2);

            Vector3 r1 = -v1 * collisionDir + v11 * collisionDir;
            Vector3 r2 = -v2 * collisionDir + v21 * collisionDir;

            return new TwinVecter3(r1, r2, collision);
        }

    }
}
