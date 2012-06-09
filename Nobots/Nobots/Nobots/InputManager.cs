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
        Keys keyboardA = Keys.Space;
        Keys keyboardY = Keys.LeftControl;
        Keys keyboardB = Keys.LeftAlt;
        Keys keyboardX = Keys.RightControl;

        public InputManager(Game game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Target != null)
                processKeyboard();
        }

        float threshold = 0.95f;
        protected KeyboardState previousKeyboardState;
        protected GamePadState previosGamepadState;
        protected void processKeyboard()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if (Game.IsActive && System.Windows.Forms.Form.ActiveForm != null && System.Windows.Forms.Form.ActiveForm.Text.Equals(Game.Window.Title))
            {
                if ((currentGamepadState.Buttons.A == ButtonState.Pressed && previosGamepadState.Buttons.A == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardA) && previousKeyboardState.IsKeyUp(keyboardA)))
                {
                    Target.AActionStart();
#if DEBUG_INPUT
                    Console.WriteLine("AActionStart");
#endif
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Pressed && previosGamepadState.Buttons.B == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardB) && previousKeyboardState.IsKeyUp(keyboardB))))
                {
                    Target.BActionStart();
#if DEBUG_INPUT
                    Console.WriteLine("BActionStart");
#endif
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Pressed && previosGamepadState.Buttons.X == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardX) && previousKeyboardState.IsKeyUp(keyboardX)))
                {
                    Target.XActionStart();
#if DEBUG_INPUT
                    Console.WriteLine("XActionStart");
#endif
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Pressed && previosGamepadState.Buttons.Y == ButtonState.Released) ||
                    (currentKeyboardState.IsKeyDown(keyboardY) && previousKeyboardState.IsKeyUp(keyboardY)))
                {
                    Target.YActionStart();
#if DEBUG_INPUT
                    Console.WriteLine("YActionStart");
#endif
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Pressed && previosGamepadState.DPad.Left == ButtonState.Released) ||
                    (currentGamepadState.ThumbSticks.Left.X < -threshold && (previosGamepadState.ThumbSticks.Left.X >= -threshold)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left)))
                {
                    Target.LeftActionStart();
#if DEBUG_INPUT
                    Console.WriteLine("LeftActionStart");
#endif
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Pressed && previosGamepadState.DPad.Right == ButtonState.Released) ||
                    (currentGamepadState.ThumbSticks.Left.X > threshold && (previosGamepadState.ThumbSticks.Left.X <= threshold)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right)))
                {
                    Target.RightActionStart();
#if DEBUG_INPUT
                    Console.WriteLine("RightActionStart");
#endif
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Pressed && previosGamepadState.DPad.Up == ButtonState.Released) ||
                    (currentGamepadState.ThumbSticks.Left.Y > threshold && (previosGamepadState.ThumbSticks.Left.Y <= threshold)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
                {
                    Target.UpActionStart();
#if DEBUG_INPUT
                    Console.WriteLine("UpActionStart");
#endif
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Pressed && previosGamepadState.DPad.Down == ButtonState.Released) ||
                    (currentGamepadState.ThumbSticks.Left.Y < -threshold && (previosGamepadState.ThumbSticks.Left.Y >= -threshold)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down)))
                {
                    Target.DownActionStart();
#if DEBUG_INPUT
                    Console.WriteLine("DownActionStart");
#endif
                }
                if ((currentGamepadState.Buttons.A == ButtonState.Released && previosGamepadState.Buttons.A == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardA) && previousKeyboardState.IsKeyDown(keyboardA)))
                {
                    Target.AActionStop();
#if DEBUG_INPUT
                    Console.WriteLine("AActionStop");
#endif
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Released && previosGamepadState.Buttons.B == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardB) && previousKeyboardState.IsKeyDown(keyboardB))))
                {
                    Target.BActionStop();
#if DEBUG_INPUT
                    Console.WriteLine("BActionStop");
#endif
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Released && previosGamepadState.Buttons.X == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardX) && previousKeyboardState.IsKeyDown(keyboardX)))
                {
                    Target.XActionStop();
#if DEBUG_INPUT
                    Console.WriteLine("XActionStop");
#endif
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Released && previosGamepadState.Buttons.Y == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyUp(keyboardY) && previousKeyboardState.IsKeyDown(keyboardY)))
                {
                    Target.YActionStop();
#if DEBUG_INPUT
                    Console.WriteLine("YActionStop");
#endif
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Released && previosGamepadState.DPad.Left == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.X > -threshold && (previosGamepadState.ThumbSticks.Left.X < -threshold)) ||
                    (currentKeyboardState.IsKeyUp(Keys.Left) && previousKeyboardState.IsKeyDown(Keys.Left)))
                {
                    Target.LeftActionStop();
#if DEBUG_INPUT
                    Console.WriteLine("LeftActionStop");
#endif
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Released && previosGamepadState.DPad.Right == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.X < threshold && (previosGamepadState.ThumbSticks.Left.X > threshold)) ||
                    (currentKeyboardState.IsKeyUp(Keys.Right) && previousKeyboardState.IsKeyDown(Keys.Right)))
                {
                    Target.RightActionStop();
#if DEBUG_INPUT
                    Console.WriteLine("RightActionStop");
#endif
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Released && previosGamepadState.DPad.Up == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.Y < threshold && (previosGamepadState.ThumbSticks.Left.Y > threshold)) ||
                    (currentKeyboardState.IsKeyUp(Keys.Up) && previousKeyboardState.IsKeyDown(Keys.Up)))
                {
                    Target.UpActionStop();
#if DEBUG_INPUT
                    Console.WriteLine("UpActionStop");
#endif
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Released && previosGamepadState.DPad.Down == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.Y > -threshold && (previosGamepadState.ThumbSticks.Left.Y < -threshold)) ||
                    (currentKeyboardState.IsKeyUp(Keys.Down) && previousKeyboardState.IsKeyDown(Keys.Down)))
                {
                    Target.DownActionStop();
#if DEBUG_INPUT
                    Console.WriteLine("DownActionStop");
#endif
                }
                if ((currentGamepadState.Buttons.A == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardA)))
                {
                    Target.AAction();
#if DEBUG_INPUT
                    Console.WriteLine("AAction");
#endif
                }
                if (((currentGamepadState.Buttons.B == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardB))))
                {
                    Target.BAction();
#if DEBUG_INPUT
                    Console.WriteLine("BAction");
#endif
                }
                if ((currentGamepadState.Buttons.X == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardX)))
                {
                    Target.XAction();
#if DEBUG_INPUT
                    Console.WriteLine("XAction");
#endif
                }
                if ((currentGamepadState.Buttons.Y == ButtonState.Pressed) ||
                    (currentKeyboardState.IsKeyDown(keyboardY)))
                {
                    Target.YAction();
#if DEBUG_INPUT
                    Console.WriteLine("YAction");
#endif
                }
                if ((currentGamepadState.DPad.Left == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.X < -threshold) ||
                    (currentKeyboardState.IsKeyDown(Keys.Left)))
                {
                    Target.LeftAction();
#if DEBUG_INPUT
                    Console.WriteLine("LeftAction");
#endif
                }
                if ((currentGamepadState.DPad.Right == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.X > threshold) ||
                    (currentKeyboardState.IsKeyDown(Keys.Right)))
                {
                    Target.RightAction();
#if DEBUG_INPUT
                    Console.WriteLine("RightAction");
#endif
                }
                if ((currentGamepadState.DPad.Up == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.Y > threshold) ||
                    (currentKeyboardState.IsKeyDown(Keys.Up)))
                {
                    Target.UpAction();
#if DEBUG_INPUT
                    Console.WriteLine("UpAction");
#endif
                }
                if ((currentGamepadState.DPad.Down == ButtonState.Pressed) ||
                    (currentGamepadState.ThumbSticks.Left.Y < -threshold) ||
                    (currentKeyboardState.IsKeyDown(Keys.Down)))
                {
                    Target.DownAction();
#if DEBUG_INPUT
                    Console.WriteLine("DownAction");
#endif
                }
            }

            previousKeyboardState = currentKeyboardState;
            previosGamepadState = currentGamepadState;
        }
    }
}
