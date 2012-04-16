using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nobots.Elements;

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
                form.Selection = selection;
            }
        }

        public SelectionManager(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;

            System.Windows.Forms.Application.EnableVisualStyles();
            this.form = new Editor.FormProperties(Game, scene);
            this.form.Show();

            Initialize();
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
                if (Keyboard.GetState().IsKeyDown(Keys.Delete))
                {
                    scene.GarbageElements.Add(selection);
                    selection = null;
                }
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && previous.LeftButton == ButtonState.Released && IsMouseInWindow(Mouse.GetState()))
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
                Element element = null;
                switch (form.NewElementType)
                {
                    case "Platform":
                        element = new Platform(Game, scene, scene.Camera.ScreenToWorld(previous), Vector2.One);
                        break;
                    case "Ladder":
                        element = new Ladder(Game, scene, 1, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Stone":
                        element = new Stone(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "ElectricityBox":
                        element = new ElectricityBox(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "PressurePlate":
                        element = new PressurePlate(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Character":
                        element = new Character(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Box":
                        element = new Box(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Chandelier":
                        element = new Chandelier(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Closet":
                        element = new Closet(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Elevator":
                        element = new Elevator(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Forklift":
                        element = new Forklift(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "LaserBarrier":
                        element = new LaserBarrier(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Socket":
                        element = new Socket(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "ConveyorBelt":
                        element = new ConveyorBelt(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                }
                if (element != null)
                    scene.Elements.Add(element);
            }

            previous = Mouse.GetState();

            base.Update(gameTime);
        }

        public bool IsMouseInWindow(MouseState mouseState)
        {
            return Game.IsActive &&
                mouseState.X >= 0 && mouseState.X < GraphicsDevice.Viewport.Width &&
                mouseState.Y >= 0 && mouseState.Y < GraphicsDevice.Viewport.Height &&
                System.Windows.Forms.Form.ActiveForm != null &&
                System.Windows.Forms.Form.ActiveForm.Text.Equals(Game.Window.Title);
        }
    }
}
