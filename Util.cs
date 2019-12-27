using System;
using System.Collections.Generic;
using System.Text;

namespace RocketGame
{
    public static class Util
    {
        public static float Bounds(float v, float min, float max) => MathF.Min(max, MathF.Max(v, min));
    }
}
