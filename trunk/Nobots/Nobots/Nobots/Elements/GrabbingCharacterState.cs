using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots
{
    class GrabbingCharacterState : CharacterState
    {
        public GrabbingCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("girl_moving");
            characterWidth = texture.Width/8;
            characterHeight = texture.Height/5;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            changeRunningTextures();
        }

        private Vector2 changeRunningTextures()
        {
            textureXmin += texture.Width / 8;

            if (textureXmin == (texture.Width/8)*5 && textureYmin == (texture.Height/5)*4)
            {
                textureXmin = 0;
                textureYmin = 0;
            }
            else if (textureXmin == texture.Width)
            {
                textureXmin = 0;
                textureYmin += texture.Height / 5;
            }

            return new Vector2(textureXmin, textureYmin);
        }

        public override void AAction()
        {
            character.State = new JumpingCharacterState(scene, character);
        }

        public override void Enter()
        {
            character.touchedBoxFriction = character.touchedBox.Friction;
            character.touchedBoxMass = character.touchedBox.Mass;
            character.touchedBox.Friction = 0.0f;
            character.touchedBox.Mass = 0.0f;
            character.sliderJoint = new SliderJoint(character.torso, character.touchedBox, Vector2.Zero, Vector2.Zero, 0, Vector2.Distance(character.torso.Position, character.touchedBox.Position));
            character.sliderJoint.CollideConnected = true;
            scene.World.AddJoint(character.sliderJoint);
        }

        public override void Exit()
        {
            scene.World.RemoveJoint(character.sliderJoint);
            character.touchedBox.Friction = character.touchedBoxFriction;
            character.touchedBox.Mass = character.touchedBoxMass;
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
            character.body.FixedRotation = false;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 50;
            character.Effect = SpriteEffects.None;
        }

        public override void LeftAction()
        {
            character.body.FixedRotation = false;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = -50;
            character.Effect = SpriteEffects.FlipHorizontally;
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
