﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;
using IrrKlang;

namespace Nobots.Elements
{
    public class Socket : Element
    {
        private Socket otherSocket;
        public Socket OtherSocket
        {
            get
            {
                if (otherSocket == null)
                    foreach (Element e in scene.Elements)
                    {
                        if (e as Socket != null && e.Id == otherSocketId)
                        {
                            
                            otherSocket = (Socket)e;                            
                            break;
                        }
                    }

                return otherSocket;
            }
        }

        private String otherSocketId;
        public String OtherSocketId
        {
            get { return otherSocketId;     
            }
            set { otherSocketId = value; otherSocket = null; }
        }

        Body body;
        Texture2D texture;
        Texture2D texture2;
        Random rand = new Random();

        public override Vector2 Position
        {
            get
            {
                return body.Position;
            }
            set
            {
                body.Position = value;
            }
        }

        public override float Rotation
        {
            get { return body.Rotation; }
            set 
            {
                body.Rotation = value;
            }
        }

        public override float Width
        {
            get { return Conversion.ToWorld(texture.Width); }
            set { }
        }

        public override float Height
        {
            get { return Conversion.ToWorld(texture.Height); }
            set { }
        }

        public Socket(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("wires_end");
            texture2 = Game.Content.Load<Texture2D>("wires_end2");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 20f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollidesWith = Category.None | ElementCategory.ENERGY;

            body.UserData = this;
        }

        public override void Draw(GameTime gameTime)
        {
            if (scene.InputManager.Target is Energy)
                scene.SpriteBatch.Draw(texture2, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture2.Width / 2.0f, texture2.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
            else
                scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        public void Travel(Energy energy)
        {
            if (OtherSocket != null)
            {
                scene.VortexParticleSystem.AddParticle(Position, Vector2.Zero);
                scene.VortexParticleSystem.AddParticle(Position, Vector2.Zero);
                scene.VortexParticleSystem.AddParticle(Position, Vector2.Zero);
                scene.VortexParticleSystem.AddParticle(Position, Vector2.Zero);

                Energy newEnergy = new Energy(Game, scene, OtherSocket.Position);
                scene.RespawnElements.Add(newEnergy);
                scene.Camera.Target = newEnergy;
                scene.InputManager.Target = newEnergy;
                scene.GarbageElements.Add(energy);

                scene.VortexOutParticleSystem.AddParticle(OtherSocket.Position, Vector2.Zero);
                scene.VortexOutParticleSystem.AddParticle(OtherSocket.Position, Vector2.Zero);
                scene.VortexOutParticleSystem.AddParticle(OtherSocket.Position, Vector2.Zero);
                scene.VortexOutParticleSystem.AddParticle(OtherSocket.Position, Vector2.Zero);

                scene.SoundManager.ISoundEngine.Play2D(scene.SoundManager.socket[rand.Next(scene.SoundManager.socket.Count)], false, false, false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }

        
    }
}
