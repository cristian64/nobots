using System;
using System.Collections.Generic;
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
        public SpriteBatch SpriteBatch;
        public Camera Camera;
        public World World;
        public DebugViewXNA physicsDebug;
        public List<Element> Elements;
        public List<Background> Backgrounds;
        public List<Background> Foregrounds;

        public Scene(Game game)
            : base(game)
        {
            Camera = new Camera(Game);
            Elements = new List<Element>();
            Backgrounds = new List<Background>();
            Foregrounds = new List<Background>();
            World = new World(new Vector2(0, 13));
            physicsDebug = new DebugViewXNA(World);

            PlasmaExplosionParticleSystem = new PlasmaExplosionParticleSystem(Game, this);
        }

        public override void Initialize()
        {
            base.Initialize();

            PlasmaExplosionParticleSystem.Initialize();

            Backgrounds.Add(new Background(Game, this));
            Foregrounds.Add(new Background(Game, this));
            Elements.Add(new Box(Game, this));
            Elements.Add(new Character(Game, this));
            Camera.Target = Elements[1];
            Elements.Add(new Box(Game, this));
            Elements.Add(new Box(Game, this));
            Elements.Add(new Box(Game, this));
            Elements.Add(new Box(Game, this));
            Elements.Add(new Box(Game, this));
            Elements.Add(new Box(Game, this));

            Elements.Add(new Elevator(Game, this));

            Platform platform1 = new Platform(Game, this);
            Elements.Add(platform1);
            Platform platform2 = new Platform(Game, this);
            Elements.Add(platform2);
            Platform platform3 = new Platform(Game, this);
            Elements.Add(platform3);
            Platform platform4 = new Platform(Game, this);
            Elements.Add(platform4);
            Platform platform5 = new Platform(Game, this);
            Elements.Add(platform5);
            Platform platform6 = new Platform(Game, this);
            Elements.Add(platform6);
            Platform platform7 = new Platform(Game, this);
            Elements.Add(platform7);

            Foregrounds[0].Texture = Game.Content.Load<Texture2D>("tree");
            Foregrounds[0].Speed = 1.5f * Vector2.One;
            Foregrounds[0].Position = new Vector2(5.0f, 0.0f);

            platform1.Position = new Vector2(0.7f, 3f);
            platform1.Rotation = 1.0f;
            platform2.Position = new Vector2(3.8f, 3.75f);
            platform3.Position = new Vector2(5.0f, 5f);
            platform4.Position = new Vector2(9.7f, 4.4f);
            platform5.Position = new Vector2(13.8f, 3.5f);
            platform6.Position = new Vector2(17.8f, 3.25f);
            platform7.Position = new Vector2(21.8f, 3.25f);
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
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                PlasmaExplosionParticleSystem.AddParticle(new Vector3(random.Next(75), random.Next(50), 0), Vector3.Zero);
                PlasmaExplosionParticleSystem.AddParticle(new Vector3(random.Next(40), random.Next(45), 0), Vector3.Zero);
            }

            PlasmaExplosionParticleSystem.Update(gameTime);
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Update(gameTime);
            foreach (Background i in Backgrounds)
                i.Update(gameTime);
            foreach (Element i in Elements)
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


            physicsDebug.RenderDebugData(ref Camera.Projection, ref Camera.View);
            foreach (Background i in Foregrounds)
                i.Draw(gameTime);
            
            base.Update(gameTime);
        }
    }
}
