using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Nobots.ParticleSystem;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;

namespace Nobots
{
    public class Scene : DrawableGameComponent
    {
        public PlasmaExplosionParticleSystem PlasmaExplosionParticleSystem;
        public LaserParticleSystem LaserParticleSystem;
        public VortexParticleSystem VortexParticleSystem;
        public SpriteBatch SpriteBatch;
        public Camera Camera;
        public World World;
        public DebugViewXNA physicsDebug;
        public SortedList<Element> Elements;
        public List<Background> Backgrounds;
        public List<Background> Foregrounds;

        public Scene(Game game)
            : base(game)
        {
            Camera = new Camera(Game);
            Elements = new SortedList<Element>();
            Backgrounds = new List<Background>();
            Foregrounds = new List<Background>();
            World = new World(new Vector2(0, 13));
            physicsDebug = new DebugViewXNA(World);
        }

        public override void Initialize()
        {
            base.Initialize();

            PlasmaExplosionParticleSystem = new PlasmaExplosionParticleSystem(Game, this);
            LaserParticleSystem = new LaserParticleSystem(Game, this);
            VortexParticleSystem = new VortexParticleSystem(Game, this);

            Backgrounds.Add(new Background(Game, this));
            Foregrounds.Add(new Background(Game, this));
            Box box1 = new Box(Game, this, new Vector2(5.812996f, 0.583698f));
            Elements.Add(box1);
            Elements.Add(new Stone(Game, this, new Vector2(7.812996f, 0f)));

            Platform platform1 = new Platform(Game, this, Conversion.ToWorld(new Vector2(0, 400)));
            Elements.Add(platform1);
            Platform platform2 = new Platform(Game, this, new Vector2(platform1.Position.X + platform1.Width, platform1.Position.Y));
            Elements.Add(platform2);
            PressurePlate pressurePlate1 = new PressurePlate(Game, this, new Vector2(3.84f, 3.809996f));
            Elements.Add(pressurePlate1);
            Platform platform3 = new Platform(Game, this, new Vector2(platform2.Position.X, platform2.Position.Y - Conversion.ToWorld(300)));
            Elements.Add(platform3);
            Platform platform4 = new Platform(Game, this, new Vector2(platform3.Position.X + platform3.Width, platform3.Position.Y));
            Elements.Add(platform4);

            Platform platform5 = new Platform(Game, this, new Vector2(platform4.Position.X + platform4.Width + Conversion.ToWorld(300), platform4.Position.Y));
            Elements.Add(platform5);
            Platform platform6 = new Platform(Game, this, new Vector2(18.92004f, 1));
            Elements.Add(platform6);

            Platform platform7 = new Platform(Game, this, new Vector2(platform6.Position.X - platform6.Width/2, platform2.Position.Y));
            Elements.Add(platform7);
            Platform platform8 = new Platform(Game, this, new Vector2(platform7.Position.X + platform7.Width, platform7.Position.Y));
            Elements.Add(platform8);
            Platform platform9 = new Platform(Game, this, new Vector2(platform8.Position.X + platform8.Width, platform8.Position.Y));
            Elements.Add(platform9);
            Platform platform10 = new Platform(Game, this, new Vector2(platform9.Position.X + platform9.Width, platform9.Position.Y));
            Elements.Add(platform10);

            Platform platform11 = new Platform(Game, this, new Vector2(platform10.Position.X + platform10.Width*3/2, platform10.Position.Y));
            Elements.Add(platform11);
            Platform platform12 = new Platform(Game, this, new Vector2(platform11.Position.X, platform11.Position.Y/3));
            Elements.Add(platform12);

            Platform platform13 = new Platform(Game, this, new Vector2(31.2f, -1.333333f));
            Elements.Add(platform13);
            Platform platform14 = new Platform(Game, this, new Vector2(35.60955f, -1.333333f));
            Elements.Add(platform14);
            Platform platform15 = new Platform(Game, this, new Vector2(platform14.Position.X + platform14.Width, platform14.Position.Y));
            Elements.Add(platform15);
            Platform platform16 = new Platform(Game, this, new Vector2(platform15.Position.X + platform15.Width/2, platform1.Position.Y));
            Elements.Add(platform16);
            Platform platform17 = new Platform(Game, this, new Vector2(platform16.Position.X + platform16.Width, platform16.Position.Y));
            Elements.Add(platform17);
            Platform platform18 = new Platform(Game, this, new Vector2(platform17.Position.X + platform17.Width, platform17.Position.Y));
            Elements.Add(platform18);
            Platform platform19 = new Platform(Game, this, new Vector2(platform18.Position.X + platform18.Width, platform18.Position.Y));
            Elements.Add(platform19);

            Elements.Add(new Forklift(Game, this, new Vector2(49.04955f, 0f)));
            //Elements.Add(new Stone(Game, this, new Vector2(49.04955f, 3.5f)));

            Ladder ladder1 = new Ladder(Game, this, 20, new Vector2(1.640002f, 1.360001f));
            Elements.Add(ladder1);
            Ladder ladder2 = new Ladder(Game, this, 20, new Vector2(16.71999f, 1.360001f));
            Elements.Add(ladder2);
            Ladder ladder3 = new Ladder(Game, this, 20, new Vector2(33.39991f, -1.309999f));
            Elements.Add(ladder3);
            Ladder ladder4 = new Ladder(Game, this, 26, new Vector2(41.70982f, 0.6433345f));
            Elements.Add(ladder4);

            Elements.Add(Camera.Target = new Character(Game, this));
            Elements.Add(new Energy(Game, this));

            Elevator elevator1 = new Elevator(Game, this, new Vector2(platform10.Position.X + platform10.Width/2, platform10.Position.Y - platform10.Height*4));
            Elements.Add(elevator1);

            Elevator elevator2 = new Elevator(Game, this, new Vector2(-1.5f, 1.870001f));
            elevator2.FinalPosition = new Vector2(-1.5f, -1.569997f);
            Elements.Add(elevator2);
            LaserBarrier laserBarrier1 = new LaserBarrier(Game, this, new Vector2(35.60955f, -2.823332f));
            Elements.Add(laserBarrier1);
            pressurePlate1.activableElement = elevator2;
            PressurePlate pressurePlate2 = new PressurePlate(Game, this, new Vector2(17.78031f, 0.7999996f));
            Elements.Add(pressurePlate2);
            Box box2 = new Box(Game, this, new Vector2(19.70855f, 0.4243353f));
            Elements.Add(box2);

            Socket socket = new Socket(Game, this, new Vector2(30.60955f, -2.223332f));
            Elements.Add(socket);

            Foregrounds[0].Texture = Game.Content.Load<Texture2D>("tree");
            Foregrounds[0].Speed = 1.5f * Vector2.One;
            Foregrounds[0].Position = new Vector2(5.0f, 0.0f);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            physicsDebug.LoadContent(GraphicsDevice, Game.Content);
            physicsDebug.AppendFlags(DebugViewFlags.Shape);
            physicsDebug.AppendFlags(DebugViewFlags.PolygonPoints);
            physicsDebug.AppendFlags(DebugViewFlags.CenterOfMass);

            base.LoadContent();
        }

