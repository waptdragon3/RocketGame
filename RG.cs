using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace RocketGame
{
    class RG
    {
        public static float FrameTime;
        public static bool Paused = false;
        public static Planet Planet { get; private set; }
        public static Rocket Rocket;
        static void Main(string[] args)
        {
            Rocket = new Rocket();
            Planet = new Moon();
            Console.WriteLine("Total Dv = " + Rocket.DVRemaining);
            RenderWindow window = new RenderWindow(new VideoMode(1280, 720), "Rocket Game");
            window.Closed += Window_Closed;
            Clock clock = new Clock();
            Font font = new Font("resources/prstart.ttf");
            UI ui = new UI(window);
            while(window.IsOpen)
            {
                FrameTime = clock.Restart().AsSeconds();
                window.DispatchEvents();

                window.Clear(Color.Black);
                Rocket.Update();
                window.Draw(Rocket.ActiveSprite);

                Text dv = new Text(Rocket.Velocity.Mag.ToString("0.00")+ " m/s", font);
                window.Draw(dv);

                ui.Draw();

                window.Display();
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            ((Window)sender).Close();
        }
    }
}
