﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace Nobots
{
    public class Elevator : Element, IActivable
    {
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
            }
        }

        public Vector2 InitialPosition;
        public Vector2 FinalPosition;
        public float Speed = 1f;

        Body body;
        Texture2D texture;
        Texture2D chainsTexture;
        Texture2D thingTexture;

        public override float Height
        {
            get
            {
                return Conversion.ToWorld(texture.Height);
            }
            set
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                return body.Rotation;
            }
            set
            {
                body.Rotation = value;
            }
        }

        public Elevator(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 6f;
            chainsTexture = Game.Content.Load<Texture2D>("elevator_chains");
            texture = Game.Content.Load<Texture2D>("elevator");
            thingTexture = Game.Content.Load<Texture2D>("elevator_upperthing");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width - 30), Conversion.ToWorld(texture.Height - 15), 150f);

            body.Position = position;
            //body.Position = new Vector2(37.08941f, 16.78384f);
            body.BodyType = BodyType.Kinematic;

            InitialPosition = body.Position;
            FinalPosition = body.Position - Vector2.UnitY * 3;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 targetPosition = Active ? FinalPosition : InitialPosition;
            if (Vector2.DistanceSquared(targetPosition, Position) > Speed * Speed * gameTime.ElapsedGameTime.TotalSeconds * gameTime.ElapsedGameTime.TotalSeconds)
            {
                Vector2 direction = Vector2.Normalize(targetPosition - Position);
                body.LinearVelocity = Speed * direction;
            }
            else
            {
                Position = targetPosition;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(chainsTexture, scene.Camera.Scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) - new Vector2(-6, chainsTexture.Height + 30)), null, Color.White, 0, new Vector2(texture.Width / 2, 0), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(thingTexture, scene.Camera.Scale * (Conversion.ToDisplay(InitialPosition - scene.Camera.Position) - new Vector2(80, thingTexture.Height + chainsTexture.Height)), null, Color.White, 0, new Vector2(texture.Width / 2, 0), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2 - 3, texture.Height / 2 + 7), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}