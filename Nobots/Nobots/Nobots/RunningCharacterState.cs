using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class RunningCharacterState : CharacterState
    {
        public RunningCharacterState(Scene scene, Character character)
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

        public override void Exit()
        {
            character.body.FixedRotation = true;
            character.body.AngularVelocity = 0;
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

        public override void AActionStart()
        {
            character.State = new JumpingCharacterState(scene, character);
        }

        public override void RightAction()
        {
            character.body.FixedRotation = false;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = +80;
            character.Effect = SpriteEffects.None;
        }

        public override void LeftAction()
        {
            character.body.FixedRotation = false;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = -80;
            character.Effect = SpriteEffects.FlipHorizontally;
        }

        public override void RightActionStop()
        {
            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
            character.State = new IdleCharacterState(scene, character);
        }

        public override void LeftActionStop()
        {
            character.body.FixedRotation = true;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.AngularVelocity = 0;
            character.State = new IdleCharacterState(scene, character);
        }
    }
}
