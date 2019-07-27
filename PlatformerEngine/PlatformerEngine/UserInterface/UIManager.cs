using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// manages ui updates and drawing
    /// </summary>
    public class UIManager
    {
        /// <summary>
        /// asset manager for ui elements to refer to
        /// </summary>
        public AssetManager Assets;
        /// <summary>
        /// dictionary to find elements based on their name
        /// </summary>
        public Dictionary<string, UIElement> Elements;
        /// <summary>
        /// the current input we are typing to
        /// </summary>
        public IInputable CurrentInput;
        /// <summary>
        /// multiplier for scrolling
        /// </summary>
        public float ScrollMultiplier;
        /// <summary>
        /// the previous mouse state
        /// </summary>
        public MouseState PreviousMouseState;
        /// <summary>
        /// the current mouse state
        /// </summary>
        public MouseState MouseState;
        /// <summary>
        /// the top ui group element that we draw and update to
        /// </summary>
        public GroupElement TopUINode;
        /// <summary>
        /// the current keyboard state
        /// </summary>
        public KeyboardState KeyboardState;
        /// <summary>
        /// the previous keyboard state
        /// </summary>
        public KeyboardState PreviousKeyboardState;
        /// <summary>
        /// if the shift key is being pressed down and we need to move to the other set of key input characters
        /// </summary>
        public bool InputShifted;
        /// <summary>
        /// dictionary of keys to chars
        /// </summary>
        public Dictionary<Keys, char> KeyToCharMap;
        /// <summary>
        /// dictionary of keyst to chars but the shifted version of them
        /// </summary>
        public Dictionary<Keys, char> KeyToShiftedCharMap;
        /// <summary>
        /// a reference to the parent game
        /// </summary>
        public Game Game;
        /// <summary>
        /// sort mode for drawing the ui
        /// </summary>
        public SpriteSortMode SortMode;
        private Keys[] lastPressedKeys;
        private int lastScrollAmount;
        /// <summary>
        /// creates a new ui manager
        /// </summary>
        /// <param name="game">the parent game to reference to</param>
        /// <param name="assetManager">the asset manager for the ui elements to reference to</param>
        public UIManager(Game game, AssetManager assetManager)
        {
            Game = game;
            SortMode = SpriteSortMode.FrontToBack;
            Assets = assetManager;
            Elements = new Dictionary<string, UIElement>();
            CurrentInput = null;
            ScrollMultiplier = -16;
            lastPressedKeys = new Keys[0];
            lastScrollAmount = 0;
            KeyToCharMap = new Dictionary<Keys, char>();
            KeyToShiftedCharMap = new Dictionary<Keys, char>();
            InputShifted = false;
            TopUINode = new GroupElement(this, new Vector2(0, 0), new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), 0f, "top");
            AddKeyToChar(Keys.D0, '0', ')'); //lmk if there is a better way to go about adding these characters
            AddKeyToChar(Keys.D1, '1', '!');
            AddKeyToChar(Keys.D2, '2', '@');
            AddKeyToChar(Keys.D3, '3', '#');
            AddKeyToChar(Keys.D4, '4', '$');
            AddKeyToChar(Keys.D5, '5', '%');
            AddKeyToChar(Keys.D6, '6', '^');
            AddKeyToChar(Keys.D7, '7', '&');
            AddKeyToChar(Keys.D8, '8', '*');
            AddKeyToChar(Keys.D9, '9', '(');
            AddKeyToChar(Keys.OemPipe, '\\', '|');
            AddKeyToChar(Keys.OemQuestion, '/', '?');
            AddKeyToChar(Keys.OemMinus, '-', '_');
            AddKeyToChar(Keys.OemPlus, '=', '+');
            AddKeyToChar(Keys.OemComma, ',', '<');
            AddKeyToChar(Keys.OemPeriod, '.', '>');
            AddKeyToChar(Keys.OemQuotes, '\'', '"');
            AddKeyToChar(Keys.OemSemicolon, ';', ':');
            AddKeyToChar(Keys.OemOpenBrackets, '[', '{');
            AddKeyToChar(Keys.OemCloseBrackets, ']', '}');
            AddKeyToChar(Keys.OemTilde, '`', '~');
            AddKeyToChar(Keys.A, 'a', 'A');
            AddKeyToChar(Keys.B, 'b', 'B');
            AddKeyToChar(Keys.C, 'c', 'C');
            AddKeyToChar(Keys.D, 'd', 'D');
            AddKeyToChar(Keys.E, 'e', 'E');
            AddKeyToChar(Keys.F, 'f', 'F');
            AddKeyToChar(Keys.G, 'g', 'G');
            AddKeyToChar(Keys.H, 'h', 'H');
            AddKeyToChar(Keys.I, 'i', 'I');
            AddKeyToChar(Keys.J, 'j', 'J');
            AddKeyToChar(Keys.K, 'k', 'K');
            AddKeyToChar(Keys.L, 'l', 'L');
            AddKeyToChar(Keys.M, 'm', 'M');
            AddKeyToChar(Keys.N, 'n', 'N');
            AddKeyToChar(Keys.O, 'o', 'O');
            AddKeyToChar(Keys.P, 'p', 'P');
            AddKeyToChar(Keys.Q, 'q', 'Q');
            AddKeyToChar(Keys.R, 'r', 'R');
            AddKeyToChar(Keys.S, 's', 'S');
            AddKeyToChar(Keys.T, 't', 'T');
            AddKeyToChar(Keys.U, 'u', 'U');
            AddKeyToChar(Keys.V, 'v', 'V');
            AddKeyToChar(Keys.W, 'w', 'W');
            AddKeyToChar(Keys.X, 'x', 'X');
            AddKeyToChar(Keys.Y, 'y', 'Y');
            AddKeyToChar(Keys.Z, 'z', 'Z');
        }
        /// <summary>
        /// gets a ui element given its name
        /// </summary>
        /// <param name="name">the name to look for</param>
        /// <returns>the ui element with the given name, if we found it</returns>
        public UIElement GetUIElement(string name)
        {
            if (Elements.ContainsKey(name))
            {
                return Elements[name];
            }
            return null;
        }
        /// <summary>
        /// destroys the given ui element
        /// </summary>
        /// <param name="element">the element to remove</param>
        public void DestroyUIElement(UIElement element)
        {
            string victim = null;
            foreach (KeyValuePair<string, UIElement> pair in Elements)
            {
                if (pair.Value == element)
                {
                    victim = pair.Key;
                    break;
                }
            }
            if (victim != null)
            {
                Elements.Remove(victim);
            }
        }
        /// <summary>
        /// defines a key's corresponding text input characters
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="normal">normal character</param>
        /// <param name="shift">shifted character</param>
        public void AddKeyToChar(Keys key, char normal, char shift)
        {
            KeyToCharMap[key] = normal;
            KeyToShiftedCharMap[key] = shift;
        }
        /// <summary>
        /// converts a key to a char given if we're pressing shift and the key to char dictionaries
        /// </summary>
        /// <param name="key">the key to convert</param>
        /// <param name="shifted">if we're shifting it</param>
        /// <returns>the corresponding key if found, ' ' char if no</returns>
        public char KeyToChar(Keys key, bool shifted) //there oughta be a better way to do this
        {
            if (shifted)
            {
                if (KeyToShiftedCharMap.ContainsKey(key))
                {
                    return KeyToShiftedCharMap[key];
                }
            }
            else
            {
                if (KeyToCharMap.ContainsKey(key))
                {
                    return KeyToCharMap[key];
                }
            }
            return ' ';
        }
        /// <summary>
        /// finds the differences that array A has that array B does not have
        /// (general function, may want to move)
        /// </summary>
        /// <typeparam name="T">type of object for the array</typeparam>
        /// <param name="a">first array</param>
        /// <param name="b">second array</param>
        /// <returns>differences that A has that B doesn't</returns>
        public List<T> FindChanges<T>(T[] a, T[] b)
        {
            List<T> changes = new List<T>();
            for (int i = 0; i < a.Length; i++)
            {
                T itemA = a[i];
                bool foundItem = false;
                for (int j = 0; j < b.Length; j++)
                {
                    T itemB = b[j];
                    if (itemA.Equals(itemB))
                    {
                        foundItem = true;
                        break;
                    }
                }
                if (!foundItem)
                {
                    changes.Add(itemA);
                }
            }
            return changes;
        }
        /// <summary>
        /// updates the for input and updates the ui elements
        /// </summary>
        public void Update()
        {
            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || KeyboardState.IsKeyDown(Keys.Escape))
                Game.Exit();
            TopUINode.Update();
            //mouse clicks
            Vector2 mousePos = new Vector2(MouseState.X, MouseState.Y);
            if ((MouseState.LeftPressed() && !PreviousMouseState.LeftPressed()) || (MouseState.RightPressed() && !PreviousMouseState.RightPressed()) || (MouseState.MiddlePressed() && !PreviousMouseState.MiddlePressed()))
            {
                CurrentInput = null;
                TopUINode.MousePressed(MouseState, new Vector2(0, 0));
            }
            if ((!MouseState.LeftPressed() && PreviousMouseState.LeftPressed()) || (!MouseState.RightPressed() && PreviousMouseState.RightPressed()) || (!MouseState.MiddlePressed() && PreviousMouseState.MiddlePressed()))
            {
                TopUINode.MouseReleased(MouseState, new Vector2(0, 0));
            }
            //scrolling
            int scrollAmount = MouseState.ScrollWheelValue;
            float scrollValue = Math.Sign(lastScrollAmount - scrollAmount) * ScrollMultiplier;
            if (scrollValue != 0)
            {
                TopUINode.Scroll(MouseState, scrollValue);
            }
            //text input
            Keys[] pressedKeys = KeyboardState.GetPressedKeys();
            List<Keys> newKeys = FindChanges(pressedKeys, lastPressedKeys);
            List<Keys> releasedKeys = FindChanges(lastPressedKeys, pressedKeys);
            if (newKeys.Contains(Keys.LeftShift) || newKeys.Contains(Keys.RightShift))
            {
                InputShifted = true;
            }
            else if (!releasedKeys.Contains(Keys.LeftShift) && !releasedKeys.Contains(Keys.RightShift))
            {
                InputShifted = false;
            }
            lastPressedKeys = pressedKeys;
            if (CurrentInput != null)
            {
                for (int i = 0; i < newKeys.Count; i++)
                {
                    Keys key = newKeys[i];
                    char keyChar = KeyToChar(key, InputShifted);
                    char[] validKeys = CurrentInput.ValidKeys;
                    bool isValid = false;
                    for (int j = 0; j < validKeys.Length; j++)
                    {
                        if (validKeys[j].Equals(keyChar))
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (isValid)
                    {
                        CurrentInput.Text = CurrentInput.Text + keyChar;
                    }
                    else
                    {
                        if (key.Equals(Keys.Back))
                        {
                            if (CurrentInput.Text.Length > 0)
                            {
                                CurrentInput.Text = CurrentInput.Text.Substring(0, CurrentInput.Text.Length - 1);
                            }
                        }
                    }
                }
            }

            PreviousMouseState = MouseState;
            PreviousKeyboardState = KeyboardState;
            lastScrollAmount = scrollAmount;
        }
        /// <summary>
        /// draws the top group element
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SortMode);
            TopUINode.Draw(spriteBatch, new Vector2(0, 0));
            spriteBatch.End();
        }
    }
}
