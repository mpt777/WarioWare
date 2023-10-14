using Midterm.Entities;
using Midterm.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Breakout.Particles;

namespace Midterm.Views
{
    public class Level : GameFrame
    {

        private Song _music;
        private Player _player;

        private int _blockHeight = 20;
        private int _halfGap = 100;
        private float _gapCenter;

        private float _accumulator = 0f;
        private TimeSpan _interval;
        private Vector2 _wallVelocity;

        private Texture2D _texture;

        private List<Wall> _walls = new();
        private List<Wall> _preWalls = new();

        

        private TextDisplay _scoreDisplay;
        private int _milliseconds = 0;
        public Level(Game1 game, Vector2 position, Vector2 dimensions) : base(game, position, dimensions)
        {
            this._player = new Player(game, this.position.X, this.position.X + this.dimensions.X);
            this._gapCenter = this.dimensions.X / 2;


            this._wallVelocity = new Vector2(0, 200);
            this._interval = new TimeSpan(0, 0, 0, 0, 700);

            _scoreDisplay = new TextDisplay(game, "Time: 0", new Vector2(this.position.X + this.dimensions.X + 10, 10));

            this.CreatePreWalls();
        }
        
        protected override void LoadContent()
        {
            //this._music = this.game.Content.Load<Song>("Audio/summer");
            this._texture = this.game.Content.Load<Texture2D>("Images/sunset");
        }

        private void SetGapCenter()
        {
            Random random = new Random();
            float min = 0.15f;
            float max = 0.75f;

            float randomFloat = (float)(random.NextDouble() * (max - min) + min);

            int sign = random.Next(2) == 0 ? -1 : 1; // Generates -1 or 1 randomly

            float delta = randomFloat * _halfGap * 2 * sign;

            if (delta + _gapCenter - _halfGap < 0 || delta + _gapCenter + _halfGap > this.dimensions.X)
            {
                delta *= -1;
            }

            _gapCenter += delta;
        }
        private void CreatePreWalls()
        {
            _preWalls.Clear();
            CreateWalls();
            SetGapCenter();

            // Left wall
            this._preWalls.Add(new Wall(
                this.game, 
                new Vector2(this.position.X, 0), 
                (int)(this._gapCenter - this._halfGap), 
                _blockHeight, 
                new Vector2(0, 0)
            ));

            // right wall
            this._preWalls.Add(new Wall(
                this.game, 
                new Vector2((this.position.X + this._gapCenter + this._halfGap), 0), 
                (int)(this.dimensions.X - this._gapCenter - this._halfGap), 
                _blockHeight, 
                new Vector2(0, 0)
            ));

            foreach(Wall wall in _preWalls)
            {
                wall.SetColor(Color.Pink);
            }
        }
        private void CreateWalls()
        {
            Wall wallL = new Wall(
                this.game,
                new Vector2(this.position.X, 0),
                (int)(this._gapCenter - this._halfGap),
                _blockHeight,
                _wallVelocity
            );

            Wall wallR = new Wall(
                this.game,
                new Vector2((this.position.X + this._gapCenter + this._halfGap), 0),
                (int)(this.dimensions.X - this._gapCenter - this._halfGap),
                _blockHeight,
                _wallVelocity
            );

            wallL.StartEmmitter();
            wallR.StartEmmitter();


            this._walls.Add(wallL);
            this._walls.Add(wallR);

        }
        private void ProcessCollisions()
        {
            if (_player == null)
            {
                return;
            }
            Rectangle playerRectangle = _player.Rectangle();
            foreach(Wall wall in _walls)
            {
                if (wall.Rectangle().Intersects(playerRectangle))
                {
                    KillPlayer();
                }
            }
        }
        private void RemoveOldWalls()
        {
            List<Wall> wallsToRemove = new();

            foreach (Wall wall in _walls)
            {
                if (wall.Position().Y >= this.dimensions.Y)
                {
                    wallsToRemove.Add(wall);
                }
            }
            foreach (Wall wall in wallsToRemove)
            {
                _walls.Remove(wall);
            }
            wallsToRemove.Clear();
        }
        private void ProcessWallGenerator(GameTime gameTime)
        { 
            _accumulator += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_accumulator >= _interval.TotalSeconds)
            {
                _accumulator -= (float)_interval.TotalSeconds;
                CreatePreWalls();
            }
        }
        private void Pause()
        {
            
        }
        private void KillPlayer() 
        {
            _player.Kill();
            
            this.game.AddHighScore(new State.GameState((uint)this._milliseconds, 1));

            this.game.AddFrame(new PauseMenu(this.game, this._milliseconds, _player.DestroyedEmitter()));
            //this.game.RemoveFrame();
            _player = null;
            this.paused = true;
        }
        private void UpdateScore(GameTime gameTime)
        {
            _milliseconds += gameTime.ElapsedGameTime.Milliseconds;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(_milliseconds);
            string formattedTime = string.Format("{0} {1:D3}", (int)timeSpan.TotalSeconds, timeSpan.Milliseconds);
            _scoreDisplay.SetString($"Time: {formattedTime}");
        }
        public override void Update(GameTime gameTime)
        {
            this.ProcessWallGenerator(gameTime);
            this.UpdateScore(gameTime);
            if (_player != null)
            {
                _player.Update(gameTime);
            }
            foreach (Wall wall in _walls)
            {
                wall.Update(gameTime);
            }
            foreach (Wall wall in _preWalls)
            {
                wall.Update(gameTime);
            }
            RemoveOldWalls();
            ProcessCollisions();

            _scoreDisplay.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.dimensions.X, (int)this.dimensions.Y), Color.White);

            if (_player != null)
            {
                _player.Draw(spriteBatch);
            }
            foreach (Wall wall in _walls)
            {
                wall.Draw(spriteBatch);
            }
            foreach (Wall wall in _preWalls)
            {
                wall.Draw(spriteBatch);
            }
            _scoreDisplay.Draw(spriteBatch);
        }
    }
}
