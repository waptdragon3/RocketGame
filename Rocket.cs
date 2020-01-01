using System;
using System.Collections.Generic;
using System.Text;
using RocketGame.FlightSystem;
using RocketGame.Maths;
using SFML.Graphics;

namespace RocketGame
{
    public class Rocket
    {
        public bool Running = false;
        public Engine Engine;
        public const float MaxFuelMass = 2000;
        public float FuelPercent => RemainingFuelMass / MaxFuelMass;

        public float DryMass = 20000;
        public float RemainingFuelMass;
        public float TotalMass => DryMass + RemainingFuelMass;
        public float DVRemaining => Engine.Ve * (float)Math.Log(TotalMass / DryMass);
        public float Throttle = 0;
        public float Height => (RG.ScreenSize.Y - Position.X);
        public float TWR => Engine.F / TotalMass;

        public Program ActiveProgram;

        public Vector2 Position;
        public float Rotation;
        public float TGTRotation;
        public Vector2 Fwd => Vector2.FromAngle(Rotation);
        public Vector2 Velocity;
        public float AngularVelocity;
        private readonly Sprite RocketS, FiringS;

        public Sprite ActiveSprite => (Throttle < .001) ? RocketS : FiringS;

        public Rocket()
        {
            Engine = new Engine();
            RocketS = new Sprite(new Texture("resources/rocket.png")) { Scale = Vector2.One * .1f, Origin = new Vector2(159, 256) };
            FiringS = new Sprite(new Texture("resources/rocketFiring.png")) { Scale = Vector2.One * .1f, Origin = new Vector2(159, 256) };
            Reset();
            //AngularVelocity = 360;
        }

        public void Reset()
        {
            RemainingFuelMass = MaxFuelMass;
            this.Position = new Vector2(640, 650 - 320);
            Rotation = 0;
            TGTRotation = 0;
            //string[] prog = System.IO.File.ReadAllLines("program.txt");
            ActiveProgram = Program.Parse(new string[] { "hlt" });
            Throttle = 0;
            ActiveProgram.Halted = false;
            Velocity = Vector2.Zero;
            AngularVelocity = 0;
        }

        public void Update()
        {
            updateSprites();
            updatePhysics();

            //Rotation += 360f * Program.FrameTime.AsSeconds();
            //Position += Vector2.FromAngle(Rotation) * 10000f * Program.FrameTime;
        }

        private void updateSprites()
        {
            RocketS.Position = Position;
            FiringS.Position = Position;
            RocketS.Rotation = Rotation;
            FiringS.Rotation = Rotation;
        }
        private void updatePhysics()
        {
            if (Running)
            {
                Rotation += AngularVelocity * RG.FrameTime;
                Position += Velocity * RG.FrameTime * 1;

                ActiveProgram.RunFrame(RG.FrameTime);
                Throttle = MathF.Max(0, MathF.Min(1, Throttle));

                AngularVelocity += (TGTRotation - Rotation) * RG.FrameTime * 90;

                Velocity += Vector2.Down * RG.Planet.Gravity * RG.FrameTime;
                Vector2 thrust = Fwd * Engine.F * Throttle;
                if (RemainingFuelMass > 0)
                {
                    Velocity += thrust * RG.FrameTime / TotalMass;
                    RemainingFuelMass -= Engine.MassFlowRate * Throttle * RG.FrameTime;
                }
                else
                {

                }
            }
        }
    }
    public class Engine
    {
        public readonly float Ve = 400 * 9.81f;
        public readonly float MassFlowRate = 140;
        public float F => Ve * MassFlowRate;
    }
}
