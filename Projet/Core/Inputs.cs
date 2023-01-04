using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HypoluxAdventure.Core
{
    internal static class Inputs
    {
        private static bool _firstFrame = true;

        public static void Update()
        {
            if (_firstFrame)
            {
                _prevKeyboard = Keyboard.GetState();
                _prevMouse = Mouse.GetState();
                _firstFrame = false;
            }
            else
            {
                _prevKeyboard = _currKeyboard;
                _prevMouse = _currMouse;
            }

            _currKeyboard = Keyboard.GetState();
            _currMouse = Mouse.GetState();
        }

        #region Keyboard
        private static KeyboardState _prevKeyboard;
        private static KeyboardState _currKeyboard;

        public static bool IsKeyDown(Keys key) => _currKeyboard.IsKeyDown(key);
        public static bool IsKeyUp(Keys key) => _currKeyboard.IsKeyUp(key);
        public static bool IsKeyPressed(Keys key) => !_prevKeyboard.IsKeyDown(key) && _currKeyboard.IsKeyDown(key);
        public static bool IsKeyReleased(Keys key) => _prevKeyboard.IsKeyDown(key) && !_currKeyboard.IsKeyDown(key);
        #endregion

        #region Mouse
        public enum MouseButton { Left, Middle, Right, XButton1, XButton2 }
        
        private static MouseState _prevMouse;
        private static MouseState _currMouse;

        public static Vector2 MousePosition => _currMouse.Position.ToVector2();
        public static int ScrollWheelValue => _currMouse.ScrollWheelValue;
        public static int HorizontalScrollWheelValue => _currMouse.HorizontalScrollWheelValue;

        private static bool IsClickDown(MouseState mouseState, MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => mouseState.LeftButton,
                MouseButton.Middle => mouseState.MiddleButton,
                MouseButton.Right => mouseState.RightButton,
                MouseButton.XButton1 => mouseState.XButton1,
                _ => mouseState.XButton2
            } == ButtonState.Pressed;
        }

        public static bool IsClickDown(MouseButton button) => IsClickDown(_currMouse, button);
        public static bool IsClickUp(MouseButton button) => !IsClickDown(_currMouse, button);

        public static bool IsClickPressed(MouseButton button) =>
            !IsClickDown(_prevMouse, button) && IsClickDown(_currMouse, button);

        public static bool IsClickReleased(MouseButton button) =>
            IsClickDown(_prevMouse, button) && !IsClickDown(_currMouse, button);
        #endregion
    }
}
