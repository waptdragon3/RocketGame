using System;
using System.Collections.Generic;
using System.Text;

namespace RocketGame.Maths
{
    public struct Vector2
    {
        public float X, Y;
        public float Mag => MathF.Sqrt(X * X + Y * Y);
        public float SMag => X * X + Y * Y;
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public float Direction => MathF.Atan(X / -Y)* 57.2958f;

        public static Vector2 One => new Vector2(1, 1);
        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 Down => new Vector2(0, 1);
        public static Vector2 Up => new Vector2(0, -1);

        public static Vector2 FromAngle(float angle)
        {
            return new Vector2(MathF.Sin(angle* 0.0174533f), -MathF.Cos(angle* 0.0174533f));
        }

        public static implicit operator SFML.System.Vector2f(Vector2 v) => new SFML.System.Vector2f(v.X, v.Y);
        public static implicit operator Vector2(SFML.System.Vector2f v) => new Vector2(v.X, v.Y);

        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        public static Vector2 operator *(Vector2 v, float f) => new Vector2(v.X * f, v.Y * f);
        public static Vector2 operator *(float f, Vector2 v) => new Vector2(v.X * f, v.Y * f);
        public static Vector2 operator /(Vector2 v, float f) => new Vector2(v.X / f, v.Y / f);

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}
