using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nobots.Elements;
using IrrKlang;

namespace Nobots
{
    public class SoundManager : DrawableGameComponent
    {
        private Scene scene;

        public ISoundEngine ISoundEngine;
        public ISoundSource Lever, Computer, AmbienceNormal, AmbienceEnergy, woodenBox, laserBarrierLoop,checkpoint,steam,elevatorBegin,elevatorEnd, stomp
            ,machineloop1;
        public List<ISoundSource> toEnergy = new List<ISoundSource>();
        public List<ISoundSource> toNormal = new List<ISoundSource>();
        public List<ISoundSource> socket = new List<ISoundSource>();
        public List<ISoundSource> laserBarrierShocks = new List<ISoundSource>();
        public List<ISoundSource> powerDown = new List<ISoundSource>();
        public List<ISoundSource> powerUp = new List<ISoundSource>();
        

        public SoundManager(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            ISoundEngine = new ISoundEngine();
            

            // AMBIENCE SOUNDS AND TRANSITIONS

            AmbienceNormal = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\ambiencelabnormal.mp3");
            AmbienceNormal.DefaultVolume = 0.5f;

            AmbienceEnergy = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\ambiencelabenergy.mp3");
            AmbienceEnergy.DefaultVolume = 0.5f;
                      

            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy1.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy2.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy3.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy4.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy5.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy6.wav"));

            foreach (ISoundSource i in toEnergy)
            {
                i.DefaultVolume = 0.3f;
            }

            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal1.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal2.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal3.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal4.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal5.wav"));

            foreach (ISoundSource i in toNormal)
            {
                i.DefaultVolume = 0.3f;
            }


            Lever = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\lever.wav");
            Lever.DefaultVolume = 0.7f;

            Computer = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\computerbleep.wav");
            Computer.DefaultVolume = 0.25f;
            Computer.DefaultMinDistance = 0.02f;
            

            woodenBox = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\woodencratefall.wav");

            socket.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\travelcord1.wav"));
            socket.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\travelcord2.wav"));
            socket.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\travelcord3.wav"));

            foreach (ISoundSource i in socket) {
                i.DefaultVolume = 0.2f;
            }

            laserBarrierLoop = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\electricbarrierloop.wav");
            //laserBarrierLoop.DefaultVolume = 0.25f;
            laserBarrierLoop.DefaultMinDistance = 0.1f;
           

            laserBarrierShocks.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\electricbarriershock1.wav"));
            laserBarrierShocks.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\electricbarriershock2.wav")); 
            laserBarrierShocks.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\electricbarriershock3.wav"));
            laserBarrierShocks.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\electricbarriershock4.wav"));

            foreach (ISoundSource i in laserBarrierShocks)
            {
                i.DefaultVolume = 0.7f;
            }

            checkpoint = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\checkpoint.wav");
            checkpoint.DefaultVolume = 0.7f;

            steam = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\steam.wav");
            steam.DefaultMinDistance = 0.05f;

            elevatorBegin = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\elevatorbegin.wav");

            elevatorEnd = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\elevatorend.wav");

            stomp = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\stomp.wav");

            powerDown.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerdown1.wav"));
            powerDown.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerdown2.wav"));
            powerDown.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerdown3.wav"));
            powerDown.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerdown4.wav"));

            powerUp.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerup1.wav"));
            powerUp.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerup2.wav"));
            powerUp.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerup3.wav"));
            powerUp.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerup4.wav"));

            machineloop1 = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\machine1loop.wav");
            machineloop1.DefaultMinDistance = 0.1f;



        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
