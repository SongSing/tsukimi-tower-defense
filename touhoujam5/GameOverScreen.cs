using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class GameOverScreen : Entity
    {
        public bool ReadyToStart = false;
        public bool IsWinScreen = false;
        private RectangleShape _retryLevelBackground, _returnToTitleBackground;
        private Text _retryLevelText, _returnToTitleText, _gameOverText;
        public enum ChoiceOption
        {
            None,
            RetryLevel,
            ReturnToTitle
        };
        public ChoiceOption Choice = ChoiceOption.None;

        public GameOverScreen()
        {
            _retryLevelBackground = new RectangleShape(new Vector2f(Game.TileSize * 6, Game.TileSize * 2))
            {
                FillColor = new Color(50, 50, 100, 255),
                Position = new Vector2f(Game.Window.Size.X / 2, Game.Window.Size.Y / 2 + Game.TileSize * 2),
                Origin = new Vector2f(Game.TileSize * 3, Game.TileSize * 1)
            };
            _retryLevelText = new Text("Retry Level", Game.Font)
            {
                Position = Utils.RoundV(_retryLevelBackground.Position),
                CharacterSize = 22
            };
            _retryLevelText.Origin = Utils.RoundV(new Vector2f(_retryLevelText.GetLocalBounds().Width / 2, 11));

            _returnToTitleBackground = new RectangleShape(new Vector2f(Game.TileSize * 10, Game.TileSize * 2))
            {
                FillColor = new Color(50, 50, 100, 255),
                Position = new Vector2f(Game.Window.Size.X / 2, Game.Window.Size.Y / 2 + Game.TileSize * 5),
                Origin = new Vector2f(Game.TileSize * 5, Game.TileSize * 1)
            };
            _returnToTitleText = new Text("Return to Title Screen", Game.Font)
            {
                Position = Utils.RoundV(_returnToTitleBackground.Position),
                CharacterSize = 22
            };
            _returnToTitleText.Origin = Utils.RoundV(new Vector2f(_returnToTitleText.GetLocalBounds().Width / 2, 11));

            _gameOverText = new Text("Game Over :(", Game.Font)
            {
                Position = new Vector2f(Game.Window.Size.X / 2, Game.Window.Size.Y / 2 - Game.TileSize * 5),
                CharacterSize = 32
            };
            _gameOverText.Origin = Utils.RoundV(new Vector2f(_gameOverText.GetLocalBounds().Width / 2, 11));
        }

        public void Update(float delta)
        {
            if (AKS.WasJustPressed(Mouse.Button.Left) && Utils.PointIsInRect(Game.MousePosition, _retryLevelBackground))
            {
                Choice = ChoiceOption.RetryLevel;
            }
            else if (AKS.WasJustPressed(Mouse.Button.Left) && Utils.PointIsInRect(Game.MousePosition, _returnToTitleBackground))
            {
                Choice = ChoiceOption.ReturnToTitle;
            }
        }

        public void Draw(RenderTarget target)
        {
            _gameOverText.DisplayedString = IsWinScreen ? "You Won!" : "Game Over :(";
            _gameOverText.Origin = Utils.RoundV(new Vector2f(_gameOverText.GetLocalBounds().Width / 2, 11));

            if (Utils.PointIsInRect(Game.MousePosition, _retryLevelBackground))
            {
                _retryLevelBackground.FillColor = Color.White;
                _retryLevelText.FillColor = Color.Black;
            }
            else
            {
                _retryLevelBackground.FillColor = new Color(50, 50, 100, 255);
                _retryLevelText.FillColor = Color.White;
            }

            if (Utils.PointIsInRect(Game.MousePosition, _returnToTitleBackground))
            {
                _returnToTitleBackground.FillColor = Color.White;
                _returnToTitleText.FillColor = Color.Black;
            }
            else
            {
                _returnToTitleBackground.FillColor = new Color(50, 50, 100, 255);
                _returnToTitleText.FillColor = Color.White;
            }

            target.Draw(_retryLevelBackground);
            target.Draw(_retryLevelText);
            target.Draw(_returnToTitleBackground);
            target.Draw(_returnToTitleText);
            target.Draw(_gameOverText);
        }
    }
}
