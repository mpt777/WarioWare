using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Breakout.Particles;

namespace Midterm.Entities
{
    public class Player : Entity
    {
        private float _speed = 500f;
        private Vector2 _velocity = new Vector2(0, 0);
        private float minX;
        private float maxX;
        public Player(Game1 game, float minX, float maxX) : base(game)
        {
            height = 20;
            width = 20;
            this.minX= minX;
            this.maxX = maxX;
            this._position = new Vector2((this.maxX - this.minX) + (this.width/2), this.game.height - (this.height * 2));
            this.LoadContent();
        }
        protected override void LoadContent()
        {
            _sprite = new Texture2D(game.GraphicsDevice, 1, 1);
            _sprite.SetData(new[] { Color.Blue });
        }
        protected override void ProcessInput()
        {
            _velocity.X = 0;
            if (game.keyboard.IsKeyDown(Keys.Left))
            {
                _velocity.X = -_speed;
            }
            if (game.keyboard.IsKeyDown(Keys.Right))
            {
                _velocity.X = _speed;
            }
        }
        override public void Update(GameTime gameTime)
        {
            _prevPosition = _position;

            ProcessInput();
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ProcessCollision();

        }
        public void ProcessCollision()
        {
            _position = Vector2.Clamp(_position, new Vector2(minX, Position().Y), new Vector2(maxX - width, Position().Y));
        }
        public void Kill()
        {

        }
        public ParticleEmitter DestroyedEmitter()
        {
            Vector2 pos = this.Position();
            ParticleEmitter emitter = new ParticleEmitter(
                this.game.Content,
                    new TimeSpan(0, 0, 0, 0, 2),
                    new Rectangle((int)pos.X, (int)pos.Y, this.width, this.height),
                    5,
                    5,
                    new TimeSpan(0, 0, 0, 0, 600),
                    _sprite,
                    new TimeSpan(0, 0, 0, 0, 100)
                 ); ;
            emitter.Gravity = new Vector2(0, 0.1f);
            return emitter;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Rectangle((int)_position.X, (int)_position.Y, width, height), Color.White);
        }
    }
}
