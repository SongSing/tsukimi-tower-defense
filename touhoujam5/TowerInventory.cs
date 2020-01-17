using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class TowerInventory : Entity
    {
        public Vector2i SizeTiles;
        public Vector2f Position;
        private RenderTexture _renderTexture;
        private Sprite _sprite;

        public TowerInventory(Vector2f position, Vector2i sizeTiles)
        {
            Position = position;
            SizeTiles = sizeTiles;

            var size = Utils.Vi2u(sizeTiles * Game.TileSize);
            _renderTexture = new RenderTexture(size.X, size.Y);
            _sprite = new Sprite(_renderTexture.Texture);
            _sprite.Position = Position;
        }

        public void Update(float delta)
        {
            if (AKS.WasJustPressed(Mouse.Button.Left))
            {
                if (Game.PlayArea.CurrentlyPlacing == null)
                {
                    int i = 0;
                    Vector2f mousePos = Utils.Snap2Grid(Game.MousePosition);

                    foreach (Tower tower in Game.UnplacedTowers)
                    {
                        float x = i % SizeTiles.X * Game.TileSize + Game.TileSize / 2;
                        float y = (i / SizeTiles.Y) * Game.TileSize + Game.TileSize / 2;
                        Vector2f towerPos = Position + new Vector2f(x, y);
                        if (mousePos == towerPos)
                        {
                            Game.PlayArea.CurrentlyPlacing = tower;
                            break;
                        }
                        i++;
                    }
                }
            }
        }

        public void Draw(RenderTarget target)
        {
            _renderTexture.Clear(Color.Blue);

            int i = 0;
            foreach (Tower tower in Game.UnplacedTowers)
            {
                if (Game.PlayArea.CurrentlyPlacing != tower)
                {
                    float x = i % SizeTiles.X * Game.TileSize + Game.TileSize / 2;
                    float y = (i / SizeTiles.Y) * Game.TileSize + Game.TileSize / 2;
                    tower.Position = new Vector2f(x, y);
                    tower.Draw(_renderTexture);
                    i++;
                }
            }

            _renderTexture.Display();
            target.Draw(_sprite);
        }
    }
}
