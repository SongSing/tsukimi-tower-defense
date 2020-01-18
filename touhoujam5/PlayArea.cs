using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class PlayArea : Component
    {
        public float MaxHp = 1000;
        public float Hp = 1000;
        public bool IsFinished = false;
        public bool IsStarted { get; private set; }
        public bool IsPaused = false;
        public Vector2i Size { get; private set; }
        public List<CreepWave> Waves = new List<CreepWave>();
        public List<Bullet> Bullets = new List<Bullet>();
        public Tower CurrentlyPlacing = null;
        public Tower CurrentlySelected = null;
        public Tower CurrentlyHovered = null;
        public Tower InfoTower => CurrentlyPlacing ?? CurrentlyHovered ?? CurrentlySelected;

        private List<Bullet> _bulletsToRemove = new List<Bullet>();
        private HashSet<CreepWave> _finishedWaves = new HashSet<CreepWave>();
        private int _widthTiles;
        private int _heightTiles;
        private int _tileSize;
        private Level _level;
        private Texture _tilesTexture;
        private Sprite[] _tiles;
        private bool _softFinished = false;
        private bool _awaitingCooldown = false;
        private float _cooldownTimer = 0;
        private float _cooldownGoal = 3;
        private float _waveCooldownTimer = 0;
        private float _waveCooldownGoal;
        private CreepWave _waveToSpawn = null;

        public IEnumerable<Creep> Creeps
        {
            get
            {
                foreach (CreepWave wave in Waves)
                {
                    foreach (Creep creep in wave.Creeps)
                    {
                        yield return creep;
                    }
                }
            }
        }

        public PlayArea(Vector2i sizeTiles, Vector2f position) : base(Utils.Vi2u(sizeTiles * Game.TileSize), position)
        {
            _tileSize = Game.TileSize;
            _widthTiles = sizeTiles.X;
            _heightTiles = sizeTiles.Y;

            Size = sizeTiles * Game.TileSize;

            _tilesTexture = new Texture("Content/tiles.png");

            Game.Towers.Add(new ReimuTower());
            Game.Towers.Add(new YoumuTower());
            Game.Towers.Add(new YoumuTower());
        }

        public void Start()
        {
            IsPaused = false;
            IsStarted = true;
            var wave = _level.SpawnWave();
            Waves.Add(wave);
        }

        public List<Creep> GetCreeps()
        {
            List<Creep> ret = new List<Creep>();

            foreach (Creep creep in Creeps)
            {
                ret.Add(creep);
            }

            return ret;
        }

        public void SetLevel(Level level)
        {
            if (level.Data.GetLength(0) != _widthTiles || level.Data.GetLength(1) != _heightTiles)
            {
                throw new Exception("fuckkkkk seriously?");
            }

            IsStarted = false;
            IsFinished = false;
            _softFinished = false;
            _awaitingCooldown = false;
            _cooldownTimer = 0;
            _level = level;
            _tiles = new Sprite[_widthTiles * _heightTiles];
            Hp = MaxHp = level.Hp;
            CurrentlySelected = null;
            CurrentlyHovered = null;
            _finishedWaves.Clear();

            int i = 0;
            for (int x = 0; x < this._widthTiles; x++)
            {
                for (int y = 0; y < this._heightTiles; y++)
                {
                    _tiles[i++] = new Sprite(_tilesTexture, new IntRect(_level.Data[x, y] * _tileSize, 0, _tileSize, _tileSize))
                    {
                        Position = Utils.Grid2f(new Vector2i(x, y)),
                        Origin = new Vector2f(Game.TileSize, Game.TileSize) / 2
                    };
                }
            }

            foreach (Tower tower in Game.Towers)
            {
                tower.IsPlaced = false;
            }

            Distributor.Reset();
            Waves.Clear();
        }

        public override void Update(float delta)
        {
            bool didPlace = false;

            if (CurrentlyPlacing != null)
            {
                if (AKS.WasJustReleased(Mouse.Button.Left))
                {
                    Vector2i gridPos = Utils.Vf2Grid(Game.MousePositionf);
                    if (gridPos.X < _widthTiles && gridPos.Y < _heightTiles && CurrentlyPlacing.Cost <= Game.Money)
                    {
                        CurrentlyPlacing.Position = Utils.Grid2f(gridPos);
                        Game.Money -= CurrentlyPlacing.Cost;
                        CurrentlyPlacing.IsPlaced = true;
                        CurrentlySelected = CurrentlyPlacing;
                        CurrentlyPlacing = null;
                        didPlace = true;
                    }
                }
                else if (AKS.WasJustReleased(Mouse.Button.Right))
                {
                    CurrentlyPlacing = null;
                }
            }

            var snappedPos = Utils.Snap2Grid(Game.MousePosition);
            bool newSelected = false;
            CurrentlyHovered = null;

            foreach (var tower in Game.PlacedTowers)
            {
                if (tower.Position == snappedPos)
                {
                    if (AKS.WasJustPressed(Mouse.Button.Left))
                    {
                        CurrentlySelected = tower;
                        newSelected = true;
                    }
                    else
                    {
                        CurrentlyHovered = tower;
                    }
                }
            }

            if (AKS.WasJustPressed(Mouse.Button.Left) && !newSelected)
            {
                Vector2i gridPos = Utils.Vf2Grid(Game.MousePositionf);
                if (gridPos.X < _widthTiles && gridPos.Y < _heightTiles)
                {
                    CurrentlySelected = null;
                }
            }

            if (!IsStarted || IsPaused) return;

            for (int frame = 0; frame < (AKS.IsKeyDown(Keyboard.Key.LControl) ? 10 : 1); frame++)
            {
                foreach (var tower in Game.PlacedTowers)
                {
                    tower.Update(delta);
                }

                foreach (var wave in Waves)
                {
                    wave.Update(delta);

                    Hp -= wave.FrameDamage;
                    if (Hp <= 0)
                    {
                        // TODO: GAME OVER :(
                    }

                    if (wave.IsFinishedSpawning && !_finishedWaves.Contains(wave))
                    {
                        _finishedWaves.Add(wave);

                        if (_finishedWaves.Count == _level.NumWaves)
                        {
                            _softFinished = true;
                        }
                        else
                        {
                            _waveCooldownTimer = 0;
                            _waveCooldownGoal = wave.Cooldown;
                            _waveToSpawn = _level.SpawnWave();
                        }
                    }
                }

                if (_waveToSpawn != null)
                {
                    _waveCooldownTimer += delta;

                    if (_waveCooldownTimer >= _waveCooldownGoal)
                    {
                        Waves.Add(_waveToSpawn);
                        _waveToSpawn = null;
                    }
                }

                foreach (var bullet in Bullets)
                {
                    bullet.Update(delta);

                    if (bullet.ShouldBeCulled)
                    {
                        _bulletsToRemove.Add(bullet);
                    }
                    else if (bullet.IsCollidable)
                    {
                        foreach (var wave in Waves)
                        {
                            if (wave.TryBullet(bullet) && !bullet.IsPiercing)
                            {
                                _bulletsToRemove.Add(bullet);
                            }
                        }
                    }
                }

                foreach (var bullet in _bulletsToRemove)
                {
                    Bullets.Remove(bullet);
                }

                _bulletsToRemove.Clear();

                foreach (var wave in Waves)
                {
                    wave.CullDeadCreeps();
                }

                if (_softFinished)
                {
                    if (!_awaitingCooldown)
                    {
                        int totalCreeps = 0;

                        foreach (CreepWave wave in Waves)
                        {
                            totalCreeps += wave.Creeps.Count;
                        }

                        if (totalCreeps == 0)
                        {
                            _awaitingCooldown = true;
                        }
                    }

                    if (_awaitingCooldown)
                    {
                        _cooldownTimer += delta;
                        if (_cooldownTimer >= _cooldownGoal)
                        {
                            IsFinished = true;
                            break;
                        }
                    }
                }
            }
        }

        public override void Draw(RenderTarget target)
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                _renderTexture.Draw(_tiles[i]);
            }

            foreach (var tower in Game.PlacedTowers)
            {
                tower.Draw(_renderTexture);
            }

            foreach (var wave in Waves)
            {
                wave.Draw(_renderTexture);
            }

            foreach (var bullet in Bullets)
            {
                bullet.Draw(_renderTexture);
            }

            if (CurrentlyPlacing != null)
            {
                CurrentlyPlacing.Position = Utils.Snap2Grid(Game.MousePosition);
                CurrentlyPlacing.Draw(_renderTexture);
            }

            base.Draw(target);
        }
    }
}
