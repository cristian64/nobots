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
        public ISoundSource Lever, Computer, AmbienceNormal, AmbienceEnergy, woodenBox;
        public List<ISoundSource> toEnergy = new List<ISoundSource>();
        public List<ISoundSource> toNormal = new List<ISoundSource>();
        

        public SoundManager(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            ISoundEngine = new ISoundEngine();

            // AMBIENCE SOUNDS AND TRANSITIONS

            AmbienceNormal = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\ambiencelabnormal.mp3");
            //AmbienceNormal.DefaultVolume = 0.2f;

            AmbienceEnergy = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\ambiencelabenergy.mp3");
                      

            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy1.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy2.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy3.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy4.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy5.wav"));
            toEnergy.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\realtoenergy6.wav"));

            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal1.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal2.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal3.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal4.wav"));
            toNormal.Add(ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\music\\energytoreal5.wav"));


            Lever = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\lever.wav");
            //Lever.DefaultVolume = 0.15f;

            Computer = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\computerbleep.wav");
            //Computer.DefaultVolume = 0.2f;
            woodenBox = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\woodencratefall.wav");


        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
