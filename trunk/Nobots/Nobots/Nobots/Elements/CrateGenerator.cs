using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using IrrKlang;
using FarseerPhysics.Common;

namespace Nobots.Elements
{
    public class CrateGenerator : Element, IActivable
    {
        Body body;
        Texture2D texture;
        Texture2D crateTexture;
        SpriteFont crateFont;
        static Random random = new Random();

        String crateId = "";

        public int CratesNumber = 1;
        float delayCounter = 0;
        private bool isActive = false;
        public bool Active
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
                if (isActive && delayCounter <= 0)
                {
                    delayCounter = 1;

                    int cratesCount = 0;
                    foreach (Element i in scene.Elements)
                        if (i.Id == crateId)
                            cratesCount++;

                    foreach (Element i in scene.Elements)
                    {
                        if (i.Id == crateId && cratesCount-- >= CratesNumber)
                        {
                            for (int j = 0; j < 150; j++)
                            {
                                Vector2 increment = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
                                scene.ExplosionSmokeParticleSystem.AddParticle(i.Position + increment, Vector2.Zero);
                            }
                            scene.GarbageElements.Add(i);
                        }
                    }

                    Crate crate = new Crate(Game, scene, body.Position - new Vector2(0, 1.7f));
                    crate.Id = crateId;
                    crate.Rotation = (float)random.NextDouble();
                    scene.RespawnElements.Add(crate);
                }
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
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public CrateGenerator(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            crateId = random.Next(int.MaxValue).ToString() + " (random id)";
            ZBuffer = 1f;
            texture = Game.Content.Load<Texture2D>("crategenerator");
            crateTexture = Game.Content.Load<Texture2D>("crate_white");
            crateFont = Game.Content.Load<SpriteFont>("cratefont");

            body = BodyFactory.CreateEdge(scene.World, new Vector2(-Width / 2, -Height / 2), new Vector2(Width / 2, -Height / 2));
            FixtureFactory.AttachEdge(new Vector2(-Width / 2, -Height / 2), new Vector2(-Width / 2, Height / 2), body);
            FixtureFactory.AttachEdge(new Vector2(Width / 2, -Height / 2), new Vector2(Width / 2, Height / 2), body);

            body.Position = position;
            body.BodyType = BodyType.Static;
            body.FixedRotation = true;
            body.UserData = this;
        }

        public override void Update(GameTime gameTime)
        {
            delayCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(crateTexture, scene.Camera.Scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) + new Vector2(60, -20)), null, Color.White, 0, new Vector2(crateTexture.Width / 2.0f, crateTexture.Height / 2.0f), scene.Camera.Scale * 0.2f, SpriteEffects.None, 0);
            scene.SpriteBatch.DrawString(crateFont, CratesNumber.ToString(), scene.Camera.Scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) + new Vector2(85, -40)), Color.LightGray, 0, Vector2.Zero, scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
