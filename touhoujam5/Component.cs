using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
namespace touhoujam5
{
    abstract class Component : Entity
    {
        protected RenderTexture _renderTexture;
        protected Sprite _renderSprite;

        public Component(Vector2u size, Vector2f position)
        {
            _renderTexture = new RenderTexture(size.X, size.Y);
            _renderSprite = new Sprite(_renderTexture.Texture)
            {
                Position = position
            };
        }

        /// <summary>
        /// Should be called after drawing to the _renderTexture
        /// </summary>
        /// <param name="target"></param>
        public virtual void Draw(RenderTarget target)
        {
            _renderTexture.Display();
            target.Draw(_renderSprite);
        }

        public abstract void Update(float delta);
    }
}
