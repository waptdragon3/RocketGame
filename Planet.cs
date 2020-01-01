using System;
using System.Collections.Generic;
using System.Text;
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
        public Noise Noise;
        protected float width;

        public Planet()
        {
            width = 1024;
            Noise = new Noise(10, .5f, width);
        }

        public virtual float GetHeight(float x)
        {
            return Util.Bounds(Noise.GetValue(x+width/2), -1, 1) * (MaxHeight - MaxDepth) + MaxDepth;
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
