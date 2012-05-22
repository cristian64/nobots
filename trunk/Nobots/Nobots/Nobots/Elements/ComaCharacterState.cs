using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    class ComaCharacterState : CharacterState
    {
        int rows;
        int columns;

        Vector2? energyPosition = null;
        public ComaCharacterState(Scene scene, Character character, Vector2? energyPosition = null) 
            : base(scene, character)
        { /*
            texture = scene.Game.Content.Load<Texture2D>("idle");
            character.texture = texture;
            characterWidth = texture.Width / 10;
            characterHeight = texture.Height / 2;
            textureXmin = 0;
            textureYmin = characterHeight;
           */
            texture = scene.Game.Content.Load<Texture2D>("coma");
            rows = 2;
            columns = 5;
            character.texture = texture;
            characterWidth = texture.Width / columns;
            characterHeight = texture.Height / rows;
            textureXmin = 0;
            textureYmin = characterHeight;
             

            this.energyPosition = energyPosition;
        }

        public override void Update(GameTime gameTime)
        {
            changeComaTextures(gameTime);
        }

        float seconds = 0;
        float delay = 0.15f;
        private Vector2 changeComaTextures(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds > delay)
            {
                seconds -= delay;
                textureXmin += texture.Width / columns;

                if (textureXmin == texture.Width / columns && textureYmin == texture.Height / rows)
                {
                    textureXmin = 0;
                    textureYmin = 0;
                }
                else if (textureXmin == texture.Width)
                {
                    textureXmin = 0;
                    textureYmin += texture.Height / rows;
                }

                if (textureXmin == 0 && textureYmin == 0)
                    delay = 2;
                else// if (textureXmin == texture.Width / columns)
                    delay = 0.15f;
               //   delay -= 0.04f;
               // if (delay < 0)
                 //   delay = 0;
            }

            return new Vector2(textureXmin, textureYmin);
        }

        public override void Exit(CharacterState nextState)
        {
        }

        public override void Enter()
        {
            character.body.FixedRotation = true;
            character.body.AngularVelocity = 0;

            Energy energy = new Energy(scene.Game, scene, energyPosition != null ? (Vector2)energyPosition : character.Position);
            energy.State = new FallingCharacterState(scene, energy);
            energy.Position = energyPosition != null ? (Vector2)energyPosition : character.Position;
            scene.InputManager.Character = energy;
            scene.Camera.Target = energy;
            scene.RespawnElements.Add(energy);
            Random random = new Random();
            for (int j = 0; j < 50; j++)
            {
                scene.PlasmaExplosionParticleSystem.AddParticle(energy.Position - Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                scene.PlasmaExplosionParticleSystem.AddParticle(energy.Position + Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
            }
        }
    }
}
