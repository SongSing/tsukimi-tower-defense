using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class PlayArea : Entity
    {
        public List<CreepWave> Waves = new List<CreepWave>();
        public List<Bullet> Bullets = new List<Bullet>();
        public Tower CurrentlyPlacing = null;
        public Tower CurrentlySelected = null;
        public Tower CurrentlyHovered = null;
        public Tower InfoTower => CurrentlyPlacing ?? CurrentlyHovered ?? CurrentlySelected;

        private List<Bullet> _bulletsToRemove = new List<Bullet>();
        private int _widthTiles;
        private int _heightTiles;
        private int _tileSize;
        private int[,] _level;
        private Texture _tilesTexture;
        private Sprite[] _tiles;
        private RenderTexture _renderTexture;
        private Sprite _sprite;

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

        public PlayArea(int widthTiles, int heightTiles)
        {
            _tileSize = Game.TileSize;
            _widthTiles = widthTiles;
            _heightTiles = heightTiles;

            _tilesTexture = new Texture("Content/tiles.png");

            var tower = new ReimuTower(new Vector2i(2, 4));
            tower.IsPlaced = true;
            Game.Towers.Add(tower);
            Game.Towers.Add(new ReimuTower(new Vector2i(2, 4)));

            _renderTexture = new RenderTexture((uint)(widthTiles * Game.TileSize), (uint)(heightTiles * Game.TileSize));
            _sprite = new Sprite(_renderTexture.Texture);
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

            _level = level.Data;
            _tiles = new Sprite[_widthTiles * _heightTiles];

            int i = 0;
            for (int x = 0; x < this._widthTiles; x++)
            {
                for (int y = 0; y < this._heightTiles; y++)
                {
                    _tiles[i++] = new Sprite(_tilesTexture, new IntRect(_level[x, y] * _tileSize, 0, _tileSize, _tileSize))
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
            this.Waves.Clear();

            var wave = level.SpawnWave();
            this.Waves.Add(wave);
        }

        public void Update(float delta)
        {
            foreach (var wave in Waves)
            {
                wave.Update(delta);
            }

            var snappedPos = Utils.Snap2Grid(Game.MousePosition);
            bool newSelected = false;
            CurrentlyHovered = null;

            foreach (var tower in Game.PlacedTowers)
            {
                tower.Update(delta);

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
                CurrentlySelected = null;
            }

            foreach (var bullet in Bullets)
            {
                bullet.Update(delta);

                foreach (var wave in Waves)
                {
                    if (wave.TryBullet(bullet))
                    {
                        _bulletsToRemove.Add(bullet);
                    }
                }
            }

            foreach (var bullet in _bulletsToRemove)
            {
                Bullets.Remove(bullet);
            }

            if (CurrentlyPlacing != null)
            {
                if (AKS.WasJustPressed(Mouse.Button.Left))
                {
                    Vector2i gridPos = Utils.Vf2Grid(Game.MousePositionf);
                    if (gridPos.X < _widthTiles && gridPos.Y < _heightTiles)
                    {
                        CurrentlyPlacing.Position = Utils.Grid2f(gridPos);
                        CurrentlyPlacing.IsPlaced = true;
                        CurrentlyPlacing = null;
                    }
                }
                else if (AKS.WasJustPressed(Mouse.Button.Right))
                {
                    CurrentlyPlacing = null;
                }
            }
        }

        public void Draw(RenderTarget target)
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                _renderTexture.Draw(_tiles[i]);
            }

            foreach (var wave in Waves)
            {
                wave.Draw(_renderTexture);
            }

            foreach (var tower in Game.PlacedTowers)
            {
                tower.Draw(_renderTexture);
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

            _renderTexture.Display();
            target.Draw(_sprite);
        }
    }
}
