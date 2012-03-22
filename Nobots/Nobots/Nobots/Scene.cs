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
            Elements.Add(new Stone(Game, this));
            Elements.Add(new Circuit(Game, this, Conversion.ToWorld(new Vector2(1000, 300)), Conversion.ToWorld(new Vector2(1100, 500))));


            Elements.Add(Camera.Target = new Character(Game, this));

            Platform platform1 = new Platform(Game, this, Conversion.ToWorld(new Vector2(0, 400)));
            Elements.Add(platform1);
            Platform platform2 = new Platform(Game, this, new Vector2(platform1.Position.X + Conversion.ToWorld(platform1.Width), platform1.Position.Y));
            Elements.Add(platform2);
            Platform platform3 = new Platform(Game, this, new Vector2(platform2.Position.X, platform2.Position.Y - Conversion.ToWorld(300)));
            Elements.Add(platform3);
            Platform platform4 = new Platform(Game, this, new Vector2(platform3.Position.X + Conversion.ToWorld(platform3.Width), platform3.Position.Y));
            Elements.Add(platform4);

            Platform platform5 = new Platform(Game, this, new Vector2(platform4.Position.X + Conversion.ToWorld(platform4.Width + 150), platform4.Position.Y));
            Elements.Add(platform5);
            Platform platform6 = new Platform(Game, this, new Vector2(platform5.Position.X + Conversion.ToWorld(platform5.Width), platform5.Position.Y));
            Elements.Add(platform6);

            Platform platform7 = new Platform(Game, this, new Vector2(platform6.Position.X - Conversion.ToWorld(platform6.Width/2), platform2.Position.Y));
            Elements.Add(platform7);
            Platform platform8 = new Platform(Game, this, new Vector2(platform7.Position.X + Conversion.ToWorld(platform7.Width), platform7.Position.Y));
            Elements.Add(platform8);
            Platform platform9 = new Platform(Game, this, new Vector2(platform8.Position.X + Conversion.ToWorld(platform8.Width), platform8.Position.Y));
            Elements.Add(platform9);
            Platform platform10 = new Platform(Game, this, new Vector2(platform9.Position.X + Conversion.ToWorld(platform9.Width), platform9.Position.Y));
            Elements.Add(platform10);

            Platform platform11 = new Platform(Game, this, new Vector2(platform10.Position.X + Conversion.ToWorld(platform10.Width*3/2), platform10.Position.Y));
            Elements.Add(platform11);
            Platform platform12 = new Platform(Game, this, new Vector2(platform11.Position.X, platform11.Position.Y/3));
            Elements.Add(platform12);

            Platform platform13 = new Platform(Game, this, new Vector2(platform12.Position.X - Conversion.ToWorld(platform12.Width), platform12.Position.Y - (platform11.Position.Y - platform12.Position.Y)));
            Elements.Add(platform13);
            Platform platform14 = new Platform(Game, this, new Vector2(platform12.Position.X, platform13.Position.Y));
            Elements.Add(platform14);

            Ladder ladder1 = new Ladder(Game, this, 9, new Vector2(platform2.Position.X, (platform2.Position.Y + platform3.Position.Y)/2 - Conversion.ToWorld(platform2.Height)));
            Elements.Add(ladder1);
            Ladder ladder2 = new Ladder(Game, this, 9, new Vector2(platform7.Position.X - Conversion.ToWorld(platform7.Width/3), (platform7.Position.Y + platform6.Position.Y)/2 - Conversion.ToWorld(platform9.Height)));
            Elements.Add(ladder2);
            Ladder ladder3 = new Ladder(Game, this, 9, new Vector2(platform14.Position.X, (platform14.Position.Y + platform12.Position.Y) / 2 - Conversion.ToWorld(platform12.Height)));
            Elements.Add(ladder3);
            Elevator elevator1 = new Elevator(Game, this, new Vector2(platform10.Position.X + Conversion.ToWorld(platform10.Width/2), platform10.Position.Y - Conversion.ToWorld(platform10.Height*4)));
            Elements.Add(elevator1);


          /*  Platform platform1 = new Platform(Game, this, new Vector2(0.7f, 3f));
            Platform platform2 = new Platform(Game, this, new Vector2(3.8f, 3.75f));
            Elements.Add(platform2);
            Platform platform3 = new Platform(Game, this, new Vector2(5.0f, 5f));
            Elements.Add(platform3);
            Platform platform4 = new Platform(Game, this, new Vector2(9.7f, 4.4f));
            Elements.Add(platform4);
            Platform platform5 = new Platform(Game, this, new Vector2(13.8f, 3.5f));
            Elements.Add(platform5);
            Platform platform6 = new Platform(Game, this, new Vector2(17.8f, 3.25f));
            Elements.Add(platform6);
            Platform platform7 = new Platform(Game, this, new Vector2(21.8f, 3.25f));
            Elements.Add(platform7);
            */
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
