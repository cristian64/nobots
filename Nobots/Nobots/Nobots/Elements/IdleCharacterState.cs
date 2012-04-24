using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    public class IdleCharacterState : CharacterState
    {
        public IdleCharacterState(Scene scene, Character character) 
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("idle");//("girl_moving");
            characterWidth = texture.Width / 10;// 8;
            characterHeight = texture.Height / 2;///5;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            changeIdleTextures(gameTime);
        }

        float seconds = 0;
        private Vector2 changeIdleTextures(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
       
            if (seconds > 0.04f)
            {
                seconds -= 0.04f;
                textureXmin += texture.Width / 10;

                if (textureXmin == (texture.Width / 10) * 5 && textureYmin == texture.Height / 2)
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
            if (textureYmin == 0 && textureXmin == texture.Width / 10)
            {
                seconds -= 4f; ;
                textureXmin = (texture.Width / 10 * 2);
            }

            return new Vector2(textureXmin, textureYmin);
        }

        public override void Enter()
        {
            character.body.FixedRotation = true;
            character.body.AngularVelocity = 0;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.LinearVelocity = Vector2.UnitY * character.body.LinearVelocity;
        }

        public override void YActionStart()
        {
            character.State = new ComaCharacterState(scene, character);
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

        public override void  BActionStart()
        {
            if (character.isTouchingBody && !scene.World.JointList.Contains(character.sliderJoint))
            {
                character.State = new GrabbingCharacterState(scene, character);
                character.State.BActionStart();
            }
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
