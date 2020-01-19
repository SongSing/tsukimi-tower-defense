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
        public static int S = 0b00000001;
        public static int U = 0b00000010;
        public static int D = 0b00000100;
        public static int L = 0b00001000;
        public static int R = 0b00010000;
        public static int E = 0b00100000;

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
        public abstract float Hp { get; }
        public virtual float Reward => Hp;
        public int NextWave = 0;
        public virtual CreepWave SpawnWave()
        {
            return Waves[NextWave++];
        }
        public abstract int NumWaves { get; }

        public Vector2i[] PositionsWith(int flag)
        {
            List<Vector2i> ret = new List<Vector2i>();
            int[,] levelData = Data;

            for (int x = 0; x < levelData.GetLength(0); x++)
            {
                for (int y = 0; y < levelData.GetLength(1); y++)
                {
                    if ((Data[x, y] & flag) != 0)
                    {
                        ret.Add(new Vector2i(x, y));
                    }
                }
            }

            return ret.ToArray();
        }

        public Vector2i[] StartPositions()
        {
            return PositionsWith(S);
        }

        public Vector2i[] EndPositions()
        {
            return PositionsWith(E);
        }
    }
}
