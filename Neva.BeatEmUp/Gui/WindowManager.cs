using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Gui.Cursor;
using Neva.BeatEmUp.Gui.Cursor.Args;
using Neva.BeatEmUp.Gui.Cursor.Components;
using Neva.BeatEmUp.Gui.Transitions;

namespace Neva.BeatEmUp.Gui
{
    internal sealed class WindowManager : DrawableGameComponent
    {
        #region Vars
        private readonly WindowGroup windowGroup;
        private SpriteBatch spriteBatch;

        private GuiCursor cursor;
        private Transition transition;
        private Window topmost;
        private Window next;
        private Sprite background;

        private bool visible;
        #endregion

        #region Properties
        public Sprite Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
            }
        }
        #endregion

        #region Events
        public event GuiWindowManagerEventHandler<GuiWindowChangingEventArgs> WindowChanging;
        #endregion

        public WindowManager(Game game)
            : base(game)
        {
            this.windowGroup = new WindowGroup();

            WindowChanging += delegate { };

            visible = true;
        }
        public WindowManager(Game game, Dictionary<string, Window> windows)
            : this(game)
        {
            this.windowGroup = new WindowGroup(windows);
        }
        public WindowManager(Game game, List<Window> windows)
            : this(game)
        {
            this.windowGroup = new WindowGroup(windows);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        private void CreateCursor(Window window)
        {
            CursorBehaviour behaviour = null;
#if WINDOWS || LINUX
            behaviour = new FreeCursorBehaviour(window, new Vector2(0f, 0f), new Vector2(1f, 1f));
#elif ANDROID || IOS || WINDOWSPHONE
            // TODO: aseta touch cursor.
#endif

            cursor = new GuiCursor(behaviour, new EmptyRenderer());
        }
        private void UpdateCursor(GameTime gameTime)
        {
            if (cursor != null)
            {
                MouseState mouseState = Mouse.GetState();

                cursor.MoveTo(new Vector2(mouseState.X, mouseState.Y));
                cursor.Update(gameTime);

                cursor.Click(new CursorInputArgs(mouseState));
            }
        }

        private void InternalChange(Window window)
        {
            GuiWindowChangingEventArgs e = new GuiWindowChangingEventArgs(topmost, window);

            WindowChanging(e, this);

            if (e.Cancel)
            {
                transition = null;
                return;
            }
            else if (transition == null)
            {
                if (topmost != null)
                {
                    topmost.Disable();
                }

                topmost = window;

                topmost.Enable();
            }
            else
            {
                next = window;

                transition.Next = next;
                transition.Current = topmost;

                transition.Start();
            }

            CreateCursor(window);
        }
        private void HandleTopMost(string name = "", Window window = null)
        {
            if (topmost == null)
            {
                return;
            }

            if (window != null && ReferenceEquals(window, topmost))
            {
                topmost = null;
            }
            else if (name != string.Empty && topmost.Name == name)
            {
                topmost = null;
            }
        }
        private void DrawBackground()
        {
            if (background != null)
            {
                spriteBatch.Begin();

                background.Draw(spriteBatch);

                spriteBatch.End();
            }
        }

        public void AddWindow(string name, Window window)
        {
            window.Disable();

            windowGroup.AddWindow(name, window);
        }
        public void AddWindow(Window window)
        {
            window.Disable();

            windowGroup.AddWindow(window);
        }
        public bool RemoveWindow(string name)
        {
            HandleTopMost(name);

            return windowGroup.RemoveWindow(name);
        }
        public bool RemoveWindow(Window window)
        {
            HandleTopMost("", window);

            return windowGroup.RemoveWindow(window);
        }

        public Window GetWindow(string name)
        {
            return windowGroup.GetWindow(name);
        }

        public void MoveToFront(string name, Transition transition = null)
        {
            Window window = windowGroup.GetWindow(name);

            if (window != null)
            {
                this.transition = transition;

                InternalChange(window);
            }
        }
        public void MoveToFront(Window window, Transition transition = null)
        {
            if (windowGroup.ContainsWindow(window))
            {
                this.transition = transition;

                InternalChange(window);
            }
        }

        public void Hide(bool fadeOut)
        {
            visible = false;
        }
        public void Show(bool fadeIn)
        {
            visible = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!visible)
            {
                return;
            }

            if (transition != null)
            {
                transition.Update(gameTime);

                if (transition.Finished)
                {
                    topmost.Disable();
                    next.Enable();

                    topmost = next;

                    next = null;
                    transition = null;
                }
            }

            if (topmost != null)
            {
                topmost.Update(gameTime);
                UpdateCursor(gameTime);
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (!visible || spriteBatch == null)
            {
                return;
            }

            GraphicsDevice.Clear(Color.Black);

            DrawBackground();

            if (transition != null)
            {
                spriteBatch.Begin();

                transition.Draw(spriteBatch);

                spriteBatch.End();
            }
            else if (topmost != null)
            {
                spriteBatch.Begin();

                topmost.Draw(spriteBatch);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
