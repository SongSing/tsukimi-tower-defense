using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace touhoujam5
{
    interface Entity
    {
        void Update(float delta);
        void Draw(RenderTarget target);
    }
}
