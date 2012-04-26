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
using Nobots.Elements;
using FarseerPhysics.DebugViews;
using IrrKlang;

namespace Nobots
{
    public class Scene : DrawableGameComponent
    {
        public ISoundEngine ISoundEngine;
        public AmbienceSound AmbienceSound;
        public PlasmaExplosionParticleSystem PlasmaExplosionParticleSystem;
        public SmokePlumeParticleSystem SmokePlumeParticleSystem;
        public SteamParticleSystem SteamParticleSystem;
        public LaserParticleSystem LaserParticleSystem;
        public VortexParticleSystem VortexParticleSystem;
        public VortexOutParticleSystem VortexOutParticleSystem;
        public SpriteBatch SpriteBatch;
        public InputManager InputManager;
        public SelectionManager SelectionManager;
        public SceneLoader SceneLoader;
        public Camera Camera;
        public World World;
        public DebugViewXNA PhysicsDebug;
        public SortedList<Element> GarbageElements;
        public SortedList<Element> RespawnElements;
        public SortedList<Element> Elements;
        public List<Background> Backgrounds;
        public List<Foreground> Foregrounds;

        RenderTarget2D sceneTarget;
        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;
        Effect bloomExtractEffect;
        Effect bloomCombineEffect;
        Effect gaussianBlurEffect;

        Texture2D blank;

        public Scene(Game game)
            : base(game)
        {

            

    ISoundEngine = new ISoundEngine(
     SoundOutputDriver.AutoDetect,
             SoundEngineOptionFlag.DefaultOptions );

    //ISoundEngine.SetRolloffFactor(10000000f);

   
    
    //ISoundEngine.Default3DSoundMinDistance = 1f;
    ISoundEngine.Default3DSoundMaxDistance = 1000000f;

            
            
            
            
            

           
            AmbienceSound = new AmbienceSound(Game, this);
            World = new World(new Vector2(0, 13));
            PhysicsDebug = new DebugViewXNA(World);
            InputManager = new InputManager(Game);
            SelectionManager = new SelectionManager(Game, this);
            SceneLoader = new SceneLoader(Game);
            Camera = new Camera(Game, this);
            GarbageElements = new SortedList<Element>();
            RespawnElements = new SortedList<Element>();
            Elements = new SortedList<Element>();
            Backgrounds = new List<Background>();
            Foregrounds = new List<Foreground>();
        }

        public override void Initialize()
        {
            base.Initialize();

            PlasmaExplosionParticleSystem = new PlasmaExplosionParticleSystem(Game, this);
            SmokePlumeParticleSystem = new SmokePlumeParticleSystem(Game, this);
            SteamParticleSystem = new SteamParticleSystem(Game, this);
            LaserParticleSystem = new LaserParticleSystem(Game, this);
            VortexParticleSystem = new VortexParticleSystem(Game, this);
            VortexOutParticleSystem = new VortexOutParticleSystem(Game, this);

            SceneLoader.SceneFromXml(@"Content\levels\level1.xml", this);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            PhysicsDebug.LoadContent(GraphicsDevice, Game.Content);
            PhysicsDebug.AppendFlags(DebugViewFlags.Shape);
            PhysicsDebug.AppendFlags(DebugViewFlags.PolygonPoints);
            PhysicsDebug.AppendFlags(DebugViewFlags.CenterOfMass);

            bloomExtractEffect = Game.Content.Load<Effect>("BloomExtract");
            bloomCombineEffect = Game.Content.Load<Effect>("BloomCombine");
            gaussianBlurEffect = Game.Content.Load<Effect>("GaussianBlur");

            GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            sceneTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            renderTarget1 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

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
            SceneLoader.Update(gameTime, this);

            foreach (Element i in GarbageElements)
            {
                Elements.Remove(i);
                i.Dispose();
            }
            
            GarbageElements.Clear();

            PlasmaExplosionParticleSystem.Update(gameTime);
            SmokePlumeParticleSystem.Update(gameTime);
            SteamParticleSystem.Update(gameTime);
            LaserParticleSystem.Update(gameTime);
            VortexParticleSystem.Update(gameTime);
            VortexOutParticleSystem.Update(gameTime);
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Update(gameTime);
            InputManager.Update(gameTime);
            AmbienceSound.Update(gameTime);
            foreach (Background i in Backgrounds)
                i.Update(gameTime);
            foreach (Element i in Elements)
                i.Update(gameTime);
            foreach (Foreground i in Foregrounds)
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

            SelectionManager.Draw(gameTime);
            PhysicsDebug.RenderDebugData(ref Camera.Projection, ref Camera.View);
            if (PhysicsDebug.Enabled)
            {
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                foreach (Element i in Elements)
                {
                    if (i is Stone || i is Lamp || i is Alarm || i is Sound) // TODO: ADD watedrops or things without physics body
                        PrimitiveDrawings.DrawBoundingBox(SpriteBatch, blank, Camera.WorldToScreen(i.Position), Camera.Scale * Conversion.ToDisplay(i.Width), Camera.Scale * Conversion.ToDisplay(i.Height), i.Rotation, new Color(0.5f, 0.9f, 0.5f));
                }
                foreach (Background i in Backgrounds)
                    PrimitiveDrawings.DrawBoundingBox(SpriteBatch, blank, Camera.WorldToScreen(i.Position), Camera.Scale * Conversion.ToDisplay(i.Width), Camera.Scale * Conversion.ToDisplay(i.Height), i.Rotation, i == SelectionManager.Selection ? Color.Yellow : Color.Blue);
                foreach (Foreground i in Foregrounds)
                    PrimitiveDrawings.DrawBoundingBox(SpriteBatch, blank, Camera.WorldToScreen(i.Position), Camera.Scale * Conversion.ToDisplay(i.Width), Camera.Scale * Conversion.ToDisplay(i.Height), i.Rotation, i == SelectionManager.Selection ? Color.Yellow : Color.Green);

                PrimitiveDrawings.DrawBoundingBox(SpriteBatch, blank, Camera.WorldToScreen(Camera.ListenerPosition), 30, 30, MathHelper.PiOver4, Color.White);

                SpriteBatch.End();
            }

            base.Update(gameTime);
        }

        public void drawScene(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            foreach (Background i in Backgrounds)
                i.Draw(gameTime);
            foreach (Element i in Elements)
                i.Draw(gameTime);
            PlasmaExplosionParticleSystem.Draw(gameTime);
            SmokePlumeParticleSystem.Draw(gameTime);
            SteamParticleSystem.Draw(gameTime);
            LaserParticleSystem.Draw(gameTime);
            VortexParticleSystem.Draw(gameTime);
            VortexOutParticleSystem.Draw(gameTime);
            SpriteBatch.End();
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            foreach (Foreground i in Foregrounds)
                i.Draw(gameTime);
            SpriteBatch.End();

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
