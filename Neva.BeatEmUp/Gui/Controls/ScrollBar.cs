using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Gui.Controls.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.Gui.Controls.Components;

namespace Neva.BeatEmUp.Gui.Controls
{
    internal sealed class ScrollBar : Control, IContainer
    {
        #region Event keys
        private readonly object EventScrollValueChanged = new object();
        private readonly object EventMaxValueChanged = new object();
        #endregion

        #region Vars
        private readonly ChildCollection childs;

        private ScrollThumb thumb;
        private Button backScrollButton;
        private Button forwardScrollButton;
        private ScrollbarType type;

        private int value;
        private int maxValue;
        #endregion

        #region Properties
        /// <summary>
        /// Palauttaa minkä tyyppinen scrollbar on.
        /// </summary>
        public ScrollbarType Type
        {
            get
            {
                return type;
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa buttonien brushit.
        /// </summary>
        public Brush ButtonBrush
        {
            get
            {
                CheckDefaultButtons();

                return backScrollButton.Brush;
            }
            set
            {
                CheckDefaultButtons();

                backScrollButton.Brush = value;
                forwardScrollButton.Brush = value;
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa thumbin brushin.
        /// </summary>
        public Brush ThumbBrush
        {
            get
            {
                return thumb.Brush;
            }
            set
            {
                thumb.Brush = value;
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa scroll valuen.
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    UpdateValue(value);
                }
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa maksimi scroll valuen.
        /// </summary>
        public int MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                if (maxValue != value)
                {
                    int oldValue = maxValue;
                    maxValue = value;

                    OnMaxValueChanged(new GuiScrollEventArgs(maxValue, oldValue), this);
                }
            }
        }
        /// <summary>
        /// Palauttaa minimi scroll valuen.
        /// </summary>
        public int MinValue
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// Palauttaa montako childiä controllilla on.
        /// </summary>
        public int ChildsCount
        {
            get
            {
                return childs.ChildsCount;
            }
        }
        /// <summary>
        /// Palauttaa käyttääkö scrollbar default buttoneita.
        /// </summary>
        public bool HasDefaultButtons
        {
            get
            {
                return backScrollButton != null && forwardScrollButton != null;
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa thumbin näkyvyyden.
        /// </summary>
        public bool ShowThumb
        {
            get
            {
                return thumb.Visible;
            }
            set
            {
                if (value)
                {
                    thumb.Show();
                }
                else
                {
                    thumb.Hide();
                }
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa default buttonien näkyvyyden.
        /// </summary>
        public bool ShowDefaultButtons
        {
            get
            {
                CheckDefaultButtons();

                return backScrollButton.Visible;
            }
            set
            {
                CheckDefaultButtons();

                if (value)
                {
                    backScrollButton.Show();
                    forwardScrollButton.Show();
                }
                else
                {
                    backScrollButton.Hide();
                    backScrollButton.Hide();
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Laukaistaan kun scroll value muuttuu.
        /// </summary>
        public event GuiEventHandler<GuiScrollEventArgs> ScrollValueChanged
        {
            add
            {
                eventHandlers.AddHandler(EventScrollValueChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventScrollValueChanged, value);
            }
        }
        /// <summary>
        /// Laukaistaan ku max value muuttuu.
        /// </summary>
        public event GuiEventHandler<GuiScrollEventArgs> MaxValueChanged
        {
            add
            {
                eventHandlers.AddHandler(EventMaxValueChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventMaxValueChanged, value);
            }
        }
        /// <summary>
        /// Asettaa molemmille scroll buttoneille annetun keyboard input eventin.
        /// </summary>
        public event GuiEventHandler<GuiKeyboardInputEventArgs> ButtonsKeyboardInput
        {
            add
            {
                CheckDefaultButtons();

                backScrollButton.KeyboardInput += value;
                forwardScrollButton.KeyboardInput += value;
            }
            remove
            {
                CheckDefaultButtons();

                backScrollButton.KeyboardInput -= value;
                forwardScrollButton.KeyboardInput -= value;
            }
        }
        /// <summary>
        /// Asettaa molemmille scroll buttoneille annetun mouse input eventin.
        /// </summary>
        public event GuiEventHandler<GuiCursorInputEventArgs> ButtonsMouseInput
        {
            add
            {
                CheckDefaultButtons();

                backScrollButton.MouseInput += value;
                forwardScrollButton.MouseInput += value;
            }
            remove
            {
                CheckDefaultButtons();

                backScrollButton.MouseInput -= value;
                forwardScrollButton.MouseInput -= value;
            }
        }
        /// <summary>
        /// Asettaa molemmille scroll buttoneille annetun game pad input eventin.
        /// </summary>
        public event GuiEventHandler<GuiGamePadInputEventArgs> ButtonsGamePadInput
        {
            add
            {
                CheckDefaultButtons();

                backScrollButton.GamePadInput += value;
                forwardScrollButton.GamePadInput += value;
            }
            remove
            {
                CheckDefaultButtons();

                backScrollButton.GamePadInput -= value;
                forwardScrollButton.GamePadInput -= value;
            }
        }
        #endregion

        public ScrollBar(Microsoft.Xna.Framework.Game game, ScrollbarType type)
            : base(game)
        {
            this.type = type;

            childs = new ChildCollection(this);
            renderer = new BasicScrollbarRenderer(game, this);
            focusable = false;
            value = 0;

            thumb = new ScrollThumb(game, type);
            childs.AddChild(thumb);

            thumb.SetParent(this);
        }

        private void RemoveDisposedControls()
        {
            List<Control> disposedControls = childs.RemoveDisposedControls();

            if (disposedControls.Count == 0)
            {
                return;
            }

            for (int i = 0; i < disposedControls.Count; i++)
            {
                OnLayoutChanged(new GuiLayoutEventArgs(disposedControls[i]), this);
            }

            UpdateLayout(GuiLayoutEventArgs.Empty);
        }
        
        // Alustaa viitteenä saadun scroll buttonin.
        private void InitializeScrollButton(ref Button button, GuiEventHandler<GuiButtonEventArgs> pressEvent)
        {
            button = new Button(game);
            button.ButtonPressed += pressEvent;
            button.ButtonDown += pressEvent;

            childs.AddChild(button);
            button.SetParent(this);
        }
        // Poistaa viitteenä saadun scroll buttonin.
        private void RemoveScrollButton(ref Button button, GuiEventHandler<GuiButtonEventArgs> pressEvent)
        {
            button.ButtonPressed -= pressEvent;
            button.ButtonDown -= pressEvent;
            button.ReleaseParent();
            button.Dispose();

            childs.RemoveChild(button);
        }

        private InvalidGuiOperationException NoDefaultButtons()
        {
            return new InvalidGuiOperationException("Scrollbar dosent have default buttons.");
        }
        private UnsupportedGuiOperationException UnsupportedScrollbarType()
        {
            return new UnsupportedGuiOperationException("Unsupported scrollbar type.");
        }
        private void CheckDefaultButtons()
        {
            if (!HasDefaultButtons)
            {
                throw NoDefaultButtons();
            }
        }
        // Päivittää buttonien layoutin.
        private void UpdateButtonLayout()
        {
            if (!HasDefaultButtons)
            {
                return;
            }

            UpdateButtonHeightPercent();
            UpdateButtonWidthPercent();
            UpdateButtonPositions();
        }
        // Päivittää buttonien korkeuden scrollbar tyypin perusteella.
        private void UpdateButtonHeightPercent()
        {
            float height = 0.0f;

            switch (type)
            {
                case ScrollbarType.Horizontal:
                    height = 100.0f;
                    break;
                case ScrollbarType.Vertical:
                    height = 10.0f;
                    break;
                default:
                    throw UnsupportedScrollbarType();
            }

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Size = new Vector2(childs[i].Size.X, height);
            }
        }
        // Päivittää buttonien leveyden scrollbar tyypin perusteella.
        private void UpdateButtonWidthPercent()
        {
            float width = 0.0f;

            switch (type)
            {
                case ScrollbarType.Horizontal:
                    width = 10.0f;
                    break;
                case ScrollbarType.Vertical:
                    width = 100.0f;
                    break;
                default:
                    throw UnsupportedScrollbarType();
            }

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Size = new Vector2(width, childs[i].Size.Y);
            }
        }
        // Päivittää buttonien sijainnin scrollbar tyypin perusteella.
        private void UpdateButtonPositions()
        {
            switch (type)
            {
                case ScrollbarType.Horizontal:
                    backScrollButton.Position = new Vector2(Position.X, Position.Y);
                    forwardScrollButton.Position = new Vector2(Area.Right - forwardScrollButton.SizeInPixels.X, Position.Y);
                    break;
                case ScrollbarType.Vertical:
                    backScrollButton.Position = new Vector2(Position.X, Position.Y);
                    forwardScrollButton.Position = new Vector2(Position.X, Area.Bottom - forwardScrollButton.SizeInPixels.Y);
                    break;
                default:
                    throw UnsupportedScrollbarType();
            }
        }
        // Päivittää scroll valueta ja laukaisee eventin jos value muuttuu.
        private void UpdateValue(int newValue)
        {
            int oldValue = value;

            if (value != newValue)
            {
                if (newValue <= maxValue && newValue >= 0)
                {
                    value = newValue;

                    OnScrollValueChanged(new GuiScrollEventArgs(value, oldValue), this);
                }
            }
        }

        #region Event handlers
        private void OnForwardScroll(GuiButtonEventArgs e, object sender)
        {
            ScrollForward(1);
        }
        private void OnBackScroll(GuiButtonEventArgs e, object sender)
        {
            ScrollBack(1);
        }
        #endregion

        #region Event methods
        private void OnScrollValueChanged(GuiScrollEventArgs e, object sender)
        {
            GuiEventHandler<GuiScrollEventArgs> eventHandler = (GuiEventHandler<GuiScrollEventArgs>)eventHandlers[EventScrollValueChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        private void OnMaxValueChanged(GuiScrollEventArgs e, object sender)
        {
            GuiEventHandler<GuiScrollEventArgs> eventHandler = (GuiEventHandler<GuiScrollEventArgs>)eventHandlers[EventMaxValueChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected override void OnDisposed(GuiEventArgs e, object sender)
        {
            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Dispose();
            }

            base.OnDisposed(e, sender);
        }
        #endregion

        protected override Positioning GetChildPositioning()
        {
            return Positioning.Absolute;
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            base.OnDraw(spriteBatch);

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Draw(spriteBatch);
            }

            RemoveDisposedControls();
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Update(gameTime);
            }

            RemoveDisposedControls();
        }

        public override void Enable()
        {
            base.Enable();

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Enable();
            }
        }
        public override void Disable()
        {
            base.Disable();

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Disable();
            }
        }
        public override void Show()
        {
            base.Show();

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Show();
            }
        }
        public override void Hide()
        {
            base.Hide();

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].Hide();
            }
        }

        /// <summary>
        /// Alustaa scrollbarille default buttonit.
        /// </summary>
        public void InitializeDefaultButtons()
        {
            if (HasDefaultButtons)
            {
                throw new InvalidGuiOperationException("Scrollbar already has default buttons.");
            }

            InitializeScrollButton(ref forwardScrollButton, OnForwardScroll);
            InitializeScrollButton(ref backScrollButton, OnBackScroll);

            UpdateButtonLayout();

            thumb.SetButtonSizes(forwardScrollButton.SizeInPixels, backScrollButton.SizeInPixels);
        }
        /// <summary>
        /// Poistaa scrollbarin default buttonit.
        /// </summary>
        public void RemoveDefaultButtons()
        {
            if (!HasDefaultButtons)
            {
                throw new InvalidGuiOperationException("Scrollbar dosent have default buttons to clear.");
            }

            RemoveScrollButton(ref forwardScrollButton, OnForwardScroll);
            RemoveScrollButton(ref backScrollButton, OnBackScroll);

            thumb.ResetButtonSizes();
        }
        /// <summary>
        /// Asettaa eteen päin scrollaavan buttonin renderöijän.
        /// </summary>
        public void SetForwardScrollButtonRenderer(Renderer<Button> buttonRenderer)
        {
            forwardScrollButton.Renderer = buttonRenderer;
        }
        /// <summary>
        /// Asettaa taakse päin scrollaavan buttonin renderöijän.
        /// </summary>
        /// <param name="buttonRenderer"></param>
        public void SetBackScrollButtonRenderer(Renderer<Button> buttonRenderer)
        {
            backScrollButton.Renderer = buttonRenderer;
        }
        /// <summary>
        /// Asettaa thumbin renderöijän.
        /// </summary>
        /// <param name="thumbRenderer"></param>
        public void SetThumbRenderer(Renderer<ScrollThumb> thumbRenderer)
        {
            thumb.Renderer = thumbRenderer;
        }
        public override void UpdateLayout(GuiLayoutEventArgs guiLayoutEventArgs)
        {
            base.UpdateLayout(guiLayoutEventArgs);

            if (LayoutSuspended)
            {
                return;
            }

            UpdateButtonLayout();

            if (HasDefaultButtons)
            {
                thumb.SetButtonSizes(forwardScrollButton.SizeInPixels, backScrollButton.SizeInPixels);
            }

            thumb.CalculateStep(maxValue);
            thumb.CalculatePosition(value);

            for (int i = 0; i < childs.ChildsCount; i++)
            {
                childs[i].UpdateLayout(guiLayoutEventArgs);
            }
        }
        public void ScrollBack(int value)
        {
            UpdateValue(this.value - value);
        }
        public void ScrollForward(int value)
        {
            UpdateValue(this.value + value);
        }
        public IEnumerable<Control> Childs()
        {
            return childs.Childs();
        }
    }
}
