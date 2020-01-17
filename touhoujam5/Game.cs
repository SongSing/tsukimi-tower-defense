using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class Game
    {
        public static int TileSize = 32;
        public static PlayArea PlayArea;
        public static List<Tower> Towers = new List<Tower>();
        public static IEnumerable<Tower> PlacedTowers
        {
            get
            {
                foreach (Tower tower in Towers)
                {
                    if (tower.IsPlaced)
                    {
                        yield return tower;
                    }
                }
            }
        }
        public static IEnumerable<Tower> UnplacedTowers
        {
            get
            {
                foreach (Tower tower in Towers)
                {
                    if (!tower.IsPlaced)
                    {
                        yield return tower;
                    }
                }
            }
        }

        public static Vector2i MousePosition => Mouse.GetPosition(Window);
        public static Vector2f MousePositionf => Utils.Vi2f(Mouse.GetPosition(Window));

        public static RenderWindow Window;
        private float framerate = 1 / 60.0f;
        private float counter = 0.0f;
        private Clock clock;
        private PlayArea _playArea;
        private TowerInventory _inventory;

        public Game() : this(900, 700) { }

        public Game(uint width, uint height)
        {
            RenderWindow window = new RenderWindow(new VideoMode(width, height), "TOU8HOU JAM 5 BABEY");
            window.SetVerticalSyncEnabled(true);
            window.SetActive();
            Window = window;

            AKS.InitializeWindow(window);

            this.clock = new Clock();

            window.Closed += (sender, e) =>
            {
                window.Close();
            };

            window.KeyPressed += (sender, e) =>
            {
                if (e.Code == Keyboard.Key.Escape)
                {
                    window.Close();
                }
            };

            _playArea = new PlayArea(20, 20);
            PlayArea = _playArea;
            _playArea.SetLevel(new Level1());

            _inventory = new TowerInventory(new Vector2f(20 * Game.TileSize, 0), new Vector2i(8, 8));

            this.Run();
        }

        protected void Run()
        {
            while (Window.IsOpen)
            {
                AKS.Update();

                float delta = this.clock.Restart().AsSeconds();
                this.counter += delta;
                Window.DispatchEvents();

                if (this.counter >= this.framerate)
                {
                    int times = (int)(this.counter / this.framerate);
                    this.counter %= this.framerate;

                    for (int i = 0; i < times; i++)
                    {
                        this.Update(this.framerate);
                    }
                }

                this.Draw(Window);
            }
        }

        protected void Update(float delta)
        {
            _playArea.Update(delta);
            _inventory.Update(delta);
        }

        protected void Draw(RenderWindow window)
        {
            window.Clear();
            _playArea.Draw(window);
            _inventory.Draw(window);
            window.Display();
        }
    }
}
