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
        private ToggleButton StartButton;
        public UI(RenderWindow screen)
        {
            Vector2 fuelGaugeSize = new Vector2(1280 * .25f, 720 * .05f);
            FuelGauge = new Gauge(Color.Green, fuelGaugeSize, new Vector2(1280, fuelGaugeSize.Y*3)-fuelGaugeSize*.5f, screen);
            ToggleButton.State[] states = new ToggleButton.State[] {new ToggleButton.State("Start", new RectangleShape(new Vector2(1280 * .1f, 720 * .05f)) { FillColor = Color.Green }),
                                                                    new ToggleButton.State("Reset", new RectangleShape(new Vector2(1280 * .1f, 720 * .05f)) { FillColor = Color.Red }) };
            StartButton = new ToggleButton(new Vector2(1280*.94f, 720*.95f), states, screen);
            screen.MouseButtonPressed += StartButton.Screen_MouseButtonPressed;
            StartButton.OnPush += StartPressed;
        }

        public void Draw()
        {
            FuelGauge.Percent = RG.Rocket.FuelPercent;
            FuelGauge.render();

            StartButton.render();
        }

        private void StartPressed()
        {
            if (RG.Rocket.ActiveProgram.Running)
            {
                RG.Rocket.ActiveProgram.Running = false;
                RG.Rocket.Reset();
            }else RG.Rocket.ActiveProgram.Running = true;
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
    class Button
    {
        Vector2 Position;
        protected string Text;
        protected RectangleShape Shape;
        private RenderWindow Screen;
        public delegate void OnPushDel();
        public event OnPushDel OnPush;

        public Button(Vector2 position, string text, RectangleShape shape, RenderWindow screen)
        {
            Position = position;
            Text = text;
            Shape = shape;
            Screen = screen;
            Shape.Position = Position;
            Shape.Origin = Shape.Size / 2f;
        }
        
        public void render()
        {
            Text t = new Text(Text, RG.font, 12) { Position = Position };
            FloatRect tBounds = t.GetLocalBounds();
            t.Origin = new Vector2(tBounds.Width, tBounds.Height) * .5f;
            Screen.Draw(Shape);
            Screen.Draw(t);
        }

        public void Screen_MouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            Vector2 mousePos = SFML.Window.Mouse.GetPosition(Screen);
            Vector2 relativePos = mousePos - Position + (Vector2)Shape.Size / 2f;
            if (relativePos.X > 0 && relativePos.X < Shape.Size.X)
                if (relativePos.Y > 0 && relativePos.Y < Shape.Size.Y)
                    OnPush?.Invoke();
        }
    }
    class ToggleButton : Button
    {
        private State[] states;
        private int stateIndex;
        public struct State
        {
            public string Text;
            public RectangleShape shape;

            public State(string text, RectangleShape shape)
            {
                Text = text;
                this.shape = shape;
            }
        }
        public ToggleButton(Vector2 position, State[] states, RenderWindow window):base(position, states[0].Text, states[0].shape, window)
        {
            this.states = states;
            stateIndex = 0;
            this.OnPush += ToggleButton_OnPush;
            foreach (var s in this.states)
            {
                s.shape.Position = position;
                s.shape.Origin = s.shape.Size / 2f;
            }
        }

        private void ToggleButton_OnPush()
        {
            stateIndex = (stateIndex + 1) % states.Length;
            this.Text = states[stateIndex].Text;
            this.Shape = states[stateIndex].shape;
        }
    }

}
