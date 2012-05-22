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
        Random random = new Random();

        String crateId = "";

        float counter = 0;
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
                if (isActive && counter <= 0)
                {
                    counter = 1;

                    foreach (Element i in scene.Elements)
                    {
                        if (i.Id == crateId)
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
                    switch (random.Next(4))
                    {
                        case 0: crate.Color = "pink"; break;
                        case 1: crate.Color = "orange"; break;
                        case 2: crate.Color = "blue"; break;
                        case 3: crate.Color = "red"; break;
                    }
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
            counter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
