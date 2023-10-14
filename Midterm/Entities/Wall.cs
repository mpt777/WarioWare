using Breakout.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midterm.Entities
{
    public class Wall : Entity
    {
        private float _speed = 500f;
        private Vector2 _velocity = new Vector2(0, 0);
        private Color _color;
        private ParticleEmitter _emitter;
        private Texture2D _emitterSprite;
        public Wall(Game1 game, Vector2 position, int width, int height, Vector2 velocity) : base(game)
        {
            this._position = position;
            this.width = width;
            this.height = height;
            this._velocity = velocity;
            this.LoadContent();
            this.SetColor(Color.Purple);

            _emitterSprite = new Texture2D(game.GraphicsDevice, 1, 1);
            _emitterSprite.SetData(new[] { Color.Green });
        }
        public void SetColor(Color color)
        {
            this._color = color;
            _sprite.SetData(new[] { this._color });
        }

        public void StartEmmitter()
        {
            Vector2 pos = this.Position();
            int amount = 0;
            if (width != 0)
            {
                amount = 10000 / width;
            }
            _emitter = new ParticleEmitter(
                this.game.Content,
                    new TimeSpan(0, 0, 0, 0, amount),
                    new Rectangle((int)pos.X-2, (int)pos.Y, this.width, 1),
                    8,
                    1,
                    new TimeSpan(0, 0, 0, 0, 400),
                    _emitterSprite,
                    new TimeSpan(0, 0, 100)
                 ); ;
            _emitter.Gravity = new Vector2(0, 0);
        }

        override public void Update(GameTime gameTime)
        {
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_emitter != null)
            {
                this._emitter.SetRectangle(new Rectangle((int)_position.X, (int)_position.Y, width, height));
                this._emitter.Update(gameTime);
            }
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Rectangle((int)_position.X, (int)_position.Y, width, height), Color.White);
            if (_emitter != null)
            {
                this._emitter.Draw(spriteBatch);
            }
        }
    }
}
