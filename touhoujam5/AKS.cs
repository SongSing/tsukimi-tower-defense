using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace touhoujam5
{
    class AKS
    {
        public enum KeyState
        {
            JustReleased = -1,
            Up = 0,
            JustPressed = 1,
            Down = 2,
        }

        private static Dictionary<Keyboard.Key, KeyState> lastFrameKeys = new Dictionary<Keyboard.Key, KeyState>();
        private static Dictionary<Keyboard.Key, KeyState> keys = new Dictionary<Keyboard.Key, KeyState>();
        private static List<Keyboard.Key> watchingKeys = new List<Keyboard.Key>();
        private static Dictionary<Keyboard.Key, bool> keysDown = new Dictionary<Keyboard.Key, bool>();

        private static Dictionary<Mouse.Button, KeyState> lastFrameMouse = new Dictionary<Mouse.Button, KeyState>();
        private static Dictionary<Mouse.Button, KeyState> mouse = new Dictionary<Mouse.Button, KeyState>();
        private static Dictionary<Mouse.Button, bool> mouseButtonsDown = new Dictionary<Mouse.Button, bool>();

        public static void InitializeWindow(Window w)
        {
            w.KeyPressed += (sender, e) =>
            {
                keysDown[e.Code] = true;
            };

            w.KeyReleased += (sender, e) =>
            {
                keysDown[e.Code] = false;
            };

            mouse[Mouse.Button.Left] = KeyState.Up;
            mouse[Mouse.Button.Right] = KeyState.Up;
            mouse[Mouse.Button.Middle] = KeyState.Up;
        }

        public static bool IsKeyDown(Keyboard.Key key)
        {
            keysDown.TryGetValue(key, out bool ret); // will put false if key doesnt exist
            return ret;
        }

        public static bool IsKeyUp(Keyboard.Key key)
        {
            return keysDown.TryGetValue(key, out bool ret) ? ret : true;
        }

        public static bool IsMouseDown(Mouse.Button button)
        {
            mouseButtonsDown.TryGetValue(button, out bool ret);
            return ret;
        }

        public static bool isMouseUp(Mouse.Button button)
        {
            return mouseButtonsDown.TryGetValue(button, out bool ret) ? ret : true;
        }

        public static bool IsAnyKeyDown(Keyboard.Key[] keys)
        {
            foreach (Keyboard.Key key in keys)
            {
                if (IsKeyDown(key))
                {
                    return true;
                }
            }

            return false;
        }

        public static void WatchKey(Keyboard.Key key)
        {
            watchingKeys.Add(key);
            keys[key] = IsKeyDown(key) ? KeyState.Down : KeyState.Up;
            lastFrameKeys[key] = IsKeyDown(key) ? KeyState.Down : KeyState.Up;
        }

        public static void Update()
        {
            foreach (var key in watchingKeys)
            {
                lastFrameKeys[key] = keys[key];
                keys[key] = IsKeyDown(key) ? KeyState.Down : KeyState.Up;

                if (keys[key] == KeyState.Up && (int)lastFrameKeys[key] >= 1)
                {
                    keys[key] = KeyState.JustReleased;
                }
                else if (keys[key] == KeyState.Down && (int)lastFrameKeys[key] <= 0)
                {
                    keys[key] = KeyState.JustPressed;
                }
            }

            UpdateMouseButton(Mouse.Button.Left);
            UpdateMouseButton(Mouse.Button.Right);
            UpdateMouseButton(Mouse.Button.Middle);
        }

        private static void UpdateMouseButton(Mouse.Button button)
        {
            bool isDown = Mouse.IsButtonPressed(button);
            mouseButtonsDown[button] = isDown;

            KeyState last;

            if (mouse.ContainsKey(button))
            {
                last = mouse[button];
            }
            else
            {
                last = KeyState.Up;
            }

            lastFrameMouse[button] = last;
            mouse[button] = IsMouseDown(button) ? KeyState.Down : KeyState.Up;

            if (mouse[button] == KeyState.Up && (int)lastFrameMouse[button] >= 1)
            {
                mouse[button] = KeyState.JustReleased;
            }
            else if (mouse[button] == KeyState.Down && (int)lastFrameMouse[button] <= 0)
            {
                mouse[button] = KeyState.JustPressed;
            }
        }

        public static KeyState GetKeyState(Keyboard.Key key)
        {
            if (watchingKeys.Contains(key))
            {
                return keys[key];
            }
            else
            {
                throw new Exception("Not watching for key: " + key.ToString());
            }
        }

        public static KeyState GetMouseState(Mouse.Button button)
        {
            return mouse[button];
        }

        public static bool WasJustPressed(Keyboard.Key key)
        {
            return GetKeyState(key) == KeyState.JustPressed;
        }

        public static bool WasJustPressed(Mouse.Button button)
        {
            return GetMouseState(button) == KeyState.JustPressed;
        }

        public static int Axis(Keyboard.Key negative, Keyboard.Key positive)
        {
            return Convert.ToInt32(IsKeyDown(positive)) - Convert.ToInt32(IsKeyDown(negative));
        }
    }
}
