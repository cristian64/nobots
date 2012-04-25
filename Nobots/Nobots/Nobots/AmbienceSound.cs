using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IrrKlang;
using Nobots.Elements;

namespace Nobots
{
    public class AmbienceSound : DrawableGameComponent
    {
        private Scene scene;
        private ISound ambienceLabNormal;
        private ISound ambienceLabEnergy;
        private Element previous;
        private float fadeSpeed = 1;
        private bool inTransitionToEnergy = false;
        private bool inTransitionToNormal = false;


        public AmbienceSound(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;

            ambienceLabNormal = scene.ISoundEngine.Play2D("Content\\sounds\\music\\ambiencelabnormal.mp3");
            ambienceLabNormal.Looped = true;

            ambienceLabEnergy = scene.ISoundEngine.Play2D("Content\\sounds\\music\\ambiencelabenergy.mp3");
            ambienceLabEnergy.Looped = true;
            ambienceLabEnergy.Paused = true;
            ambienceLabEnergy.Volume = 0;
        }

        public override void Update(GameTime gameTime)
        {
            // Check when the player change the state.
            if (scene.InputManager.Character is Energy && !(previous is Energy))
            {
                inTransitionToEnergy = true;
                inTransitionToNormal = false;
                Console.WriteLine("Normal -> Energy");
            }
            else if (!(scene.InputManager.Character is Energy) && previous is Energy)
            {
                inTransitionToEnergy = false;
                inTransitionToNormal = true;
                Console.WriteLine("Energy -> Normal");
            }
            previous = scene.InputManager.Character;

            // Process sound effect according to the transitions.
            if (inTransitionToEnergy)
            {
                ambienceLabEnergy.Paused = false;
                ambienceLabEnergy.Volume = Math.Min(1, ambienceLabEnergy.Volume + fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                ambienceLabNormal.Volume = Math.Max(0, ambienceLabNormal.Volume - fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (ambienceLabNormal.Volume == 0)
                {
                    ambienceLabNormal.Paused = true;
                    inTransitionToEnergy = false;
                }
            }

            if (inTransitionToNormal)
            {
                ambienceLabNormal.Paused = false;
                ambienceLabEnergy.Volume = Math.Max(0, ambienceLabEnergy.Volume - fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                ambienceLabNormal.Volume = Math.Min(1, ambienceLabNormal.Volume + fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (ambienceLabEnergy.Volume == 0)
                {
                    ambienceLabEnergy.Paused = true;
                    inTransitionToNormal = false;
                }
            }
        }
    }
}
