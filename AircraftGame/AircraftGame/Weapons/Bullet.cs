using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GameSpace
{
    public class Bullet : MyObject
    {
        public WeaponType weaponType;

        public Bullet(SpaceGame game) : base(game) { }
        public virtual void Initial() { }
        public virtual void LoadContent(ContentManager content) { }
        public virtual void Update(float delta) { }
        public void Remove() {
            //game.Components.Remove(blast.machineBulletParticleSystem);
            game.gameLevel1.bullets.m_Bullets.Remove(this);
        
        }

        public override void HitSomething(MyObject myObject)
        {
            //if (playerOwner == PlayerGroup.PLAYER1)//the player owns me
            if (sourceGroup == SourceGroup.PLAYER1)
            {
                //game.uICombat.Player1HitIt(myObject, pilotName, aircraftName, maxHitpoint, hitpoint, maxShieldPoint, shieldPoint, maxEnergyPoint, energyPoint);
                game.uICombat.Player1HitIt(myObject);
            }
        }

    } 
}
