using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class CharacterState
    {
        protected Scene scene;
        protected Character character;
        protected Texture2D texture;
        public int characterWidth;
        public int characterHeight;
        public int textureXmin;
        public int textureYmin;
        
        public CharacterState(Scene scene, Character character)
        {
            this.character = character;
            this.scene = scene;
        }

        public void ChangeState(CharacterState newState)
        {
            character.State = newState;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}
