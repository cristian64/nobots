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
        public ISoundSource Lever;
        public ISoundSource Computer;

        public SoundManager(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            ISoundEngine = new ISoundEngine();

            Lever = ISoundEngine.AddSoundSourceFromFile("Content\\sounds\\effects\\lever.wav");
            Lever.DefaultVolume = 0.15f;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
