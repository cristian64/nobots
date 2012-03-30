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
            ZBuffer = 1.0f;
            body.Position = new Vector2(32.60955f, -2.803332f);

            body.CollisionCategories = ElementCategory.ENERGY;
            torso.CollisionCategories = ElementCategory.ENERGY;
            body.CollidesWith = Category.All & ~ElementCategory.CHARACTER;
            torso.CollidesWith = Category.All & ~ElementCategory.CHARACTER;

            effect = Game.Content.Load<Effect>("energy");
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

        public override void BActionStart()
        {
        }

        public override void BAction()
        {
        }

        public override void BActionStop()
        {
        }

        public override void XActionStart()
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
                        scene.VortexOutParticleSystem.AddParticle(socket.OtherSocket.Position, Vector2.Zero);
                        scene.VortexOutParticleSystem.AddParticle(socket.OtherSocket.Position, Vector2.Zero);
                        scene.VortexOutParticleSystem.AddParticle(socket.OtherSocket.Position, Vector2.Zero);
                        scene.VortexOutParticleSystem.AddParticle(socket.OtherSocket.Position, Vector2.Zero);
                        break;
                    }
                }

                ElectricityBox eBox = i as ElectricityBox;
                if (eBox != null)
                {
                    if (IsTouchingElement(i))
                    {
                        eBox.activableElement.Active = !eBox.activableElement.Active;
                        break;
                    }
                }
            }
        }

        public override void YActionStart()
        {
            foreach (Element i in scene.Elements)
            {
                Character character = i as Character;
                if (character != null && character != this)
                {
                    if (IsTouchingElement(i))
                    {
                        character.State = new IdleCharacterState(scene, character);
                        Random random = new Random();
                        for (int j = 0; j < 50; j++)
                        {
                            scene.PlasmaExplosionParticleSystem.AddParticle(Position - Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                            scene.PlasmaExplosionParticleSystem.AddParticle(Position + Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                        }
                        scene.World.RemoveBody(body);
                        scene.World.RemoveBody(torso);
                        scene.World.RemoveJoint(revoluteJoint);
                        scene.GarbageElements.Add(this);
                        scene.InputManager.Character = character;
                        scene.Camera.Target = character;
                    }
                    break;
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
