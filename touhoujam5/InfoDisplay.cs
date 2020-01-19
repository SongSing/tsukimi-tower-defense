using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class InfoDisplay : Component
    {
        private Text _text;
        private RectangleShape _upgradeBackground;
        private Text _upgradeText;

        public InfoDisplay(Vector2f position, Vector2u size) : base(size, position)
        {
            _text = new Text("", Game.Font)
            {
                Position = new Vector2f(8, 8),
                CharacterSize = 16
            };

            _upgradeBackground = new RectangleShape(new Vector2f(size.X, Game.TileSize * 2))
            {
                FillColor = new Color(50, 150, 60, 255),
                Position = new Vector2f(0, size.Y - Game.TileSize * 2)
            };

            _upgradeText = new Text("Upgrade", Game.Font)
            {
                Position = Utils.RoundV(_upgradeBackground.Position + _upgradeBackground.Size / 2),
                CharacterSize = 32
            };

            _upgradeText.Origin = Utils.RoundV(new Vector2f(_upgradeText.GetLocalBounds().Width / 2, 20));
        }

        public override void Update(float delta)
        {
            Tower s = Game.PlayArea.CurrentlySelected;
            RectangleShape r = new RectangleShape(_upgradeBackground);
            r.Position += _renderSprite.Position;

            if (s != null && s.Level + 1 != s.NumLevels && 
                AKS.WasJustPressed(Mouse.Button.Left) && Utils.PointIsInRect(Game.MousePosition, r) &&
                s.Cost <= Game.Money)
            {
                Game.Money -= s.Cost;
                s.LevelUp();
            }
        }

        public override void Draw(RenderTarget target)
        {
            Tower tower = Game.TowerInventory.CurrentlyHovered ?? Game.PlayArea.InfoTower;

            if (tower != null)
            {
                _text.DisplayedString = tower.Name + "\n" +
                    tower.Description + "\n\n" +
                    (tower.IsPlaced ? "Level: " + (tower.Level + 1).ToString() + "\n" : "") +
                    (tower.IsPlaced ? "" : "Cost: " + tower.Cost.ToString() + "\n") +
                    "Range: " + tower.Range.ToString() + "\n" +
                    "Fire Rate: " + (tower.ShotRate >= 60 ? "--" : tower.ShotRate.ToString()) + "\n" +
                    "Strength: " + tower.Strength.ToString() + "\n" +
                    tower.GetExtraInfo(tower.Level);

                if (tower.Level + 1 != tower.NumLevels)
                {
                    _text.DisplayedString += "\n\nNext Upgrade:\n" +
                        "Cost: " + tower.UpgradeCost.ToString() + "\n" +
                        "Range: " + tower.NextRange.ToString() + "\n" +
                        "Fire Rate: " + (tower.NextShotRate >= 60 ? "--" : tower.NextShotRate.ToString()) + "\n" +
                        "Strength: " + tower.NextStrength.ToString() + "\n" +
                        tower.GetExtraInfo(tower.Level + 1);
                }
            }
            else
            {
                _text.DisplayedString = "";
            }

            _renderTexture.Clear();
            _renderTexture.Draw(_text);

            if (Game.PlayArea.InfoTower != null && Game.PlayArea.InfoTower.Level + 1 != Game.PlayArea.InfoTower.NumLevels && Game.PlayArea.InfoTower != Game.PlayArea.CurrentlyPlacing)
            {
                _upgradeBackground.FillColor = Game.PlayArea.InfoTower.Cost <= Game.Money ? new Color(50, 150, 60, 255) : new Color(75, 75, 75, 255);
                _upgradeText.FillColor = Game.PlayArea.InfoTower.Cost <= Game.Money ? Color.White : new Color(160, 160, 160);
                _renderTexture.Draw(_upgradeBackground);
                _renderTexture.Draw(_upgradeText);
            }

            base.Draw(target);
        }
    }
}
