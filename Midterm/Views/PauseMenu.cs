using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midterm.UI;
using Microsoft.Xna.Framework.Input;
using Breakout.Particles;

namespace Midterm.Views
{
    public class PauseMenu : GameFrame
    {
        private TextDisplay _mainMenu;
        private TextDisplay _title;
        private TextDisplay _scoreDisplay;
        private Level _level;
        private Texture2D _sprite;
        public ParticleEmitter _emitter;

        private float _accumulator = 0;
        private TimeSpan _timeSpan = new TimeSpan(0,0,2);
        public PauseMenu(Game1 game, int score, ParticleEmitter emitter) : base(game) 
        {
            this._title = new TextDisplay(this.game, "-- Game Over --", new Vector2(this.game.width/2, 250));
            this._mainMenu = new TextDisplay(this.game, "", new Vector2(this.game.width/2, 450));

            TimeSpan timeSpan = TimeSpan.FromMilliseconds(score);
            string formattedTime = string.Format("{0} {1:D3}", (int)timeSpan.TotalSeconds, timeSpan.Milliseconds);

            this._scoreDisplay = new TextDisplay(this.game, $"Score: {score}", new Vector2(this.game.width/2, 350));
            this._title.Center();
            this._mainMenu.Center();
            this._scoreDisplay.Center();

            this._emitter = emitter;

            this.LoadContent();
        }
        override protected void LoadContent()
        {
            _sprite = new Texture2D(game.GraphicsDevice, 1, 1);
            _sprite.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {

            if (this.game.keyboard.IsOver(this._mainMenu.bounds))
            {
                if (this.game.keyboard.JustLeftMouseDown())
                {
                    this.game.RemoveFrame();
                    this.game.RemoveFrame();
                }
            }

            _emitter.Update(gameTime);
            this._accumulator += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this._accumulator >= _timeSpan.TotalSeconds)
            {
                this._mainMenu.SetString("Main Menu");
                this._mainMenu.Center();

            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Rectangle(this.game.width/2-200, 150, 400, 400), Color.Black);
            this._title.Draw(spriteBatch);
            this._mainMenu.Draw(spriteBatch);
            this._scoreDisplay.Draw(spriteBatch);

            _emitter.Draw(spriteBatch);
        }
    }
}
