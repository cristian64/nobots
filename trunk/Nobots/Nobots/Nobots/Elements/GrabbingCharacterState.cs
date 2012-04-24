using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots.Elements
{
    class GrabbingCharacterState : CharacterState
    {
        public GrabbingCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("running");//("girl_moving");
            characterWidth = texture.Width / 10;// 8;
            characterHeight = texture.Height / 2;///5;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            changeRunningTextures(gameTime);
        }

        float seconds = 0;
        private Vector2 changeRunningTextures(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds > 0.04f)
            {
                seconds -= 0.04f;
                textureXmin += texture.Width / 10;

                if (textureXmin == (texture.Width / 10) * 4 && textureYmin == texture.Height / 2)
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

            return new Vector2(textureXmin, textureYmin);
        }

        public override void AAction()
        {
            character.State = new JumpingCharacterState(scene, character);
        }

        public override void Enter()
        {
            character.touchedBodyFriction = character.touchedBody.Friction;
            character.touchedBodyMass = character.touchedBody.Mass;
            character.touchedBody.Friction = 0.0f;
            character.touchedBody.Mass = 0.0f;
            character.sliderJoint = new SliderJoint(character.torso, character.touchedBody, Vector2.Zero, Vector2.Zero, 0, Vector2.Distance(character.torso.Position, character.touchedBody.Position));
            character.sliderJoint.CollideConnected = true;
            scene.World.AddJoint(character.sliderJoint);
        }

        public override void Exit()
        {
            scene.World.RemoveJoint(character.sliderJoint);
            if (character.touchedBody != null)
            {
                character.touchedBody.Friction = character.touchedBodyFriction;
                character.touchedBody.Mass = character.touchedBodyMass;
            }
        }

        public override void BActionStart()
        {
        }

        public override void BAction()
        {
        }

        public override void BActionStop()
        {
            character.State = new IdleCharacterState(scene, character);
        }

        public override void RightAction()
        {
            if (character.touchedBody != null && ((character.touchedBody.UserData is IPushable && character.Position.X < character.touchedBody.Position.X) ||
                (character.touchedBody.UserData is IPullable && character.Position.X > character.touchedBody.Position.X)))
            {
                character.body.FixedRotation = false;
                character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
                character.body.AngularVelocity = 50;
                character.Effect = SpriteEffects.None;
            }
        }

        public override void LeftAction()
        {
            if (character.touchedBody != null && ((character.touchedBody.UserData is IPushable && character.Position.X > character.touchedBody.Position.X) ||
                (character.touchedBody.UserData is IPullable && character.Position.X < character.touchedBody.Position.X)))
            {
                character.body.FixedRotation = false;
                character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
                character.body.AngularVelocity = -50;
                character.Effect = SpriteEffects.FlipHorizontally;
            }
        }

        public override void RightActionStop()
        {
            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
        }

        public override void LeftActionStop()
        {
            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
        }
    }
}
