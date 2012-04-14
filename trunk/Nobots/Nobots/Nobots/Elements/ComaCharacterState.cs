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

        public override void Exit()
        {
            character.torso.Position = character.body.Position - new Vector2(0, character.Height / 2 - character.body.FixtureList[0].Shape.Radius / 2);
            character.torso.Rotation = 0;
            character.torso.AngularVelocity = 0;
            character.torso.FixedRotation = true;
        }

        public override void Enter()
        {
            Energy energy = new Energy(scene.Game, scene);
            character.torso.FixedRotation = false;
            if(character.Effect == SpriteEffects.None)
                character.torso.ApplyForce(new Vector2(-10, 0));
            else
                character.torso.ApplyForce(new Vector2(10, 0));
            character.torso.Friction = 100f;
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
