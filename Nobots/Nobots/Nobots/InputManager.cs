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
        public IControllable Target;
        Keys keyboardJump = Keys.Space;
        Keys keyboardChangeForm = Keys.LeftControl;
        Keys keyboardPush = Keys.LeftAlt;
        Keys keyboardAction = Keys.LeftControl;

        public InputManager(Game game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Target != null)
                processKeyboard();
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
                    Target.AActionStart();
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Pressed && previosGamepadState.Buttons.B == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardPush) && previousKeyboardState.IsKeyUp(keyboardPush))))
                {
                    Target.BActionStart();
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Pressed && previosGamepadState.Buttons.X == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardAction) && previousKeyboardState.IsKeyUp(keyboardAction)))
                {
                    Target.XActionStart();
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Pressed && previosGamepadState.Buttons.Y == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardChangeForm) && previousKeyboardState.IsKeyUp(keyboardChangeForm)))
                {
                    Target.YActionStart();
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Pressed && previosGamepadState.DPad.Left == ButtonState.Released) ||
                    (currentGamepadState.ThumbSticks.Left.X < 0 && (previosGamepadState.ThumbSticks.Left.X >= 0)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left)))
                {
                    Target.LeftActionStart();
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Pressed && previosGamepadState.DPad.Right == ButtonState.Released) ||
                    (currentGamepadState.ThumbSticks.Left.X > 0 && (previosGamepadState.ThumbSticks.Left.X <= 0)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right)))
                {
                    Target.RightActionStart();
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Pressed && previosGamepadState.DPad.Up == ButtonState.Released) ||
                    (currentGamepadState.ThumbSticks.Left.Y > 0 && (previosGamepadState.ThumbSticks.Left.Y <= 0)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
                {
                    Target.UpActionStart();
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Pressed && previosGamepadState.DPad.Down == ButtonState.Released) ||
                    (currentGamepadState.ThumbSticks.Left.Y < 0 && (previosGamepadState.ThumbSticks.Left.Y >= 0)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down)))
                {
                    Target.DownActionStart();
                }
                if ((currentGamepadState.Buttons.A == ButtonState.Released && previosGamepadState.Buttons.A == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardJump) && previousKeyboardState.IsKeyDown(keyboardJump)))
                {
                    Target.AActionStop();
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Released && previosGamepadState.Buttons.B == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardPush) && previousKeyboardState.IsKeyDown(keyboardPush))))
                {
                    Target.BActionStop();
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Released && previosGamepadState.Buttons.X == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardAction) && previousKeyboardState.IsKeyDown(keyboardAction)))
                {
                    Target.XActionStop();
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Released && previosGamepadState.Buttons.Y == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardChangeForm) && previousKeyboardState.IsKeyDown(keyboardChangeForm)))
                {
                    Target.YActionStop();
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Released && previosGamepadState.DPad.Left == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.X == 0 && (previosGamepadState.ThumbSticks.Left.X < 0)) ||
                    (currentKeyboardState.IsKeyUp(Keys.Left) && previousKeyboardState.IsKeyDown(Keys.Left)))
                {
                    Target.LeftActionStop();
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Released && previosGamepadState.DPad.Right == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.X == 0 && (previosGamepadState.ThumbSticks.Left.X > 0)) ||
                    (currentKeyboardState.IsKeyUp(Keys.Right) && previousKeyboardState.IsKeyDown(Keys.Right)))
                {
                    Target.RightActionStop();
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Released && previosGamepadState.DPad.Up == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.Y == 0 && (previosGamepadState.ThumbSticks.Left.Y > 0)) ||
                    (currentKeyboardState.IsKeyUp(Keys.Up) && previousKeyboardState.IsKeyDown(Keys.Up)))
                {
                    Target.UpActionStop();
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Released && previosGamepadState.DPad.Down == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.Y == 0 && (previosGamepadState.ThumbSticks.Left.Y < 0)) ||
                    (currentKeyboardState.IsKeyUp(Keys.Down) && previousKeyboardState.IsKeyDown(Keys.Down)))
                {
                    Target.DownActionStop();
                }
                if ((currentGamepadState.Buttons.A == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardJump)))
                {
                    Target.AAction();
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardPush))))
                {
                    Target.BAction();
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardAction)))
                {
                    Target.XAction();
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardChangeForm)))
                {
                    Target.YAction();
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.X < 0) ||
                    (currentKeyboardState.IsKeyDown(Keys.Left)))
                {
                    Target.LeftAction();
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.X > 0) ||
                    (currentKeyboardState.IsKeyDown(Keys.Right)))
                {
                    Target.RightAction();
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.Y > 0) ||
                    (currentKeyboardState.IsKeyDown(Keys.Up)))
                {
                    Target.UpAction();
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.Y < 0) ||
                    (currentKeyboardState.IsKeyDown(Keys.Down)))
                {
                    Target.DownAction();
                }
            }

            previousKeyboardState = currentKeyboardState;
            previosGamepadState = currentGamepadState;
        }
    }
}
