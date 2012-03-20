using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Nobots
{
    class JumpingCharacterState : CharacterState
    {
        public JumpingCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("girl_moving");
            characterWidth = texture.Width/8;
            characterHeight = texture.Height/5;
            character.texture = texture;
            textureXmin = (texture.Width * 3) / 8;
            textureYmin = 0;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
