using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touhoujam5
{
    class Distributor
    {
        private static Dictionary<int, Dictionary<int, int>> _map = new Dictionary<int, Dictionary<int, int>>();

        public static void Reset()
        {
            _map.Clear();
        }

        public static int GetNext(int outOf, int level)
        {
            if (!_map.ContainsKey(outOf))
            {
                _map[outOf] = new Dictionary<int, int>();
            }

            Dictionary<int, int> dict = _map[outOf];

            if (!dict.ContainsKey(level))
            {
                dict[level] = 0;
            }

            int ret = dict[level];

            if (ret == outOf - 1)
            {
                dict[level] = 0;
            }
            else
            {
                dict[level]++;
            }

            return ret;
        }
    }
}
