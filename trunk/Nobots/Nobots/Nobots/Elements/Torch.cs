using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Nobots.ParticleSystem;
using FarseerPhysics.Dynamics.Contacts;
using IrrKlang;

namespace Nobots.Elements
{
    public class Torch : Element, IActivable
    {
        Texture2D texture;
        ISound sound;

        private bool isActive = true;
        public bool Active
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
                sound.Paused = !value;
            }
        }
        
        public override float Height
        {
            get
            {
                return Conversion.ToWorld(texture.Height);
            }
            set
            {
            }
        }

        public override float Width
        {
            get
            {
                return Conversion.ToWorld(texture.Width);
            }
            set
            {
            }
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
                sound.Position = new Vector3D(position.X, position.Y, 0);
            }
        }

        public override float Rotation
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public Torch(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -7f;
            texture = scene.Game.Content.Load<Texture2D>("torch");
            this.position = position;

            sound = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.Torch, position.X, position.Y, 0, true, false, false);
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                scene.FireParticleSystem.AddParticle(Position - new Vector2(0, Height / 2), Vector2.Zero);
                scene.FireParticleSystem.AddParticle(Position - new Vector2(0, Height / 2), Vector2.Zero);
                scene.FireParticleSystem.AddParticle(Position - new Vector2(0, Height / 2), Vector2.Zero);
                scene.FireParticleSystem.AddParticle(Position - new Vector2(0, Height / 2), Vector2.Zero);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0); 
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
