using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Input;

namespace GameSpace
{
    public class Aircraft : MyObject
    {
        /*Properties*/
        public float MaxThrust;  // Maximum thrust (cm/sec^2)
        public float CurrentThrust;  // Maximum thrust (cm/sec^2)
        public float ThrustSpeed = 1000;
        public float UserThrust;
        public float MaxShieldRegeneration;
        public float CurrentShieldRegeneration;
        public float MaxEnergyRegeneration;
        public float CurrentEnergyRegeneration;
        public float MaxShieldCoolDown;
        public float CurrentShieldCoolDown;

        /*Game Properties*/
        public AircraftStatus aircraftStatus;
        public float Drag;
        public float Acc;
        public float Deacc;

        /*Physical Properties*/
        public Vector3 SmokePos1;
        public Vector3 SmokePos2;
        public float DragCoff;   // Deceleration due to drag

        //sound
        public string engineSoundName;//initialized in inherited class
        private Cue engineSound = null;//initialized in the first play

        public string EffectFileName;
        //private Effect AircraftEffect;

        /*Weapon*/
        public int maxWeaponNumber;
        public WeaponSlot weaponSlot;
        public WeaponLaserGun weaponLaserGun;

        /*Particle System*/       
        /*Step 1*/
        Particle3D.ParticleSystem damageParticleSystem;
        Particle3D.ParticleSystem damageSmokeParticleSystem;
        Particle3D.ParticleSystem explosionParticleSystem;
        Particle3D.ParticleSystem explosionSmokeParticleSystem;
        Particle3D.ParticleSystem LightSystem;

        /*Bank*/
        float maxBankAngle;
        float currentBankAngle;
        float lastStepTurnRate;

        /*Dodge*/
        public float dodgeDuration;
        public float dodgeTime = 0;
        public float dodgeMaxCooldown;
        public float dodgeCooldown = 0;
        public int dodgeDirection;
        public bool lastDodgeState = false;

        /*Avoid Turning Tremble*/
        public int thisBoundary;
        public int lastBoundary; // to avoid direction swap
        public int lastlastBoundary;
        public float boundaryCoff;

        /*Effect*/
        //private Texture2D PointTexture;

        public int pilotIndex;

        public Aircraft(SpaceGame game) : base(game)
        {
            weaponSlot = new WeaponSlot();
        }

        public void AddSmoke()
        {
            Color colorAlpha = game.utilities.GetMarkerColor(Relation);
            LightSystem = new Particle3D.LightSystem(game, game.Content, 4, colorAlpha, 3.0f);
            game.Components.Add(LightSystem);
        }

        public virtual void Initialize()
        {
            /*Step 2*/                       
            damageParticleSystem = new Particle3D.FireParticleSystem(game, game.Content, 5, Color.White);
            damageSmokeParticleSystem = new Particle3D.SmokeParticleSystem(game, game.Content, 10, Color.White, 4f);
            explosionParticleSystem = new Particle3D.ExplosionParticleSystem(game, game.Content, 40, Color.White);
            explosionSmokeParticleSystem = new Particle3D.ExplosionSmokeParticleSystem(game, game.Content, 6, Color.White);
            /*Step 3*/            
            game.Components.Add(damageParticleSystem);
            game.Components.Add(damageSmokeParticleSystem);
            game.Components.Add(explosionParticleSystem);
            game.Components.Add(explosionSmokeParticleSystem);

            maxBankAngle = PI / 4.0f;
            currentBankAngle = 0;

            thisBoundary = 0;
            lastBoundary = 0;
            lastlastBoundary = 0;
            boundaryCoff = 1.0f;
        }

        public void Dodge()
        {
            if (dodgeCooldown <=0)
            {
                dodgeCooldown = dodgeMaxCooldown;
                dodgeTime = dodgeDuration;
                this.Dodged = true;
                dodgeDirection = RandomNumber(0, 1);
                if (dodgeDirection == 0) dodgeDirection = -1;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Destroyed) return;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //OpenThrust(delta);
            if (Speed > 0) Smoke();
            MitigateTremble();
            Matrix MatrixTransform = UpdateOrientation(delta);

            UpdateDodge(MatrixTransform, delta);

            UpdateCollision(delta);

            UpdateVelocity(MatrixTransform, delta);
            UpdatePosition(delta);

            UpdateEnergyPoint(delta);
            UpdateShieldPoint(delta);
            UpdateWeaponCooldown(gameTime);

        }

        public void UpdateCollision(float delta)
        {
            CollisionCooldown += delta;
            if (CollisionCooldown > CollisionMaxCooldown) CanCollision = true;
            else CanCollision = false;
        }

