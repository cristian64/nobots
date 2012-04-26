using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    class DyingCharacterState : CharacterState
    {
        float totalSeconds = 0;

        public DyingCharacterState(Scene scene, Character character) 
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("idle");
            characterWidth = texture.Width / 10;
            characterHeight = texture.Height / 2;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            totalSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            changeIdleTextures(gameTime);
            if(totalSeconds > 2)
                character.State = new DeadCharacterState(scene, character);
        }

        float seconds = 0;
        private Vector2 changeIdleTextures(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
       
            if (seconds > 0.04f)
            {
                seconds -= 0.04f;
                textureXmin += texture.Width / 10;

                if (textureXmin == (texture.Width / 10) * 5 && textureYmin == texture.Height / 2)
                {
                    textureXmin = 0;
                    textureYmin = 0;
                }
                else if (textureXmin == texture.Width)
                {
                    textureXmin = 0;
                    textureYmin += texture.Height / 2;
                }
            }
            if (textureYmin == 0 && textureXmin == texture.Width / 10)
            {
                seconds -= 4f; ;
                textureXmin = (texture.Width / 10 * 2);
            }

            return new Vector2(textureXmin, textureYmin);
        }

        public override void Enter()
        {
            character.torso.Enabled = false;
            character.body.Enabled = false;
            character.body.FixedRotation = true;
            character.body.AngularVelocity = 0;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.LinearVelocity = Vector2.UnitY * character.body.LinearVelocity;
        }

        public override void YActionStart()
        {
        }

        public override void UpAction()
        {
        }

        public override void DownAction()
        {
        }

        public override void AActionStart()
        {
        }

        public override void  BActionStart()
        {
        }

        public override void RightAction()
        {
        }

        public override void LeftAction()
        {
        }

        public override void RightActionStart()
        {
        }

        public override void LeftActionStart()
        {
        }
    }
}
