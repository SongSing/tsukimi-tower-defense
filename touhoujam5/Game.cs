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
        public static TowerInventory TowerInventory;
        public static List<Tower> Towers = new List<Tower>();
        public static float Money = 250;
        public static Font Font = Resources.Font("Content/unifont_jp-12.1.04.ttf");
        public static Level[] Levels = new Level[] { new Level1(), new Level2() };
        public static int CurrentLevelNum = 0;

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
        private RectangleShape _hpBackground, _hpForeground, _moneyBackground, _startBackground;
        private TowerInventory _inventory;
        private InfoDisplay _infoDisplay;
        private Text _moneyText, _startText;

        public Game() : this((uint)TileSize * 28, (uint)TileSize * 21) { }

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

            _playArea = new PlayArea(new Vector2i(20, 20), new Vector2f(0, 0));
            _playArea.SetLevel(new Level1());

            _hpBackground = new RectangleShape(new Vector2f(TileSize * 20, TileSize));
            _hpForeground = new RectangleShape(new Vector2f(TileSize * 20, TileSize));

            _hpBackground.FillColor = Color.Red;
            _hpForeground.FillColor = Color.Green;

            _hpBackground.OutlineColor = _hpForeground.OutlineColor = Color.Black;
            _hpBackground.OutlineThickness = _hpForeground.OutlineThickness = 1;

            _hpBackground.Position = _hpForeground.Position = new Vector2f(0, TileSize * 20);

            _moneyBackground = new RectangleShape(new Vector2f(TileSize * 4, TileSize * 2))
            {
                FillColor = new Color(50, 50, 50, 255),
                Position = new Vector2f(TileSize * 20, TileSize * (21 - 2))
            };

            _startBackground = new RectangleShape(new Vector2f(TileSize * 4, TileSize * 2))
            {
                FillColor = new Color(50, 50, 100, 255),
                Position = new Vector2f(TileSize * 24, TileSize * 19)
            };

            _inventory = new TowerInventory(new Vector2f(20 * Game.TileSize, 0), new Vector2i(8, 8));
            _infoDisplay = new InfoDisplay(new Vector2f(20 * TileSize, 8 * TileSize), new Vector2u((uint)(8 * TileSize), (uint)(11 * TileSize)));

            _moneyText = new Text(Money.ToString(), Font)
            {
                Position = Utils.RoundV(_moneyBackground.Position + _moneyBackground.Size / 2),
                CharacterSize = 22
            };

            _startText = new Text("Start", Font)
            {
                Position = Utils.RoundV(_startBackground.Position + _startBackground.Size / 2),
                CharacterSize = 22
            };

            PlayArea = _playArea;
            TowerInventory = _inventory;

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
            _infoDisplay.Update(delta);

            if (_playArea.IsFinished)
            {
                Money += Levels[CurrentLevelNum].Reward;

                switch (CurrentLevelNum)
                {
                    case 0:
                        Towers.Add(new MarisaTower());
                        break;
                }

                _playArea.SetLevel(Levels[++CurrentLevelNum]);
            }

            if (AKS.WasJustPressed(Mouse.Button.Left) && Utils.PointIsInRect(MousePosition, _startBackground))
            {
                if (_playArea.IsStarted)
                {
                    _playArea.IsPaused = !_playArea.IsPaused;
                }
                else
                {
                    _playArea.Start();
                }
            }
        }

        protected void Draw(RenderWindow window)
        {
            window.Clear();

            _playArea.Draw(window);
            _inventory.Draw(window);
            _infoDisplay.Draw(window);

            _hpForeground.Size = new Vector2f(_hpBackground.Size.X * (_playArea.Hp / _playArea.MaxHp), _hpBackground.Size.Y);
            window.Draw(_hpBackground);
            window.Draw(_hpForeground);
            window.Draw(_moneyBackground);
            window.Draw(_startBackground);

            _moneyText.DisplayedString = "点 " + Money.ToString();
            _moneyText.Origin = Utils.RoundV(new Vector2f(_moneyText.GetLocalBounds().Width / 2, _moneyText.GetLocalBounds().Height / 2));
            window.Draw(_moneyText);

            _startText.DisplayedString = PlayArea.IsStarted ? (PlayArea.IsPaused ? "Resume" : "Pause") : "Start";
            _startText.Origin = Utils.RoundV(new Vector2f(_startText.GetLocalBounds().Width / 2, _moneyText.GetLocalBounds().Height / 2));
            window.Draw(_startText);

            window.Display();
        }
    }
}
