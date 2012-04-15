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