        public void UpdateDodge(Matrix MatrixTransform, float delta)
        {
            if (dodgeCooldown > 0) 
                dodgeCooldown -= delta;
            if (dodgeTime > 0)
            {
                dodgeTime -= delta;

                currentBankAngle += delta * 2 * PI / dodgeDuration * dodgeDirection;
                bankOrientation = Quaternion.CreateFromAxisAngle(Vector3.TransformNormal(new Vector3(0, 0, 1), MatrixTransform), currentBankAngle);
            }
            else
            {
                this.Dodged = false;

                if(lastDodgeState == true) currentBankAngle = 0;

                BankAnimationFromTurning(MatrixTransform, delta);
            }
            lastDodgeState = this.Dodged;
        }

        public void UpdateWeaponCooldown(GameTime gameTime)
        {
            foreach (Slot slot in weaponSlot.slots)
            {
                if (slot.weapon != null)
                    slot.weapon.Update(gameTime);
            }
        }

        public void UpdateEnergyPoint(float delta)
        {
            CurrentEnergyPoint += (CurrentEnergyRegeneration * delta);
            if (CurrentEnergyPoint > MaxEnergyPoint)
                CurrentEnergyPoint = MaxEnergyPoint;
        }

        public void UpdateShieldPoint(float delta)
        {
            if (CurrentShieldCoolDown >= MaxShieldCoolDown)
            {
                CurrentShieldPoint += (CurrentShieldRegeneration * delta);
                if (CurrentShieldPoint > MaxShieldPoint)
                    CurrentShieldPoint = MaxShieldPoint;
            }
            else
            {
                CurrentShieldCoolDown += delta;
            }
        }

        public void BankAnimationFromTurning(Matrix MatrixTransform, float delta){
            float changeValue = delta * maxBankAngle * 3;
            if (CurrentTurnRate > 0 && lastStepTurnRate > 0)/*Turn*/{
                if (currentBankAngle < 0.1f) changeValue *= boundaryCoff;
                if (currentBankAngle > -maxBankAngle) currentBankAngle -= changeValue;
            }
            else if (CurrentTurnRate < 0 && lastStepTurnRate < 0)/*Turn*/{
                if (currentBankAngle < 0.1f) changeValue *= boundaryCoff;
                if (currentBankAngle < maxBankAngle) currentBankAngle += changeValue;
            }
            else /*Turn Back*/{
                if (currentBankAngle > 0){
                    currentBankAngle -= changeValue * 2;
                    if (currentBankAngle < 0) currentBankAngle = 0;
                }
                if (currentBankAngle < 0){
                    currentBankAngle += changeValue * 2;
                    if (currentBankAngle > 0) currentBankAngle = 0;
                }
            }
            bankOrientation = Quaternion.CreateFromAxisAngle(Vector3.TransformNormal(new Vector3(0, 0, 1), MatrixTransform), currentBankAngle);

            lastStepTurnRate = CurrentTurnRate;

        }

        public Matrix UpdateOrientation(float delta)
        {
            Quaternion newQri = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), CurrentTurnRate * delta * boundaryCoff);
            Orientation *= newQri;/*Set orientation, (0,1,0) is the initial ship direction to Orientation*/
            Orientation.Normalize();

