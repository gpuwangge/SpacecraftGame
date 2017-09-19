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
    public class UIEquipment : UIManager
    {
        ControlButton OperatorPhoto;

        ControlButton btnName;
        ControlButton btnOperator;
        ControlButton btnEmpty;

        ControlButton[] btnSlots;
        int maxInventoryCount = 12;
        ControlButton[] btnInventory;
        int maxLabelCount = 5;
        ControlButton[] btnLables;

        Bar barHP;
        Bar barShield;
        Bar barEnergy;
        Bar barShieldReg;
        Bar barEnergyReg;
        Bar barThrust;
        Bar barTurnRate;
        Bar barWeight;
        Bar barShieldCD;
        Bar barDodgeCD;
        Bar barDodgeDuration;
        Bar barPower;
        Bar barArmor;

        public UIEquipment(SpaceGame game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
            OperatorPhoto = new ControlButton(game);
            btnName = new ControlButton(game);
            btnOperator = new ControlButton(game);
            btnEmpty = new ControlButton(game);

            btnSlots = new ControlButton[9];
            for (int i = 0; i < 9; i++)
                btnSlots[i] = new ControlButton(game);

            btnInventory = new ControlButton[maxInventoryCount];
            for (int i = 0; i < maxInventoryCount; i++)
                btnInventory[i] = new ControlButton(game);

            btnLables = new ControlButton[maxLabelCount];
            for (int i = 0; i < maxLabelCount; i++)
                btnLables[i] = new ControlButton(game);

            barHP = new Bar(game);
            barShield = new Bar(game);
            barEnergy = new Bar(game);
            barShieldReg = new Bar(game);
            barEnergyReg = new Bar(game);
            barThrust = new Bar(game);
            barTurnRate = new Bar(game);
            barWeight = new Bar(game);
            barShieldCD = new Bar(game); 
            barDodgeCD = new Bar(game); 
            barDodgeDuration = new Bar(game); 
            barPower = new Bar(game); 
            barArmor = new Bar(game); 
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            OperatorPhoto.LoadContent("pilot1", null, "", Color.White, Color.White, Color.White);
            btnName.LoadContent("white3");
            btnOperator.LoadContent("white3");
            btnEmpty.LoadContent("white3");

            for (int i = 0; i < 9; i++) btnSlots[i].LoadContent("white3");
            for (int i = 0; i < maxInventoryCount; i++) btnInventory[i].LoadContent("white3");
            for (int i = 0; i < maxLabelCount; i++) btnLables[i].LoadContent("ButtonWhite");

            barHP.LoadGreenContent("HP");
            barShield.LoadGreenContent("Shield");
            barEnergy.LoadGreenContent("Energy");
            barShieldReg.LoadGreenContent("Shield Regeneration");
            barEnergyReg.LoadGreenContent("Energy Regeneration");
            barThrust.LoadGreenContent("Thrust");
            barTurnRate.LoadGreenContent("Turn Rate");
            barWeight.LoadGreenContent("Weight");
            barShieldCD.LoadGreenContent("Shield Cooldown");
            barDodgeCD.LoadGreenContent("Dodge Cooldown");
            barDodgeDuration.LoadGreenContent("Dodge Duration");
            barPower.LoadGreenContent("Collision Power");
            barArmor.LoadGreenContent("Armor");

            /*No Arrange Control here because this UI is called only from UICombat*/
        }

        public override void ArrangeControl()
        {
            GlobalCamera = game.uICombat.GlobalCamera;
            GlobalCamera.UpdateCameralLevel(0);
           
            /*General*/
            Aircraft player1Aircraft = game.gameLevel1.player1.aircraft;
            int Wid = 400;
            int Hit = 25;
            int posX = 10;
            int posY = 10;

            /*Photo*/
            OperatorPhoto.RenderRect = new Rectangle(posX, posY, Wid/2, Wid/2);/*Squre*/

            posY += Wid/2;

            /*Avator*/
            btnOperator.RenderRect = new Rectangle(posX, posY + Hit * 0, Wid / 2, Hit);
            btnOperator.text = game.gameLevel1.pilots.GetPilot(0).OperaterName;
            btnName.RenderRect = new Rectangle(posX, posY + Hit * 1, Wid/2, Hit);
            btnName.text = player1Aircraft.ObjectName;
            btnEmpty.RenderRect = new Rectangle(posX, posY + Hit * 2, Wid / 2, Hit * 3);

            posX = 10 + Wid/2;
            posY = 10;

            /*Properties*/
            barHP.SetMaxValue(player1Aircraft.MaxHitpoint, Wid,                  new Rectangle(posX, posY + Hit * 0, Wid, Hit));
            barShield.SetMaxValue(player1Aircraft.MaxShieldPoint, Wid,           new Rectangle(posX, posY + Hit * 1, Wid, Hit));  
            barShieldReg.SetMaxValue(player1Aircraft.MaxShieldRegeneration, Wid, new Rectangle(posX, posY + Hit * 2, Wid, Hit));
            barShieldCD.SetMaxValue(player1Aircraft.MaxShieldCoolDown, Wid,      new Rectangle(posX, posY + Hit * 3, Wid, Hit));
            barEnergy.SetMaxValue(player1Aircraft.MaxEnergyPoint, Wid,           new Rectangle(posX, posY + Hit * 4, Wid, Hit));
            barEnergyReg.SetMaxValue(player1Aircraft.MaxEnergyRegeneration, Wid, new Rectangle(posX, posY + Hit * 5, Wid, Hit));

            barArmor.SetMaxValue(player1Aircraft.MaxArmor, Wid,                  new Rectangle(posX, posY + Hit * 6, Wid, Hit));
            barThrust.SetMaxValue(player1Aircraft.MaxThrust, Wid,                new Rectangle(posX, posY + Hit * 7, Wid, Hit));
            barPower.SetMaxValue(player1Aircraft.MaxPower, Wid,                  new Rectangle(posX, posY + Hit * 8, Wid, Hit));
            barDodgeCD.SetMaxValue(player1Aircraft.dodgeMaxCooldown, Wid,        new Rectangle(posX, posY + Hit * 9, Wid, Hit));
            barDodgeDuration.SetMaxValue(player1Aircraft.dodgeDuration, Wid,     new Rectangle(posX, posY + Hit * 10, Wid, Hit));
            barTurnRate.SetMaxValue(player1Aircraft.MaxTurnRate, Wid,            new Rectangle(posX, posY + Hit * 11, Wid, Hit));
            barWeight.SetMaxValue(player1Aircraft.ObjectWeight, Wid,             new Rectangle(posX, posY + Hit * 12, Wid, Hit));
           
            posX = 10 + Wid / 2 * 3;
            posY = 10;

            /*Weapon Slots*/
            int posCount = 0;
            int labelCount = 0;
            WeaponType lastWeaponType = WeaponType.NONE;
            for (int i = 0; i < player1Aircraft.maxWeaponNumber; i++)
            {
                Weapon weapon = player1Aircraft.weaponSlot.slots.ElementAt(i).weapon;
                WeaponType weaponType = player1Aircraft.weaponSlot.slots.ElementAt(i).weaponType;

                /*Add label*/
                if (weaponType != lastWeaponType)
                    if (AddLabel(labelCount, posX, posY, Wid, Hit, posCount, weaponType)) { labelCount++; posCount++; }
                /*If not enough label*/
                if (i == (player1Aircraft.maxWeaponNumber - 1))
                    while (labelCount < 4) if (AddLabel(labelCount, posX, posY, Wid, Hit, posCount, WeaponType.NONE)) { labelCount++; posCount++; }

                /*Weapon*/
                btnSlots[i].RenderRect = new Rectangle(posX, posY + Hit * posCount, Wid / 2, Hit);
                posCount++;

                switch (weaponType)
                {
                    case WeaponType.GUN:
                        if(weapon != null) btnSlots[i].text = weapon.weaponName;
                        break;
                    case WeaponType.TORPEDO:
                        if (weapon != null) btnSlots[i].text = weapon.weaponName;
                        break;
                    case WeaponType.MISSILE:
                        if (weapon != null) btnSlots[i].text = weapon.weaponName;
                        break;
                    case WeaponType.UTILITY:
                        if (weapon != null) btnSlots[i].text = weapon.weaponName;
                        break;
                    case WeaponType.NONE:
                        btnSlots[i].text = "Not Available";
                        break;
                }

                lastWeaponType = weaponType;
            }

            posX = 10 + Wid / 2 * 4;
            posY = 10;

            /*Inventory*/            
            for (int i = 0; i < maxInventoryCount; i++)
            {
                btnInventory[i].RenderRect = new Rectangle(posX, posY + Hit * (i + 1), Wid / 2, Hit);                
            }
            btnLables[4].RenderRect = new Rectangle(posX, posY + Hit * 0, Wid / 2, Hit);
            btnLables[4].text = "--INVENTORY--";
        }

        public override void Update(GameTime gameTime)
        {
            Aircraft player1Aircraft = game.gameLevel1.player1.aircraft;
            
            barHP.UpdateValue(player1Aircraft.CurrentHitpoint);
            barShield.UpdateValue(player1Aircraft.CurrentShieldPoint);
            barEnergy.UpdateValue(player1Aircraft.CurrentEnergyPoint);
            barShieldReg.UpdateValue(player1Aircraft.CurrentShieldRegeneration);
            barEnergyReg.UpdateValue(player1Aircraft.CurrentEnergyRegeneration);
            barThrust.UpdateValue(player1Aircraft.CurrentThrust);
            barTurnRate.UpdateValue(Math.Abs(player1Aircraft.CurrentTurnRate));
            barWeight.UpdateValue(player1Aircraft.CurrentWeight);
            barShieldCD.UpdateValue(player1Aircraft.CurrentShieldCoolDown);
            barDodgeCD.UpdateValue(player1Aircraft.dodgeMaxCooldown - player1Aircraft.dodgeCooldown);
            barDodgeDuration.UpdateValue(player1Aircraft.dodgeDuration);
            barPower.UpdateValue(player1Aircraft.CurrentPower); 
            barArmor.UpdateValue(player1Aircraft.CurrentArmor);

            GlobalCamera.Update(gameTime);
        }

        public bool AddLabel(int labelCount, int posX, int posY, int Wid, int Hit, int posCount, WeaponType weaponType)
        {
            if (labelCount < 4)
            {
                btnLables[labelCount].RenderRect = new Rectangle(posX, posY + Hit * posCount, Wid / 2, Hit);
                switch (weaponType)
                {
                    case WeaponType.GUN:
                        btnLables[labelCount].text = "----GUN----";
                        break;
                    case WeaponType.TORPEDO:
                        btnLables[labelCount].text = "----TORPEDO----";
                        break;
                    case WeaponType.MISSILE:
                        btnLables[labelCount].text = "----MISSILE----";
                        break;
                    case WeaponType.UTILITY:
                        btnLables[labelCount].text = "----UTILITY----";
                        break;
                    case WeaponType.NONE:
                        btnLables[labelCount].text = "Not Available";
                        break;
                }
                return true;
            }

            return false;
        }

        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            OperatorPhoto.DrawSprites(gameTime, spriteBatch);
            btnName.DrawSprites(gameTime, spriteBatch);
            btnOperator.DrawSprites(gameTime, spriteBatch);
            btnEmpty.DrawSprites(gameTime, spriteBatch);

            for (int i = 0; i < 9; i++) btnSlots[i].DrawSprites(gameTime, spriteBatch);
            for (int i = 0; i < maxInventoryCount; i++) btnInventory[i].DrawSprites(gameTime, spriteBatch);
            for (int i = 0; i < maxLabelCount; i++) btnLables[i].DrawSprites(gameTime, spriteBatch);

            barHP.DrawSprites(gameTime, spriteBatch);
            barShield.DrawSprites(gameTime, spriteBatch);
            barEnergy.DrawSprites(gameTime, spriteBatch);
            barShieldReg.DrawSprites(gameTime, spriteBatch);
            barEnergyReg.DrawSprites(gameTime, spriteBatch);
            barThrust.DrawSprites(gameTime, spriteBatch);
            barTurnRate.DrawSprites(gameTime, spriteBatch);
            barWeight.DrawSprites(gameTime, spriteBatch);
            barShieldCD.DrawSprites(gameTime, spriteBatch);
            barDodgeCD.DrawSprites(gameTime, spriteBatch);
            barDodgeDuration.DrawSprites(gameTime, spriteBatch);
            barPower.DrawSprites(gameTime, spriteBatch);
            barArmor.DrawSprites(gameTime, spriteBatch);
        }

    }
}
