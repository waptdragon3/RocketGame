using System;
using System.Collections.Generic;
using System.Text;
using LibNoise;
using RocketGame.Maths;

namespace RocketGame
{
    public class Planet
    {
        //default values are going to be Earth
        public float Gravity = 9.81f;
        public Vector2 StartingVel;
        public float MaxHeight = 10;
        public float MaxDepth = -10;
        protected LibNoise.Primitive.SimplexPerlin perlin;

        public Planet()
        {
            perlin = new LibNoise.Primitive.SimplexPerlin(0, NoiseQuality.Best);
        }

        public virtual float GetHeight(float x)
        {
            return Util.Bounds(perlin.GetValue(x), -1, 1) * (MaxHeight - MaxDepth) + MaxDepth;
        }
    }
    /// <summary>
    /// I know, but whatever
    /// </summary>
    public class Moon:Planet
    {
        public Moon()
        {
            Gravity = 1.62f;
        }
    }
}
