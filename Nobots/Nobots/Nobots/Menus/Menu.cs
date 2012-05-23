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

        SpriteFont menuoptionfont;
        SpriteFont menufont;
        int selectedIndex;

        Keys keyboardAAction = Keys.Enter;
        Keys keyboardYAction = Keys.Y;
        Keys keyboardBAction = Keys.Back;
        Keys keyboardXAction = Keys.X;

        Scene scene;
        public Menu(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            Initialize();
            Enabled = false;

            menuoptionfont = Game.Content.Load<SpriteFont>("menuoption");
            menufont = Game.Content.Load<SpriteFont>("menu");

            options = new List<Option>();
            Option option1 = new ResumeOption(scene);
            Option option2 = new RestartLevelOption(scene);
            Option option3 = new LoadLevelOption(scene);
            Option option4 = new ExitOption(scene);
            options.Add(option1);
            options.Add(option2);
            options.Add(option3);
            options.Add(option4);

            selectedIndex = 0;
        }

        public override void Update(GameTime gameTime)
        {
            processKeyboard();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Enabled)
            {
                scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                scene.SpriteBatch.DrawString(menufont, "MENU", new Vector2(50, 100), Color.White);

                Vector2 position = new Vector2(70, 200);
                for (int i = 0; i < options.Count; i++)
                {
                    Option option = options[i];
                    scene.SpriteBatch.DrawString(menuoptionfont, option.Text, position, selectedIndex == i ? Color.White : Color.Gray);
                    position += new Vector2(0, menuoptionfont.MeasureString(option.Text).Y + 0);
                }
                scene.SpriteBatch.End();
            }
        }

        protected KeyboardState previousKeyboardState;
        protected GamePadState previosGamepadState;
        protected void processKeyboard()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if ((currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape)) ||
                (currentGamepadState.Buttons.Start == ButtonState.Pressed && previosGamepadState.Buttons.Start == ButtonState.Released) ||
                (currentGamepadState.Buttons.BigButton == ButtonState.Pressed && previosGamepadState.Buttons.BigButton == ButtonState.Released))
            {
                Enabled = !Enabled;
                if (!Enabled)
                    scene.Transitioner.AlphaTarget = 0;
                else
                    scene.Transitioner.AlphaTarget = 0.8f;
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
                        (currentGamepadState.ThumbSticks.Left.X < 0 && (previosGamepadState.ThumbSticks.Left.X >= 0)) ||
                        (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left)))
                    {
                        LeftActionStart();
                    }
                    if ((currentGamepadState.DPad.Right == ButtonState.Pressed && previosGamepadState.DPad.Right == ButtonState.Released) ||
                        (currentGamepadState.ThumbSticks.Left.X > 0 && (previosGamepadState.ThumbSticks.Left.X <= 0)) ||
                        (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right)))
                    {
                        RightActionStart();
                    }
                    if ((currentGamepadState.DPad.Up == ButtonState.Pressed && previosGamepadState.DPad.Up == ButtonState.Released) ||
                        (currentGamepadState.ThumbSticks.Left.Y > 0 && (previosGamepadState.ThumbSticks.Left.Y <= 0)) ||
                        (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
                    {
                        UpActionStart();
                    }
                    if ((currentGamepadState.DPad.Down == ButtonState.Pressed && previosGamepadState.DPad.Down == ButtonState.Released) ||
                        (currentGamepadState.ThumbSticks.Left.Y < 0 && (previosGamepadState.ThumbSticks.Left.Y >= 0)) ||
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
                        (currentGamepadState.ThumbSticks.Left.X == 0 && (previosGamepadState.ThumbSticks.Left.X < 0)) ||
                        (currentKeyboardState.IsKeyUp(Keys.Left) && previousKeyboardState.IsKeyDown(Keys.Left)))
                    {
                        LeftActionStop();
                    }
                    if ((currentGamepadState.DPad.Right == ButtonState.Released && previosGamepadState.DPad.Right == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.X == 0 && (previosGamepadState.ThumbSticks.Left.X > 0)) ||
                        (currentKeyboardState.IsKeyUp(Keys.Right) && previousKeyboardState.IsKeyDown(Keys.Right)))
                    {
                        RightActionStop();
                    }
                    if ((currentGamepadState.DPad.Up == ButtonState.Released && previosGamepadState.DPad.Up == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.Y == 0 && (previosGamepadState.ThumbSticks.Left.Y > 0)) ||
                        (currentKeyboardState.IsKeyUp(Keys.Up) && previousKeyboardState.IsKeyDown(Keys.Up)))
                    {
                        UpActionStop();
                    }
                    if ((currentGamepadState.DPad.Down == ButtonState.Released && previosGamepadState.DPad.Down == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.Y == 0 && (previosGamepadState.ThumbSticks.Left.Y < 0)) ||
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
                        (currentGamepadState.ThumbSticks.Left.X < 0) ||
                        (currentKeyboardState.IsKeyDown(Keys.Left)))
                    {
                        LeftAction();
                    }
                    if ((currentGamepadState.DPad.Right == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.X > 0) ||
                        (currentKeyboardState.IsKeyDown(Keys.Right)))
                    {
                        RightAction();
                    }
                    if ((currentGamepadState.DPad.Up == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.Y > 0) ||
                        (currentKeyboardState.IsKeyDown(Keys.Up)))
                    {
                        UpAction();
                    }
                    if ((currentGamepadState.DPad.Down == ButtonState.Pressed) ||
                        (currentGamepadState.ThumbSticks.Left.Y < 0) ||
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
            Enabled = false;
            scene.Transitioner.AlphaTarget = 0;
        }

        void StartActionStart()
        {
        }

        void StartAction()
        {
        }

        void StartActionStop()
        {
            Enabled = false;
            scene.Transitioner.AlphaTarget = 0;
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
        }
    }
}
