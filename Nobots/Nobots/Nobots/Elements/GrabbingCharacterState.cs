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
        private float touchedBodyMass;
        private float touchedBodyFriction;
        private SliderJoint sliderJoint;

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
            touchedBodyFriction = character.touchedBody.Friction;
            touchedBodyMass = character.touchedBody.Mass;
            character.touchedBody.Friction = 0;
            character.touchedBody.Mass = 100;
            sliderJoint = new SliderJoint(character.torso, character.touchedBody, Vector2.Zero, Vector2.Zero, 0, Vector2.Distance(character.torso.Position, character.touchedBody.Position));
            sliderJoint.CollideConnected = true;
            scene.World.AddJoint(sliderJoint);
        }

        public override void Exit()
        {
            scene.World.RemoveJoint(sliderJoint);
            if (character.touchedBody != null)
            {
                character.touchedBody.Friction = touchedBodyFriction;
                character.touchedBody.Mass = touchedBodyMass;
            }

            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
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
                character.torso.LinearVelocity = new Vector2(2, character.torso.LinearVelocity.Y);
                character.Effect = SpriteEffects.None;
            }
        }

        public override void LeftAction()
        {
            if (character.touchedBody != null && ((character.touchedBody.UserData is IPushable && character.Position.X > character.touchedBody.Position.X) ||
                (character.touchedBody.UserData is IPullable && character.Position.X < character.touchedBody.Position.X)))
            {
                character.body.FixedRotation = false;
                character.torso.LinearVelocity = new Vector2(-2, character.torso.LinearVelocity.Y);
                character.Effect = SpriteEffects.FlipHorizontally;
            }
        }

        public override void RightActionStop()
        {
            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
            if (character.touchedBody != null)
                character.touchedBody.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
        }

        public override void LeftActionStop()
        {
            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
            if (character.touchedBody != null)
                character.touchedBody.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
        }
    }
}
