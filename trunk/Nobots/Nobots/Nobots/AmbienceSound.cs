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
        public ISoundSource Nav, Select;
        private IControllable previous;
        private float fadeSpeed = 0.1f;        
        private bool inTransitionToEnergy = false;
        private bool inTransitionToNormal = false;
        private bool transitionPlayed = false;        
        static Random rand = new Random();

        public ISoundEngine ISoundEngine;
        ISoundSource AmbienceNormal, AmbienceEnergy;
        List<ISoundSource> toEnergy = new List<ISoundSource>();
        List<ISoundSource> toNormal = new List<ISoundSource>();

        public AmbienceSound(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;

            ISoundEngine = new ISoundEngine();

            // AMBIENCE SOUNDS AND TRANSITIONS

            AmbienceNormal = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\ambiencelabnormal.ogg");
            AmbienceNormal.DefaultVolume = 0.3f;

            AmbienceEnergy = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\ambiencelabenergy.ogg");
            AmbienceEnergy.DefaultVolume = 0.05f;

            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy1.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy2.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy3.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy4.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy5.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy6.wav"));

            foreach (ISoundSource i in toEnergy)
            {
                i.DefaultVolume = 0.1f;
            }

            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal1.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal2.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal3.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal4.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal5.wav"));

            foreach (ISoundSource i in toNormal)
            {
                i.DefaultVolume = 0.1f;
            }

            ambienceLabNormal = ISoundEngine.Play2D(AmbienceNormal, true, false, false);
            ambienceLabEnergy = ISoundEngine.Play2D(AmbienceEnergy, true, true, false);            
            ambienceLabEnergy.Volume = 0;

            //INTERFACE

            Nav = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\choose.wav");
            Nav.DefaultVolume = 0.1f;

            Select = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\select.wav");
            Select.DefaultVolume = 0.1f;
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
                    ISound aux = ISoundEngine.Play2D(toEnergy[rand.Next(toEnergy.Count)], false, false,false);
                    transitionPlayed = true;
                }

                Console.WriteLine("energvol" + ambienceLabEnergy.Volume + "normalvol" + ambienceLabNormal.Volume + "enerdef" + AmbienceEnergy.DefaultVolume + "normaldef" + AmbienceNormal.DefaultVolume);
                ambienceLabEnergy.Paused = false;
                ambienceLabEnergy.Volume = Math.Min(AmbienceEnergy.DefaultVolume, ambienceLabEnergy.Volume + fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                ambienceLabNormal.Volume = Math.Max(0, ambienceLabNormal.Volume - fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (ambienceLabNormal.Volume == 0 && ambienceLabEnergy.Volume == AmbienceEnergy.DefaultVolume)
                {
                    ambienceLabNormal.Paused = true;
                    inTransitionToEnergy = false;
                    Console.WriteLine("energvol" + ambienceLabEnergy.Volume + "normalvol" + ambienceLabNormal.Volume + "enerdef" + AmbienceEnergy.DefaultVolume + "normaldef" + AmbienceNormal.DefaultVolume);
                }
            }

            if (inTransitionToNormal)
            {
                if (!transitionPlayed)
                {
                    ISoundEngine.Play2D(toNormal[rand.Next(toNormal.Count)], false, false,false);
                    transitionPlayed = true;
                }
                Console.WriteLine("energvol" + ambienceLabEnergy.Volume + "normalvol" + ambienceLabNormal.Volume + "enerdef" + AmbienceEnergy.DefaultVolume + "normaldef" + AmbienceNormal.DefaultVolume);
                ambienceLabNormal.Paused = false;
                ambienceLabNormal.Volume = Math.Min(AmbienceNormal.DefaultVolume, ambienceLabNormal.Volume + fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                ambienceLabEnergy.Volume = Math.Max(0, ambienceLabEnergy.Volume - fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                

                if (ambienceLabEnergy.Volume == 0 && ambienceLabNormal.Volume == AmbienceNormal.DefaultVolume)
                {
                    ambienceLabEnergy.Paused = true;
                    inTransitionToNormal = false;
                    Console.WriteLine("energvol" + ambienceLabEnergy.Volume + "normalvol" + ambienceLabNormal.Volume + "enerdef" + AmbienceEnergy.DefaultVolume + "normaldef" + AmbienceNormal.DefaultVolume);
                }
            }

            if (isFadingOut)
            {
                ambienceLabEnergy.Volume = Math.Max(0, ambienceLabEnergy.Volume - (float)gameTime.ElapsedGameTime.TotalSeconds / fadeOutDuration);
                ambienceLabNormal.Volume = Math.Max(0, ambienceLabNormal.Volume - (float)gameTime.ElapsedGameTime.TotalSeconds / fadeOutDuration);

                if (ambienceLabEnergy.Volume == 0 && ambienceLabNormal.Volume == 0)
                    isFadingOut = false;
            }

            if (isFadingIn)
            {
                ambienceLabEnergy.Volume = Math.Max(0, ambienceLabEnergy.Volume - (float)gameTime.ElapsedGameTime.TotalSeconds / fadeInDuration);
                ambienceLabNormal.Volume = Math.Min(AmbienceNormal.DefaultVolume, ambienceLabNormal.Volume + (float)gameTime.ElapsedGameTime.TotalSeconds / fadeInDuration);

                if (ambienceLabEnergy.Volume == 0 && ambienceLabNormal.Volume == AmbienceNormal.DefaultVolume)
                    isFadingIn = false;
            }
        }
    }
}
