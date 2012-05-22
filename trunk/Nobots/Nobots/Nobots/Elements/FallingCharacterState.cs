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
    public class FallingCharacterState : CharacterState
    {
        int rows = 1;
        int columns = 3;

        public FallingCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("falling");
            characterWidth = texture.Width / columns;
            characterHeight = texture.Height / rows;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            changeRunningTextures(gameTime);
        }

        float seconds = 0;
        private Vector2 changeRunningTextures(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds > 0.1f)
            {
                seconds -= 0.1f;
                textureXmin += texture.Width / columns;

                if (textureXmin == texture.Width)
                    textureXmin = 0;
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
        }

        public override void Exit()
        {
            character.LastLadder = null;
            character.body.OnCollision -= body_OnCollision;
        }

        public override void UpAction()
        {
            if (character.Ladder != null && character.Ladder != character.LastLadder)
            {
                character.State = new ClimbingCharacterState(scene, character);
                character.State.UpAction();
            }
        }

        public override void DownAction()
        {
            if (character.Ladder != null && character.Ladder != character.LastLadder)
            {
                character.State = new ClimbingCharacterState(scene, character);
                character.State.DownAction();
            }
        }

        public override void RightAction()
        {
            character.torso.LinearVelocity = new Vector2(3, character.torso.LinearVelocity.Y);
            character.Effect = SpriteEffects.None;
        }

        public override void LeftAction()
        {
            character.torso.LinearVelocity = new Vector2(-3, character.torso.LinearVelocity.Y);
            character.Effect = SpriteEffects.FlipHorizontally;
        }
    }
}
