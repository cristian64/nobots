using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nobots.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace Nobots
{
    public class SelectionManager : DrawableGameComponent
    {
        SpriteFont font;
        Texture2D elementEmblem;
        Texture2D selectionEmblem;
        Texture2D backgroundEmblem;
        Texture2D foregroundEmblem;
        public static bool ShowEmblems = true;

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

            font = Game.Content.Load<SpriteFont>("debugfont");
            elementEmblem = Game.Content.Load<Texture2D>("icons\\elementemblem");
            selectionEmblem = Game.Content.Load<Texture2D>("icons\\selectionemblem");
            backgroundEmblem = Game.Content.Load<Texture2D>("icons\\backgroundemblem");
            foregroundEmblem = Game.Content.Load<Texture2D>("icons\\foregroundemblem");

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
                float speed = Conversion.ToWorld(1);
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                    speed = Conversion.ToWorld(20);

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    Selection.Position = Selection.Position - Vector2.UnitY * speed;
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    Selection.Position = Selection.Position + Vector2.UnitY * speed;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    Selection.Position = Selection.Position - Vector2.UnitX * speed;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    Selection.Position = Selection.Position + Vector2.UnitX * speed;

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
                    if (selection is Background)
                        scene.Backgrounds.Remove((Background)selection);
                    else if (selection is Foreground)
                        scene.Foregrounds.Remove((Foreground)selection);
                    else scene.Elements.Remove(selection);
                    selection.Dispose();
                    Selection = null;
                }
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && previous.LeftButton == ButtonState.Released && IsMouseInWindow(Mouse.GetState()))
            {
                Console.WriteLine(scene.Camera.ScreenToWorld(Mouse.GetState()));
                Element newSelection = null;
                List<Element> elements = new List<Element>();
                elements.AddRange(scene.Elements);
                elements.AddRange(scene.Backgrounds);
                elements.AddRange(scene.Foregrounds);
                foreach (Element i in elements)
                {
                    if (Vector2.Distance(scene.Camera.WorldToScreen(i.Position), new Vector2(Mouse.GetState().X, Mouse.GetState().Y)) < 8)
                    {
                        newSelection = i;
                        Console.WriteLine("Selected one at " + i.Position + ", Width " + i.Width + ", Height " + i.Height);
                        break;
                    }
                }
                if (newSelection != Selection)
                    Selection = newSelection;
                else
                    Selection = null;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && previous.RightButton == ButtonState.Released)
            {
                Element element = null;
                Background background = null;
                Foreground foreground = null;
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
                    case "Stomper":
                        element = new Stomper(Game, scene, scene.Camera.ScreenToWorld(previous));
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
                    case "ExperimentalTube":
                        element = new ExperimentalTube(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Box":
                        element = new Box(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Crate":
                        element = new Crate(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Computer":
                        element = new Computer(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Lamp":
                        element = new Lamp(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Alarm":
                        element = new Alarm(Game, scene, scene.Camera.ScreenToWorld(previous));
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
                    case "MovingPlatform":
                        element = new MovingPlatform(Game, scene, scene.Camera.ScreenToWorld(previous));
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
                    case "Lever":
                        element = new Lever(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "ImpulsePlatform":
                        element = new ImpulsePlatform(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Checkpoint":
                        element = new Checkpoint(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Endpoint":
                        element = new Endpoint(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Background":
                        background = new Background(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                    case "Foreground":
                        foreground = new Foreground(Game, scene, scene.Camera.ScreenToWorld(previous));
                        break;
                }
                if (element != null)
                    scene.Elements.Add(element);
                if (background != null)
                    scene.Backgrounds.Add(background);
                if (foreground != null)
                    scene.Foregrounds.Add(foreground);
            }

            previous = Mouse.GetState();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (ShowEmblems)
            {
                scene.SpriteBatch.Begin();
                foreach (Background i in scene.Backgrounds)
                    scene.SpriteBatch.Draw(backgroundEmblem, scene.Camera.Scale * Conversion.ToDisplay(i.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(backgroundEmblem.Width / 2, backgroundEmblem.Height / 2), 1.0f, SpriteEffects.None, 0);
                foreach (Element i in scene.Elements)
                    scene.SpriteBatch.Draw(elementEmblem, scene.Camera.Scale * Conversion.ToDisplay(i.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(backgroundEmblem.Width / 2, backgroundEmblem.Height / 2), 1.0f, SpriteEffects.None, 0);
                foreach (Foreground i in scene.Foregrounds)
                    scene.SpriteBatch.Draw(foregroundEmblem, scene.Camera.Scale * Conversion.ToDisplay(i.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(backgroundEmblem.Width / 2, backgroundEmblem.Height / 2), 1.0f, SpriteEffects.None, 0);

                if (selection != null && !(selection is Energy && !scene.World.BodyList.Contains(((Energy)selection).body)))
                {
                    String selectionName = selection.GetType().Name;
                    Vector2 selectionPosition = scene.Camera.WorldToScreen(selection.Position) - font.MeasureString(selectionName) / 2 + new Vector2(0, -20);
                    scene.SpriteBatch.DrawString(font, selectionName, selectionPosition + Vector2.UnitX, Color.Black);
                    scene.SpriteBatch.DrawString(font, selectionName, selectionPosition - Vector2.UnitX, Color.Black);
                    scene.SpriteBatch.DrawString(font, selectionName, selectionPosition + Vector2.UnitY, Color.Black);
                    scene.SpriteBatch.DrawString(font, selectionName, selectionPosition - Vector2.UnitY, Color.Black);
                    scene.SpriteBatch.DrawString(font, selectionName, selectionPosition, Color.White);

                    scene.SpriteBatch.Draw(selectionEmblem, scene.Camera.WorldToScreen(selection.Position), null, Color.White, 0, new Vector2(backgroundEmblem.Width / 2, backgroundEmblem.Height / 2), 1.0f, SpriteEffects.None, 0);
                }
                scene.SpriteBatch.End();
            }
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
