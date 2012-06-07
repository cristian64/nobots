using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots.Elements
{
    public class Energy : Character
    {
        Effect effect;

        public Energy(Game game, Scene scene, Vector2 position)
            : base(game, scene, position)
        {
            ZBuffer = 1.0f;

            body.CollisionCategories = ElementCategory.ENERGY;
            torso.CollisionCategories = ElementCategory.ENERGY;
            body.CollidesWith = Category.All & ~ElementCategory.CHARACTER;
            torso.CollidesWith = Category.All & ~ElementCategory.CHARACTER;

            effect = Game.Content.Load<Effect>("effects\\energy");
        }

        static Random random = new Random();

        public void Die()
        {
            for (int j = 0; j < 50; j++)
            {
                scene.PlasmaExplosionParticleSystem.AddParticle(Position - Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                scene.PlasmaExplosionParticleSystem.AddParticle(Position + Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
            }
            scene.GarbageElements.Add(this);

            bool foundComa = false;
            foreach (Element el in scene.Elements)
                if (el is Character && !(el is Energy) && ((Character)el).State is ComaCharacterState)
                {
                    ((Character)el).State = new DyingCharacterState(scene, (Character)el);
                    scene.InputManager.Target = (Character)el;
                    foundComa = true;
                    break;
                }

            if (!foundComa)
            {
                //TODO: should die here the energy after some time too
            }
        }

        public override void UpActionStart()
        {
        }

        public override void UpAction()
        {
        }

        public override void UpActionStop()
        {
        }

        public override void DownActionStart()
        {
        }

        public override void DownAction()
        {
        }

        public override void DownActionStop()
        {
        }

        public override void BActionStart()
        {
            foreach (Element i in scene.Elements)
            {
                Socket socket = i as Socket;
                if (socket != null && IsTouchingElement(i))
                {
                    socket.Travel(this);
                    break;
                }

                Activator activator = i as Activator;
                if (activator != null && activator.EnergyElement && IsTouchingElement(i))
                {
                    activator.Activate();
                }
            }
        }

        public override void BAction()
        {
        }

        public override void BActionStop()
        {
        }

        public override void YActionStart()
        {
            foreach (Element i in scene.Elements)
            {
                IControllable controllable = i as IControllable;
                if (controllable != null && controllable != this && IsTouchingElement(i))
                {
                    if (controllable is Character)
                        ((Character)controllable).State = new IdleCharacterState(scene, (Character)controllable);

                    Random random = new Random();
                    for (int j = 0; j < 50; j++)
                    {
                        scene.PlasmaExplosionParticleSystem.AddParticle(Position - Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                        scene.PlasmaExplosionParticleSystem.AddParticle(Position + Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                    }
                    scene.GarbageElements.Add(this);
                    scene.InputManager.Target = controllable;
                    scene.Camera.Target = (Element)controllable;

                    break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.End();
            scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, effect);
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * (Conversion.ToDisplay(Position - scene.Camera.Position) - Vector2.UnitY * 10),
                new Rectangle(State.textureXmin, State.textureYmin, State.characterWidth, State.characterHeight),
                Color.White, 0.0f, new Vector2(State.characterWidth / 2.0f, State.characterHeight / 2.0f), scene.Camera.Scale, Effect, 0);
            scene.SpriteBatch.End();
            scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        }
    }
}
