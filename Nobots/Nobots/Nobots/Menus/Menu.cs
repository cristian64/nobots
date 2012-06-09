using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nobots.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace Nobots.Menus
{
    public class Menu : DrawableGameComponent
    {
        List<Option> options;

        Texture2D logo;
        Texture2D controls;
        SpriteFont menuoptionfont;
        SpriteFont menufont;
        int selectedIndex;

        Keys keyboardAAction = Keys.Enter;
        Keys keyboardYAction = Keys.LeftControl;
        Keys keyboardBAction = Keys.Back;
        Keys keyboardXAction = Keys.RightControl;

        Scene scene;
        public Menu(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            Initialize();
#if !FINAL_RELEASE
            Enabled = false;
#endif

            menuoptionfont = Game.Content.Load<SpriteFont>("fonts\\menuoption");
            menufont = Game.Content.Load<SpriteFont>("fonts\\menu");
            logo = Game.Content.Load<Texture2D>("icons/logo");
            controls = Game.Content.Load<Texture2D>("icons/controls");

            options = new List<Option>();
            options.Add(new ResumeOption(scene));
            options.Add(new LoadLevelOption(scene));
            options.Add(new ControlsOption(scene));
#if FINAL_RELEASE
            if (System.IO.File.Exists("Synergy (editor).exe"))
                options.Add(new EditorOption(scene));
#endif
            options.Add(new ExitOption(scene));

            selectedIndex = 0;

            this.EnabledChanged += new EventHandler<EventArgs>(Menu_EnabledChanged);
        }

        void Menu_EnabledChanged(object sender, EventArgs e)
        {
            if (!Enabled)
                alpha = 0;
        }

        float alpha = 0;

        public override void Update(GameTime gameTime)
        {
            if (Enabled && alpha < 1)
                alpha = Math.Min(1, alpha + (float)gameTime.ElapsedGameTime.TotalSeconds);
            processKeyboard();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Enabled)
            {
                scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                scene.SpriteBatch.Draw(logo, new Vector2(90, 90), null, Color.White * alpha, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                Vector2 position = new Vector2(140, 230);
                for (int i = 0; i < options.Count; i++)
                {
                    Option option = options[i];
                    scene.SpriteBatch.DrawString(menuoptionfont, option.Text, position, (selectedIndex == i ? Color.White : Color.Gray)  * alpha);
                    position += new Vector2(0, menuoptionfont.MeasureString(option.Text).Y + 0);
                }

                if (options[selectedIndex] is ControlsOption)
                    scene.SpriteBatch.Draw(controls, new Vector2(GraphicsDevice.Viewport.Width / 1.5f, GraphicsDevice.Viewport.Height / 2.0f), null, Color.White * alpha, 0, new Vector2(controls.Width / 2.0f, controls.Height / 2.0f), 1, SpriteEffects.None, 0);

                scene.SpriteBatch.End();
            }
        }

        float threshold = 0.95f;
        protected KeyboardState previousKeyboardState;
        protected GamePadState previosGamepadState;
        protected void processKeyboard()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if ((currentKeyboardState.IsKeyUp(Keys.Escape) && previousKeyboardState.IsKeyDown(Keys.Escape)) ||
                (currentGamepadState.Buttons.Start == ButtonState.Released && previosGamepadState.Buttons.Start == ButtonState.Pressed) ||
                (currentGamepadState.Buttons.BigButton == ButtonState.Released && previosGamepadState.Buttons.BigButton == ButtonState.Pressed))
            {
                // IF it's enable but there are not levels loaded, forbid the menu to disappear by ending the method here.
#if FINAL_RELEASE
                if (Enabled && scene.Elements.Count + scene.Backgrounds.Count + scene.Backgrounds.Count == 0)
                {
                    previousKeyboardState = currentKeyboardState;
                    previosGamepadState = currentGamepadState;
                    return;
                }
#endif

                Enabled = !Enabled;
                if (!Enabled)
                    scene.Transitioner.AlphaTarget = 0;
                else
                    scene.Transitioner.AlphaTarget = 0.9f;

                options[selectedIndex].Refresh(false);
                selectedIndex = 0;
                options[selectedIndex].Refresh(true);
                scene.AmbienceSound.ISoundEngine.Play2D(scene.AmbienceSound.Select, false, false, false);

                // Check if there is no restart option but there is indeed a level loaded.
                bool thereIsRestart = false;
                foreach (Option i in options)
                    if (i is RestartLevelOption)
                        thereIsRestart = true;
                if (!thereIsRestart && scene.SceneLoader.LastLevel != "")
                {
                    options.Insert(1, new RestartLevelOption(scene));
                    options.Insert(1, new LastCheckpointOption(scene));
                }
            }

            if (Enabled)
            {
                if (Game.IsActive && System.Windows.Forms.Form.ActiveForm != null && System.Windows.Forms.Form.ActiveForm.Text.Equals(Game.Window.Title))
                {
                    if (currentGamepadState.Buttons.BigButton == ButtonState.Pressed && previosGamepadState.Buttons.BigButton == ButtonState.Released)
                    {
                        BigButtonActionStart();
                    }
                    if (currentGamepadState.Buttons.Start == ButtonState.Pressed && previosGamepadState.Buttons.Start == ButtonState.Released)
                    {
                        StartActionStart();
                    }
                    if (currentGamepadState.Buttons.Back == ButtonState.Pressed && previosGamepadState.Buttons.Back == ButtonState.Released)
                    {
                        BackActionStart();
                    }
                    if ((currentGamepadState.Buttons.A == ButtonState.Pressed && previosGamepadState.Buttons.A == ButtonState.Released) ||
                        (currentKeyboardState.IsKeyDown(keyboardAAction) && previousKeyboardState.IsKeyUp(keyboardAAction)))
                    {
                        AActionStart();
                    }
                    if (((currentGamepadState.Buttons.B == ButtonState.Pressed && previosGamepadState.Buttons.B == ButtonState.Released) ||
                        (currentKeyboardState.IsKeyDown(keyboardBAction) && previousKeyboardState.IsKeyUp(keyboardBAction))))
                    {
                        BActionStart();
                    }
                    if ((currentGamepadState.Buttons.X == ButtonState.Pressed && previosGamepadState.Buttons.X == ButtonState.Released) ||
                        (currentKeyboardState.IsKeyDown(keyboardXAction) && previousKeyboardState.IsKeyUp(keyboardXAction)))
                    {
                        XActionStart();
                    }
                    if ((currentGamepadState.Buttons.Y == ButtonState.Pressed && previosGamepadState.Buttons.Y == ButtonState.Released) ||
                        (currentKeyboardState.IsKeyDown(keyboardYAction) && previousKeyboardState.IsKeyUp(keyboardYAction)))
                    {
                        YActionStart();
                    }
                    if ((currentGamepadState.DPad.Left == ButtonState.Pressed && previosGamepadState.DPad.Left == ButtonState.Released) ||
                        (currentGamepadState.ThumbSticks.Left.X < -threshold && (previosGamepadState.ThumbSticks.Left.X >= -threshold)) ||
                        (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left)))
                    {
                        LeftActionStart();
                    }
                    if ((currentGamepadState.DPad.Right == ButtonState.Pressed && previosGamepadState.DPad.Right == ButtonState.Released) ||
                        (currentGamepadState.ThumbSticks.Left.X > threshold && (previosGamepadState.ThumbSticks.Left.X <= threshold)) ||
                        (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right)))
                    {
                        RightActionStart();
                    }
                    if ((currentGamepadState.DPad.Up == ButtonState.Pressed && previosGamepadState.DPad.Up == ButtonState.Released) ||
                        (currentGamepadState.ThumbSticks.Left.Y > threshold && (previosGamepadState.ThumbSticks.Left.Y <= threshold)) ||
                        (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
                    {
                        UpActionStart();
                    }
                    if ((currentGamepadState.DPad.Down == ButtonState.Pressed && previosGamepadState.DPad.Down == ButtonState.Released) ||
                        (currentGamepadState.ThumbSticks.Left.Y < -threshold && (previosGamepadState.ThumbSticks.Left.Y >= -threshold)) ||
                        (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down)))
                    {
                        DownActionStart();
                    }
                    if (currentGamepadState.Buttons.BigButton == ButtonState.Released && previosGamepadState.Buttons.BigButton == ButtonState.Pressed)
                    {
                        BigButtonActionStop();
                    }
                    if (currentGamepadState.Buttons.Start == ButtonState.Released && previosGamepadState.Buttons.Start == ButtonState.Pressed)
                    {
                        StartActionStop();
                    }
                    if (currentGamepadState.Buttons.Back == ButtonState.Released && previosGamepadState.Buttons.Back == ButtonState.Pressed)
                    {
                        BackActionStop();
                    }
                    if ((currentGamepadState.Buttons.A == ButtonState.Released && previosGamepadState.Buttons.A == ButtonState.Pressed) ||
                        (currentKeyboardState.IsKeyUp(keyboardAAction) && previousKeyboardState.IsKeyDown(keyboardAAction)))
                    {
                        AActionStop();
                    }
                    if (((currentGamepadState.Buttons.B == ButtonState.Released && previosGamepadState.Buttons.B == ButtonState.Pressed) ||
                        (currentKeyboardState.IsKeyUp(keyboardBAction) && previousKeyboardState.IsKeyDown(keyboardBAction))))
                    {
                        BActionStop();
                    }
                    if ((currentGamepadState.Buttons.X == ButtonState.Released && previosGamepadState.Buttons.X == ButtonState.Pressed) ||
                        (currentKeyboardState.IsKeyUp(keyboardXAction) && previousKeyboardState.IsKeyDown(keyboardXAction)))
                    {
                        XActionStop();
                    }
                    if ((currentGamepadState.Buttons.Y == ButtonState.Released && previosGamepadState.Buttons.Y == ButtonState.Pressed) ||
                        (currentKeyboardState.IsKeyUp(keyboardYAction) && previousKeyboardState.IsKeyDown(keyboardYAction)))
                    {
                        YActionStop();
                    }
                    if ((currentGamepadState.DPad.Left == ButtonState.Released && previosGamepadState.DPad.Left == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.X > -threshold && (previosGamepadState.ThumbSticks.Left.X < -threshold)) ||
                        (currentKeyboardState.IsKeyUp(Keys.Left) && previousKeyboardState.IsKeyDown(Keys.Left)))
                    {
                        LeftActionStop();
                    }
                    if ((currentGamepadState.DPad.Right == ButtonState.Released && previosGamepadState.DPad.Right == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.X < threshold && (previosGamepadState.ThumbSticks.Left.X > threshold)) ||
                        (currentKeyboardState.IsKeyUp(Keys.Right) && previousKeyboardState.IsKeyDown(Keys.Right)))
                    {
                        RightActionStop();
                    }
                    if ((currentGamepadState.DPad.Up == ButtonState.Released && previosGamepadState.DPad.Up == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.Y < threshold && (previosGamepadState.ThumbSticks.Left.Y > threshold)) ||
                        (currentKeyboardState.IsKeyUp(Keys.Up) && previousKeyboardState.IsKeyDown(Keys.Up)))
                    {
                        UpActionStop();
                    }
                    if ((currentGamepadState.DPad.Down == ButtonState.Released && previosGamepadState.DPad.Down == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.Y > -threshold && (previosGamepadState.ThumbSticks.Left.Y < -threshold)) ||
                        (currentKeyboardState.IsKeyUp(Keys.Down) && previousKeyboardState.IsKeyDown(Keys.Down)))
                    {
                        DownActionStop();
                    }
                    if (currentGamepadState.Buttons.BigButton == ButtonState.Pressed)
                    {
                        BigButtonAction();
                    }
                    if (currentGamepadState.Buttons.Start == ButtonState.Pressed)
                    {
                        StartAction();
                    }
                    if (currentGamepadState.Buttons.Back == ButtonState.Pressed)
                    {
                        BackAction();
                    }
                    if ((currentGamepadState.Buttons.A == ButtonState.Pressed) ||
                        (currentKeyboardState.IsKeyDown(keyboardAAction)))
                    {
                        AAction();
                    }
                    if (((currentGamepadState.Buttons.B == ButtonState.Pressed) ||
                        (currentKeyboardState.IsKeyDown(keyboardBAction))))
                    {
                        BAction();
                    }
                    if ((currentGamepadState.Buttons.X == ButtonState.Pressed) ||
                        (currentKeyboardState.IsKeyDown(keyboardXAction)))
                    {
                        XAction();
                    }
                    if ((currentGamepadState.Buttons.Y == ButtonState.Pressed) ||
                        (currentKeyboardState.IsKeyDown(keyboardYAction)))
                    {
                        YAction();
                    }
                    if ((currentGamepadState.DPad.Left == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.X < -threshold) ||
                        (currentKeyboardState.IsKeyDown(Keys.Left)))
                    {
                        LeftAction();
                    }
                    if ((currentGamepadState.DPad.Right == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.X > threshold) ||
                        (currentKeyboardState.IsKeyDown(Keys.Right)))
                    {
                        RightAction();
                    }
                    if ((currentGamepadState.DPad.Up == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.Y > threshold) ||
                        (currentKeyboardState.IsKeyDown(Keys.Up)))
                    {
                        UpAction();
                    }
                    if ((currentGamepadState.DPad.Down == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.Y < -threshold) ||
                        (currentKeyboardState.IsKeyDown(Keys.Down)))
                    {
                        DownAction();
                    }
                }
            }

            previousKeyboardState = currentKeyboardState;
            previosGamepadState = currentGamepadState;
        }

        void BigButtonActionStart()
        {
        }

        void BigButtonAction()
        {
        }

        void BigButtonActionStop()
        {
        }

        void StartActionStart()
        {
        }

        void StartAction()
        {
        }

        void StartActionStop()
        {
        }

        void BackActionStart()
        {
        }

        void BackAction()
        {
        }

        void BackActionStop()
        {
            Enabled = false;
            scene.Transitioner.AlphaTarget = 0;
            scene.AmbienceSound.ISoundEngine.Play2D(scene.AmbienceSound.Select, false, false, false);
        }

        void AActionStart()
        {
        }

        void AAction()
        {
        }

        void AActionStop()
        {
            options[selectedIndex].AActionStop();
            scene.AmbienceSound.ISoundEngine.Play2D(scene.AmbienceSound.Select, false, false, false);
        }

        void BActionStart()
        {
        }

        void BAction()
        {
        }

        void BActionStop()
        {
            Enabled = false;
            scene.Transitioner.AlphaTarget = 0;
            scene.AmbienceSound.ISoundEngine.Play2D(scene.AmbienceSound.Select, false, false, false);
        }

        void XActionStart()
        {
        }

        void XAction()
        {
        }

        void XActionStop()
        {
        }

        void YActionStart()
        {
        }

        void YAction()
        {
        }

        void YActionStop()
        {
        }

        void RightActionStart()
        {
        }

        void RightAction()
        {
        }

        void RightActionStop()
        {
            options[selectedIndex].RightActionStop();
        }

        void LeftActionStart()
        {
        }

        void LeftAction()
        {
        }

        void LeftActionStop()
        {
            options[selectedIndex].LeftActionStop();
        }

        void UpActionStart()
        {
        }

        void UpAction()
        {
        }

        void UpActionStop()
        {
            options[selectedIndex].Refresh(false);
            selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : options.Count - 1;
            options[selectedIndex].Refresh(true);
            scene.AmbienceSound.ISoundEngine.Play2D(scene.AmbienceSound.Nav, false, false, false);
        }

        void DownActionStart()
        {
        }

        void DownAction()
        {
        }

        void DownActionStop()
        {
            options[selectedIndex].Refresh(false);
            selectedIndex = (selectedIndex + 1) % options.Count;
            options[selectedIndex].Refresh(true);
            scene.AmbienceSound.ISoundEngine.Play2D(scene.AmbienceSound.Nav, false, false, false);
        }
    }
}
