﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Input;

namespace Nobots.Elements
{
    class ClimbingCharacterState : CharacterState
    {
        public ClimbingCharacterState(Scene scene, Character character)
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("climbing");
            characterWidth = texture.Width / 7;
            characterHeight = texture.Height;
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

            if (seconds > 0.08f  && (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.Down)))
            {
                seconds -= 0.08f;
                textureXmin += texture.Width / 7;

                if (textureXmin == texture.Width)
                    textureXmin = 0;
            }

            return new Vector2(textureXmin, textureYmin);
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
            character.torso.LinearVelocity = new Vector2(0, -3);
            character.body.LinearVelocity = new Vector2(0, -3);
        }

        public override void DownAction()
        {
            character.torso.LinearVelocity = new Vector2(0, 3);
            character.body.LinearVelocity = new Vector2(0, 3);
        }

        public override void UpActionStop()
        {
            character.torso.LinearVelocity = new Vector2(0, 0);
            character.body.LinearVelocity = new Vector2(0, 0);
        }

        public override void DownActionStop()
        {
            character.torso.LinearVelocity = new Vector2(0, 0);
            character.body.LinearVelocity = new Vector2(0, 0);
        }

        public override void AActionStart()
        {
            character.State = new JumpingCharacterState(scene, character);
        }
    }
}
