using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class PathFinder
    {
        public static Vector2i[] FindPath(Level level, int startIndex)
        {
            List<Vector2i> retList = new List<Vector2i>();
            Vector2i pos = level.StartPositions()[startIndex];
            int tile;

            while (true)
            {
                retList.Add(pos);
                tile = level.Data[pos.X, pos.Y];

                if ((tile & Level.E) != 0)
                {
                    break;
                }

                List<Vector2i> candidates = new List<Vector2i>();

                if ((tile & Level.L) != 0)
                {
                    candidates.Add(new Vector2i(pos.X - 1, pos.Y));
                }
                if ((tile & Level.R) != 0)
                {
                    candidates.Add(new Vector2i(pos.X + 1, pos.Y));
                }
                if ((tile & Level.U) != 0)
                {
                    candidates.Add(new Vector2i(pos.X, pos.Y - 1));
                }
                if ((tile & Level.D) != 0)
                {
                    candidates.Add(new Vector2i(pos.X, pos.Y + 1));
                }

                pos = candidates[Distributor.GetNext(candidates.Count, pos.X + pos.Y * level.Data.GetLength(0))];
            }

            Vector2i delta = retList[1] - retList[0];            
            retList.Insert(0, retList[0] - delta);
            delta = retList[retList.Count - 1] - retList[retList.Count - 2];
            retList.Add(retList[retList.Count - 1] + delta);

            return retList.ToArray();
        }
    }
}
