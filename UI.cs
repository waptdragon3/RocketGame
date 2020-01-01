using System;
using System.Collections.Generic;
using System.Text;
using RocketGame.Maths;
using SFML.Graphics;
using RocketGame.FlightSystem;

namespace RocketGame
{
    public class UI
    {
        private Gauge FuelGauge;
        private ToggleButton StartButton;
        private TextField ProgramInput;
        public UI(RenderWindow screen)
        {
            Vector2 fuelGaugeSize = new Vector2(RG.ScreenSize.X * .25f, RG.ScreenSize.Y * .05f);
            FuelGauge = new Gauge(Color.Green, fuelGaugeSize, new Vector2(RG.ScreenSize.X, fuelGaugeSize.Y*3)-fuelGaugeSize*.5f, screen);

            ToggleButton.State[] states = new ToggleButton.State[] {new ToggleButton.State("Start", new RectangleShape(new Vector2(RG.ScreenSize.X * .1f, RG.ScreenSize.Y * .05f)) { FillColor = Color.Green }),
                                                                    new ToggleButton.State("Reset", new RectangleShape(new Vector2(RG.ScreenSize.X * .1f, RG.ScreenSize.Y * .05f)) { FillColor = Color.Red }) };
            StartButton = new ToggleButton(new Vector2(RG.ScreenSize.X * .94f, RG.ScreenSize.Y * .95f), states, screen);
            screen.MouseButtonPressed += StartButton.Screen_MouseButtonPressed;
            StartButton.OnPush += StartPressed;

            Vector2 textInputSize = new Vector2(RG.ScreenSize.X * .25f, RG.ScreenSize.Y * .95f);
            ProgramInput = new TextField("Enter Program Here:", textInputSize * .5f + Vector2.One * 10, textInputSize, screen);
            screen.TextEntered += ProgramInput.Screen_TextEntered;
            screen.MouseButtonPressed += ProgramInput.Screen_MouseButtonPressed;
            screen.KeyPressed += ProgramInput.Screen_KeyPressed;
        }

        

        public void Draw()
        {
            FuelGauge.Percent = RG.Rocket.FuelPercent;
            FuelGauge.render();

            StartButton.render();
            ProgramInput.render();
        }

        private void StartPressed()
        {
            if (RG.Rocket.Running)
            {
                RG.Rocket.Running = false;
                RG.Rocket.Reset();
            }
            else
            {
                try
                {
                    Program p = Program.Parse(ProgramInput.Text.ToArray());
                    RG.Rocket.ActiveProgram = p;

                    RG.Rocket.Running = true;
                } catch (Exception e)
                {
                    RG.Rocket.Running = false;
                    RG.Log(e);
                    StartButton.StateIndex = 0;
                }
            }
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

        public int StateIndex { get => stateIndex; set { stateIndex = value; updateState(); } }

        private void updateState()
        {
            this.Text = states[StateIndex].Text;
            this.Shape = states[StateIndex].shape;
        }

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
            StateIndex = 0;
            this.OnPush += ToggleButton_OnPush;
            foreach (var s in this.states)
            {
                s.shape.Position = position;
                s.shape.Origin = s.shape.Size / 2f;
            }
        }

        private void ToggleButton_OnPush()
        {
            StateIndex = (StateIndex + 1) % states.Length;
            updateState();
        }
    }
    class TextField
    {
        public List<string> Text;
        private bool Selected;
        int CursorY;
        int CursorX;
        private Vector2 Position;
        private Vector2 Size;
        private RenderWindow Screen;

        public TextField(string text, Vector2 position, Vector2 size, RenderWindow screen)
        {
            Text = new List<string>
            {
                text
            };
            Position = position;
            Size = size;
            Screen = screen;
            Selected = false;
        }

        public void render()
        {
            RenderTexture target = new RenderTexture((uint)Size.X, (uint)Size.Y);
            for(int i = 0; i < Text.Count; i++)
            {
                string t = i.ToString("00:")+Text[i];
                if (i == CursorY)
                    t = i.ToString("00:")+((Text[i].Length==0)?"|":Text[i].Insert(CursorX, "|"));
                target.Draw(new Text(t, RG.font, 12) { Position = Vector2.Down * 13 * i });
            }
            RectangleShape rect = new RectangleShape(Size) { Texture = target.Texture, Origin = Size * .5f, Position = Position, OutlineThickness=2f, OutlineColor = (Selected?Color.Green: Color.White), Scale = new Vector2(1,-1) };
            Screen.Draw(rect);
        }
        public void Screen_MouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            Selected = false;
            Vector2 mousePos = SFML.Window.Mouse.GetPosition(Screen);
            Vector2 relativePos = mousePos - Position + Size / 2f;
            if (relativePos.X > 0 && relativePos.X < Size.X)
                if (relativePos.Y > 0 && relativePos.Y < Size.Y)
                    Selected = true;
        }
        public void Screen_TextEntered(object sender, SFML.Window.TextEventArgs e)
        {
            if (Selected)
            {
                //RG.Log(string.Format("#{0:X}", (int)e.Unicode[0]));
                if (e.Unicode == "\u0008") //backspace
                {
                    if (CursorX == 0 && CursorY == 0) return;
                    if(CursorX == 0 && CursorY != 0)
                    {
                        Text[CursorY - 1] += Text[CursorY];
                        Text.RemoveAt(CursorY);
                        CursorY--;
                        CursorX = Text[CursorY].Length;
                    }
                    else
                    {
                        Text[CursorY] = Text[CursorY].Remove(CursorX - 1, 1);
                        CursorX--;
                    }
                }
                else if(e.Unicode == "\u000d") //newline
                {
                    string l = Text[CursorY];
                    Text[CursorY] = l.Substring(0, CursorX);
                    Text.Insert(CursorY+1, l.Substring(CursorX));
                    CursorY ++;
                    CursorX = 0;
                }
                else
                {
                    Text[CursorY] = Text[CursorY].Insert(CursorX,e.Unicode);
                    CursorX++;
                }
            }
        }
        public void Screen_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            if (Selected)
            {
                if (e.Code == SFML.Window.Keyboard.Key.Left)
                {
                    CursorX--;
                    if (CursorX < 0)
                    {
                        CursorX = 0;
                        CursorY = Math.Max(0, CursorY - 1);
                    }
                }
                else if (e.Code == SFML.Window.Keyboard.Key.Right)
                {
                    CursorX++;
                    if (CursorX > Text[CursorY].Length)
                    {
                        CursorY = Math.Min(Text.Count - 1, CursorY + 1);
                        CursorX = Text[CursorY].Length;
                    }
                }
                else if (e.Code == SFML.Window.Keyboard.Key.Up)
                {
                    CursorY--;
                    if (CursorY < 0) { CursorY = 0; CursorX = 0; }
                    CursorX = Math.Min(Text[CursorY].Length, Math.Max(0, CursorX));
                }
                else if (e.Code == SFML.Window.Keyboard.Key.Down)
                {
                    CursorY++;
                    if (CursorY > Text.Count - 1) { CursorY = Text.Count-1; CursorX = 9999; }
                    CursorX = Math.Min(Text[CursorY].Length, Math.Max(0, CursorX));
                }

            }
        }
    }
}
