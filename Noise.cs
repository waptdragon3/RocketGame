using System;
using System.Collections.Generic;
using System.Text;
using RocketGame.Maths;
namespace RocketGame
{
    //Based on the answer here: https://gamedev.stackexchange.com/questions/10984/what-is-the-simplest-method-to-generate-smooth-terrain-for-a-2d-game
    public class Noise
    {
        private Vector2[] anchorPoints;
        public float PotMax { get; private set; }
        public float PotMin { get; private set; }
        public Noise(uint divisions, float persistance, float width)
        {
            Generate(divisions, persistance, width);
        }
        public float GetValue(float x)
        {
            if (Math.Abs(x) < .01)
                return anchorPoints[0].Y;
            for(int i = 0; i < anchorPoints.Length; i++)
            {
                if (anchorPoints[i].X > x)
                {
                    Vector2 left = anchorPoints[(int)Math.Floor(i / 2f)];
                    Vector2 right = anchorPoints[(int)Math.Ceiling(i / 2f)];
                    return (left.Y + right.Y) / 2f;
                }
            }
            return anchorPoints[anchorPoints.Length - 1].Y;
        }

        private void Generate(uint divisions, float persistance, float width)
        {
            anchorPoints = new Vector2[] { new Vector2(0, 0), new Vector2(width, 0) };
            float strength = 1;
            Random r = new Random(0);
            for(int i  = 0; i < divisions; i++)
            {
                Vector2[] newPoints = new Vector2[1 + (int)Math.Pow(2,i)];
                for(int j = 0; j < newPoints.Length; j++)
                {
                    if (j % 2 == 0) newPoints[j] = anchorPoints[j / 2];
                    else
                    {
                        Vector2 left = anchorPoints[(int)Math.Floor(j / 2f)];
                        Vector2 right = anchorPoints[(int)Math.Ceiling(j / 2f)];
                        Vector2 newV = (left + right) / 2f + Vector2.Up * ((float)r.NextDouble() - .5f) * strength;
                        newPoints[j] = newV;
                    }
                }
                strength *= persistance;
                anchorPoints = newPoints;
            }
        }
    }
}
