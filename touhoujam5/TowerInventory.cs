﻿using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class TowerInventory : Component
    {
        public Vector2i SizeTiles;
        public Vector2f Position;
        public Tower CurrentlyHovered;
        public List<Tower> Allowed = new List<Tower>();

        public TowerInventory(Vector2f position, Vector2i sizeTiles) : base(Utils.Vi2u(sizeTiles * Game.TileSize), position)
        {
            Position = position;
            SizeTiles = sizeTiles;
        }

        public override void Update(float delta)
        {
            int i = 0;
            Vector2f mousePos = Utils.Snap2Grid(Game.MousePosition);
            CurrentlyHovered = null;

            foreach (Tower tower in Allowed)
            {
                float x = i % SizeTiles.X * Game.TileSize + Game.TileSize / 2;
                float y = (i / SizeTiles.X) * Game.TileSize + Game.TileSize / 2;
                Vector2f towerPos = Position + new Vector2f(x, y);
                if (mousePos == towerPos)
                {
                    CurrentlyHovered = tower;
                    if (AKS.WasJustPressed(Mouse.Button.Left))
                    {
                        var toAdd = tower.CloneLevel0();
                        Game.Towers.Add(toAdd);
                        Game.PlayArea.CurrentlyPlacing = toAdd;
                    }
                    break;
                }
                i++;
            }
        }

        public override void Draw(RenderTarget target)
        {
            _renderTexture.Clear(new Color(20, 20, 20));

            int i = 0;
            foreach (Tower tower in Allowed)
            {
                float x = i % SizeTiles.X * Game.TileSize + Game.TileSize / 2;
                float y = (i / SizeTiles.X) * Game.TileSize + Game.TileSize / 2;
                tower.Position = new Vector2f(x, y);
                tower.Draw(_renderTexture);
                i++;
            }

            base.Draw(target);
        }
    }
}
