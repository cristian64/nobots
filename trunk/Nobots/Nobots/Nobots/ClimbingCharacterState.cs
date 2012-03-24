using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots
{
    class ClimbingState : CharacterState
    {
        public ClimbingState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("girl_moving");
            characterWidth = texture.Width / 8;
            characterHeight = texture.Height / 5;
            character.texture = texture;
            textureXmin = (texture.Width * 3) / 8;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
/*            if (character.Ladder != null)
                character.State = new IdleCharacterState(scene, character);*/
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
            character.torso.LinearVelocity = new Vector2(character.torso.LinearVelocity.X, -1);
        }

        public override void DownAction()
        {
            character.torso.LinearVelocity = new Vector2(character.torso.LinearVelocity.X, 1);
        }

        public override void UpActionStop()
        {
            character.torso.LinearVelocity = new Vector2(character.torso.LinearVelocity.X, 0);
        }

        public override void DownActionStop()
        {
            character.torso.LinearVelocity = new Vector2(character.torso.LinearVelocity.X, 0);
        }

        public override void AActionStart()
        {
            character.State = new JumpingCharacterState(scene, character);
        }
    }
}
