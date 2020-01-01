using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace RocketGame
{
    public class Console
    {
        readonly Font font;
        List<string> lines;
        readonly uint charSize;
        Maths.Vector2 size;
        private RenderWindow window;

        public Console(Font f, uint charSize, Maths.Vector2 size)
        {
            font = f;
            this.charSize = charSize;
            this.size = size;
            window = new RenderWindow(new SFML.Window.VideoMode((uint)size.X, (uint)size.Y), "Console");
            lines = new List<string>();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            window.SetVisible(false);
        }

        public void Render()
        {
            window.Clear();
            window.DispatchEvents();

            for (int i = 0; i < lines.Count; i++)
            {
                Text text = new Text(lines[i], font, charSize) { Position = Maths.Vector2.Down * (charSize + 1) * i };
                //System.Console.WriteLine(lines[i]);
                window.Draw(text);
            }

            window.Display();
        }
        public void Close() => window.Close();
        public void Log(Exception e)
        {
            lines.Add(string.Format("ERROR: {0}", e.Message));
        }
        public void Log(object o)
        {
            if (o is Exception)
                Log((Exception)o);
            else
                lines.Add(o.ToString());
        }
    }
}
