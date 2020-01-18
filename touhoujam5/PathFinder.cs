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
        public static Vector2i[] FindPath(Level level, int startIndex, int endIndex)
        {
            List<Vector2i> visited = new List<Vector2i>();
            Vector2i start = level.StartPositions()[startIndex];
            Vector2i end = level.EndPositions()[endIndex];
            List<Vector2i> retList = new List<Vector2i>();

            if (Search(level, start, end, retList, visited))
            {
                var delta = retList[1] - retList[0];
                retList.Insert(0, retList[0] - delta);
                delta = retList[retList.Count - 1] - retList[retList.Count - 2];
                retList.Add(retList[retList.Count - 1] + delta);
                return retList.ToArray();
            }
            else
            {
                throw new Exception("bad path :(");
            }
        }

        private static bool Search(Level level, Vector2i pos, Vector2i end, List<Vector2i> path, List<Vector2i> visited)
        {
            visited.Add(pos);
            path.Add(pos);

            Vector2i[] candidates = new Vector2i[]
            {
                new Vector2i(pos.X - 1, pos.Y),
                new Vector2i(pos.X, pos.Y - 1),
                new Vector2i(pos.X + 1, pos.Y),
                new Vector2i(pos.X, pos.Y + 1)
            };

            List<Vector2i> canMoveTo = new List<Vector2i>();

            //Console.WriteLine("---");
            //Console.Write("pos: ");
            //Console.WriteLine(pos.ToString());

            foreach (var v in candidates)
            {
                if (v.X >= 0 && v.X < level.Data.GetLength(0) && v.Y >= 0 && v.Y < level.Data.GetLength(1)
                    && (level.Data[v.X, v.Y] == 1 || level.Data[v.X, v.Y] == 3) && !visited.Contains(v))
                {
                    canMoveTo.Add(v);
                    //Console.WriteLine(v.ToString());
                }
            }


            if (canMoveTo.Count == 0)
            {
                path.RemoveAt(path.Count - 1);
                return false;
            }
            else
            {
                Vector2i ret;
                int i = 0;
                bool wasTrue = false;

                do
                {
                    i++;
                    ret = canMoveTo[Distributor.GetNext(canMoveTo.Count, pos.X + pos.Y * level.Data.GetLength(0))];
                    //Console.WriteLine(ret.ToString() + " / " + end.ToString());
                    if (ret == end)
                    {
                        path.Add(end);
                        return true;
                    }
                } while ((!(wasTrue = Search(level, ret, end, path, visited))) && i < canMoveTo.Count);

                if (i >= canMoveTo.Count && !wasTrue)
                {
                    path.RemoveAt(path.Count - 1);
                    return false;
                }

                return true;
            }
        }
    }
}
