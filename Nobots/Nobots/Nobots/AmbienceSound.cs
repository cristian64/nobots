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
        private float fadeSpeed = 1;        
        private bool inTransitionToEnergy = false;
        private bool inTransitionToNormal = false;
        private bool transitionPlayed = false;        
        Random rand = new Random();

        




        public AmbienceSound(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;


            ambienceLabNormal = scene.SoundManager.ISoundEngine.Play2D(scene.SoundManager.AmbienceNormal, true, false, false);

           


            ambienceLabEnergy = scene.SoundManager.ISoundEngine.Play2D(scene.SoundManager.AmbienceEnergy, true, true, false);            
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
                    ISound aux = scene.SoundManager.ISoundEngine.Play2D(scene.SoundManager.toEnergy[rand.Next(scene.SoundManager.toEnergy.Count)], false, false,false);
                  
                    transitionPlayed = true;
                }

                ambienceLabEnergy.Paused = false;
                ambienceLabEnergy.Volume = Math.Min(scene.SoundManager.AmbienceEnergy.DefaultVolume, ambienceLabEnergy.Volume + fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
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
                    scene.SoundManager.ISoundEngine.Play2D(scene.SoundManager.toNormal[rand.Next(scene.SoundManager.toNormal.Count)], false, false,false);
                 
                    transitionPlayed = true;
                }

                ambienceLabNormal.Paused = false;
                ambienceLabEnergy.Volume = Math.Max(0, ambienceLabEnergy.Volume - fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                ambienceLabNormal.Volume = Math.Min(scene.SoundManager.AmbienceNormal.DefaultVolume, ambienceLabNormal.Volume + fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (ambienceLabEnergy.Volume == 0)
                {
                                 

                    ambienceLabEnergy.Paused = true;
                    inTransitionToNormal = false;
                }
            }
        }
    }
}
