using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class Utils
    {
        public static Vector2f Vi2f(Vector2i v)
        {
            return new Vector2f(v.X, v.Y);
        }
        public static Vector2u Vi2u(Vector2i v)
        {
            return new Vector2u((uint)v.X, (uint)v.Y);
        }

        public static Vector2f Vu2f(Vector2u v)
        {
            return new Vector2f(v.X, v.Y);
        }

        public static Vector2i Vf2i(Vector2f v)
        {
            return new Vector2i((int)v.X, (int)v.Y);
        }

        public static Vector2f Grid2f(Vector2i gridPos)
        {
            return Vi2f(gridPos) * Game.TileSize + new Vector2f(Game.TileSize, Game.TileSize) / 2;
        }

        public static Vector2f Snap2Grid(Vector2i pos)
        {
            return Grid2f(new Vector2i(pos.X / Game.TileSize, pos.Y / Game.TileSize));
        }

        public static Vector2i Vf2Grid(Vector2f v)
        {
            return new Vector2i((int)v.X / Game.TileSize, (int)v.Y / Game.TileSize);
        }
    }
}
