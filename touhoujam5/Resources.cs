using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touhoujam5
{
    class Resources
    {
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();

        public static Texture Texture(string filename)
        {
            if (_cache.ContainsKey(filename))
            {
                return _cache[filename] as Texture;
            }
            else
            {
                var ret = new Texture(filename);
                _cache[filename] = ret;
                return ret;
            }
        }
    }
}
