using System;
using System.Collections.Generic;
using System.Text;

namespace RocketGame.FlightSystem
{
    public class Instructions
    {
        private static void CheckArguments(Argument[] recieved, string opcode, params AType[] types)
        {
            if (recieved.Length != types.Length) throw new ArgumentException(string.Format("Expected {0} arguments for opcode {1}, got {2}", types.Length, opcode, recieved.Length));
            for(int i = 0; i < recieved.Length; i++)
            {
                if(!types[i].HasFlag(recieved[i].Type))
                {
                    throw new ArgumentException(string.Format("The {0} argument for the opcode {1} needs to be {2}, but {3} was recieved", i, opcode, types[i], recieved[i].Type));
                }
            }
        }
        public static float Add(Argument[] args)
        {
            CheckArguments(args, "add", AType.GETABLE, AType.GETABLE, AType.SETTABLE);
            args[2].Value = args[0].Value + args[1].Value;
            RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float Sub(Argument[] args)
        {
            CheckArguments(args, "sub", AType.GETABLE, AType.GETABLE, AType.SETTABLE);
            args[2].Value = args[0].Value - args[1].Value;
            RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float Mul(Argument[] args)
        {
            CheckArguments(args, "mul", AType.GETABLE, AType.GETABLE, AType.SETTABLE);
            args[2].Value = args[0].Value * args[1].Value;
            RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float Div(Argument[] args)
        {
            CheckArguments(args, "div", AType.GETABLE, AType.GETABLE, AType.SETTABLE);
            args[2].Value = args[0].Value / args[1].Value;
            RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float Mod(Argument[] args)
        {
            CheckArguments(args, "mod", AType.GETABLE, AType.GETABLE, AType.SETTABLE);
            args[2].Value = args[0].Value % args[1].Value;
            RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float Set(Argument[] args)
        {
            CheckArguments(args, "set", AType.SETTABLE, AType.GETABLE);
            args[0].Value = args[1].Value;
            RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float Wait(Argument[] args)
        {
            CheckArguments(args, "wait", AType.GETABLE);
            RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return args[0].Value;
        }
        public static float Jump(Argument[] args)
        {
            CheckArguments(args, "jmp", AType.GETABLE);
            RG.Rocket.ActiveProgram.CurrentInstructionIndex = (int)args[0].Value;
            return 0.0f;
        }
        public static float JumpLT(Argument[] args)
        {
            CheckArguments(args, "jlt", AType.GETABLE, AType.GETABLE, AType.GETABLE);
            if (args[0].Value < args[1].Value) RG.Rocket.ActiveProgram.CurrentInstructionIndex = (int)args[2].Value;
            else RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float JumpGT(Argument[] args)
        {
            CheckArguments(args, "jgt", AType.GETABLE, AType.GETABLE, AType.GETABLE);
            if (args[0].Value > args[1].Value) RG.Rocket.ActiveProgram.CurrentInstructionIndex = (int)args[2].Value;
            else RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float JumpEQ(Argument[] args)
        {
            CheckArguments(args, "jeq", AType.GETABLE, AType.GETABLE, AType.GETABLE);
            if (Math.Abs(args[0].Value - args[1].Value) < .01) RG.Rocket.ActiveProgram.CurrentInstructionIndex = (int)args[2].Value;
            else RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
        public static float Out(Argument[] args)
        {
            CheckArguments(args, "out", AType.GETABLE);
            Console.WriteLine(args[0].Value);
            RG.Rocket.ActiveProgram.CurrentInstructionIndex++;
            return 0.0f;
        }
    }
    public class Instruction
    {
        public Argument[] args;
        public Opcode Opcode;

        public Instruction(Opcode opcode, Argument[] args)
        {
            this.args = args;
            Opcode = opcode;
        }
        public static Instruction Parse(string line)
        {
            string[] words = line.Split(" ");
            Opcode opcode = Opcode.GetOpcode(words[0]);
            Argument[] args = new Argument[words.Length - 1];
            for(int i = 0; i < args.Length; i++)
            {
                switch(words[i+1][0])
                {
                    case '$':
                        args[i] = new MEMArg(words[i + 1].Substring(1));
                        break;
                    case '#':
                        args[i] = new LITArg(words[i + 1].Substring(1));
                        break;
                    case '!':
                        args[i] = new REGArg(words[i + 1].Substring(1));
                        break;
                }
            }
            return new Instruction(opcode, args);
        }

        public float Run() => Opcode.Func.Invoke(args);
    }
    public class Opcode
    {
        public string Label;
        public Func<Argument[], float> Func;
        private Opcode(string l, Func<Argument[], float> func)
        {
            Label = l;
            Func = func;
        }
        public static Opcode GetOpcode(string code)
        {
            foreach (var opc in Opcodes)
            {
                if (opc.Label == code)
                    return opc;
            }
            return null;
        }
        public static Opcode[] Opcodes = new Opcode[] { 
            new Opcode("add", Instructions.Add), new Opcode("sub", Instructions.Sub), new Opcode("mul", Instructions.Mul), new Opcode("div", Instructions.Div), new Opcode("mod", Instructions.Mod),
            new Opcode("jmp", Instructions.Jump),new Opcode("jlt", Instructions.JumpLT),new Opcode("jgt", Instructions.JumpGT),new Opcode("jeq", Instructions.JumpEQ),
            new Opcode("wait", Instructions.Wait), new Opcode("set",Instructions.Set), new Opcode("out",Instructions.Out)};
    }
}
