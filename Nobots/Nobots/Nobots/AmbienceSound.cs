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
        private IControllable previous;
        private float fadeSpeed = 0.1f;        
        private bool inTransitionToEnergy = false;
        private bool inTransitionToNormal = false;
        private bool transitionPlayed = false;        
        static Random rand = new Random();

        ISoundEngine iSoundEngine;
        ISoundSource AmbienceNormal, AmbienceEnergy;
        List<ISoundSource> toEnergy = new List<ISoundSource>();
        List<ISoundSource> toNormal = new List<ISoundSource>();

        public AmbienceSound(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;

            iSoundEngine = new ISoundEngine();

            // AMBIENCE SOUNDS AND TRANSITIONS

            AmbienceNormal = iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\ambiencelabnormal.ogg");
            AmbienceNormal.DefaultVolume = 0.3f;

            AmbienceEnergy = iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\ambiencelabenergy.ogg");
            AmbienceEnergy.DefaultVolume = 0.3f;

            toEnergy.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy1.wav"));
            toEnergy.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy2.wav"));
            toEnergy.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy3.wav"));
            toEnergy.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy4.wav"));
            toEnergy.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy5.wav"));
            toEnergy.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy6.wav"));

            foreach (ISoundSource i in toEnergy)
            {
                i.DefaultVolume = 0.1f;
            }

            toNormal.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal1.wav"));
            toNormal.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal2.wav"));
            toNormal.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal3.wav"));
            toNormal.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal4.wav"));
            toNormal.Add(iSoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal5.wav"));

            foreach (ISoundSource i in toNormal)
            {
                i.DefaultVolume = 0.1f;
            }

            ambienceLabNormal = iSoundEngine.Play2D(AmbienceNormal, true, false, false);
            ambienceLabEnergy = iSoundEngine.Play2D(AmbienceEnergy, true, true, false);            
            ambienceLabEnergy.Volume = 0;
        }

        float fadeOutDuration = 2;
        bool isFadingOut = false;
        public void FadeOut(float fadeOutDuration = 2)
        {
            this.fadeOutDuration = fadeOutDuration;
            isFadingOut = true;
            inTransitionToEnergy = inTransitionToNormal = false;
        }

        float fadeInDuration = 2;
        bool isFadingIn = false;
        public void FadeIn(float fadeInDuration = 2)
        {
            this.fadeInDuration = fadeInDuration;
            isFadingIn = true;
            inTransitionToEnergy = inTransitionToNormal = false;
        }

        public override void Update(GameTime gameTime)
        {
            // Check when the player change the state.
            if (scene.InputManager.Character is Energy && !(previous is Energy))
            {
                inTransitionToEnergy = true;
                transitionPlayed = false;
                inTransitionToNormal = false;
                Console.WriteLine("Normal -> Energy");
            }
            else if (!(scene.InputManager.Character is Energy) && previous is Energy)
            {
                inTransitionToEnergy = false;
                transitionPlayed = false;
                inTransitionToNormal = true;
                Console.WriteLine("Energy -> Normal");
            }
            previous = scene.InputManager.Character;

            // Process sound effect according to the transitions.
            if (inTransitionToEnergy)
            {
                if (!transitionPlayed)
                {
                    ISound aux = iSoundEngine.Play2D(toEnergy[rand.Next(toEnergy.Count)], false, false,false);
                    transitionPlayed = true;
                }

                ambienceLabEnergy.Paused = false;
                ambienceLabEnergy.Volume = Math.Min(AmbienceEnergy.DefaultVolume, ambienceLabEnergy.Volume + fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                ambienceLabNormal.Volume = Math.Max(0, ambienceLabNormal.Volume - fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (ambienceLabNormal.Volume == 0)
                {
                    ambienceLabNormal.Paused = true;
                    inTransitionToEnergy = false;
                }
            }

            if (inTransitionToNormal)
            {
                if (!transitionPlayed)
                {
                    iSoundEngine.Play2D(toNormal[rand.Next(toNormal.Count)], false, false,false);
                    transitionPlayed = true;
                }

                ambienceLabNormal.Paused = false;
                ambienceLabEnergy.Volume = Math.Max(0, ambienceLabEnergy.Volume - fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                ambienceLabNormal.Volume = Math.Min(AmbienceNormal.DefaultVolume, ambienceLabNormal.Volume + fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (ambienceLabEnergy.Volume == 0)
                {
                    ambienceLabEnergy.Paused = true;
                    inTransitionToNormal = false;
                }
            }

            if (isFadingOut)
            {
                ambienceLabEnergy.Volume = Math.Max(0, ambienceLabEnergy.Volume - (float)gameTime.ElapsedGameTime.TotalSeconds) / fadeOutDuration;
                ambienceLabNormal.Volume = Math.Max(0, ambienceLabNormal.Volume - (float)gameTime.ElapsedGameTime.TotalSeconds) / fadeOutDuration;

                if (ambienceLabEnergy.Volume == 0 && ambienceLabNormal.Volume == 0)
                    isFadingOut = false;
            }

            if (isFadingIn)
            {
                ambienceLabEnergy.Volume = Math.Min(1, ambienceLabEnergy.Volume + (float)gameTime.ElapsedGameTime.TotalSeconds) / fadeOutDuration;
                ambienceLabNormal.Volume = Math.Min(1, ambienceLabNormal.Volume + (float)gameTime.ElapsedGameTime.TotalSeconds) / fadeOutDuration;

                if (ambienceLabEnergy.Volume == 1 && ambienceLabNormal.Volume == 1)
                    isFadingIn = false;
            }
        }
    }
}
