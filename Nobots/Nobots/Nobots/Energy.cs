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

namespace Nobots
{
    class Energy : Character
    {
        Effect effect;

        public Energy(Game game, Scene scene)
            : base(game, scene)
        {
            body.Position = new Vector2(32.60955f, -2.803332f);

            body.CollisionCategories = ElementCategory.ENERGY;
            torso.CollisionCategories = ElementCategory.ENERGY;

            effect = Game.Content.Load<Effect>("energy");
        }

        protected override void UpActionStart()
        {
        }

        protected override void UpAction()
        {
        }

        protected override void UpActionStop()
        {
        }

        protected override void BActionStart()
        {
        }

        protected override void BAction()
        {
        }

        protected override void BActionStop()
        {
        }

        protected override void XActionStart()
        {
            foreach (Element i in scene.Elements)
            {
                Socket socket = i as Socket;
                if (socket != null)
                {
                    if (IsTouchingElement(i))
                    {
                        scene.VortexParticleSystem.AddParticle(socket.Position, Vector2.Zero);
                        scene.VortexParticleSystem.AddParticle(socket.Position, Vector2.Zero);
                        scene.VortexParticleSystem.AddParticle(socket.Position, Vector2.Zero);
                        scene.VortexParticleSystem.AddParticle(socket.Position, Vector2.Zero);
                        Position = socket.OtherSocket.Position;
                        break;
                    }
                }
            }
        }

        protected override void YActionStart()
        {
            foreach (Element i in scene.Elements)
            {
                Character character = i as Character;
                if (character != null)
                {
                    if (IsTouchingElement(i))
                    {
                        character.State = new IdleCharacterState(scene, character);
                        scene.PlasmaExplosionParticleSystem.AddParticle(Position, Vector2.Zero);
                        scene.PlasmaExplosionParticleSystem.AddParticle(Position, Vector2.Zero);
                        scene.PlasmaExplosionParticleSystem.AddParticle(Position, Vector2.Zero);
                        scene.PlasmaExplosionParticleSystem.AddParticle(Position, Vector2.Zero);
                        scene.GarbageElements.Add(this);
                        break;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, effect);
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(Position - scene.Camera.Position),
                new Rectangle(State.textureXmin, State.textureYmin, State.characterWidth, State.characterHeight),
                Color.White, 0.0f, new Vector2(State.characterWidth / 2, State.characterHeight / 2), 1.0f, Effect, 0);
            scene.SpriteBatch.End();
        }
    }
}
