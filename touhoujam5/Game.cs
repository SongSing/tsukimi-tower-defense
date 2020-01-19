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
        public static readonly float StartingMoney = 450;
        public static float Money = StartingMoney;
        private static float _moneyAtStartOfLevel = Money;
        public static Font Font = Resources.Font("Content/unifont_jp-12.1.04.ttf");
        public static Music Music;
        public static Level[] Levels => new Level[] { new Level1(), new Level2() };
        public static int CurrentLevelNum = 0;
        public static int DefaultMusicVolume = 75;
        public static TitleScreen TitleScreen;
        public static GameOverScreen GameOverScreen;
        public static bool IsGameOver = false;
        public static List<string> TextQueue = new List<string>();

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
        private RectangleShape _hpBackground, _hpForeground, _moneyBackground, _startBackground, _textBackground, _dimBackground;
        private TowerInventory _inventory;
        private InfoDisplay _infoDisplay;
        private Text _moneyText, _startText, _textText;
        private Vector2f _viewSize;

        public Game() : this((uint)TileSize * 28, (uint)TileSize * 21) { }

        public Game(uint width, uint height)
        {
            RenderWindow window = new RenderWindow(new VideoMode(width, height), "TOU8HOU JAM 5 BABEY", Styles.Close);
            window.SetVerticalSyncEnabled(true);
            window.SetActive();
            Window = window;

            _viewSize = new Vector2f(width, height);
            View v = new View(new FloatRect(new Vector2f(0, 0), _viewSize));
            Window.SetView(v);

            //Window.Resized += (sender, eeee) =>
            //{
            //    Window.SetView(this.CalcView());
            //};

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

            TitleScreen = new TitleScreen();
            GameOverScreen = new GameOverScreen();

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
                FillColor = new Color(50, 50, 100, 255),
                Position = new Vector2f(TileSize * 20, TileSize * (21 - 2))
            };
            _moneyText = new Text(Money.ToString(), Font)
            {
                Position = Utils.RoundV(_moneyBackground.Position + _moneyBackground.Size / 2),
                CharacterSize = 22
            };

            _startBackground = new RectangleShape(new Vector2f(TileSize * 4, TileSize * 2))
            {
                FillColor = new Color(50, 100, 50, 255),
                Position = new Vector2f(TileSize * 24, TileSize * 19)
            };
            _startText = new Text("Start", Font)
            {
                Position = Utils.RoundV(_startBackground.Position + _startBackground.Size / 2),
                CharacterSize = 22
            };

            _textBackground = new RectangleShape(new Vector2f(TileSize * 28, TileSize * 4))
            {
                FillColor = new Color(0, 0, 0, 255),
                OutlineColor = Color.White,
                OutlineThickness = 2,
                Position = new Vector2f(0, TileSize * 17)
            };
            _textText = new Text("hi :)", Font)
            {
                Position = new Vector2f(8, TileSize * 17 + 8),
                CharacterSize = 22
            };

            _dimBackground = new RectangleShape(new Vector2f(TileSize * 28, TileSize * 21))
            {
                FillColor = new Color(0, 0, 0, 200)
            };

            _inventory = new TowerInventory(new Vector2f(20 * Game.TileSize, 0), new Vector2i(8, 2));
            _infoDisplay = new InfoDisplay(new Vector2f(20 * TileSize, 2 * TileSize), new Vector2u((uint)(8 * TileSize), (uint)(17 * TileSize)));

            PlayArea = _playArea;
            TowerInventory = _inventory;

            //Money += 500000;

            //AdvanceLevel();

            ReturnToTitle();

            Music = new Music("Content/bgm.ogg");
            Music.Loop = true;
            Music.Volume = 0;
            Music.Play();

            AKS.WatchKey(Keyboard.Key.M);
            AKS.WatchKey(Keyboard.Key.R);

            this.Run();
        }

        private View CalcView()
        {
            Vector2f windowSize = new Vector2f(Window.Size.X, Window.Size.Y);

            float w = (float)(windowSize.X / _viewSize.X);
            float h = (float)(windowSize.Y / _viewSize.Y);
            float factor = Math.Min(w, h);

            Vector2f scaledSize = new Vector2f(TileSize * 28, TileSize * 21) * factor;
            Vector2f scaledSizeP = new Vector2f(scaledSize.X / windowSize.X, scaledSize.Y / windowSize.Y);
            Vector2f margin = windowSize - scaledSize;
            Vector2f marginP = new Vector2f(margin.X / 2 / windowSize.X, margin.Y / 2 / windowSize.Y);

            View v = new View(new FloatRect(new Vector2f(0, 0), _viewSize))
            {
                Viewport = new FloatRect(marginP, scaledSizeP)
            };

            return v;
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

        private void ReturnToTitle()
        {
            TitleScreen.ReadyToStart = false;
            TextQueue.Clear();
            _inventory.Allowed.Clear();
            _inventory.Allowed.Add(new ReimuTower());
            _inventory.Allowed.Add(new YoumuTower());
            CurrentLevelNum = 0;
            _moneyAtStartOfLevel = StartingMoney;
            ReloadLevel();

            TextQueue.Add("Youmu: Reimu, this is horrible.\nIt's a catastrophe.");
            TextQueue.Add("Reimu: What is it, Youmu? Has there been an incident?");
            TextQueue.Add("Youmu: You know damn well there's been an incident, Reimu.");
            TextQueue.Add("Reimu: But I thought we perished the night?\nMade it perishable... um...\nAnd we even offered food for the Tsukimi Festival!");
            TextQueue.Add("Youmu: That's the problem.\nThe moon doesn't like what we've been offering.");
            TextQueue.Add("Reimu: What a brat...!");
            TextQueue.Add("Youmu: She's brought our offerings to life as youkai.\nLuckily, Yukari has shortened the boundary\nbetween reality and a tower defense game.");
            TextQueue.Add("Reimu: (Was that really the right boundary to adjust...?)");
        }

        private void ReloadLevel()
        {
            Money = _moneyAtStartOfLevel;
            _playArea.SetLevel(Levels[CurrentLevelNum]);
        }

        private void AdvanceLevel()
        {
            Money += Levels[CurrentLevelNum].Reward;

            foreach (Tower tower in PlacedTowers)
            {
                Money += tower.Worth;
            }

            _moneyAtStartOfLevel = Money;

            switch (CurrentLevelNum)
            {
                case 0:
                    _inventory.Allowed.Add(new MarisaTower());
                    _inventory.Allowed.Add(new AliceTower());
                    TextQueue.AddRange(new string[]
                    {
                        "Reimu: This is scary... food isn't supposed to fight back...",
                        "Marisa: What's this about food?",
                        "Youmu: Marisa, we'll need your help.\nYukari says someone tried to offer a \"super food\",\nso we should be expecting fast youkai.",
                        "Alice: Maybe my dolls can help...",
                        "Reimu: So you'll let the youkai play with them, but not me?",
                        "Youmu: Now's no time for playing!",
                        "Marisa: Yeah!\nNow's time for eating! >:~D",
                        "(Note: Press R to reload a level)"
                    });
                    break;
            }

            if (CurrentLevelNum == Levels.Length - 1)
            {
                // TODO: U WIN
            }
            else
            {
                _playArea.SetLevel(Levels[++CurrentLevelNum]);
            }
        }

        protected void Update(float delta)
        {
            if (AKS.WasJustPressed(Keyboard.Key.M))
            {
                Music.Volume = Music.Volume == 0 ? DefaultMusicVolume : 0;
            }

            if (TitleScreen.ReadyToStart)
            {
                if (IsGameOver)
                {
                    GameOverScreen.Update(delta);

                    if (GameOverScreen.Choice == GameOverScreen.ChoiceOption.RetryLevel)
                    {
                        IsGameOver = false;
                        ReloadLevel();
                    }
                    else if (GameOverScreen.Choice == GameOverScreen.ChoiceOption.ReturnToTitle)
                    {
                        IsGameOver = false;
                        ReturnToTitle();
                    }
                }
                else
                {
                    if (TextQueue.Count > 0)
                    {
                        if (AKS.WasJustPressed(Mouse.Button.Left))
                        {
                            TextQueue.RemoveAt(0);
                        }
                    }
                    else
                    {
                        if (AKS.WasJustPressed(Keyboard.Key.R))
                        {
                            Towers.Clear();
                            ReloadLevel();
                        }
                        else
                        {
                            _inventory.Update(delta);
                            _infoDisplay.Update(delta);

                            _playArea.Update(delta);

                            if (IsGameOver)
                            {
                                GameOverScreen.Choice = GameOverScreen.ChoiceOption.None;
                                Towers.Clear();
                                return;
                            }

                            if (_playArea.IsFinished)
                            {
                                AdvanceLevel();
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
                    }
                }
            }
            else
            {
                TitleScreen.Update(delta);
            }
        }

        protected void Draw(RenderWindow window)
        {
            window.Clear();

            if (TitleScreen.ReadyToStart)
            {
                if (IsGameOver)
                {
                    GameOverScreen.Draw(window);
                }
                else
                {
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

                    if (TextQueue.Count > 0)
                    {
                        window.Draw(_dimBackground);
                        window.Draw(_textBackground);
                        _textText.DisplayedString = TextQueue[0];
                        window.Draw(_textText);
                    }
                }
            }
            else
            {
                TitleScreen.Draw(window);
            }

            window.Display();
        }
    }
}
