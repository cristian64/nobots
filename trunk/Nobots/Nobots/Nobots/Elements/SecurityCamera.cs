using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using IrrKlang;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    public class SecurityCamera : Element
    {
        Texture2D cameraTexture;
        Texture2D baseTexture;
        SpriteEffects effect = SpriteEffects.None;

        public override float Width
        {
            get
            {
                return Conversion.ToWorld(cameraTexture.Width + baseTexture.Width);
            }
            set
            {
            }
        }

        public override float Height
        {
            get
            {
                return Conversion.ToWorld(cameraTexture.Height + baseTexture.Height);
            }
            set
            {
            }
        }

        private Vector2 position;
        public override Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        private float rotation = 0;
        public override float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public SecurityCamera(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 0f;
            cameraTexture = Game.Content.Load<Texture2D>("securitycamera");
            baseTexture = Game.Content.Load<Texture2D>("securitycamera_base");
            this.Position = position;
            effect = SpriteEffects.None;
        }

        public override void Update(GameTime gameTime)
        {
            if (scene.Camera.Target != null)
                if (Vector2.DistanceSquared(scene.Camera.Target.Position, Position) < 150)
                {
                    if (scene.Camera.Target.Position.Y >= position.Y)
                    {
                        float rot = (float)Math.Atan2(scene.Camera.Target.Position.Y - position.Y, scene.Camera.Target.Position.X - Position.X);
                        if (Rotation != rot)
                        {
                            Rotation += ((rot - Rotation) >= 0 ? 1 : -1) * 0.007f;
                            if (rot - Rotation >= -0.007f && rot - Rotation <= 0.007f)
                                Rotation = rot;
                        }
                    }
                }
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(baseTexture, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), null, Color.White, 0, new Vector2(baseTexture.Width / 2.0f, baseTexture.Height), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(cameraTexture, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), null, Color.White, Rotation, new Vector2(cameraTexture.Width/2.0f, cameraTexture.Height / 2.0f), scene.Camera.Scale, effect, 0);
        }
    }
}
