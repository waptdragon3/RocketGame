using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using RocketGame.Maths;

namespace RocketGame
{
    class RG
    {
        public static Vector2 ScreenSize;
        public static float FrameTime;
        public static bool Paused = false;
        public static Planet Planet { get; private set; }
        public static Rocket Rocket;
        public static Font font;
        static void Main(string[] args)
        {
            Rocket = new Rocket();
            Planet = new Moon();
            ScreenSize = new Vector2(1280, 720);
            Log("Total Dv = " + Rocket.DVRemaining);
            RenderWindow window = new RenderWindow(new VideoMode((uint)ScreenSize.X, (uint)ScreenSize.Y), "Rocket Game");
            window.Closed += Window_Closed;
            window.KeyPressed += Window_KeyPressed;
            Clock clock = new Clock();
            font = new Font("resources/prstart.ttf");
            UI ui = new UI(window);
            while(window.IsOpen)
            {
                FrameTime = clock.Restart().AsSeconds();
                window.DispatchEvents();

                window.Clear(Color.Black);
                Rocket.Update();
                window.Draw(Rocket.ActiveSprite);

                Text dv = new Text(Rocket.Velocity.Mag.ToString("0.00") + " m/s", font) { Position = new Vector2(ScreenSize.X * .9f, 0) };
                window.Draw(dv);

                ui.Draw();

                window.Display();
            }
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Space)
            {
                Rocket.ActiveProgram.Halted = !Rocket.ActiveProgram.Halted;
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            ((Window)sender).Close();
        }
        public static void Log(object o)
        {
            Console.WriteLine(o);
        }
    }

}
