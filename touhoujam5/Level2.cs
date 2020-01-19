using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touhoujam5
{
    class Level2 : Level
    {
        private int[,] _data = Transpose(new int[20, 20]
        {
            {0,S|D,S|D,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, D, R, R, R, R, R, R, R, R, R, R, R, R, R, R, R, R, D, 0 },
            { 0, R, R, R, R, R, R, R, R, R, R, R, R, R, R, R, R, D, D, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, D, D, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, D, D, 0 },
            { E, L, L, L, L, L, L, L, L, L, L, L, L, L, L, 0, 0, D, D, 0 },
            { E, L, L, L, L, L, L, L, L, L, L, L, L, L, U, 0, 0, D, D, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, U, U, 0, 0, D, D, 0 },
            { 0, R, R, R, R, R, R, R, R, R, R, D, 0, U, U, 0, 0, D, D, 0 },
            { 0, U, R, R, R, R, R, R, R, R, D, D, 0, U, U, 0, 0, D, D, 0 },
            { 0, U, U, 0, 0, 0, 0, 0, 0, 0, D, D, 0, U, U, 0, 0, D, D, 0 },
            { 0, U, U, 0, 0, 0, 0, 0, 0, 0, D, D, 0, U, U, 0, 0, D, D, 0 },
            { 0, U, U, 0, 0, 0, 0, 0, 0, 0, D, D, 0, U, U, 0, 0, D, D, 0 },
            { 0, U, U, 0, 0, 0, 0, 0, 0, 0, D, R, R, U, U, 0, 0, D, D, 0 },
            { 0, U, U, 0, 0, 0, 0, 0, 0, 0, R, R, R, R, U, 0, 0, D, D, 0 },
            { 0, U, U, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, D, D, 0 },
            { 0, U, U, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, D, D, 0 },
            { 0, U, U, L, L, L, L, L, L, L, L, L, L, L, L, L, L, L, D, 0 },
            { 0, U, L, L, L, L, L, L, L, L, L, L, L, L, L, L, L, L, L, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        });

        public override int[,] Data => _data;
        public override int NumWaves => 3;
        public override float Hp => 100;

        public override CreepWave[] Waves => new CreepWave[]
        {
            new CreepWave(40, GenCreeps(0)),
            new CreepWave(20, GenCreeps(1)),
            new CreepWave(5, GenCreeps(2))
        };

        private List<Creep> GenCreeps(int level)
        {
            List<Creep> ret = new List<Creep>();

            switch (level)
            {
                case 0:
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            ret.Add(new TaroCreep(this, 40, 40, 0.8f, 0, 0));
                            ret.Add(new TaroCreep(this, 40, 40, 0.8f, 1, 1.5f));
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            ret.Add(new EdamameCreep(this, 15, 30, 4f, 0, 0));
                            ret.Add(new EdamameCreep(this, 15, 30, 4f, 1, 0));
                            ret.Add(new ChestnutCreep(this, 30, 40, 0.6f, 0, 0));
                            ret.Add(new ChestnutCreep(this, 40, 40, 0.6f, 1, 1.5f));
                        }
                        break;
                    }
                case 2:
                    {
                        ret.Add(new ChestnutCreep(this, 1000, 1, 0.1f, 0, 1));
                        break;
                    }
            }


            return ret;
        }
    }
}
