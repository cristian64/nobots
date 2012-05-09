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
        Vector2? energyPosition = null;
        public ComaCharacterState(Scene scene, Character character, Vector2? energyPosition = null) 
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("idle");
            character.texture = texture;
            characterWidth = texture.Width / 10;
            characterHeight = texture.Height / 2;
            textureXmin = 0;
            textureYmin = characterHeight;

            this.energyPosition = energyPosition;
        }

        public override void Exit()
        {
        }

        public override void Enter()
        {
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
