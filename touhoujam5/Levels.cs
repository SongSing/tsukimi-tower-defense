using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    abstract class Level
    {
        public static int[,] Transpose(int[,] data)
        {
            int[,] ret = new int[data.GetLength(1), data.GetLength(0)];
            for (int x = 0; x < data.GetLength(1); x++)
            {
                for (int y = 0; y < data.GetLength(0); y++)
                {
                    ret[x, y] = data[y, x];
                }
            }

            return ret;
        }

        public abstract int[,] Data { get; }
        public abstract CreepWave[] Waves { get; }
        public int CurrentWave = 0;
        public CreepWave SpawnWave()
        {
            return Waves[CurrentWave++];
        }
        public abstract int NumWaves { get; }

        public Vector2i[] PositionsEqualTo(int num)
        {
            List<Vector2i> ret = new List<Vector2i>();
            int[,] levelData = Data;

            for (int x = 0; x < levelData.GetLength(0); x++)
            {
                for (int y = 0; y < levelData.GetLength(1); y++)
                {
                    if (Data[x, y] == num)
                    {
                        ret.Add(new Vector2i(x, y));
                    }
                }
            }

            return ret.ToArray();
        }

        public Vector2i[] StartPositions()
        {
            return PositionsEqualTo(2);
        }

        public Vector2i[] EndPositions()
        {
            return PositionsEqualTo(3);
        }
    }
}
