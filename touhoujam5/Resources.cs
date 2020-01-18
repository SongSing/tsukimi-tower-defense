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

        private static bool TryGet<T>(string filename, out T ret) where T : class
        {
            if (_cache.ContainsKey(filename))
            {
                ret = _cache[filename] as T;
                return true;
            }

            ret = null;
            return false;
        }

        public static Texture Texture(string filename)
        {
            if (!TryGet(filename, out Texture ret))
            {
                ret = new Texture(filename);
            }

            return ret;
        }

        public static Font Font(string filename)
        {
            if (!TryGet(filename, out Font ret))
            {
                ret = new Font(filename);
            }

            return ret;
        }
    }
}
