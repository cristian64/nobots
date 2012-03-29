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
        public InputManager InputManager;
        public Camera Camera;
        public World World;
        public DebugViewXNA physicsDebug;
        public SortedList<Element> GarbageElements;
        public SortedList<Element> RespawnElements;
        public SortedList<Element> Elements;
        public List<Background> Backgrounds;
        public List<Background> Foregrounds;

        RenderTarget2D sceneTarget;
        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;
        Effect bloomExtractEffect;
        Effect bloomCombineEffect;
        Effect gaussianBlurEffect;

        public Scene(Game game)
            : base(game)
        {
            InputManager = new InputManager(Game);
            Camera = new Camera(Game);
            GarbageElements = new SortedList<Element>();
            RespawnElements = new SortedList<Element>();
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

            Backgrounds.Add(new Background(Game, this, Game.Content.Load<Texture2D>("background1")));
            Backgrounds.Add(new Background(Game, this, Game.Content.Load<Texture2D>("background2")));
            Backgrounds[1].Position = Backgrounds[0].Position + new Vector2(Backgrounds[0].Width, 0);
            Backgrounds.Add(new Background(Game, this, Game.Content.Load<Texture2D>("background3")));
            Backgrounds[2].Position = Backgrounds[1].Position + new Vector2(Backgrounds[1].Width, 0);
            Backgrounds.Add(new Background(Game, this, Game.Content.Load<Texture2D>("background4")));
            Backgrounds[3].Position = Backgrounds[2].Position + new Vector2(Backgrounds[2].Width, 0);

            Foregrounds.Add(new Background(Game, this, Game.Content.Load<Texture2D>("tree")));
            Box box1 = new Box(Game, this, new Vector2(5.812996f, 0.583698f));
            Elements.Add(box1);
            Elements.Add(new Stone(Game, this, new Vector2(7.812996f, 0f)));

            Platform platform1 = new Platform(Game, this, Conversion.ToWorld(new Vector2(0, 400)), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform1);
            Platform platform2 = new Platform(Game, this, new Vector2(platform1.Position.X + platform1.Width, platform1.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform2);
            PressurePlate pressurePlate1 = new PressurePlate(Game, this, new Vector2(3.84f, 3.809996f));
            Elements.Add(pressurePlate1);
            Platform platform3 = new Platform(Game, this, new Vector2(platform2.Position.X, platform2.Position.Y - Conversion.ToWorld(300)), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform3);
            Platform platform4 = new Platform(Game, this, new Vector2(platform3.Position.X + platform3.Width, platform3.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform4);

            Platform platform5 = new Platform(Game, this, new Vector2(platform4.Position.X + platform4.Width + Conversion.ToWorld(300), platform4.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform5);
            Platform platform6 = new Platform(Game, this, new Vector2(18.92004f, 1), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform6);

            Platform platform7 = new Platform(Game, this, new Vector2(platform6.Position.X - platform6.Width / 2, platform2.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform7);
            Platform platform8 = new Platform(Game, this, new Vector2(platform7.Position.X + platform7.Width, platform7.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform8);
            Platform platform9 = new Platform(Game, this, new Vector2(platform8.Position.X + platform8.Width, platform8.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform9);
            Platform platform10 = new Platform(Game, this, new Vector2(platform9.Position.X + platform9.Width, platform9.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform10);

            Platform platform11 = new Platform(Game, this, new Vector2(platform10.Position.X + platform10.Width * 3 / 2, platform10.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform11);
            Platform platform12 = new Platform(Game, this, new Vector2(platform11.Position.X, platform11.Position.Y / 3), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform12);

            Platform platform13 = new Platform(Game, this, new Vector2(31.2f, -1.333333f), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform13);
            Platform platform14 = new Platform(Game, this, new Vector2(35.60955f, -1.333333f), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform14);
            Platform platform15 = new Platform(Game, this, new Vector2(platform14.Position.X + platform14.Width, platform14.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform15);
            Platform platform16 = new Platform(Game, this, new Vector2(platform15.Position.X + platform15.Width / 2, platform1.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform16);
            Platform platform17 = new Platform(Game, this, new Vector2(platform16.Position.X + platform16.Width, platform16.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform17);
            Platform platform18 = new Platform(Game, this, new Vector2(platform17.Position.X + platform17.Width, platform17.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform18);
            Platform platform19 = new Platform(Game, this, new Vector2(platform18.Position.X + platform18.Width, platform18.Position.Y), Conversion.ToWorld(new Vector2(384, 48)));
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

            Elements.Add(Camera.Target = InputManager.Character = new Character(Game, this));
            //Elements.Add(new Energy(Game, this));

            Elevator elevator1 = new Elevator(Game, this, new Vector2(30.440044f, 2.08f));
            Elements.Add(elevator1);
            elevator1.FinalPosition = new Vector2(elevator1.Position.X, 0f);

            Elevator elevator2 = new Elevator(Game, this, new Vector2(-1.5f, 1.870001f));
            elevator2.FinalPosition = new Vector2(-1.5f, -1.569997f);
            Elements.Add(elevator2);
            ElectricityBox eBox1 = new ElectricityBox(Game, this, new Vector2(34.36966f, 2.999999f));
            Elements.Add(eBox1);
            eBox1.activableElement = elevator1;
            LaserBarrier laserBarrier1 = new LaserBarrier(Game, this, new Vector2(35.60955f, -2.823332f));
            Elements.Add(laserBarrier1);
            pressurePlate1.activableElement = elevator2;
            PressurePlate pressurePlate2 = new PressurePlate(Game, this, new Vector2(17.78031f, 0.7999996f));
            Elements.Add(pressurePlate2);
            Box box2 = new Box(Game, this, new Vector2(19.70855f, 0.4243353f));
            Elements.Add(box2);

            Socket socket = new Socket(Game, this, new Vector2(30.60955f, -2.223332f));
            Elements.Add(socket);

            Socket socket2 = new Socket(Game, this, new Vector2(37.60955f, -2.223332f));
            Elements.Add(socket2);
            socket.OtherSocket = socket2;
            socket2.OtherSocket = socket;

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

            bloomExtractEffect = Game.Content.Load<Effect>("BloomExtract");
            bloomCombineEffect = Game.Content.Load<Effect>("BloomCombine");
            gaussianBlurEffect = Game.Content.Load<Effect>("GaussianBlur");

            GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            sceneTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            renderTarget1 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);

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

                if (Keyboard.GetState().IsKeyDown(Keys.Add) && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    selection.Height = selection.Height + Conversion.ToWorld(1);
                else if (Keyboard.GetState().IsKeyDown(Keys.Add))
                    selection.Width = selection.Width + Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.Subtract) && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    selection.Height = selection.Height - Conversion.ToWorld(1);
                else if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
                    selection.Width = selection.Width - Conversion.ToWorld(1);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                selection = null;
                foreach (Element i in Elements)
                {
                    if (Vector2.Distance(i.Position, Camera.ScreenToWorld(Mouse.GetState())) < Conversion.ToWorld(10))
                    {
                        selection = i;
                        Console.WriteLine("Selected one at " + i.Position + ", Width " + i.Width + ", Height " + i.Height);
                        break;
                    }
                }
            }

            foreach (Element i in GarbageElements)
                Elements.Remove(i);
            GarbageElements.Clear();

            PlasmaExplosionParticleSystem.Update(gameTime);
            LaserParticleSystem.Update(gameTime);
            VortexParticleSystem.Update(gameTime);
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Update(gameTime);
            InputManager.Update(gameTime);
            foreach (Background i in Backgrounds)
                i.Update(gameTime);
            foreach (Element i in Elements)
                i.Update(gameTime);
            foreach (Background i in Foregrounds)
                i.Update(gameTime);

            foreach (Element i in RespawnElements)
                Elements.Add(i);
            RespawnElements.Clear();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (InputManager.Character as Energy != null)
            {
                // Draw scene in a render target.
                GraphicsDevice.SetRenderTarget(sceneTarget);
                drawScene(gameTime);
                GraphicsDevice.SetRenderTarget(null);

                // Draw scene again to extract the bright effects.
                GraphicsDevice.SetRenderTarget(renderTarget1);
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, bloomExtractEffect);
                SpriteBatch.Draw(sceneTarget, Vector2.Zero, Color.White);
                SpriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Draw scene applying now a blur effect to the brights extracted (both vertical and horizontal).
                SetBlurEffectParameters(1.0f / (float)renderTarget1.Width, 0);
                GraphicsDevice.SetRenderTarget(renderTarget2);
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, gaussianBlurEffect);
                SpriteBatch.Draw(renderTarget1, Vector2.Zero, Color.White);
                SpriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);
                SetBlurEffectParameters(0, 1.0f / (float)renderTarget1.Height);
                GraphicsDevice.SetRenderTarget(renderTarget1);
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, gaussianBlurEffect);
                SpriteBatch.Draw(renderTarget2, Vector2.Zero, Color.White);
                SpriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Finally draw the last pass on the screen.
                GraphicsDevice.Textures[1] = sceneTarget;
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, bloomCombineEffect);
                SpriteBatch.Draw(renderTarget1, Vector2.Zero, Color.White);
                SpriteBatch.End();
            }
            else
            {
                drawScene(gameTime);
            }

            physicsDebug.RenderDebugData(ref Camera.Projection, ref Camera.View);

            base.Update(gameTime);
        }

        public void drawScene(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            foreach (Background i in Backgrounds)
                i.Draw(gameTime);
            foreach (Element i in Elements)
                i.Draw(gameTime);
            PlasmaExplosionParticleSystem.Draw(gameTime);
            LaserParticleSystem.Draw(gameTime);
            VortexParticleSystem.Draw(gameTime);
            foreach (Background i in Foregrounds)
                i.Draw(gameTime);
        }

        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        void SetBlurEffectParameters(float dx, float dy)
        {
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = gaussianBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = gaussianBlurEffect.Parameters["SampleOffsets"];

            // Look up how many samples our gaussian blur effect supports.
            int sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }


        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        float ComputeGaussian(float n)
        {
            float theta = 5;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}
