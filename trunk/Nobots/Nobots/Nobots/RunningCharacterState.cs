using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class RunningCharacterState : CharacterState
    {

        public RunningCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("girl_moving");
            characterWidth = texture.Width/8;
            characterHeight = texture.Height/5;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
            Console.WriteLine(textureXmin + ", " + textureYmin);
        }

        public override void Update()
        {
            base.Update();
            changeRunningTextures();
        }

        private Vector2 changeRunningTextures()
        {
            if (textureXmin == (texture.Width/8)*5 && textureYmin == (texture.Height/5)*4)
            {
                textureXmin = 0;
                textureYmin = 0;
            }
            else if (textureXmin == texture.Width)
            {
                textureXmin = 0;
                textureYmin += texture.Height / 5;
            }
            else
            {
                textureXmin += texture.Width / 8;
               // textureYmin += texture.Height / 5;
            }
            Console.WriteLine("X: " + textureXmin + ", Y: " + textureYmin + ", Width: " + characterWidth +
                ", Height: " + characterHeight);

            return new Vector2(textureXmin, textureYmin);
        }
    }
}
