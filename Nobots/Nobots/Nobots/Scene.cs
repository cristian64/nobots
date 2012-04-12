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
        public VortexOutParticleSystem VortexOutParticleSystem;
        public SpriteBatch SpriteBatch;
        public InputManager InputManager;
        public SelectionManager SelectionManager;
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
            SelectionManager = new SelectionManager(Game, this);
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
            VortexOutParticleSystem = new VortexOutParticleSystem(Game, this);

            /*Backgrounds.Add(new Background(Game, this, "background1"));
            Backgrounds.Add(new Background(Game, this, "background2"));
            Backgrounds[1].Position = Backgrounds[0].Position + new Vector2(Backgrounds[0].Width, 0);
            Backgrounds.Add(new Background(Game, this, "background3"));
            Backgrounds[2].Position = Backgrounds[1].Position + new Vector2(Backgrounds[1].Width, 0);
            Backgrounds.Add(new Background(Game, this, "background4"));
            Backgrounds[3].Position = Backgrounds[2].Position + new Vector2(Backgrounds[2].Width, 0);
            Backgrounds.Add(new Background(Game, this, "wires"));
            Backgrounds[4].Position = new Vector2(44.4f, 11);*/

            /*Box box1 = new Box(Game, this, new Vector2(13.22069f, 1.436227f));
            Elements.Add(box1);*/

            /*Platform platform1 = new Platform(Game, this, new Vector2(1.879999f, 10.60015f), Conversion.ToWorld(new Vector2(384, 48)));
            Elements.Add(platform1);
            Platform platform2 = new Platform(Game, this, new Vector2(5.189986f, 10.17014f), new Vector2(3.9f, 1.389999f));
            Elements.Add(platform2);*/
            PressurePlate pressurePlate1 = new PressurePlate(Game, this, new Vector2(25.9805f, 3.179997f));

            Elements.Add(pressurePlate1);
            LaserBarrier laserBarrier3 = new LaserBarrier(Game, this, new Vector2(21.94052f, 3.189997f), 1.1f);
            laserBarrier3.Rotation = MathHelper.PiOver2;
            Elements.Add(laserBarrier3);
            pressurePlate1.activableElement = laserBarrier3;

            /*Platform platform3 = new Platform(Game, this, new Vector2(6.930026f, 6.500125f), new Vector2(0.4000032f, 6.090044f));
            Elements.Add(platform3);
            Platform platform4 = new Platform(Game, this, new Vector2(10.07001f, 3.320068f), new Vector2(9.810133f, 0.44f));
            Elements.Add(platform4);

            Platform platform5 = new Platform(Game, this, new Vector2(19.78006f, 3.329998f), new Vector2(3.230002f, 0.43f));
            Elements.Add(platform5);
            Platform platform6 = new Platform(Game, this, new Vector2(30.07031f, 1.59007f), new Vector2(0.6900029f, 3.329997f));
            Elements.Add(platform6);

            Platform platform7 = new Platform(Game, this, new Vector2(20.39012f, 6.540057f), new Vector2(0.570003f, 6.040043f));
            Elements.Add(platform7);
            Platform platform8 = new Platform(Game, this, new Vector2(22.05007f, 10.17014f), new Vector2(3.88f, 1.399999f));
            Elements.Add(platform8);
            Platform platform9 = new Platform(Game, this, new Vector2(28.98014f, 10.64015f), new Vector2(10.17014f, 0.48f));
            Elements.Add(platform9);
            Platform platform10 = new Platform(Game, this, new Vector2(40.83918f, 17.21028f), new Vector2(13.65022f, 0.48f));
            Elements.Add(platform10);

            Platform platform11 = new Platform(Game, this, new Vector2(58.21865f, 17.2103f), new Vector2(21.22039f, 0.48f));
            Elements.Add(platform11);
            Platform platform12 = new Platform(Game, this, new Vector2(41.81847f, 12.5902f), new Vector2(4.380008f, 0.48f));
            Elements.Add(platform12);

            Platform platform13 = new Platform(Game, this, new Vector2(44.33905f, 11.49017f), new Vector2(0.7400029f, 11.01016f));
            Elements.Add(platform13);
            Platform platform14 = new Platform(Game, this, new Vector2(46.27872f, 5.77004f), new Vector2(8.060088f, 0.48f));
            Elements.Add(platform14);
            Platform platform15 = new Platform(Game, this, new Vector2(34.07142f, 3.43f), new Vector2(0.560003f, 7.010066f));
            Elements.Add(platform15);
            Platform platform16 = new Platform(Game, this, new Vector2(37.74145f, 5.77004f), new Vector2(6.870065f, 0.48f));
            Elements.Add(platform16);
            Platform platform17 = new Platform(Game, this, new Vector2(52.40963f, 2.76f), new Vector2(0.7400029f, 5.610034f));
            Elements.Add(platform17);
            Platform platform18 = new Platform(Game, this, new Vector2(49.7602f, 11.44017f), new Vector2(0.7800028f, 11.17016f));
            Elements.Add(platform18);

            Platform platform19 = new Platform(Game, this, new Vector2(1.999999f, 3.319997f), new Vector2(4.120001f, 0.4300005f));
            Elements.Add(platform19);
            Platform platform20 = new Platform(Game, this, new Vector2(26.44006f, 3.34f), new Vector2(7.930088f, 0.4500005f));
            Elements.Add(platform20);
            Platform platform21 = new Platform(Game, this, new Vector2(52.07965f, 5.760039f), new Vector2(1.400001f, 0.4500005f));
            Elements.Add(platform21);

            Platform platform22 = new Platform(Game, this, new Vector2(-0.09255317f, 5.400352f), new Vector2(0.4800005f, 11.05016f));
            Elements.Add(platform22);
            Platform platform23 = new Platform(Game, this, new Vector2(37.06137f, -0.3222811f), new Vector2(74.81757f, 0.6700003f));
            Elements.Add(platform23);
            Platform platform24 = new Platform(Game, this, new Vector2(33.8574f, 13.9004f), new Vector2(0.5400004f, 7.000066f));
            Elements.Add(platform24);
            Platform platform25 = new Platform(Game, this, new Vector2(16.89746f, 6.550418f), new Vector2(7.010066f, 0.4400005f));
            Elements.Add(platform25);*/

            /*Forklift forklift1 = new Forklift(Game, this, new Vector2(64.23187f, 8.605165f));
            Elements.Add(forklift1);*/

            Ladder ladder1 = new Ladder(Game, this, 14, new Vector2(4.610013f, 5.320026f));
            Elements.Add(ladder1);
            Ladder ladder2 = new Ladder(Game, this, 14, new Vector2(21.93991f, 5.300028f));
            Elements.Add(ladder2);
            Ladder ladder3 = new Ladder(Game, this, 22, new Vector2(50.83699f, 10.43015f));
            Elements.Add(ladder3);
            Ladder ladder4 = new Ladder(Game, this, 15, new Vector2(41.70982f, 7.90342f));
            Elements.Add(ladder4);

            //Elements.Add(Camera.Target = InputManager.Character = new Character(Game, this));

            /*Elevator elevator1 = new Elevator(Game, this, new Vector2(37.08941f, 16.08384f));
            Elements.Add(elevator1);
            elevator1.FinalPosition = new Vector2(37.08941f, 12.55374f);*/

            ElectricityBox eBox1 = new ElectricityBox(Game, this, new Vector2(46.83757f, 3.989998f));
            Elements.Add(eBox1);
            ElectricityBox eBox2 = new ElectricityBox(Game, this, new Vector2(56.67609f, 15.20025f));
            Elements.Add(eBox2);
            //eBox2.activableElement = forklift1;
            ElectricityBox eBox3 = new ElectricityBox(Game, this, new Vector2(41.38832f, 15.66026f));
            Elements.Add(eBox3);
            //eBox3.activableElement = elevator1;
            LaserBarrier laserBarrier1 = new LaserBarrier(Game, this, new Vector2(44.35808f, 2.756664f));
            Elements.Add(laserBarrier1);
            eBox1.activableElement = laserBarrier1;
            /*Box box2 = new Box(Game, this, new Vector2(28.55241f, 1.9716f));
            Elements.Add(box2);*/

            Socket socket = new Socket(Game, this, new Vector2(43.70762f, 15.96693f));
            Elements.Add(socket);

            Socket socket2 = new Socket(Game, this, new Vector2(43.73852f, 11.32683f));
            Elements.Add(socket2);
            socket.OtherSocket = socket2;
            socket2.OtherSocket = socket;

            SceneLoader sl = new SceneLoader(Game, this);

            sl.SceneFromXml(@"Content\levels\level1.xml", this);

            //System.IO.File.WriteAllText(@"C:\Users\Cristian\Desktop\level1.xml", sl.SceneToXml(this));
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

        public override void Update(GameTime gameTime)
        {
            if (sceneTarget.Width != GraphicsDevice.Viewport.Width || sceneTarget.Height != GraphicsDevice.Viewport.Height)
            {
                sceneTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
                renderTarget1 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
                renderTarget2 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
            }

            SelectionManager.Update(gameTime);

            foreach (Element i in GarbageElements)
                Elements.Remove(i);
            GarbageElements.Clear();

            PlasmaExplosionParticleSystem.Update(gameTime);
            LaserParticleSystem.Update(gameTime);
            VortexParticleSystem.Update(gameTime);
            VortexOutParticleSystem.Update(gameTime);
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
                SpriteBatch.Draw(renderTarget1, new Rectangle(0, 0, GraphicsDevice.Viewport.Width,  GraphicsDevice.Viewport.Height), Color.White);
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
            VortexOutParticleSystem.Draw(gameTime);
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
