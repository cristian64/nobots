using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IrrKlang;

namespace Nobots
{
    public class AmbienceSound : DrawableGameComponent
    {
        private Scene scene;
        private ISound ambienceLabNormal;
        private ISound ambienceLabEnergy;

        public AmbienceSound(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;

            ambienceLabNormal = scene.ISoundEngine.Play2D("Content\\sounds\\music\\ambiencelabnormal.mp3");
            ambienceLabEnergy = scene.ISoundEngine.Play2D("Content\\sounds\\music\\ambiencelabenergy.mp3");
            ambienceLabEnergy.Paused = true;
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
