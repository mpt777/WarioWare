using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midterm.Views
{
    public class GameFrame
    {
        protected Game1 game;
        public Vector2 position = new Vector2(0, 0);
        public Vector2 dimensions = new Vector2(0, 0);
        public bool active = true;
        public bool paused = false;
        public GameFrame(Game1 game)
        {
            this.game = game;
            this.position = new Vector2(0,0);
            this.dimensions = new Vector2(this.game.width, this.game.height);
            LoadContent();
        }
        public GameFrame(Game1 game, Vector2 position, Vector2 dimensions)
        {
            this.game = game;
            this.position = position;
            this.dimensions = dimensions;
            LoadContent();
        }
        virtual protected void LoadContent()
        {

        }
        virtual protected void Initialize()
        {

        }
        virtual public void Update(GameTime gameTime)
        {

        }
        virtual public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
