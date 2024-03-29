﻿using System;
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
        public ISoundSource Lever, Computer, AmbienceNormal, AmbienceEnergy, woodenBox, laserBarrierLoop, checkpoint, steam, elevatorBegin, elevatorEnd, stomp, Credits
            , machineloop1, Container, SwitchOn, SwitchOff, Death, Weight, ImpulsePlatform, CircularSaw, CircularSawStop, Crane, CraneStart, CraneClose, Sparks, Door, EnergyIn, EnergyOut, Torch, Grunt, StepFall, ElecBoxOn, ElecBoxOff;
        public List<ISoundSource> toEnergy = new List<ISoundSource>();
        public List<ISoundSource> toNormal = new List<ISoundSource>();
        public List<ISoundSource> socket = new List<ISoundSource>();
        public List<ISoundSource> laserBarrierShocks = new List<ISoundSource>();
        public List<ISoundSource> powerDown = new List<ISoundSource>();
        public List<ISoundSource> powerUp = new List<ISoundSource>();
        public List<ISoundSource> steps = new List<ISoundSource>();
        public List<ISoundSource> drops = new List<ISoundSource>();
        

        public SoundManager(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            ISoundEngine = new ISoundEngine();
            ISoundEngine.SoundVolume = 0;

            //OBJECTS

            Lever = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\lever.wav");
            Lever.DefaultVolume = 0.7f;

            Computer = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\computerbleep.wav");
            Computer.DefaultVolume = 0.25f;
            Computer.DefaultMinDistance = 0.02f;
            

            woodenBox = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\woodencratefall.wav");
            woodenBox.DefaultVolume = 0.4f;

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
            checkpoint.DefaultVolume = 0.2f;

            steam = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\steam.wav");
            steam.DefaultVolume = 0.3f;
            steam.DefaultMinDistance = 0.2f;

            elevatorBegin = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\elevatorbegin.wav");

            elevatorEnd = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\elevatorend.wav");

            stomp = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\stomp.wav");
            stomp.DefaultMinDistance = 0.5f;

            powerDown.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerdown1.wav"));
            powerDown.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerdown2.wav"));
            powerDown.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerdown3.wav"));
            powerDown.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerdown4.wav"));

            powerUp.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerup1.wav"));
            powerUp.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerup2.wav"));
            powerUp.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerup3.wav"));
            powerUp.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\powerup4.wav"));

            ElecBoxOn = ISoundEngine.AddSoundSourceAlias(powerUp[3], "ElecBoxOn");
            ElecBoxOn.DefaultVolume = 0.2f;

            ElecBoxOff = ISoundEngine.AddSoundSourceAlias(powerDown[3], "ElecBoxOff");
            ElecBoxOff.DefaultVolume = 0.2f;

            machineloop1 = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\machine1loop.wav");
            machineloop1.DefaultMinDistance = 0.1f;

            Container = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\metalimpact1.wav");
            Container.DefaultMinDistance = 5f;

            SwitchOn = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\switch1.wav");
            SwitchOn.DefaultVolume = 0.7f;

            SwitchOff = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\switch2.wav");
            SwitchOff.DefaultVolume = 0.7f;

            Weight = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\weight.wav");
            Weight.DefaultVolume = 0.5f;

            ImpulsePlatform = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\impulseplatform.wav");
            ImpulsePlatform.DefaultMinDistance = 0.5f;

            CircularSaw = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\circularsaw.ogg");
            CircularSaw.DefaultMinDistance = 0.2f;
            CircularSawStop = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\circularsaw_stop.ogg");
            CircularSawStop.DefaultMinDistance = 0.2f;

            Crane = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\crane.wav");
            Crane.DefaultVolume = 0.3f;
            CraneStart = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\crane_start.wav");
            CraneStart.DefaultVolume = 0.3f;
            CraneClose = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\crane_close.wav");
            CraneClose.DefaultVolume = 0.3f;

            Sparks = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\sparks.ogg");
            Sparks.DefaultMinDistance = 0.3f;
            Sparks.DefaultVolume = 0.5f;

            Door = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\door.ogg");

            EnergyIn = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\energy_in.ogg");
            EnergyIn.DefaultVolume = 0.6f;

            EnergyOut = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\energy_out.ogg");
            EnergyOut.DefaultVolume = 0.6f;

            Torch = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\torch.ogg");
            Torch.DefaultMinDistance = 0.1f;

            Grunt = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\grunt.ogg");
            StepFall = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step_fall.wav");

            Credits = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\synergycredits.ogg");
            Credits.DefaultMinDistance = 0.5f;

            drops.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\drop-01.wav"));
            drops.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\drop-02.wav"));
            drops.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\drop-03.wav"));
            drops.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\drop-04.wav"));

            foreach (ISoundSource i in drops)
            {
                i.DefaultVolume = 0.3f;
                i.DefaultMinDistance = 0.3f;
            }

            //Character
            Death = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\death.wav");
            Death.DefaultVolume = 0.3f;

            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-01.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-02.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-03.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-04.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-05.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-06.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-07.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-08.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-09.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-10.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-11.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-12.wav"));
            steps.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\step-13.wav"));

            foreach (ISoundSource i in steps)
            {
                i.DefaultVolume = 0.1f;
            }
        }

        float fadeInDuration = 2;
        float fadeInDelay = 2;

        public override void Update(GameTime gameTime)
        {
            fadeInDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fadeInDelay <= 0 && ISoundEngine.SoundVolume < 1)
                ISoundEngine.SoundVolume = Math.Min(1, ISoundEngine.SoundVolume + (float)gameTime.ElapsedGameTime.TotalSeconds / fadeInDuration);
        }

        protected override void Dispose(bool disposing)
        {
            ISoundEngine.StopAllSounds();
            ISoundEngine.Dispose();
            base.Dispose(disposing);
        }
    }
}