        Random random = new Random();
        Element selection = null;
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                LaserParticleSystem.AddParticle(new Vector3(35.60955f, -2.823332f, 0), Vector3.Zero);
            }

            if (selection != null)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    selection.Position = selection.Position - Vector2.UnitY * Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    selection.Position = selection.Position + Vector2.UnitY * Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    selection.Position = selection.Position - Vector2.UnitX * Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    selection.Position = selection.Position + Vector2.UnitX * Conversion.ToWorld(1);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                selection = null;
                foreach (Element i in Elements)
                {
                    if (Vector2.Distance(i.Position, Camera.ScreenToWorld(Mouse.GetState())) < Conversion.ToWorld(10))
                    {
                        selection = i;
                        Console.WriteLine("Selected one at " + i.Position);
                        break;
                    }
                }
            }

            PlasmaExplosionParticleSystem.Update(gameTime);
            LaserParticleSystem.Update(gameTime);
            VortexParticleSystem.Update(gameTime);
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Update(gameTime);
            foreach (Background i in Backgrounds)
                i.Update(gameTime);
            foreach (Element i in Elements)
                i.Update(gameTime);
            foreach (Background i in Foregrounds)
                i.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Background i in Backgrounds)
                i.Draw(gameTime);
            foreach (Element i in Elements)
                i.Draw(gameTime);
            PlasmaExplosionParticleSystem.Draw(gameTime);
            LaserParticleSystem.Draw(gameTime);
            VortexParticleSystem.Draw(gameTime);

            physicsDebug.RenderDebugData(ref Camera.Projection, ref Camera.View);
            foreach (Background i in Foregrounds)
                i.Draw(gameTime);
            
            base.Update(gameTime);
        }
    }
}
