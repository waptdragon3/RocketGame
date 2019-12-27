using System;
using System.Collections.Generic;
using System.Text;
using RocketGame.Maths;
using SFML.Graphics;

namespace RocketGame
{
    public class UI
    {
        private Gauge FuelGauge;
        public UI(RenderTarget screen)
        {
            Vector2 fuelGaugeSize = new Vector2(1280 * .25f, 720 * .05f);
            FuelGauge = new Gauge(Color.Green, fuelGaugeSize, new Vector2(1280, fuelGaugeSize.Y)-fuelGaugeSize*.5f, screen);
        }
        public void Draw()
        {
            FuelGauge.Percent = RG.Rocket.FuelPercent;
            FuelGauge.render();
        }

    }
    class Gauge
    {
        private RectangleShape rect;
        private Vector2 maxSize;
        private Vector2 position;
        private float percent;
        private RenderTarget rt;
        public float Percent { get => percent; set => setPercent(value); }
        public Gauge(Color color, Vector2 maxSize, Vector2 position, RenderTarget screen)
        {
            this.position = position;
            this.maxSize = maxSize;
            rect = new RectangleShape(maxSize) { FillColor = color, Position = position, Origin = maxSize * .5f };
            percent = 1;
            rt = screen;
        }

        private void setPercent(float per)
        {
            percent = per;
            Vector2 newSize = new Vector2(maxSize.X * percent, maxSize.Y);
            rect.Size = newSize;
            rect.Origin = newSize * .5f;
            rect.Position = position + Vector2.Right * (1 - percent) * maxSize.X*.5f;
        }

        public void render()
        {
            rt.Draw(rect);
        }
    }
}
