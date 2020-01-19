using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class TitleScreen : Entity
    {
        private Sprite _sprite;
        public bool ReadyToStart = false;
        private RectangleShape _startBackground;
        private Text _startText;

        public TitleScreen()
        {
            _sprite = new Sprite(Resources.Texture("Content/titlescreen.png"));

            _startBackground = new RectangleShape(new Vector2f(Game.TileSize * 4, Game.TileSize * 2))
            {
                FillColor = new Color(50, 50, 100, 255),
                Position = new Vector2f(Game.TileSize * 20, Game.TileSize * (15))
            };
            _startText = new Text("Start Game", Game.Font)
            {
                Position = Utils.RoundV(_startBackground.Position + _startBackground.Size / 2),
                CharacterSize = 22
            };
            _startText.Origin = Utils.RoundV(new Vector2f(_startText.GetLocalBounds().Width / 2, 11));
        }

        public void Update(float delta)
        {
            if (AKS.WasJustPressed(Mouse.Button.Left) && Utils.PointIsInRect(Game.MousePosition, _startBackground))
            {
                ReadyToStart = true;
            }
        }

        public void Draw(RenderTarget target)
        {
            if (Utils.PointIsInRect(Game.MousePosition, _startBackground))
            {
                _startBackground.FillColor = Color.White;
                _startText.FillColor = Color.Black;
            }
            else
            {
                _startBackground.FillColor = new Color(50, 50, 100, 255);
                _startText.FillColor = Color.White;
            }

            target.Draw(_sprite);
            target.Draw(_startBackground);
            target.Draw(_startText);
        }
    }
}
