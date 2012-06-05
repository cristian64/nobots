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
        int columns = 0;
        int rows = 0;
        int totalFrames = 0;
        int currentFrame = 0;
        public IdleCharacterState(Scene scene, Character character) 
            : base(scene, character)
        {
            columns = 10;
            rows = 2;
            totalFrames = 15;
            texture = scene.Game.Content.Load<Texture2D>(character is Energy ? "idleEnergy" : "idle");
            characterWidth = texture.Width / columns;
            characterHeight = texture.Height / rows;
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
       
            if (seconds > 0.03f)
            {
                seconds -= 0.03f;
                currentFrame = currentFrame % totalFrames;
                textureXmin = (currentFrame % columns) * characterWidth;
                textureYmin = (currentFrame / columns) * characterHeight;
                currentFrame++;

                if (currentFrame == totalFrames)
                    seconds -= 3;
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
            if (character.Active)
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
            if (character.touchedBody != null)
            {
                character.State = new GrabbingCharacterState(scene, character, character.touchedBody);
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
