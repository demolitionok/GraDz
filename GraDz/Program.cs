using System;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraDz
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press ESC key to close window");
            var window = new SimpleWindow();
            window.Run();

            Console.WriteLine("All done");
        }
    }
    class SimpleWindow
    {
        List<Obstacle> obstacles = new List<Obstacle>();
        Character character = new Character();
        RenderWindow window;

        public void ObstaclesGen()
        {
            for (int i = 0; i < 10; i++)
                obstacles.Add(new Obstacle());
            obstacles.Add(new Finish());

            obstacles[0].body.Position = new Vector2f(200, 400);
            obstacles[1].body.Position = new Vector2f(300, 400);
            obstacles[2].body.Position = new Vector2f(400, 400);
            obstacles[3].body.Position = new Vector2f(500, 400);
            obstacles[4].body.Position = new Vector2f(600, 400);
            obstacles[5].body.Position = new Vector2f(700, 400);
            obstacles[6].body.Position = new Vector2f(200, 300);
            obstacles[7].body.Position = new Vector2f(300, 300);
            obstacles[8].body.Position = new Vector2f(400, 300);
            obstacles[9].body.Position = new Vector2f(500, 300);
            obstacles[10].body.Position = new Vector2f(700, 500);
        }
        public void Run()
        {
            var mode = new VideoMode(800, 600);
            window = new RenderWindow(mode, "SFML works!");
            ObstaclesGen();
            window.Closed += (_, __) => window.Close();
            window.KeyPressed += Window_KeyPressed;


            // Start the game loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();
                DetectCollision(obstacles);
                window.Draw(character.body);
                foreach (Obstacle obstacle in obstacles)
                {
                    window.Draw(obstacle.body);
                }

                // Finally, display the rendered frame on screen
                window.Display();
                window.Clear();
            }
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
            if (e.Code == Keyboard.Key.W)
            {
                character.body.Position += new Vector2f(0, -Character.charSpeed);
            }
            if (e.Code == Keyboard.Key.A)
            {
                character.body.Position += new Vector2f(-Character.charSpeed, 0);
            }
            if (e.Code == Keyboard.Key.S)
            {
                character.body.Position += new Vector2f(0, Character.charSpeed);
            }
            if (e.Code == Keyboard.Key.D)
            {
                character.body.Position += new Vector2f(Character.charSpeed, 0);
            }
        }


        public void DetectCollision(List<Obstacle> obstacles)
        {
            foreach (Obstacle obstacle in obstacles)
            {
                if (
                    (
                       (
                           character.body.Position.X + Character.bodySize >= obstacle.body.Position.X &&
                           character.body.Position.X + Character.bodySize <= obstacle.body.Position.X + Obstacle.bodySize
                       ) || (
                           character.body.Position.X <= obstacle.body.Position.X + Obstacle.bodySize &&
                           character.body.Position.X >= obstacle.body.Position.X
                       )
                   ) && (
                       (
                           character.body.Position.Y + Character.bodySize >= obstacle.body.Position.Y &&
                           character.body.Position.Y + Character.bodySize <= obstacle.body.Position.Y + Obstacle.bodySize
                       ) || (
                           character.body.Position.Y <= obstacle.body.Position.Y + Obstacle.bodySize &&
                           character.body.Position.Y >= obstacle.body.Position.Y
                       )
                   )
               )
                {
                    obstacle.OnCollide(character, window);
                }
            }
        }
    }
    class Character
    {
        public RectangleShape body;
        
        public static float charSpeed = 5f;
        public static float bodySize = 15f;

        public static Color charColor = Color.Green;
        public Character()
        {
            body = new RectangleShape(new Vector2f(bodySize, bodySize))
            {
                FillColor = charColor
            };
            //body.Origin = new Vector2f(bodySize / 2, bodySize / 2);
        }
    }

    class Obstacle
    {
        public Vector2f teleportPos = new Vector2f(0, 0);
        public RectangleShape body;
        public static float bodySize = 15f;
        public static Color obstacleColor = Color.Cyan;

        public virtual void OnCollide(Character character, RenderWindow window) 
        {
            character.body.Position = teleportPos;
        }
        public Obstacle()
        {
            body = new RectangleShape(new Vector2f(bodySize, bodySize))
            {
                FillColor = obstacleColor
            };
            //body.Origin = new Vector2f(bodySize/2, bodySize/2);
        }
    }
    class Finish : Obstacle 
    {
        public static Color finishColor = Color.Red;

        public override void OnCollide(Character character, RenderWindow window) 
        {
            var texture = new Texture("Untitled.png");
            var sprite = new Sprite(texture);
            window.Draw(sprite);
            window.Display();
            Thread.Sleep(5000);
            window.Close();
        }
        public Finish() : base()
        {
            body.FillColor = finishColor;
        }
    }
}
