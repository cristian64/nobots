﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace Nobots.Elements
{
    class DyingCharacterState : CharacterState
    {
        static Random random = new Random();
        float totalSeconds = 0;
        int columns = 0;
        int rows = 0;
        int totalFrames = 0;
        int currentFrame = 0;
        bool dead = false;
        bool animating = true;

        public DyingCharacterState(Scene scene, Character character) 
            : base(scene, character)
        {
            texture = scene.Game.Content.Load<Texture2D>("dyingGas");
            columns = 5;
            rows = 4;
            totalFrames = 16;
            characterWidth = (int)(texture.Width / columns);
            characterHeight = (int)(texture.Height / rows);
            character.texture = texture;
            textureXmin = 0;
            textureYmin = 0;
        }

        public override void Update(GameTime gameTime)
        {
            totalSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (animating)
                changeIdleTextures(gameTime);
            if (totalSeconds > 3 && !dead)
            {
                dead = true;

                int characterCount = 0;
                foreach (Element i in scene.Elements)
                {
                    if (i is Character && !(i is Energy) && ((Character)i).State is ComaCharacterState)
                        characterCount++;
                }

                if ((characterCount == 0 && (scene.InputManager.Character is Energy || scene.InputManager.Character is Crane || scene.InputManager.Character == null)) || scene.InputManager.Character == character)
                {
#if !FINAL_RELEASE
                    MessageBox.Show("This is the Editor mode. After dying the level is no longer restarted not to lose pendent changes on the level.", "Warning!", MessageBoxButtons.OK);
#else
                    Vector2? checkpointPosition = null;
                    foreach (Element i in scene.Elements)
                    {
                        if (i is Checkpoint && ((Checkpoint)i).Active)
                        {
                            checkpointPosition = i.Position;
                            break;
                        }
                    }

                    if (checkpointPosition != null)
                        scene.CleanAndLoad(scene.SceneLoader.LastLevel, checkpointPosition, character.Active);
                    else
                        scene.CleanAndLoad(scene.SceneLoader.LastLevel);
#endif
                }
            }
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

                //because the animatino is not looped
                if (currentFrame >= totalFrames)
                    animating = false;
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
            scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.Death, character.Position.X, character.Position.Y, 0, false, false, false);

            int characterCount = 0;
            foreach (Element i in scene.Elements)
            {
                if (i is Character && !(i is Energy) && ((Character)i).State is ComaCharacterState)
                    characterCount++;
            }

            if (characterCount == 0 && (scene.InputManager.Character is Energy || scene.InputManager.Character is Crane))
            {
                if (scene.InputManager.Character is Crane)
                {
                    scene.InputManager.Character = null;
                }
                else if (scene.InputManager.Character is Energy)
                {
                    Energy energy = (Energy)scene.InputManager.Character;
                    for (int j = 0; j < 50; j++)
                    {
                        scene.PlasmaExplosionParticleSystem.AddParticle(energy.Position - Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                        scene.PlasmaExplosionParticleSystem.AddParticle(energy.Position + Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                    }
                    scene.GarbageElements.Add((Element)scene.InputManager.Character);
                    scene.InputManager.Character = null;
                }
            }
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
