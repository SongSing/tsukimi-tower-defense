using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class MarisaLaser : Bullet
    {
        public Tower ParentTower;
        public Creep TargetCreep = null;
        public override bool IsCollidable => false;
        public override bool IsMultiHit => true;
        public float Width;

        private List<CircleShape> _circleShapes = new List<CircleShape>();

        public MarisaLaser(Tower parentTower, float strength, float width)
			: base(2, new Hitbox(new Vector2f(0, 0), 0), new Vector2f(0, 0), strength, true)
        {
            ParentTower = parentTower;
            Width = width;

            //_sprite.Color = new Color(255, 255, 255, 128);

            //_circleShapes = new CircleShape[]
            //{
            //    new CircleShape(3) { FillColor = new Color(255, 0, 0, 128), Origin = new Vector2f(3, 3) },
            //    new CircleShape(3) { FillColor = new Color(255, 0, 0, 128), Origin = new Vector2f(3, 3) },
            //    new CircleShape(3) { FillColor = new Color(255, 0, 0, 128), Origin = new Vector2f(3, 3) },
            //    new CircleShape(3) { FillColor = new Color(255, 0, 0, 128), Origin = new Vector2f(3, 3) }
            //};
        }

        public override void Update(float delta)
        {
            _circleShapes.Clear();
			if (TargetCreep != null)
            {
				if (TargetCreep.HasDied)
                {
                    TargetCreep = null;
                    ShouldBeCulled = true;
                    return;
                }

                float angle = TargetCreep.AngleTo(ParentTower);
                float distance = TargetCreep.DistanceTo(ParentTower);

				if (distance >= ParentTower.Range)
                {
                    ShouldBeCulled = true;
                    TargetCreep = null;
                }
				else
                {
                    float _strength = Strength;
                    Strength = _strength * delta;

                    List<Hitbox> hitboxes = new List<Hitbox>();

                    for (float i = Width / 2; i < distance - Width / 2; i += Width / 2)
                    {
                        hitboxes.Add(new Hitbox(ParentTower.Position - new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle)) * i, Width / 2));
                        //_circleShapes.Add(new CircleShape(Width / 2) { Origin = new Vector2f(Width / 2, Width / 2), Position = ParentTower.Position - new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle)) * i, FillColor = new Color(255, 0, 0, 188) });
                    }

                    hitboxes.Add(new Hitbox(ParentTower.Position - new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle)) * (distance - Width / 2), Width / 2));
                    //_circleShapes.Add(new CircleShape(Width / 2) { Origin = new Vector2f(Width / 2, Width / 2), Position = ParentTower.Position - new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle)) * (distance - Width / 2), FillColor = new Color(255, 0, 0, 188) });

                    foreach (Creep creep in Game.PlayArea.Creeps)
                    {
                        if (creep != TargetCreep && hitboxes.Exists(hitbox => hitbox.Intersects(creep.Hitbox, new Vector2f(0, 0), creep.Position)))
                        {
                            creep.Damage(this);
                        }
                    }

                    TargetCreep.Damage(this);

                    Strength = _strength;
                }
            }
        }

        public override void Draw(RenderTarget target)
        {
			if (TargetCreep != null)
            {
                float angle = TargetCreep.AngleTo(ParentTower);
                float distance = TargetCreep.DistanceTo(ParentTower);

                _sprite.Scale = new Vector2f(Width / Game.TileSize, distance / Game.TileSize);
                _sprite.Rotation = angle / (float)Math.PI * 180 + 90;
                _sprite.Position = ParentTower.Position + (TargetCreep.HitboxPosition - ParentTower.Position) / 2;
                _sprite.Color = new Color(255, 255, 255, (byte)(255 - ParentTower.Level * 25));

                //var points = Utils.Laser2Points(ParentTower.Position, TargetCreep.HitboxPosition, Width, angle);

                //for (int i = 0; i < points.Length; i++)
                //{
                //    _circleShapes[i].Position = points[i];
                //    target.Draw(_circleShapes[i]);
                //}

                //foreach (CircleShape c in _circleShapes)
                //{
                //    target.Draw(c);
                //}

                base.Draw(target);
            }
        }
    }
}
