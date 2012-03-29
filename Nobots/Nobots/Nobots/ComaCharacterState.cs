using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots
{
    class ComaCharacterState : CharacterState
    {
        public ComaCharacterState(Scene scene, Character character) 
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("girl_moving");
            character.texture = texture;
            characterWidth = texture.Width / 8;
            characterHeight = texture.Height / 5;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Enter()
        {
            Energy energy = new Energy(scene.Game, scene);
            energy.Position = character.Position;
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
