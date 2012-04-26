﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots.Elements
{
    public class ExperimentalTube : Element
    {
        Body body;
        Texture2D texture;

        public override float Width
        {
            get { return Conversion.ToWorld(texture.Width); }
            set { }
        }

        public override float Height
        {
            get { return Conversion.ToWorld(texture.Height); }
            set { }
        }

        public override Vector2 Position
        {
            get { return body.Position; }
            set { body.Position = value; }
        }

        public override float Rotation
        {
            get { return body.Rotation; }
            set { body.Rotation = value; }
        }

        public ExperimentalTube(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 1f;
            texture = Game.Content.Load<Texture2D>("experimental_tube");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(175), Conversion.ToWorld(60), 150f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.CollisionCategories = ElementCategory.FLOOR;

            body.UserData = this;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) - Vector2.UnitY * (texture.Height - 60) / 2), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}