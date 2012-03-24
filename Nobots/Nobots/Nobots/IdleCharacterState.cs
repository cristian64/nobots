using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class IdleCharacterState : CharacterState
    {
        public IdleCharacterState(Scene scene, Character character) 
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("girl");
            character.texture = texture;
            characterWidth = texture.Width;
            characterHeight = texture.Height;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Enter()
        {
            character.body.FixedRotation = false;
            character.body.AngularVelocity = 0;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.LinearVelocity = Vector2.UnitY * character.body.LinearVelocity;
        }

        public override void AAction()
        {
            character.State = new JumpingCharacterState(scene, character);
        }

        public override void  BActionStart()
        {
            character.State = new GrabbingCharacterState(scene, character);
            character.State.BActionStart();
        }

        public override void RightAction()
        {
            character.State = new RunningCharacterState(scene, character);
            character.State.RightAction();
        }

        public override void LeftAction()
        {
            character.State = new RunningCharacterState(scene, character);
            character.State.LeftAction();
        }

        public override void RightActionStart()
        {
            character.State = new RunningCharacterState(scene, character);
            character.State.RightActionStart();
        }

        public override void LeftActionStart()
        {
            character.State = new RunningCharacterState(scene, character);
            character.State.LeftActionStart();
        }
    }
}
