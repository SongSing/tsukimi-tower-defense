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
            _widthTiles = sizeTiles.X;
            _heightTiles = sizeTiles.Y;

            Size = sizeTiles * Game.TileSize;

            _tilesTexture = new Texture("Content/tiles.png");
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

            IsPaused = false;
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
            CurrentlyPlacing = null;
            _finishedWaves.Clear();
            _bulletsToRemove.Clear();
            Bullets.Clear();
            Game.RemoveAllTowers();
            Distributor.Reset();
            Waves.Clear();
            _waveToSpawn = null;


            int i = 0;
            for (int x = 0; x < this._widthTiles; x++)
            {
                for (int y = 0; y < this._heightTiles; y++)
                {
                    int tile = _level.Data[x, y];
                    int index = 0;

                    if ((tile & Level.S) != 0)
                    {
                        index = 2;
                    }
                    else if ((tile & Level.E) != 0)
                    {
                        index = 3;
                    }
                    else if (tile != 0)
                    {
                        index = 1;
                    }

                    _tiles[i++] = new Sprite(_tilesTexture, new IntRect(index * Game.TileSize, 0, Game.TileSize, Game.TileSize))
                    {
                        Position = Utils.Grid2f(new Vector2i(x, y)),
                        Origin = new Vector2f(Game.TileSize, Game.TileSize) / 2
                    };
                }
            }
        }

        public override void Update(float delta)
        {
            bool didPlace = false;
            var snappedPos = Utils.Snap2Grid(Game.MousePosition);

            if (CurrentlyPlacing != null)
            {
                if (AKS.WasJustReleased(Mouse.Button.Left))
                {
                    if (CanPlaceTowerAt(snappedPos))
                    {
                        CurrentlyPlacing.Position = snappedPos;
                        Game.Money -= CurrentlyPlacing.Cost;
                        CurrentlyPlacing.IsPlaced = true;
                        CurrentlySelected = CurrentlyPlacing;
                        CurrentlyPlacing = null;
                        didPlace = true;
                    }
                }
                else if (AKS.WasJustReleased(Mouse.Button.Right))
                {
                    Game.RemoveTower(CurrentlyPlacing);
                    CurrentlyPlacing = null;
                }
            }

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

            var upgradeCandidate = CurrentlyHovered ?? CurrentlySelected;
            if (upgradeCandidate != null && AKS.WasJustPressed(Keyboard.Key.U))
            {
                Game.TryUpgradeTower(upgradeCandidate);
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
                        Game.IsGameOver = true;
                        return;
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
                    if (bullet.ShouldBeCulled)
                    {
                        _bulletsToRemove.Add(bullet);
                    }
                    else
                    {
                        bullet.Update(delta);

                        if (bullet.ShouldBeCulled)
                        {
                            _bulletsToRemove.Add(bullet);
                        }
                        if (bullet.IsCollidable)
                        {
                            foreach (var wave in Waves)
                            {
                                if (!wave.TryBullet(bullet))
                                {
                                    if (bullet.ShouldBeCulled)
                                    {
                                        _bulletsToRemove.Add(bullet);
                                    }
                                    continue;
                                }
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

        public bool CanPlaceTowerAt(Vector2f snappedPos)
        {
            var gridPos = Utils.Vf2Grid(snappedPos);
            return gridPos.X >= 0 && gridPos.Y >= 0 && gridPos.X < _level.Data.GetLength(0) && gridPos.Y < _level.Data.GetLength(1) &&
                _level.Data[gridPos.X, gridPos.Y] == 0 &&
                CurrentlyPlacing.Cost <= Game.Money &&
                !Game.Towers.Exists(tower => tower.IsPlaced && tower.Position == snappedPos);
        }

        public override void Draw(RenderTarget target)
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                _renderTexture.Draw(_tiles[i]);
            }

            foreach (var wave in Waves)
            {
                wave.Draw(_renderTexture);
            }

            var shouldBeRed = false;
            if (CurrentlyPlacing != null)
            {
                CurrentlyPlacing.Position = Utils.Snap2Grid(Game.MousePosition);
                shouldBeRed = !CanPlaceTowerAt(CurrentlyPlacing.Position);
            }

            if (InfoTower != null)
            {
                InfoTower.DrawRange(_renderTexture, shouldBeRed);
            }

            if (CurrentlyPlacing != null)
            {
                CurrentlyPlacing.Draw(_renderTexture, shouldBeRed);
            }

            foreach (var tower in Game.PlacedTowers)
            {
                tower.Draw(_renderTexture);
            }

            foreach (var bullet in Bullets)
            {
                bullet.Draw(_renderTexture);
            }

            base.Draw(target);
        }
    }
}
