using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots.Elements
{
    class ClimbingCharacterState : CharacterState
    {
        public ClimbingCharacterState(Scene scene, Character character)
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
            changeIdleTextures(gameTime);
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
            character.body.IgnoreGravity = true;
            character.torso.IgnoreGravity = true;
            character.torso.LinearVelocity = Vector2.Zero;
        }

        public override void Exit()
        {
            character.body.IgnoreGravity = false;
            character.torso.IgnoreGravity = false;
        }

        public override void UpAction()
        {
            character.torso.LinearVelocity = new Vector2(0, -3);
            character.body.LinearVelocity = new Vector2(0, -3);
        }

        public override void DownAction()
        {
            character.torso.LinearVelocity = new Vector2(0, 3);
            character.body.LinearVelocity = new Vector2(0, 3);
        }

        public override void UpActionStop()
        {
            character.torso.LinearVelocity = new Vector2(0, 0);
            character.body.LinearVelocity = new Vector2(0, 0);
        }

        public override void DownActionStop()
        {
            character.torso.LinearVelocity = new Vector2(0, 0);
            character.body.LinearVelocity = new Vector2(0, 0);
        }

        public override void AActionStart()
        {
            character.State = new JumpingCharacterState(scene, character);
        }
    }
}
