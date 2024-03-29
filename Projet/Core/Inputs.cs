﻿using HypoluxAdventure.Models;
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

        public static int ScrollChange => (_currMouse.ScrollWheelValue - _prevMouse.ScrollWheelValue) / 120;
        public static int HorizontalScrollChange => (_currMouse.HorizontalScrollWheelValue - _prevMouse.HorizontalScrollWheelValue) / 120;

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

        #region Input Layout
        public static InputLayout InputLayout { get; private set; }

        public static void ChangeInputLayout(InputLayout inputLayout)
        {
            InputLayout = inputLayout;
        }

        public static InputLayout AZERTY { get; private set; } = new InputLayout()
        {
            Name = "AZERTY",
            NegX = Keys.Q,
            NegY = Keys.S,
            PosX = Keys.D,
            PosY = Keys.Z
        };

        public static InputLayout QWERTY { get; private set; } = new InputLayout()
        {
            Name = "QWERTY",
            NegX = Keys.A,
            NegY = Keys.S,
            PosX = Keys.D,
            PosY = Keys.W
        };

        public const string INPUT_FILE_PATH = "./input.txt";
        #endregion
    }
}
