using Midterm.State;
using Midterm.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midterm.Entities;
using Microsoft.Xna.Framework.Input;

namespace Midterm.Views
{
    public class HighScoreView : GameFrame
    {
        public List<GameState> highScores = new List<GameState>();
        public List<TextDisplay> highScoresDrawable = new List<TextDisplay>();
        private TextDisplay clearScores;
        private TextDisplay _title;
        private Texture2D _background;
        public HighScoreView(Game1 game) : base(game) 
        {
            this.Initialize();
        }

        protected override void Initialize()
        {
            this.game.persistance.LoadGameState();
            this._title = new TextDisplay(this.game, "-- High Scores --", new Vector2(this.game.width/2, 100));
            this.clearScores = new TextDisplay(this.game, "Clear High Scores", new Vector2(this.game.width/2, 600));
            this._title.Center();
            this.clearScores.Center();

            this.highScores = this.game.persistance.states;
            this.game.persistance.states.Sort((a, b) => b.Score.CompareTo(a.Score));
            if (this.game.persistance.states.Count != 0)
            {
                this.game.persistance.states = this.game.persistance.states.GetRange(0, Math.Min(this.game.persistance.states.Count, 5));
            }

            this.highScores = this.game.persistance.states;
            for (int i = 0; i < this.highScores.Count; i++)
            {
                GameState highScore = this.highScores[i];

                TimeSpan timeSpan = TimeSpan.FromMilliseconds(highScore.Score);
                string formattedTime = string.Format("{0} {1:D3}", (int)timeSpan.TotalSeconds, timeSpan.Milliseconds);

                this.highScoresDrawable.Add(new TextDisplay(this.game, $"Score: {formattedTime}  | Time: {highScore.TimeStamp}", new Vector2(this.game.width/2, 200 + (50 * i))));
                this.highScoresDrawable[i].Center();
            }
            this.LoadContent();
        }
        protected override void LoadContent()
        {
            this._background = this.game.Content.Load<Texture2D>("Images/sunset");
        }
        private void ClearHighScores()
        {
            this.highScores.Clear();
            this.highScoresDrawable.Clear();
            this.game.highScores.Clear();
            this.game.persistance.states.Clear();
            this.game.persistance.SaveGameState(new List<GameState>());
        }
        public override void Update(GameTime gameTime)
        {
            if (this.game.keyboard.JustPressed(Keys.Escape))
            {
                this.game.RemoveFrame();
            }
            if (this.game.keyboard.IsOver(this.clearScores.bounds))
            {
                if (this.game.keyboard.JustLeftMouseDown())
                {
                    ClearHighScores();
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._background, new Rectangle(0, 0, this.game.width, this.game.height), Color.White);
            this.clearScores.Draw(spriteBatch); 
            this._title.Draw(spriteBatch);
            

            for (int i=0; i < this.highScoresDrawable.Count; i++)
            {
                this.highScoresDrawable[i].Draw(spriteBatch);
            }
        }
    }
}
