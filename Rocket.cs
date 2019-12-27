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
        public Engine Engine;
        public const float MaxFuelMass = 8000;

        public float DryMass = 20000;
        public float RemainingFuelMass;
        public float TotalMass => DryMass + RemainingFuelMass;
        public float DVRemaining => Engine.Ve * (float)Math.Log(TotalMass / DryMass);
        public float Throttle = 0;
        public float Height => (720 - Position.X);
        public float TWR => Engine.F / RG.Planet.Gravity / TotalMass;

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
            RemainingFuelMass = MaxFuelMass;
            RocketS = new Sprite(new Texture("resources/rocket.png")) { Scale = Vector2.One * .1f, Origin = new Vector2(159, 256) };
            FiringS = new Sprite(new Texture("resources/rocketFiring.png")) { Scale = Vector2.One * .1f, Origin = new Vector2(159, 256) };
            Throttle = 1;
            //AngularVelocity = 360;
            this.Position = new Vector2(640, 650);
            TGTRotation = 0;
            Engine = new Engine();
            string[] prog = System.IO.File.ReadAllLines("test.txt");
            Console.WriteLine();
            ActiveProgram = Program.Parse(prog);
        }

        public void Update()
        {
            updateSprites();
            updatePhysics();
            
            //System.Console.WriteLine(fwd.Direction + ": " + Rotation);

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
            Rotation += AngularVelocity * RG.FrameTime;
            Position += Velocity * RG.FrameTime*1;

            ActiveProgram.RunFrame(RG.FrameTime);
            Throttle = MathF.Max(0, MathF.Min(1, Throttle));

            AngularVelocity += (TGTRotation - Rotation) * RG.FrameTime * 90;
            //Console.WriteLine(TGTRotation); 
            Velocity += Vector2.Down * RG.Planet.Gravity * RG.FrameTime;
            Vector2 thrust = Fwd * Engine.F * Throttle;
            if (RemainingFuelMass > 0)
            {
                Velocity += thrust * RG.FrameTime / TotalMass;
                RemainingFuelMass -= Engine.MassFlowRate * Throttle*RG.FrameTime;
            }
            else
            {
                //Console.WriteLine(F);
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
