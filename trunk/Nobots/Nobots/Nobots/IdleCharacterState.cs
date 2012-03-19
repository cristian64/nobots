using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Nobots
{
    public class IdleCharacterState : CharacterState
    {
        public IdleCharacterState(Scene scene, Character character) 
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("girl");
            character.texture = texture;
            characterWidth = texture.Width;
            characterHeight = texture.Height;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
