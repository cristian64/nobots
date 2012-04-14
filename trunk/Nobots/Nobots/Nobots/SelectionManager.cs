using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Nobots
{
    public class SelectionManager : DrawableGameComponent
    {
        private Editor.FormProperties form;
        private Scene scene;
        private Element selection = null;
        public Element Selection
        {
            get
            {
                return selection;
            }
            set
            {
                selection = value;
                form.SelectedElement = selection;
            }
        }

        public SelectionManager(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            this.form = new Editor.FormProperties();
            this.form.Show();
        }

        MouseState previous;
        public override void Update(GameTime gameTime)
        {
            if (Selection != null)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    Selection.Position = Selection.Position - Vector2.UnitY * Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    Selection.Position = Selection.Position + Vector2.UnitY * Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    Selection.Position = Selection.Position - Vector2.UnitX * Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    Selection.Position = Selection.Position + Vector2.UnitX * Conversion.ToWorld(1);

                if (Keyboard.GetState().IsKeyDown(Keys.Add) && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    Selection.Height = Selection.Height + Conversion.ToWorld(1);
                else if (Keyboard.GetState().IsKeyDown(Keys.Add))
                    Selection.Width = Selection.Width + Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.Subtract) && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    Selection.Height = Selection.Height - Conversion.ToWorld(1);
                else if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
                    Selection.Width = Selection.Width - Conversion.ToWorld(1);
            }

            if (Game.IsActive && Game.Window.ClientBounds.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed && previous.LeftButton == ButtonState.Released)
            {
                Console.WriteLine(scene.Camera.ScreenToWorld(Mouse.GetState()));
                Element newSelection = null;
                foreach (Element i in scene.Elements)
                {
                    if (Vector2.Distance(i.Position, scene.Camera.ScreenToWorld(Mouse.GetState())) < Conversion.ToWorld(10))
                    {
                        newSelection = i;
                        Console.WriteLine("Selected one at " + i.Position + ", Width " + i.Width + ", Height " + i.Height);
                        break;
                    }
                }
                Selection = newSelection;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && previous.RightButton == ButtonState.Released)
            {
                Platform platform = new Platform(Game, scene, scene.Camera.ScreenToWorld(previous), Vector2.One);
                scene.Elements.Add(platform);
            }

            previous = Mouse.GetState();

            base.Update(gameTime);
        }
    }
}
