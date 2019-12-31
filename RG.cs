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
        public static Font font;
        static void Main(string[] args)
        {
            Rocket = new Rocket();
            Planet = new Moon();
            Console.WriteLine("Total Dv = " + Rocket.DVRemaining);
            RenderWindow window = new RenderWindow(new VideoMode(1280, 720), "Rocket Game");
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

                Text dv = new Text(Rocket.Velocity.Mag.ToString("0.00")+ " m/s", font);
                window.Draw(dv);

                ui.Draw();

                window.Display();
            }
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Space)
            {
                Rocket.ActiveProgram.Running = !Rocket.ActiveProgram.Running;
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            ((Window)sender).Close();
        }
    }
}
