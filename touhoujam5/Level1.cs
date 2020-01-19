using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touhoujam5
{
    class Level1 : Level
    {
        private int[,] _data = Transpose(new int[20, 20]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, R, R, R, R, R, D, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, U, R, R, R, D, D, 0, 0, 0, 0, 0, 0, 0 },
            {S|R,R, R, R, R, R, R,U|R,U, 0, 0, R, R, R, R, R, R, R, R, E },
            {S|R,R, R, R, R, R, R,D|R,D, 0, 0, R, R, R, R, R, R, R, R, E },
            { 0, 0, 0, 0, 0, 0, 0, D, R, R, R, U, U, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, R, R, R, R, R, U, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        });

        public override int[,] Data => _data;
        public override int NumWaves => 3;
        public override float Hp => 100;
        public override float Reward => 600;

        public override CreepWave[] Waves => new CreepWave[]
        {
            new CreepWave(5, GenCreeps(0)),
            new CreepWave(5, GenCreeps(1)),
            new CreepWave(5, GenCreeps(2))
        };

        private List<Creep> GenCreeps(int level)
        {
            List<Creep> ret = new List<Creep>();

            switch (level)
            {
                case 0:
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            ret.Add(new TaroCreep(this, 13, 15, 0.8f, 0, 0));
                            ret.Add(new TaroCreep(this, 13, 15, 0.8f, 1, 1.5f));
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            ret.Add(new TaroCreep(this, 13, 15, 0.8f, 0, 0));
                            ret.Add(new TaroCreep(this, 13, 15, 0.8f, 1, 1.5f));
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            ret.Add(new ChestnutCreep(this, 25, 25, 0.6f, 0, 0));
                            ret.Add(new ChestnutCreep(this, 25, 25, 0.6f, 1, 1));
                        }
                        break;
                    }
            }


            return ret;
        }
    }
}
