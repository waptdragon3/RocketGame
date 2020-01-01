using System;
using System.Collections.Generic;
using System.Text;

namespace RocketGame.FlightSystem
{
    public class Program
    {
        public RAM RAM;
        private Instruction[] Instructions;
        public int CurrentInstructionIndex = 0;
        private float waitTime = 0;

        public bool Halted { get; set; }


        public static Program Parse(string[] pro)
        {
            Instruction[] instructions = new Instruction[pro.Length];
            for (int i = 0; i < pro.Length; i++) instructions[i] = Instruction.Parse(pro[i]);
            var p = new Program();
            p.Instructions = instructions;
            p.RAM = new RAM();
            return p;
        }
        public void RunFrame(float frameTime)
        {
            if (!Halted)
            {
                if (CurrentInstructionIndex == Instructions.Length)
                    return;
                waitTime -= frameTime;
                while (waitTime <= 0)
                {
                    waitTime = Instructions[CurrentInstructionIndex].Run();
                    if (Halted) return;
                }
            }
        }
        
    }
    public class RAM
    {
        private readonly float[] mem;
        public RAM()
        {
            mem = new float[32];
        }
        public float this[float adress]
        {
            get { return mem[(int)adress]; }
            set { mem[(int)adress] = value; }
        }
    }

    [Flags] public enum AType { LITERAL = 1, REGISTER = 2, MEMORY = 4, GETABLE=LITERAL|REGISTER|MEMORY, SETTABLE=REGISTER|MEMORY}
    public abstract class Argument
    {
        public AType Type;
        public abstract float Value { get; set; }
        protected Argument(AType type) { Type = type; }
    }
    class LITArg :Argument
    {
        private readonly float v;
        public override float Value { get => v; set { throw new Exception("You cannot set a constant"); } }
        public LITArg(string s):base(AType.LITERAL)
        {
            v = float.Parse(s);
        }
    }
    class REGArg : Argument
    {
        public string Name;
        public REGArg(string name) : base(AType.REGISTER)
        {
            Name = name;
        }

        public override float Value
        {
            get
            {
                if (Name.ToLower().Equals("fuel")) return RG.Rocket.FuelPercent;
                if (Name.ToLower().Equals("velx")) return RG.Rocket.Velocity.X;
                if (Name.ToLower().Equals("vely")) return RG.Rocket.Velocity.Y;
                if (Name.ToLower().Equals("angv")) return RG.Rocket.AngularVelocity;
                if (Name.ToLower().Equals("alt")) return RG.Rocket.Height;
                if (Name.ToLower().Equals("mass")) return RG.Rocket.TotalMass;
                if (Name.ToLower().Equals("twr")) return RG.Rocket.TWR;
                if (Name.ToLower().Equals("ttl")) return RG.Rocket.Throttle;
                if (Name.ToLower().Equals("rot")) return RG.Rocket.Rotation;


                if (Name.ToLower().Equals("grav")) return RG.Planet.Gravity;
                if (Name.ToLower().Equals("dt")) return RG.FrameTime;
                throw new Exception(string.Format("The register name ({0}) is invalid", Name));
            }
            set
            {
                if (Name.ToLower().Equals("ttl")) RG.Rocket.Throttle = value;
                if (Name.ToLower().Equals("rot")) RG.Rocket.TGTRotation = value;
            }
        }
    }
    class MEMArg : Argument
    {
        public int Adress;
        public MEMArg(string adress) : base(AType.MEMORY)
        {
            Adress = int.Parse(adress);
        }

        public override float Value
        {
            get
            {
                return RG.Rocket.ActiveProgram.RAM[Adress];
            }
            set
            {
                RG.Rocket.ActiveProgram.RAM[Adress] = value;
            }
        }
    }
}