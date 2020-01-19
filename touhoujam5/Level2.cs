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
        public override int NumWaves => 9;
        public override float Hp => 100000;

        public override CreepWave[] Waves => new CreepWave[]
        {
            new CreepWave(20, GenCreeps(0)),
            new CreepWave(8, GenCreeps(1)),
            new CreepWave(15, GenCreeps(2)),
            new CreepWave(15, GenCreeps(3)),
            new CreepWave(15, GenCreeps(4)),
            new CreepWave(7, GenCreeps(5)),
            new CreepWave(15, GenCreeps(6)),
            new CreepWave(15, GenCreeps(7)),
            new CreepWave(15, GenCreeps(8))
        };

        public override CreepWave SpawnWave()
        {
            if (NextWave == NumWaves - 1)
            {
                Game.TextQueue.AddRange(new string[]
                {
                    "Youmu: Uh oh.\nYukari says there's an abnormally strong youkai approaching.",
                    "Reimu: Stronger than the edamame??",
                    "Alice: Those weren't that much trouble, Reimu...",
                    "Youmu: Does anyone know of any unusual food offered to the moon?",
                    "Reimu: ...",
                    "Marisa: Spit it out, Reimu!!",
                    "Reimu: I had an old orange in my lunch box...",
                    "Everyone: REIMU!!!"
                });
            }
            return base.SpawnWave();
        }

        private List<Creep> GenCreeps(int level)
        {
            List<Creep> ret = new List<Creep>();

            switch (level)
            {
                case 0:
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            ret.Add(new TaroCreep(this, 40, 40, 0.9f, 0, 0));
                            ret.Add(new TaroCreep(this, 40, 40, 0.9f, 1, 1.5f));
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            ret.Add(new EdamameCreep(this, 15, 20, 3.5f, 0, 0));
                            ret.Add(new EdamameCreep(this, 15, 20, 3.5f, 1, 0));
                            ret.Add(new ChestnutCreep(this, 100, 80, 0.7f, 0, 0));
                            ret.Add(new ChestnutCreep(this, 100, 80, 0.7f, 1, 1.5f));
                        }
                        break;
                    }
                case 2:
                    {
                        ret.Add(new ChestnutCreep(this, 1000, 100, 0.7f, 0, 1));
                        break;
                    }
                case 3:
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            ret.AddRange(new Creep[]
                            {
                                new TaroCreep(this, 100, 75, 0.9f, 0, 0),
                                new TaroCreep(this, 100, 75, 0.9f, 1, 1.5f),
                                new ChestnutCreep(this, 150, 100, 0.8f, 0, 0),
                                new ChestnutCreep(this, 150, 100, 0.8f, 1, 1.5f)
                            });
                        }
                        break;
                    }
                case 4:
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            ret.AddRange(new Creep[]
                            {
                                new TaroCreep       (this, 300, 100, 1.1f, 0, 1.5f),
                                new ChestnutCreep   (this, 250, 125, 1.1f, 1, 1.5f),
                                new TaroCreep       (this, 300, 100, 1.1f, 0, 1.5f),
                                new ChestnutCreep   (this, 250, 125, 1.1f, 1, 1.5f)
                            });
                        }
                        break;
                    }
                case 5:
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            ret.AddRange(new Creep[]
                            {
                                new EdamameCreep(this, 180, 200, 2.8f, 0, 1f),
                                new EdamameCreep(this, 180, 200, 2.8f, 1, 1f),
                            });
                        }
                        break;
                    }
                case 6:
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            ret.AddRange(new Creep[]
                            {
                                new ChestnutCreep   (this, 1000, 250, 1.2f, 0, 1.1f),
                                new TaroCreep       (this, 600, 150, 1.2f, 1, 1.1f),
                                new ChestnutCreep   (this, 1000, 250, 1.2f, 0, 1.1f),
                                new TaroCreep       (this, 600, 150, 1.2f, 1, 1.1f)
                            });
                        }
                        break;
                    }
                case 7:
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            ret.AddRange(new Creep[]
                            {
                                new EdamameCreep    (this, 600, 150, 2.8f, 0, 0),
                                new EdamameCreep    (this, 600, 150, 2.8f, 0, 0),
                                new ChestnutCreep   (this, 2000, 250, 1.2f, 0, 1.0f),
                                new ChestnutCreep   (this, 2000, 250, 1.2f, 1, 1.0f),
                                new TaroCreep       (this, 1500, 150, 1.2f, 0, 1.0f),
                                new TaroCreep       (this, 1500, 150, 1.2f, 1, 1.0f),
                            });
                        }
                        break;
                    }
                case 8:
                    {
                        ret.Add(new OrangeCreep(this, 50000, 1, 1.2f, 1, 1));
                        break;
                    }
            }


            return ret;
        }
    }
}
