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
    class JumpingCharacterState : CharacterState
    {
        bool isJumping = false;
        public JumpingCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("jumpingStart");
            characterWidth = texture.Width / 5;
            characterHeight = texture.Height;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            changeJumpingTextures(gameTime);
        }

        float seconds = 0;
        private Vector2 changeJumpingTextures(GameTime gameTime)
        {
            if (!isJumping)
            {
                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (seconds > 0.05f)
                {
                    seconds -= 0.05f;
                    textureXmin += texture.Width / 5;
                }

                if (textureXmin == texture.Width)
                {
                    isJumping = true;
                    textureXmin = 0;
                    textureYmin = 0;
                    texture = scene.Game.Content.Load<Texture2D>("jumpingAir");
                    character.texture = texture;
                }
            }
            else
            {
                if (!(textureXmin == (texture.Width / 5) * 2 && character.torso.LinearVelocity.Y < 0) &&
                    !(textureXmin == (texture.Width / 5) * 4 && character.torso.LinearVelocity.Y > 0))
                    seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (seconds > 0.04f)
                {
                    seconds -= 0.04f;
                    textureXmin += texture.Width / 5;

                    if (textureXmin == texture.Width)
                        textureXmin = 0;
                }
            }
            return new Vector2(textureXmin, textureYmin);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (character.contactsNumber > 0)
                character.State = new IdleCharacterState(scene, character);
            return true;
        }

        public override void Enter()
        {
            character.body.OnCollision += body_OnCollision;
            if (character.contactsNumber > 0 || character.Ladder != null)
                character.torso.ApplyForce(new Vector2(0, -14000));
        }

        public override void Exit()
        {
            character.body.OnCollision -= body_OnCollision;
        }

        public override void RightAction()
        {
            character.torso.LinearVelocity = new Vector2(2.5f, character.torso.LinearVelocity.Y);
            character.Effect = SpriteEffects.None;
        }

        public override void LeftAction()
        {
            character.torso.LinearVelocity = new Vector2(-2.5f, character.torso.LinearVelocity.Y);
            character.Effect = SpriteEffects.FlipHorizontally;
        }

        public override void UpAction()
        {
            if (character.Ladder != null)
            {
                character.State = new ClimbingCharacterState(scene, character);
                character.State.UpAction();
            }
        }

        public override void DownAction()
        {
            if (character.Ladder != null)
            {
                character.State = new ClimbingCharacterState(scene, character);
                character.State.DownAction();
            }
        }
    }
}
