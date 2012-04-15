using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nobots.Elements;

namespace Nobots
{
    public class InputManager : DrawableGameComponent
    {
        public Character Character;
        Keys keyboardJump = Keys.Space;
        Keys keyboardChangeForm = Keys.Enter;
        Keys keyboardPush = Keys.LeftAlt;
        Keys keyboardAction = Keys.LeftControl;

        public InputManager(Game game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Character != null)
                processKeyboard();
            base.Update(gameTime);
        }

        protected KeyboardState previousKeyboardState;
        protected GamePadState previosGamepadState;
        protected void processKeyboard()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if (Game.IsActive && System.Windows.Forms.Form.ActiveForm != null && System.Windows.Forms.Form.ActiveForm.Text.Equals(Game.Window.Title))
            {
                if ((currentGamepadState.Buttons.A == ButtonState.Pressed && previosGamepadState.Buttons.A == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardJump) && previousKeyboardState.IsKeyUp(keyboardJump)))
                {
                    Character.AActionStart();
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Pressed && previosGamepadState.Buttons.B == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardPush) && previousKeyboardState.IsKeyUp(keyboardPush))))
                {
                    Character.BActionStart();
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Pressed && previosGamepadState.Buttons.X == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardAction) && previousKeyboardState.IsKeyUp(keyboardAction)))
                {
                    Character.XActionStart();
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Pressed && previosGamepadState.Buttons.Y == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardChangeForm) && previousKeyboardState.IsKeyUp(keyboardChangeForm)))
                {
                    Character.YActionStart();
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Pressed && previosGamepadState.DPad.Left == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left)))
                {
                    Character.LeftActionStart();
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Pressed && previosGamepadState.DPad.Right == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right)))
                {
                    Character.RightActionStart();
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Pressed && previosGamepadState.DPad.Up == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
                {
                    Character.UpActionStart();
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Pressed && previosGamepadState.DPad.Down == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down)))
                {
                    Character.DownActionStart();
                }
                if ((currentGamepadState.Buttons.A == ButtonState.Released && previosGamepadState.Buttons.A == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardJump) && previousKeyboardState.IsKeyDown(keyboardJump)))
                {
                    Character.AActionStop();
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Released && previosGamepadState.Buttons.B == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardPush) && previousKeyboardState.IsKeyDown(keyboardPush))))
                {
                    Character.BActionStop();
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Released && previosGamepadState.Buttons.X == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardAction) && previousKeyboardState.IsKeyDown(keyboardAction)))
                {
                    Character.XActionStop();
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Released && previosGamepadState.Buttons.Y == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardChangeForm) && previousKeyboardState.IsKeyDown(keyboardChangeForm)))
                {
                    Character.YActionStop();
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Released && previosGamepadState.DPad.Left == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(Keys.Left) && previousKeyboardState.IsKeyDown(Keys.Left)))
                {
                    Character.LeftActionStop();
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Released && previosGamepadState.DPad.Right == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(Keys.Right) && previousKeyboardState.IsKeyDown(Keys.Right)))
                {
                    Character.RightActionStop();
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Released && previosGamepadState.DPad.Up == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(Keys.Up) && previousKeyboardState.IsKeyDown(Keys.Up)))
                {
                    Character.UpActionStop();
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Released && previosGamepadState.DPad.Down == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(Keys.Down) && previousKeyboardState.IsKeyDown(Keys.Down)))
                {
                    Character.DownActionStop();
                }
                if ((currentGamepadState.Buttons.A == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardJump)))
                {
                    Character.AAction();
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardPush))))
                {
                    Character.BAction();
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardAction)))
                {
                    Character.XAction();
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardChangeForm)))
                {
                    Character.YAction();
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(Keys.Left)))
                {
                    Character.LeftAction();
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(Keys.Right)))
                {
                    Character.RightAction();
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(Keys.Up)))
                {
                    Character.UpAction();
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(Keys.Down)))
                {
                    Character.DownAction();
                }
            }

            previousKeyboardState = currentKeyboardState;
            previosGamepadState = currentGamepadState;
        }
    }
}
