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
        private bool transitionPlayed = false;
        private List<String> toEnergy = new List<String>();
        private List<String> toNormal = new List<String>();
        Random rand = new Random();

        




        public AmbienceSound(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;

            toEnergy.Add("Content\\sounds\\music\\realtoenergy1.wav");
            toEnergy.Add("Content\\sounds\\music\\realtoenergy2.wav");
            toEnergy.Add("Content\\sounds\\music\\realtoenergy3.wav");
            toEnergy.Add("Content\\sounds\\music\\realtoenergy4.wav");
            toEnergy.Add("Content\\sounds\\music\\realtoenergy5.wav");
            toEnergy.Add("Content\\sounds\\music\\realtoenergy6.wav");

            toNormal.Add("Content\\sounds\\music\\energytoreal1.wav");
            toNormal.Add("Content\\sounds\\music\\energytoreal2.wav");
            toNormal.Add("Content\\sounds\\music\\energytoreal3.wav");
            toNormal.Add("Content\\sounds\\music\\energytoreal4.wav");
            toNormal.Add("Content\\sounds\\music\\energytoreal5.wav");

            
            

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
                    ISound aux = scene.ISoundEngine.Play2D(toEnergy[rand.Next(6)], false, false);
                    aux.Volume = 0.7f;
                    transitionPlayed = true;
                }

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

                if (!transitionPlayed){
                    ISound aux = scene.ISoundEngine.Play2D(toNormal[rand.Next(5)], false, false);
                    aux.Volume = 0.7f;
                    transitionPlayed = true;
                }

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
