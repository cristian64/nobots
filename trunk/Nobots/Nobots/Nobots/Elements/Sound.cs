using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using IrrKlang;

namespace Nobots.Elements
{
    public class Sound : Element
    {
        private float volume = 1;

        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                if (sound != null)
                    sound.Volume = volume;
            }
        }

        public override float Width
        {
            get { return 1f; }
            set { }
        }

        public override float Height
        {
            get { return 1f; }
            set { }
        }

        Vector2 position;
        public override Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                createSound();
            }
        }

        public override float Rotation
        {
            get
            {
                return MathHelper.PiOver4;
            }
            set
            {
            }
        }

        private String soundName = "";
        public String SoundName
        {
            get { return soundName; }
            set
            {
                soundName = value;
                createSound();
            }
        }

        ISound sound = null;

        public Sound(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            this.position = position;
        }

        private void createSound()
        {
            if (sound != null)
            {
                sound.Stop();
                sound.Dispose();
            }
            sound = scene.ISoundEngine.Play3D(@"Content\sounds\" + soundName, position.X, position.Y, 0);
            if (sound != null)
            {
                sound.Volume = volume;
                sound.Looped = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (sound != null)
            {
                sound.Stop();
                sound.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
