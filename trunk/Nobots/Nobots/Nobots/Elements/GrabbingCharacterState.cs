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

        private int rows = 2;
        private int columns = 4;
        private int framesInLastRow = 3;
        bool isPushing = false;

        public GrabbingCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("pushing");
            characterWidth = texture.Width / columns;
            characterHeight = texture.Height / rows;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        bool moving = false;
        public override void Update(GameTime gameTime)
        {
            if (moving)
                changeGrabbingTextures(gameTime);
        }

        float seconds = 0;
        private Vector2 changeGrabbingTextures(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds > 0.1f)
            {
                seconds -= 0.1f;
                if (isPushing)
                {
                    character.texture = texture = scene.Game.Content.Load<Texture2D>("pushing");
                    characterWidth = texture.Width / columns;
                    characterHeight = texture.Height / rows;

                    textureXmin += texture.Width / columns;

                    if (textureXmin == (texture.Width / columns) * framesInLastRow && textureYmin == texture.Height / rows)
                    {
                        textureXmin = 0;
                        textureYmin = 0;
                    }
                    else if (textureXmin >= texture.Width)
                    {
                        textureXmin = 0;
                        textureYmin += texture.Height / rows;
                    }
                }
                else
                {
                    character.texture = texture = scene.Game.Content.Load<Texture2D>("pulling");
                    characterWidth = texture.Width / 5;
                    characterHeight = texture.Height;
                    textureYmin = 0;

                    textureXmin += texture.Width / 5;

                    if (textureXmin >= texture.Width)
                        textureXmin = 0;
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
            moving = true;
            if (character.touchedBody != null && ((character.touchedBody.UserData is IPushable && character.Position.X < character.touchedBody.Position.X) ||
                (character.touchedBody.UserData is IPullable && character.Position.X > character.touchedBody.Position.X)))
            {
                character.body.FixedRotation = false;
                character.torso.LinearVelocity = new Vector2(2, character.torso.LinearVelocity.Y);
                isPushing = character.Position.X < character.touchedBody.Position.X ? true : false;
            }
        }

        public override void LeftAction()
        {
            moving = true;
            if (character.touchedBody != null && ((character.touchedBody.UserData is IPushable && character.Position.X > character.touchedBody.Position.X) ||
                (character.touchedBody.UserData is IPullable && character.Position.X < character.touchedBody.Position.X)))
            {
                character.body.FixedRotation = false;
                character.torso.LinearVelocity = new Vector2(-2, character.torso.LinearVelocity.Y);
                isPushing = character.Position.X > character.touchedBody.Position.X ? true : false;
            }
        }

        public override void RightActionStop()
        {
            moving = false;
            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
            if (character.touchedBody != null)
                character.touchedBody.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
        }

        public override void LeftActionStop()
        {
            moving = false;
            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
            if (character.touchedBody != null)
                character.touchedBody.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
        }
    }
}
