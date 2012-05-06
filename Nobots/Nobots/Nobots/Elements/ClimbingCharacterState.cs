using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Input;

namespace Nobots.Elements
{
    class ClimbingCharacterState : CharacterState
    {
        private bool moving = true;

        public ClimbingCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("climbing");
            characterWidth = texture.Width / 7;
            characterHeight = texture.Height;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (!moving)
            {
                character.torso.LinearVelocity = new Vector2(0, 0);
                character.body.LinearVelocity = new Vector2(0, 0);
            }
            else
                changeIdleTextures(gameTime);

            if (character.Ladder == null)
                character.State = new FallingCharacterState(scene, character);
            else if (character.contactsNumber > 0)
                character.State = new IdleCharacterState(scene, character);
        }

        float seconds = 0;
        private Vector2 changeIdleTextures(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (seconds > 0.08f)
            {
                seconds -= 0.08f;
                textureXmin += texture.Width / 7;

                if (textureXmin == texture.Width)
                    textureXmin = 0;
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
            if (character.contactsNumber == 0)
                character.LastLadder = character.Ladder;
            character.body.IgnoreGravity = false;
            character.torso.IgnoreGravity = false;
        }

        public override void UpAction()
        {
            if (character.IsLadderInRangeToGoUp(character.Ladder))
            {
                moving = true;
                character.torso.LinearVelocity = new Vector2(0, -3);
                character.body.LinearVelocity = new Vector2(0, -3);
            }
            else
            {
                moving = false;
                character.torso.LinearVelocity = new Vector2(0, 0);
                character.body.LinearVelocity = new Vector2(0, 0);
            }
        }

        public override void DownAction()
        {
            if (character.IsLadderInRangeToGoDown(character.Ladder))
            {
                moving = true;
                character.torso.LinearVelocity = new Vector2(0, 3);
                character.body.LinearVelocity = new Vector2(0, 3);
            }
            else
            {
                moving = false;
                character.torso.LinearVelocity = new Vector2(0, 0);
                character.body.LinearVelocity = new Vector2(0, 0);
            }
        }

        public override void UpActionStop()
        {
            moving = false;
            character.torso.LinearVelocity = new Vector2(0, 0);
            character.body.LinearVelocity = new Vector2(0, 0);
        }

        public override void DownActionStop()
        {
            moving = false;
            character.torso.LinearVelocity = new Vector2(0, 0);
            character.body.LinearVelocity = new Vector2(0, 0);
        }

        public override void AActionStart()
        {
            character.State = new JumpingCharacterState(scene, character);
        }

        public override void LeftAction()
        {
            character.State = new FallingCharacterState(scene, character);
            character.State.LeftAction();
        }

        public override void RightAction()
        {
            character.State = new FallingCharacterState(scene, character);
            character.State.RightAction();
        }
    }
}
