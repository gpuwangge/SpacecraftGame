using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameSpace
{
    public class Camera
    {
        #region Fields

        private GraphicsDeviceManager graphics;
        SpaceGame game;

        public CameraType cameraType;

        public int cameraLevel = 2;
        public int MaxCameraLevel = 6;
        public int MinCameraLevel = 0;
        public float[] CameraLevelZ = { -50, -200, -400, -700, -1000, -1500, -2200 };

        private Vector3 eye = new Vector3(0, 0, -400);
        public Vector3 Eye { get { return eye; } set { eye = value; ComputeView(); } }

        private Vector3 center = new Vector3(0, 0, 0);
        public Vector3 Center { get { return center; } set { center = value; ComputeView(); } }

        private Vector3 up = new Vector3(0, 1, 0);
        public Vector3 Up { get { return up; } set { up = value; ComputeView(); } }

        private float fov = MathHelper.ToRadians(45);
        private float znear = 10;
        private float zfar = 500000000;

        private Matrix view;
        public Matrix View { get { return view; } }

        private Matrix projection;
        public Matrix Projection { get { return projection; } }

        #endregion

        private Vector3 velocity = Vector3.Zero;
        private Vector3 bankVelocity = Vector3.Zero;

        private Vector3 desiredEye = new Vector3(0, 0, -400);
        public Vector3 DesiredEye { get { return desiredEye; } set { desiredEye = value; } }

        private Vector3 desiredUp = Vector3.Zero;
        public Vector3 DesiredUp { get { return desiredUp; } set { desiredUp = value; } }

        private float stiffness = 5;
        public float Stiffness { get { return stiffness; } set { stiffness = value; } }

        private float damping = 20;
        public float Damping { get { return damping; } set { damping = value; } }

        private int focus = 0;
        public int Focus { get { return focus; } set { focus = value; } }

        public Camera(SpaceGame game, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.game = game;
        }

        public void Initialize()
        {
            ComputeView();
            ComputeProjection();
            //cameraType = CameraType.NONE;
        }

        public void IncreaseCameraLevel()
        {
            UpdateCameralLevel(cameraLevel + 1);
        }

        public void DecreaseCameralLevel()
        {
            UpdateCameralLevel(cameraLevel - 1);
        }

        public void UpdateCameralLevel(int level)
        {
            if (level > MaxCameraLevel || level < MinCameraLevel) return;

            cameraLevel = level;

        }

        public void Update(GameTime gameTime)
        {
            switch (cameraType)
            {
                case CameraType.NONE:
                    break;
                case CameraType.CHASE:
                    //GlobalCamera.DesiredEye = Vector3.Transform(new Vector3(0, 10, -30), ((Aircraft)game.scenario1.aircrafts.m_Aircrafts.ElementAt(player1AircraftIndex)).Transform);
                    //GlobalCamera.Center = Vector3.Transform(new Vector3(0, 0, 20), ((Aircraft)game.scenario1.aircrafts.m_Aircrafts.ElementAt(player1AircraftIndex)).Transform);
                    //GlobalCamera.DesiredUp = ((Aircraft)game.scenario1.aircrafts.m_Aircrafts.ElementAt(player1AircraftIndex)).Transform.Up;
                    break;
                case CameraType.GLOBAL:
                    DesiredEye = new Vector3(
                        game.gameLevel1.pilots.GetAircraft(focus).Position.X,
                        game.gameLevel1.pilots.GetAircraft(focus).Position.Y,
                        DesiredEye.Z);

                    desiredEye.Z = CameraLevelZ[cameraLevel];

                    Center = game.gameLevel1.pilots.GetAircraft(focus).Position;
                    Up = new Vector3(0, 1, 0);

                    Vector3 stretch = desiredEye - eye;// Calculate spring force
                    Vector3 acceleration = stretch * stiffness - velocity * damping;
                    velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    eye += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    stretch = desiredUp - up;
                    acceleration = stretch * stiffness * .3f - bankVelocity * damping;
                    bankVelocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    up += bankVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }

        }

        public void ComputeView()
        {
            view = Matrix.CreateLookAt(eye, center, up);
        }

        public void ComputeProjection()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(fov,
                graphics.GraphicsDevice.Viewport.AspectRatio, znear, zfar);
        }

        public void Pan(float offsite)
        {
            Vector3 cameraZ = eye - center;
            Vector3 cameraX = Vector3.Cross(up, cameraZ);
            float len = cameraX.LengthSquared();
            if (len > 0)
                cameraX.Normalize();
            else
                cameraX = new Vector3(1, 0, 0);

            cameraX *= offsite;
            Matrix M = Matrix.CreateTranslation(cameraX);


            eye = Vector3.Transform(eye, M);
            center = Vector3.Transform(center, M);

            ComputeView();
        }

        public void Tilt(float offsite)
        {
            Vector3 cameraZ = eye - center;
            Vector3 cameraX = Vector3.Cross(up, cameraZ);
            Vector3 cameraY = Vector3.Cross(cameraZ, cameraX);
            float len = cameraY.LengthSquared();
            if (len > 0)
                cameraY.Normalize();
            else
                cameraY = new Vector3(0, 1, 0);

            cameraY *= offsite;
            Matrix M = Matrix.CreateTranslation(cameraY);

            eye = Vector3.Transform(eye, M);
            center = Vector3.Transform(center, M);

            ComputeView();
        }

        public void Yaw(float angle)
        {
            Vector3 cameraZ = eye - center;
            Vector3 cameraX = Vector3.Cross(up, cameraZ);
            Vector3 cameraY = Vector3.Cross(cameraZ, cameraX);
            float len = cameraY.LengthSquared();
            if (len > 0)
                cameraY.Normalize();
            else
                cameraY = new Vector3(0, 1, 0);

            Matrix t1 = Matrix.CreateTranslation(-center);
            Matrix r = Matrix.CreateFromAxisAngle(cameraY, angle);
            Matrix t2 = Matrix.CreateTranslation(center);

            Matrix M = t1 * r * t2;
            eye = Vector3.Transform(eye, M);
            ComputeView();
        }

        public void Pitch(float angle)
        {
            Vector3 cameraZ = eye - center;
            Vector3 cameraX = Vector3.Cross(up, cameraZ);
            float len = cameraX.LengthSquared();
            if (len > 0)
                cameraX.Normalize();
            else
                cameraX = new Vector3(1, 0, 0);

            Matrix t1 = Matrix.CreateTranslation(-center);
            Matrix r = Matrix.CreateFromAxisAngle(cameraX, angle);
            Matrix t2 = Matrix.CreateTranslation(center);

            Matrix M = t1 * r * t2;
            eye = Vector3.Transform(eye, M);
            ComputeView();
        }

    }
}