            return Matrix.CreateFromQuaternion(Orientation);
        }

        public void UpdatePosition(float delta)
        {
            Position += CurrentVelocity * delta;
            Position.Z = 0;
        }

        public void UpdateVelocity(Matrix MatrixTransform, float delta)
        {
            /*Anti Acc Force*/
            Drag = DragCoff * Speed;//Unit: N
            Vector3 directedThrust = Vector3.TransformNormal(new Vector3(0, 0, 1), MatrixTransform);/*(0,0,1) is the original ship direction*/

            /*Compute Acc and De-Acc*/
            Acc = UserThrust / CurrentWeight;
            Deacc = Drag / CurrentWeight;

            if (Acc > 0)
            {
                /*Compute Velocity and Speed*/
                CurrentVelocity += directedThrust * Acc * delta;
                SpeedDir = CurrentVelocity;
                if (Speed != 0 && SpeedDir != Vector3.Zero) SpeedDir.Normalize();

                CurrentVelocity -= SpeedDir * Deacc * delta;//M/S^2
                Speed = CurrentVelocity.Length();//M/S
            }
            else
            {
                SpeedDir = CurrentVelocity;
                if (Speed != 0 && SpeedDir != Vector3.Zero) SpeedDir.Normalize();

                CurrentVelocity -= SpeedDir * (Deacc-Acc) * delta;//M/S^2
                Speed = CurrentVelocity.Length();//M/S
            }
        }

        public bool IncreaseThrust()
        {
            if (UserThrust < 0) UserThrust = 0;
            UserThrust += ThrustSpeed;
            if (UserThrust > CurrentThrust){
                UserThrust = CurrentThrust;
                return true;
            }
            return false;
        }

        public bool DecreaseThrust(){
            UserThrust -= ThrustSpeed;
            if (UserThrust < 0){
                UserThrust = -CurrentThrust;
                return true;
            }
            return false;
        }

        public void DecreaseThrustToBreak(){
            while (UserThrust > 0) DecreaseThrust();
        }

        public void MitigateTremble(){
            lastlastBoundary = lastBoundary;
            lastBoundary = thisBoundary;
            if (CurrentTurnRate > 0) thisBoundary = 1;
            else if (CurrentTurnRate < 0) thisBoundary = 2;
            if (thisBoundary == lastlastBoundary && thisBoundary != lastBoundary) boundaryCoff = boundaryCoff * 0.8f;
            if (thisBoundary == lastBoundary && thisBoundary == lastlastBoundary){
                if (boundaryCoff == 1.0f){}
                else if (boundaryCoff > 1.0f) boundaryCoff = 1.0f;
                else boundaryCoff += (boundaryCoff + 0.01f);
                
            }
        }

        public void FireWeapon(int index){
            Weapon weapon = weaponSlot.slots.ElementAt(index).weapon;
            
            //float energyCost = Weapons.ElementAt(index - 1).EnergyCost;
            float energyCost = weapon.EnergyCost;
            if (CurrentEnergyPoint > energyCost){
                //if (Weapons.ElementAt(index - 1).Fire(Position, Orientation, CurrentVelocity))
                if (weapon.Fire(Position, Orientation, CurrentVelocity,
                        weaponSlot.slots.ElementAt(index).slotPosition, weaponSlot.slots.ElementAt(index).slotAngle))
                    CurrentEnergyPoint -= energyCost;
            }
        }

        public void DrawParticleSystem(){
            /*Step 5*/
            Matrix View = game.uIManager.GlobalCamera.View;
            Matrix Projection = game.uIManager.GlobalCamera.Projection;

            LightSystem.SetCamera(View, Projection);
            damageParticleSystem.SetCamera(View, Projection);
            damageSmokeParticleSystem.SetCamera(View, Projection);
            explosionParticleSystem.SetCamera(View, Projection);
            explosionSmokeParticleSystem.SetCamera(View, Projection);       

        }

        public virtual void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            //VertexDeclaration vertexDeclaration = new VertexDeclaration(VertexPositionNormalTextureTangentBinormal.VertexElements);
            //VertexBuffer vertexBuffer = new Microsoft.Xna.Framework.Graphics.VertexBuffer(game.GraphicsDevice, vertexDeclaration,100, BufferUsage.None);
            //game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            
            DrawParticleSystem();
           
            base.DrawModel(graphics, game.modelManager.GetModel(modelIndex), Transform);

            //base.DrawModel(graphics, game.modelManager.GetModel(modelIndex), TransformDestination);
        }

        //public Matrix TransformDestination
        //{
        //    get
        //    {
        //        return Matrix.CreateScale(this.ModelScale) *
        //            Matrix.CreateFromQuaternion(Orientation) *
        //            Matrix.CreateFromQuaternion(bankOrientation) *
        //            Matrix.CreateTranslation(Destination);
        //    }
        //}

        public void DrawSprites(GameTime gametime, SpriteBatch spriteBatch){
            /*Not use this code, because the mapping of screen to game map is not linear*/

            //int pointerX = 100;
            //int pointerY = 100;

            //Color colorAlpha = Color.White;                         
            //colorAlpha.A = 0;

            //float scale = game.uICombat.GlobalCamera.CameralScale[game.uICombat.GlobalCamera.cameraLevel]
            //    * game.ScreenCenter.X / 800.0f;

            //Vector2 pointerPos = new Vector2(
            //    (game.uICombat.GlobalCamera.Center.X - Position.X) * scale + game.ScreenCenter.X - pointerX / 2,
            //    (game.uICombat.GlobalCamera.Center.Y - Position.Y) * scale + game.ScreenCenter.Y - pointerY / 2
            //    );

            //spriteBatch.Draw(PointTexture, new Rectangle((int)pointerPos.X, (int)pointerPos.Y, pointerX, pointerY),
            //       null, colorAlpha, 0, new Vector2(0, 0), 0, 0);

            //System.Diagnostics.Trace.WriteLine((mouseState.X - game.ScreenCenter.X).ToString());
        }
        
        private void SetEffect(Model model, Matrix world, Texture2D normalFile, GameTime gameTime, string tech, Effect p, Matrix rotationMatrix)
        {
        }

        public override void DamageHitpointAnimation(Vector3 weaponPos, Vector3 weaponVel, ObjectType objectType){
            CurrentShieldCoolDown = 0;
            //DamageAnimation((weaponPos + Position) / 2, (CurrentVelocity + weaponVel) / 2);
            //if(objectType == ObjectType.BULLET)
            DamageAnimation(weaponPos, CurrentVelocity + weaponVel);
        }

        public override void DamageShieldAnimation(Vector3 weaponPos, Vector3 weaponVel, ObjectType objectType){
            CurrentShieldCoolDown = 0;
            //DamageAnimation((weaponPos + Position) / 2, (CurrentVelocity + weaponVel) / 2);
            //if(objectType == ObjectType.BULLET)
            DamageAnimation(weaponPos, CurrentVelocity + weaponVel);
        }

        public void DamageAnimation(Vector3 pos, Vector3 vel){
            game.soundBank.PlayCue("DamageSound1");
            for (int i = 0; i < 15; i++){
                damageParticleSystem.AddParticle(pos + new Vector3(0, 0, -10),
                    new Vector3(0, 0, 0) + vel / 50);
                if (i > 12)
                    damageSmokeParticleSystem.AddParticle(pos + new Vector3(0, 0, 0),
                      new Vector3(0, 0, 0) + vel / 50);
            }
        }

        public void ExpodeAnimation(Vector3 pos, Vector3 vel){
            game.soundBank.PlayCue("ExplosionSound1");
            for (int i = 0; i < 15; i++){
                explosionParticleSystem.AddParticle(pos + new Vector3(0, 0, -10),
                    new Vector3(RandomNumber(-5, 5), RandomNumber(-5, 5), 0));
                for (int j = 0; j < 3; j++)
                    explosionSmokeParticleSystem.AddParticle(pos + new Vector3(0, 0, -5),
                       new Vector3(RandomNumber(-30, 30), RandomNumber(-30, 30), 0) + vel);
            }

        }

        public override void DestroyAnimation(){
            CurrentShieldCoolDown = 0;
            ExpodeAnimation(Position, CurrentVelocity);
            Destroyed = true;
            string relationString = "";
            Color color = Color.White;
            if (Relation == RelationEnum.ALLY) { 
                relationString = "ally";
                color = Color.Red;
            }
            else if (Relation == RelationEnum.ENEMY1 || Relation == RelationEnum.ENEMY2){
                relationString = "enemy";
                color = Color.LightGreen;
                game.gameLevel1.EnemyAircraftDestroy();
            }
            game.uICombat.controlInformation.AddText("An " + relationString + " aircraft is destroyed!", color);
        }

        public override void HitSomething(MyObject myObject){
            if (sourceGroup == SourceGroup.PLAYER1) {//the player owns me
                //game.uICombat.Player1HitIt(myObject, pilotName, aircraftName, maxHitpoint, hitpoint, maxShieldPoint, shieldPoint, maxEnergyPoint, energyPoint);
                game.uICombat.Player1HitIt(myObject);
            }
        }

        public void Smoke(){
            if (SmokePos1 != Vector3.Zero){
                /*Step 4*/
                LightSystem.AddParticle(Position + Vector3.Transform(SmokePos1, Matrix.CreateFromQuaternion(Orientation)), Vector3.Zero);
            }
            if (SmokePos2 != Vector3.Zero){
                LightSystem.AddParticle(Position + Vector3.Transform(SmokePos2, Matrix.CreateFromQuaternion(Orientation)), CurrentVelocity);
            }
        }

        public void AddWeapon(int level, WeaponCode weaponCode, int SlotIndex){
            switch (weaponCode){
                case WeaponCode.LASER:
                    WeaponLaserGun weaponLaserGun = new WeaponLaserGun(game);

                    //weaponLaserGun.playerOwner = playerOwner;

                    //weaponLaserGun.SetPosition(
                    //    weaponSlot.slots.ElementAt(SlotIndex).slotPosition,
                    //    weaponSlot.slots.ElementAt(SlotIndex).slotAngle);

                    weaponLaserGun.relation = Relation;
                    weaponLaserGun.sourceGroup = sourceGroup;
                    weaponLaserGun.Initialize();

                    weaponSlot.slots.ElementAt(SlotIndex).weapon = weaponLaserGun;
                    
                    break;
            }
        }

        public void PlayEngineSound() {
            if (engineSound == null || engineSound.IsStopped) {
                engineSound = game.soundBank.GetCue(engineSoundName);
                engineSound.Play();
            }else if (engineSound.IsPaused) engineSound.Resume();
        }

        public void StopEngineSound() {
            if (engineSound != null && engineSound.IsPlaying) engineSound.Pause();
        }
    }
}
