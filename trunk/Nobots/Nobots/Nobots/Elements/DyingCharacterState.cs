using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    class DyingCharacterState : CharacterState
    {
        float totalSeconds = 0;
        int columns = 0;
        int rows = 0;
        int framesInLastRow = 0;
        bool dead = false;
        bool animating = true;

        public DyingCharacterState(Scene scene, Character character) 
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("dying");
            columns = 7;
            rows = 2;
            framesInLastRow = 5;
            characterWidth = texture.Width / columns;
            characterHeight = texture.Height / rows;
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            totalSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (animating)
                changeIdleTextures(gameTime);
            if (totalSeconds > 5 && !dead)
            {
                //TODO: this should be a method in Scene
                //save the last checkpoint position in a Vector2
                // reload everything in the scene
                // move the character to that checkpoint position

                foreach (Element i in scene.Elements)
                {
                    if (i is Checkpoint && ((Checkpoint)i).Active)
                    {
                        
                        scene.GarbageElements.Add(character);
                        Character character2 = new Character(scene.Game, scene, i.Position);
                        scene.RespawnElements.Add(character2);
                        scene.Camera.Target = scene.InputManager.Character = character2;
                        break;
                    }
                }

                dead = true;
            }
        }
        float seconds = 0;
        private Vector2 changeIdleTextures(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
       
            if (seconds > 0.04f)
            {
                seconds -= 0.04f;
                textureXmin += texture.Width / columns;

                if (textureXmin == (texture.Width / columns) * framesInLastRow && textureYmin == texture.Height / rows)
                {
                    animating = false;
                }
                else if (textureXmin == texture.Width)
                {
                    textureXmin = 0;
                    textureYmin += texture.Height / rows;
                }
            }

            return new Vector2(textureXmin, textureYmin);
        }

        public override void Enter()
        {
            character.torso.IsSensor = true;
            character.body.FixedRotation = true;
            character.body.AngularVelocity = 0;
            character.torso.LinearVelocity = Vector2.UnitY * character.torso.LinearVelocity;
            character.body.LinearVelocity = Vector2.UnitY * character.body.LinearVelocity;
        }

        public override void YActionStart()
        {
        }

        public override void UpAction()
        {
        }

        public override void DownAction()
        {
        }

        public override void AActionStart()
        {
        }

        public override void  BActionStart()
        {
        }

        public override void RightAction()
        {
        }

        public override void LeftAction()
        {
        }

        public override void RightActionStart()
        {
        }

        public override void LeftActionStart()
        {
        }
    }
}
