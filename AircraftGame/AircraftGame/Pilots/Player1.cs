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
    public class Player1 : Pilot
    {
        public MouseController mouseController;
        public KeyboardController keyboardController;

        public ActionManager actionManager;

        public Player1(SpaceGame game)
            : base(game)
        {
            mouseController = new MouseController(game);
            keyboardController = new KeyboardController(game);
            actionManager = new ActionManager(6);
        }

        public override void Initialize(TeamRole teamRole, int[] teamMember)
        {
            OperaterName = "username";
            this.sourceGroup = SourceGroup.PLAYER1;
            this.teamRole = teamRole;
            this.teamMember = teamMember;

            actionManager.SetAction(0, ActionType.FIREWEAPON1);

            //actionManager.SetAction(1, ActionType.FIREWEAPON1);
            //actionManager.SetAction(1, ActionType.FIREWEAPON2);
            //actionManager.SetAction(1, ActionType.FIREWEAPON3);

            actionManager.SetAction(1, ActionType.DODGE);

            mouseController.Initialize();
            keyboardController.Initialize();
        }

        public override void AddAircraft(Aircraft aircraft, Vector3 location, RelationEnum relation, Pilots pilots)
        {    
            //aircraft.pilotIndex = PilotIndex;
            aircraft.sourceGroup = sourceGroup;
            aircraft.Position = location;
            aircraft.Orientation = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -PI / 2);
            aircraft.bankOrientation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), 0);

            /*Cheat*/
            aircraft.CurrentHitpoint = 9999;
            aircraft.MaxHitpoint = 9999;

            //base.AddAircraft(aircraft, location, relation, aircrafts, pilots);
            aircraft.Relation = relation;
            //aircrafts.AddAircraft(aircraft);     
            pilots.AddPilot(this);

            aircraft.AddSmoke();

            /*Add Initial Weapon(s)*/
            aircraft.AddWeapon(0, WeaponCode.LASER, 0);
            //aircraft.AddWeapon(0, WeaponType.LASER, 1);
            //aircraft.AddWeapon(0, WeaponType.LASER, 2);
            //aircraft.AddWeapon(0, WeaponType.LASER, 3);

            this.aircraft = aircraft;
        }

        public override void Update(GameTime gameTime, bool isPaused, bool isInEquip)
        {
            keyboardController.Update(gameTime, PilotIndex);

            if (!isPaused && !isInEquip) mouseController.Update(gameTime, PilotIndex, actionManager);

            base.Update(gameTime, isPaused, isInEquip);
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            base.Draw(graphics, gameTime);
        }

        public override void DrawSprites(GameTime gametime, SpriteBatch spriteBatch)
        {
        }
    }
}
