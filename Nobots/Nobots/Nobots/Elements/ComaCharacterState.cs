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

        bool createEnergy = true;
        Vector2? energyPosition = null;
        public ComaCharacterState(Scene scene, Character character, bool createEnergy = true, Vector2? energyPosition = null) 
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
            columns = 7;
            character.texture = texture;
            characterWidth = texture.Width / columns;
            characterHeight = texture.Height / rows;
            textureXmin = 0;
            textureYmin = characterHeight;
             

            this.energyPosition = energyPosition;
            this.createEnergy = createEnergy;
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

                if (textureXmin == texture.Width && textureYmin == texture.Height / rows)
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
                    delay = 6;
                else if (textureXmin == texture.Width / columns && textureYmin == 0)
                    delay = 0.18f;
                else if (textureXmin == 0 && textureYmin == texture.Height / rows)
                    delay = 0.1f;
               // else if (textureXmin == (texture.Width / columns) * (columns-1) && textureYmin == 0)
                //    delay = 1f;

                if(textureXmin == 0)
                    delay += 0.06f;
                else
                    delay -= 0.035f;
                if (delay < 0)
                    delay = 0;
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

            if (createEnergy)
            {
                Energy energy = new Energy(scene.Game, scene, energyPosition != null ? (Vector2)energyPosition : character.Position);
                energy.State = new FallingCharacterState(scene, energy);
                energy.Position = energyPosition != null ? (Vector2)energyPosition : character.Position;
                scene.InputManager.Target = energy;
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
}
