using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Neva.BeatEmUp.Gui.Cursor;
using Neva.BeatEmUp.Gui.Controls.Components;
using Neva.BeatEmUp.Gui.Controls.Renderers;
using Neva.BeatEmUp.Gui.Cursor.Args;

namespace Neva.BeatEmUp.Gui.Controls
{
    internal abstract class Control : IDisposable
    {
        #region Event keys
        private static readonly object EventKeyboardInput = new object();
        private static readonly object EventMouseInput = new object();
        private static readonly object EventGamePadInput = new object();

        private static readonly object EventKeyboardKeyDown = new object();
        private static readonly object EventMouseButtonDown = new object();
        private static readonly object EventGamPadButtonDown = new object();

        private static readonly object EventMouseHover = new object();
        private static readonly object EventMouseLeave = new object();
        private static readonly object EventMouseEnter = new object();

        private static readonly object EventGotFocus = new object();
        private static readonly object EventLostFocus = new object();
        private static readonly object EventFocusableChanged = new object();

        private static readonly object EventSizeChanged = new object();
        private static readonly object EventPositionChanged = new object();
        private static readonly object EventParentChanged = new object();
        private static readonly object EventEnabledChanged = new object();
        private static readonly object EventVisibilityChanged = new object();
        private static readonly object EventOnDisposing = new object();
        private static readonly object EventDrawOrderChanged = new object();

        private static readonly object EventBrushChanged = new object();
        private static readonly object EventLayoutChanged = new object();

        private static readonly object EventTextChanged = new object();
        #endregion

        #region Vars
        private string name;
        private int drawOrder;
        private bool enabled;
        private bool visible;
        private bool containsFocus;
        private bool disposed;
        private bool mouseOver;
        private bool layoutSuspended;
        private string text;

        private Vector2 oldMousePosition;

        private SizeValueType sizeValueType;
        private Margin margin;
        private Padding padding;
        private Vector2 size;
        private Vector2 position;
        private Control parent;
        private Brush brush;
        private Positioning positioning;
        private Alignment alingment;
        private SizeBehaviour sizeBehaviour;
        private InvocationContainer invoker;

        protected readonly Microsoft.Xna.Framework.Game game;
        protected readonly EventHandlerList eventHandlers;
        protected readonly ResourceContainer resources;

        protected ControlRenderTarget controlRenderTarget;
        protected IRenderer renderer;

        protected bool focusable;
        #endregion

        #region Properties
        /// <summary>
        /// Asettaa tai palauttaa kontrollin nimen.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        /// <summary>
        /// Palauttaa kontrollin enabled arvon.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
        }
        /// <summary>
        /// Palauttaa kontrollin visible arvon.
        /// </summary>
        public bool Visible
        {
            get
            {
                return visible;
            }
        }
        /// <summary>
        /// Palauttaa onko kontrollilla focus.
        /// </summary>
        public bool ContainsFocus
        {
            get
            {
                return containsFocus;
            }
        }
        /// <summary>
        /// Palauttaa voiko kontrolli sisältää focuksen.
        /// </summary>
        public bool Focusable
        {
            get
            {
                return focusable;
            }
        }
        /// <summary>
        /// Palauttaa onko kontrolli disposattu.
        /// </summary>
        public bool Disposed
        {
            get
            {
                return disposed;
            }
        }
        /// <summary>
        /// Palauttaa onko kontrollilla parenttia.
        /// </summary>
        public bool HasParent
        {
            get
            {
                return parent != null;
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrolliin liitetyn tekstin.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;

                    OnTextChanged(GuControlDisplayEventArgs.Empty, this);
                }
            }
        }
        /// <summary>
        /// Palauttaa kontrollin alueen joka on sen koko pikseleissä ja oikea sijainti (ei relatiivinen parenttiin)
        /// </summary>
        public FRectangle Area
        {
            get
            {
                return new FRectangle(Position.X, Position.Y, SizeInPixels.X, SizeInPixels.Y);
            }
        }
        /// <summary>
        /// Palauttaa kontrollin alueen joka on sen koko pikseleissä ja relatiivinen sijainti.
        /// </summary>
        public FRectangle RelativeArea
        {
            get
            {
                return new FRectangle(RelativePosition.X, RelativePosition.Y, SizeInPixels.X, SizeInPixels.Y);
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin paddingi. Value ei ole koskaan null. 
        /// Jos yritetään asetetaan nulliksi, asetetaan siihen default arvot.
        /// </summary>
        public Padding Padding
        {
            get
            {
                return padding;
            }
            set
            {
                Padding newPadding = value ?? Padding.Empty();

                if (padding != newPadding)
                {
                    Vector2 oldSize = SizeInPixels;
                    padding = newPadding;

                    UpdateLayout(GuiLayoutEventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin marginin. Value ei ole koskaan null. 
        /// Jos yritetään asetetaan nulliksi, asetetaan siihen default arvot. 
        /// </summary>
        public Margin Margin
        {
            get
            {
                return margin;
            }
            set
            {
                Margin newMargin = value ?? Margin.Empty();
                
                if (margin != newMargin)
                {
                    Vector2 oldPosition = Position;
                    margin = newMargin;

                    UpdateLayout(GuiLayoutEventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin koon. Kokoon on lisätty padding arvo.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                float right = 0, bottom = 0;

                if (SizeValueType == SizeValueType.Fixed)
                {
                    right = CalculateWidthOffSet();
                    bottom = CalculateHeightOffSet();
                }

                return new Vector2(size.X + right, size.Y + bottom);
            }
            set
            {
                float right = 0, bottom = 0, x = 0, y = 0;

                if (SizeValueType == SizeValueType.Fixed)
                {
                    right = CalculateWidthOffSet();
                    bottom = CalculateHeightOffSet();
                }

                if (positioning == Positioning.Relative)
                {
                    switch (sizeBehaviour)
                    {
                        case SizeBehaviour.OverwriteWidth:
                            x = value.X;
                            y = size.Y;
                            break;
                        case SizeBehaviour.OverwriteHeight:
                            x = size.X;
                            y = value.Y;
                            break;
                        case SizeBehaviour.OverwriteBoth:
                            x = value.X;
                            y = value.Y;
                            break;
                        case SizeBehaviour.NoOverwrites:
                            x = size.X;
                            y = size.Y;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    x = value.X;
                    y = value.Y;
                }

                Vector2 newSize = new Vector2(x + right, y + bottom);

                if (size != newSize)
                {
                    Vector2 oldSize = SizeInPixels;
                    size = newSize;

                    GuiLayoutEventArgs guiLayoutEventArgs = new GuiLayoutEventArgs(oldSize, newSize, Area);
                    OnSizeChanged(guiLayoutEventArgs, this);

                    UpdateLayout(guiLayoutEventArgs);
                }
            }
        }
        /// <summary>
        /// Palauttaa kontrollin koon pikseleissä. Jos koon tyyppi
        /// on prosenttuaalinen, palauttaa se prosentuaalisen arvon 
        /// pikseli määrän. Jos fixed, palauttaa suoraan koon.
        /// </summary>
        public Vector2 SizeInPixels
        {
            get
            {
                switch (sizeValueType)
                {
                    case SizeValueType.Fixed:
                        return Size;
                    case SizeValueType.Percents:
                        Vector2 size = Size;
                        Vector2 overwrites = Vector2.Zero;
                        Vector2 parentSize = parent == null ? new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height) : parent.SizeInPixels;

                        if (positioning == Positioning.Relative)
                        {
                            switch (sizeBehaviour)
                            {
                                case SizeBehaviour.OverwriteWidth:
                                    overwrites = new Vector2(size.X, 0);
                                    break;
                                case SizeBehaviour.OverwriteHeight:
                                    overwrites = new Vector2(0, size.Y);
                                    break;
                                case SizeBehaviour.OverwriteBoth:
                                    overwrites = size;
                                    break;
                                case SizeBehaviour.NoOverwrites:
                                    overwrites = Vector2.Zero;
                                    break;
                                default:
                                    throw UnsupportedSizeBehaviour();
                            }

                            // Mitään ei ylirkijoiteta.
                            if (overwrites == Vector2.Zero)
                            {
                                return new Vector2(size.X + CalculateWidthOffSet(), size.Y + CalculateHeightOffSet());
                            }
                            else
                            {
                                Vector2 overwritePixels = StructHelpers.SizeFromPercents(parentSize, overwrites);

                                // Onko X ylikirjoitettu.
                                if (overwrites.X != 0)
                                {
                                    overwrites = new Vector2(overwritePixels.X + CalculateWidthOffSet(), overwrites.Y);
                                }
                                else
                                {
                                    overwrites = new Vector2(size.X, overwritePixels.Y);
                                }
                                // Onko Y ylikirjoitettu.
                                if (overwrites.Y != 0)
                                {
                                    overwrites = new Vector2(overwrites.X, overwritePixels.Y + CalculateHeightOffSet());
                                }
                                else
                                {
                                    overwrites = new Vector2(overwrites.X, size.Y);
                                }

                                return overwrites;
                            }
                        }
                        else
                        {
                            return StructHelpers.SizeFromPercents(parentSize, size);
                        }
                    default:
                        throw new InvalidGuiOperationException("Unsupported size value type.");
                }
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin koon arvo tyypin.
        /// </summary>
        public SizeValueType SizeValueType
        {
            get
            {
                return sizeValueType; 
            }
            set
            {
                if (value != sizeValueType)
                {
                    Vector2 oldSize = SizeInPixels;
                    sizeValueType = value;

                    GuiLayoutEventArgs guiLayoutEventArgs = new GuiLayoutEventArgs(oldSize, SizeInPixels, Area);
                    OnSizeChanged(guiLayoutEventArgs, this);

                    UpdateLayout(guiLayoutEventArgs);
                }
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin sijainnin. Sijaintiin lisätään margin.
        /// Sijaintia ei voi asettaa jos sijainnin tyyppi on relatiivinen.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return new Vector2(position.X + CalculateXOffSet(), position.Y + CalculateYOffSet());
            }
            set
            {
                // Sijaintia voidaan muokata suoraan vain jos asettelu on absoluuttinen.
                if (positioning == Gui.Positioning.Absolute)
                {
                    Vector2 newPosition = new Vector2(value.X + CalculateXOffSet(), value.Y + CalculateYOffSet());

                    if (position != newPosition)
                    {
                        Vector2 oldPosition = position;
                        position = value;

                        GuiLayoutEventArgs guiLayoutEventArgs = new GuiLayoutEventArgs(oldPosition, newPosition);
                        OnPositionChanged(guiLayoutEventArgs, this);

                        UpdateLayout(guiLayoutEventArgs);
                    }
                }
            }
        }
        /// <summary>
        /// Palauttaa kontrollin relatiivisen sijainnin parenttiin.
        /// </summary>
        public Vector2 RelativePosition
        {
            get
            {
                if (parent == null)
                {
                    throw new InvalidGuiOperationException("Cant calculate relative position to parent if its null.");
                }

                return new Vector2(parent.position.X + Position.X, parent.position.Y + Position.Y);
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin piirtovuoron. 
        /// </summary>
        public int DrawOrder
        {
            get
            {
                return drawOrder;
            }
            set
            {
                if (drawOrder != value)
                {
                    drawOrder = value;

                    OnDrawOrderChanged(GuControlDisplayEventArgs.Empty, this);
                }
            }
        }
        /// <summary>
        /// Palauttaa kontrollin parentin.
        /// </summary>
        public Control Parent
        {
            get
            {
                return parent;
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin brushin.
        /// </summary>
        public Brush Brush
        {
            get
            {
                return brush;
            }
            set
            {
                if (brush != value)
                {
                    brush = value;

                    OnBrushChanged(GuControlDisplayEventArgs.Empty, this);
                }
            }
        }
        /// <summary>
        /// Palauttaa miten kontrolli sijoitetaan parenttiinsa.
        /// </summary>
        public Positioning Positioning
        {
            get
            {
                return positioning;
            }
        }
        public Horizontal HorizontalAlingment
        {
            get
            {
                return alingment.Horizontal;
            }
            set
            {
                if (value != alingment.Horizontal)
                {
                    Horizontal oldAlingment = alingment.Horizontal;
                    alingment.Horizontal = value;

                    UpdateLayout(new GuiLayoutEventArgs(oldAlingment, alingment.Horizontal));
                }
            }
        }
        public Vertical VerticalAlingment
        {
            get
            {
                return alingment.Vertical;
            }
            set
            {
                if (value != alingment.Vertical)
                {
                    Vertical oldAlingment = alingment.Vertical; 
                    alingment.Vertical = value;

                    UpdateLayout(new GuiLayoutEventArgs(oldAlingment, alingment.Vertical));
                }
            }   
        }
        /// <summary>
        /// Palauttaa booleanin voiko kontrolli renderöidä itsensä.
        /// Jos kontrollilla on renderöijä, voi se renderöidä itsensä.
        /// </summary>
        public bool CanRender
        {
            get
            {
                return renderer != null;
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin renderöijän joka päättää
        /// miten kontrolli piirretään.
        /// </summary>
        public IRenderer Renderer
        {
            get
            {
                return renderer;
            }
            set
            {
                renderer = value;
            }
        }
        /// <summary>
        /// Miten koko voidaan asettaa jos asettelu on relatviiivinen.
        /// </summary>
        public SizeBehaviour SizeBehaviour
        {
            get
            {
                return sizeBehaviour;
            }
            set
            {
                if (sizeBehaviour != value)
                {
                    sizeBehaviour = value;

                    OnSizeChanged(GuiLayoutEventArgs.Empty, this);

                    UpdateLayout(GuiLayoutEventArgs.Empty);
                }
            }
        }
        public bool LayoutSuspended
        {
            get
            {
                return layoutSuspended;
            }
        }
        public ResourceContainer Resources
        {
            get
            {
                return resources;
            }
        }
        public InvocationContainer Invoker
        {
            get
            {
                return invoker;
            }
        }
        #endregion

        #region Events
        public event GuiEventHandler<GuiKeyboardInputEventArgs> KeyboardInput
        {
            add
            {
                eventHandlers.AddHandler(EventKeyboardInput, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventKeyboardInput, value);
            }
        }
        public event GuiEventHandler<GuiCursorInputEventArgs> MouseInput
        {
            add
            {
                eventHandlers.AddHandler(EventMouseInput, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventMouseInput, value);
            }
        }
        public event GuiEventHandler<GuiGamePadInputEventArgs> GamePadInput
        {
            add
            {
                eventHandlers.AddHandler(EventGamePadInput, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventGamePadInput, value);
            }
        }

        public event GuiEventHandler<GuiKeyboardInputEventArgs> KeyboardKeyDown
        {
            add
            {
                eventHandlers.AddHandler(EventKeyboardKeyDown, value);   
            }
            remove
            {
                eventHandlers.RemoveHandler(EventKeyboardKeyDown, value);
            }
        }
        public event GuiEventHandler<GuiCursorInputEventArgs> MouseButtonDown
        {
            add
            {
                eventHandlers.AddHandler(EventMouseButtonDown, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventMouseButtonDown, value);
            }
        }
        public event GuiEventHandler<GuiGamePadInputEventArgs> GamePadButtonDown
        {
            add
            {
                eventHandlers.AddHandler(EventGamPadButtonDown, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventGamPadButtonDown, value);
            }
        }

        public event GuiEventHandler<GuiCursorMovementEventArgs> MouseHover
        {
            add
            {
                eventHandlers.AddHandler(EventMouseHover, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventMouseHover, value);
            }
        }
        public event GuiEventHandler<GuiCursorMovementEventArgs> MouseLeave
        {
            add
            {
                eventHandlers.AddHandler(EventMouseLeave, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventMouseLeave, value);
            }
        }
        public event GuiEventHandler<GuiCursorMovementEventArgs> MouseEnter
        {
            add
            {
                eventHandlers.AddHandler(EventMouseEnter, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventMouseEnter, value);
            }
        }

        public event GuiEventHandler<GuiFocusEventArgs> GotFocus
        {
            add
            {
                eventHandlers.AddHandler(EventGotFocus, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventGotFocus, value);
            }
        }
        public event GuiEventHandler<GuiFocusEventArgs> LostFocus
        {
            add
            {
                eventHandlers.AddHandler(EventLostFocus, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventLostFocus, value);
            }
        }
        public event GuiEventHandler<GuiFocusEventArgs> FocusableChanged
        {
            add
            {
                eventHandlers.AddHandler(EventFocusableChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventFocusableChanged, value);
            }
        }

        public event GuiEventHandler<GuiParentEventArgs> ParentChanged
        {
            add
            {
                eventHandlers.AddHandler(EventParentChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventParentChanged, value);
            }
        }
        public event GuiEventHandler<GuiEventArgs> EnabledChanged
        {
            add
            {
                eventHandlers.AddHandler(EventEnabledChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventEnabledChanged, value);
            }
        }
        public event GuiEventHandler<GuiEventArgs> Disposing
        {
            add
            {
                eventHandlers.AddHandler(EventOnDisposing, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventOnDisposing, value);
            }
        }

        public event GuiEventHandler<GuiLayoutEventArgs> SizeChanged
        {
            add
            {
                eventHandlers.AddHandler(EventSizeChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventSizeChanged, value);
            }
        }
        public event GuiEventHandler<GuiLayoutEventArgs> PositionChanged
        {
            add
            {
                eventHandlers.AddHandler(EventPositionChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventPositionChanged, value);
            }
        }
        public event GuiEventHandler<GuiLayoutEventArgs> LayoutChanged
        {
            add
            {
                eventHandlers.AddHandler(EventLayoutChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventLayoutChanged, value);
            }
        }

        public event GuiEventHandler<GuControlDisplayEventArgs> VisibilityChanged
        {
            add
            {
                eventHandlers.AddHandler(EventVisibilityChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventVisibilityChanged, value);
            }
        }
        public event GuiEventHandler<GuControlDisplayEventArgs> DrawOrderChanged
        {
            add
            {
                eventHandlers.AddHandler(EventDrawOrderChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventDrawOrderChanged, value);
            }
        }
        public event GuiEventHandler<GuControlDisplayEventArgs> TextChanged
        {
            add
            {
                eventHandlers.AddHandler(EventTextChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventTextChanged, value);
            }
        }
        public event GuiEventHandler<GuControlDisplayEventArgs> BrushChanged
        {
            add
            {
                eventHandlers.AddHandler(EventBrushChanged, value);
            }
            remove
            {
                eventHandlers.RemoveHandler(EventBrushChanged, value);
            }
        }
        #endregion

        public Control(Microsoft.Xna.Framework.Game game)
        {
            this.game = game;

            invoker = new InvocationContainer();
            eventHandlers = new EventHandlerList();
            resources = new ResourceContainer();

            margin = Margin.Empty();
            padding = Padding.Empty();

            size = new Vector2(100, 100);
            position = Vector2.Zero;

            alingment = new Alignment(Horizontal.Stretch, Vertical.Stretch);
            positioning = Positioning.Absolute;
            sizeValueType = SizeValueType.Percents;
            sizeBehaviour = SizeBehaviour.NoOverwrites;

            brush = new Brush(Color.White, Color.Black, Color.CornflowerBlue);

            visible = true;
            enabled = true;
            focusable = true;
            disposed = false;
            layoutSuspended = false;

            controlRenderTarget = new ControlRenderTarget(game, this);

            drawOrder = 0;
        }

        #region Event methods

        #region Input event methods
        protected virtual void OnKeyboardInput(GuiKeyboardInputEventArgs e, object sender)
        {
            GuiEventHandler<GuiKeyboardInputEventArgs> eventHandler = (GuiEventHandler<GuiKeyboardInputEventArgs>)eventHandlers[EventKeyboardInput];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnMouseInput(GuiCursorInputEventArgs e, object sender)
        {
            GuiEventHandler<GuiCursorInputEventArgs> eventHandler = (GuiEventHandler<GuiCursorInputEventArgs>)eventHandlers[EventMouseInput];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnGamepadInput(GuiGamePadInputEventArgs e, object sender)
        {
            GuiEventHandler<GuiGamePadInputEventArgs> eventHandler = (GuiEventHandler<GuiGamePadInputEventArgs>)eventHandlers[EventGamePadInput];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnKeyboardKeyDown(GuiKeyboardInputEventArgs e, object sender)
        {
            GuiEventHandler<GuiKeyboardInputEventArgs> eventHandler = (GuiEventHandler<GuiKeyboardInputEventArgs>)eventHandlers[EventKeyboardKeyDown];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnMouseButtonDown(GuiCursorInputEventArgs e, object sender)
        {
            GuiEventHandler<GuiCursorInputEventArgs> eventHandler = (GuiEventHandler<GuiCursorInputEventArgs>)eventHandlers[EventMouseButtonDown];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnGamePadButtonDown(GuiGamePadInputEventArgs e, object sender)
        {
            GuiEventHandler<GuiGamePadInputEventArgs> eventHandler = (GuiEventHandler<GuiGamePadInputEventArgs>)eventHandlers[EventGamPadButtonDown];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnMouseHover(GuiCursorMovementEventArgs e, object sender)
        {
            GuiEventHandler<GuiCursorMovementEventArgs> eventHandler = (GuiEventHandler<GuiCursorMovementEventArgs>)eventHandlers[EventMouseHover];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnMouseLeave(GuiCursorMovementEventArgs e, object sender)
        {
            GuiEventHandler<GuiCursorMovementEventArgs> eventHandler = (GuiEventHandler<GuiCursorMovementEventArgs>)eventHandlers[EventMouseLeave];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnMouseEnter(GuiCursorMovementEventArgs e, object sender)
        {
            GuiEventHandler<GuiCursorMovementEventArgs> eventHandler = (GuiEventHandler<GuiCursorMovementEventArgs>)eventHandlers[EventMouseEnter];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        #endregion

        protected virtual void OnGotFocus(GuiFocusEventArgs e, object sender)
        {
            GuiEventHandler<GuiFocusEventArgs> eventHandler = (GuiEventHandler<GuiFocusEventArgs>)eventHandlers[EventGotFocus];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnLostFocus(GuiFocusEventArgs e, object sender)
        {
            GuiEventHandler<GuiFocusEventArgs> eventHandler = (GuiEventHandler<GuiFocusEventArgs>)eventHandlers[EventLostFocus];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnParentChanged(GuiParentEventArgs e, object sender)
        {
            GuiEventHandler<GuiParentEventArgs> eventHandler = (GuiEventHandler<GuiParentEventArgs>)eventHandlers[EventParentChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnEnabledChanged(GuiEventArgs e, object sender)
        {
            GuiEventHandler<GuiEventArgs> eventHandler = (GuiEventHandler<GuiEventArgs>)eventHandlers[EventEnabledChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnDisposed(GuiEventArgs e, object sender)
        {
            GuiEventHandler<GuiEventArgs> eventHandler = (GuiEventHandler<GuiEventArgs>)eventHandlers[EventOnDisposing];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnFontChanged(GuiEventArgs e, object sender)
        {
            GuiEventHandler<GuiEventArgs> eventHandler = (GuiEventHandler<GuiEventArgs>)eventHandlers[EventTextChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnFocusableChanged(GuiEventArgs e, object sender)
        {
            GuiEventHandler<GuiEventArgs> eventHandler = (GuiEventHandler<GuiEventArgs>)eventHandlers[EventFocusableChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnSizeChanged(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHandler = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventSizeChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnLayoutChanged(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHandler = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventLayoutChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnPositionChanged(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHandler = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventPositionChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnBrushChanged(GuControlDisplayEventArgs e, object sender)
        {
            GuiEventHandler<GuControlDisplayEventArgs> eventHandler = (GuiEventHandler<GuControlDisplayEventArgs>)eventHandlers[EventBrushChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnVisibilityChanged(GuControlDisplayEventArgs e, object sender)
        {
            GuiEventHandler<GuControlDisplayEventArgs> eventHandler = (GuiEventHandler<GuControlDisplayEventArgs>)eventHandlers[EventVisibilityChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnDrawOrderChanged(GuControlDisplayEventArgs e, object sender)
        {
            GuiEventHandler<GuControlDisplayEventArgs> eventHandler = (GuiEventHandler<GuControlDisplayEventArgs>)eventHandlers[EventDrawOrderChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        protected virtual void OnTextChanged(GuControlDisplayEventArgs e, object sender)
        {
            GuiEventHandler<GuControlDisplayEventArgs> eventHandler = (GuiEventHandler<GuControlDisplayEventArgs>)eventHandlers[EventTextChanged];

            if (eventHandler != null)
            {
                eventHandler(e, sender);
            }
        }
        #endregion

        #region Control methods
        private void HandleMovement(CursorMovementArgs cursorMovementArgs)
        {
            // Katsotaan onko hiiri tullut kontrollin päälle (MouseEnter)
            if (!mouseOver)
            {
                if (Area.Intersects(cursorMovementArgs.MouseArea))
                {
                    OnMouseEnter(new GuiCursorMovementEventArgs(cursorMovementArgs), this);

                    mouseOver = true;

                    // Returnataan jotta alemmat tarkistukset voidaan tehdä 
                    // seuraavalla framella.
                    return;
                }
            }
            
            // Katsotaan leijjaileeko hiiri kontrollin päällä ja onko se liikkunut.
            if (mouseOver)
            {
                // Onko hiiri kontrollin päällä (MouseHover)
                if (Area.Intersects(cursorMovementArgs.MouseArea))
                {
                    OnMouseHover(new GuiCursorMovementEventArgs(cursorMovementArgs), this);
                }
                else
                {
                    // Hiiri on lähtenyt kontrollin päältä (MouseLeave)
                    OnMouseLeave(new GuiCursorMovementEventArgs(cursorMovementArgs), this);

                    mouseOver = false;

                    return;
                }
            }
        }
        private void HandleKeyboardInput(Keys[] pressedKeys)
        {
            if (pressedKeys.Length > 0)
            {
                GuiKeyboardInputEventArgs guiKeyboardInputEventArgs = new GuiKeyboardInputEventArgs(pressedKeys);

                OnKeyboardInput(guiKeyboardInputEventArgs, this);
                OnKeyboardKeyDown(guiKeyboardInputEventArgs, this);
            }
        }
        private void HandleMouseInput(MouseButtons[] pressedButtons)
        {
            if (pressedButtons.Length > 0)
            {
                GuiCursorInputEventArgs guiMouseInputEventArgs = new GuiCursorInputEventArgs(pressedButtons);

                OnMouseInput(guiMouseInputEventArgs, this);
                OnMouseButtonDown(guiMouseInputEventArgs, this);
            }
        }
        private void HandleGamepadInput(Buttons[] pressedButtons)
        {
            if (pressedButtons.Length > 0)
            {
                GuiGamePadInputEventArgs guiGamePadInputArgs = new GuiGamePadInputEventArgs(pressedButtons);

                OnGamepadInput(guiGamePadInputArgs, this);
                OnGamePadButtonDown(guiGamePadInputArgs, this);
            }
        }
        private void UpdateFocus(CursorMovementArgs cursorMovementArgs)
        {
            if (cursorMovementArgs.MouseArea.Intersects(Area))
            {
                if (!containsFocus)
                {
                    Focus();
                }
            }
            else
            {
                if (containsFocus)
                {
                    DeFocus();
                }
            }
        }

        private void Focus()
        {
            if (containsFocus)
            {
                throw new InvalidGuiOperationException("Control already contains focus.");
            }

            containsFocus = true;

            OnGotFocus(GuiFocusEventArgs.Empty, this);
        }
        private void DeFocus()
        {
            if (!containsFocus)
            {
                throw new InvalidGuiOperationException("Control contains no focus.");
            }

            containsFocus = false;

            OnLostFocus(GuiFocusEventArgs.Empty, this);
        }

        private InvalidGuiOperationException UnsupportedVerticalAlingment()
        {
            return new InvalidGuiOperationException("Unsupported vertical alignment type.");
        }
        private InvalidGuiOperationException UnsupportedHorizontalAlingment()
        {
            return new InvalidGuiOperationException("Unsupported horizontal alignment type.");
        }
        private InvalidGuiOperationException UnsupportedSizeBehaviour()
        {
            return new InvalidGuiOperationException("Unsupported size behaviour type.");
        }

        private float CalculateWidthOffSet()
        {
            if (padding.Right == 0)
            {
                return padding.Left - margin.Right - margin.Left;
            }
            else
            {
                return padding.Right - margin.Right + (padding.Left == 0 ? 0 : padding.Right) - margin.Left;
            }
        }
        private float CalculateHeightOffSet()
        {
            if (padding.Bottom == 0)
            {
                return padding.Top - margin.Bottom - margin.Top;
            }
            else
            {
                return padding.Bottom - margin.Bottom + (padding.Top == 0 ? 0 : padding.Bottom) - margin.Top;
            }
        }
        private float CalculateXOffSet()
        {
            return margin.Left - padding.Left;
        }
        private float CalculateYOffSet()
        {
            return margin.Top - padding.Top;
        }

        private void CalculatePositionFromAlignment()
        {
            switch (alingment.Horizontal)
            {
                case Horizontal.Left:
                    position = new Vector2(parent.Position.X, position.Y);
                    break;
                case Horizontal.Center:
                    float center = SizeInPixels.X / 2;
                    float parentCenter = parent.SizeInPixels.X / 2;

                    position = new Vector2(parent.Position.X + parentCenter - center, position.Y);
                    break;
                case Horizontal.Right:
                    position = new Vector2(Parent.Area.Right - SizeInPixels.X, position.Y);
                    break;
                case Horizontal.Stretch:
                    position = new Vector2(parent.Position.X, position.Y);
                    break;
                default:
                    throw UnsupportedHorizontalAlingment();
            }
            switch (alingment.Vertical)
            {
                case Vertical.Top:
                    position = new Vector2(position.X, parent.Position.Y);
                    break;
                case Vertical.Center:
                    float center = SizeInPixels.Y / 2;
                    float parentCenter = parent.SizeInPixels.Y / 2;

                    position = new Vector2(position.X, parent.Area.Top + parentCenter - center);
                    break;
                case Vertical.Bottom:
                    position = new Vector2(position.X, parent.Area.Bottom - SizeInPixels.Y);
                    break;
                case Vertical.Stretch:
                    position = new Vector2(position.X, parent.Position.Y);
                    break;
                default:
                    throw UnsupportedVerticalAlingment();
            }
        }
        private void CalculateSizeFromAlignment()
        {
            switch (sizeBehaviour)
            {
                case SizeBehaviour.OverwriteWidth:
                    CalculateHeightFromAlignment();
                    break;
                case SizeBehaviour.OverwriteHeight:
                    CalculateWidthFromAlignment();
                    break;
                case SizeBehaviour.OverwriteBoth:
                    break;
                case SizeBehaviour.NoOverwrites:
                    CalculateWidthFromAlignment();
                    CalculateHeightFromAlignment();
                    break;
                default:
                    throw UnsupportedSizeBehaviour();
            }
        }

        private void CalculateWidthFromAlignment()
        {
            switch (alingment.Horizontal)
            {
                case Horizontal.Left:
                    size = new Vector2(10, Size.Y);
                    break;
                case Horizontal.Center:
                    size = new Vector2(10, Size.Y);
                    break;
                case Horizontal.Right:
                    size = new Vector2(10, Size.Y);
                    break;
                case Horizontal.Stretch:
                    size = new Vector2(parent.SizeInPixels.X, Size.Y);
                    break;
                default:
                    throw UnsupportedHorizontalAlingment();
            }
        }
        private void CalculateHeightFromAlignment()
        {
            switch (alingment.Vertical)
            {
                case Vertical.Top:
                    size = new Vector2(Size.X, 10);
                    break;
                case Vertical.Center:
                    size = new Vector2(Size.X, 10);
                    break;
                case Vertical.Bottom:
                    size = new Vector2(Size.X, 10);
                    break;
                case Vertical.Stretch:
                    size = new Vector2(Size.X, parent.SizeInPixels.Y);
                    break;
                default:
                    throw UnsupportedVerticalAlingment();
            }
        }

        /// <summary>
        /// Palauttaa sijoittelun mitä lapsi kontrollien tulee käyttää.
        /// </summary>
        /// <returns></returns>
        protected virtual Positioning GetChildPositioning()
        {
            return positioning;
        }

        public void SetParent(Control control)
        {
            if (HasParent)
            {
                throw new InvalidGuiOperationException("Control already has a parent, release it before setting new parent.");
            }

            parent = control;

            OnParentChanged(new GuiParentEventArgs(parent, this), this);

            positioning = parent.GetChildPositioning();
        }
        public void ReleaseParent()
        {
            if (!HasParent)
            {
                throw new InvalidGuiOperationException("Control dosent have a parent to release.");
            }

            positioning = Positioning.Absolute;
            parent = null;

            OnParentChanged(new GuiParentEventArgs(null, this), this);
        }

        public void AllowFocusing()
        {
            if (focusable)
            {
                throw new InvalidGuiOperationException("Control is already focusable.");
            }

            focusable = true;

            OnFocusableChanged(GuiEventArgs.Empty, this);
        }
        public void DisableFocusing()
        {
            if (!focusable)
            {
                throw new InvalidGuiOperationException("Control is already unfocusable.");
            }

            focusable = false;

            OnFocusableChanged(GuiEventArgs.Empty, this);
        }

        public virtual void Hide()
        {
            if (!visible)
            {
                return;
            }

            visible = false;

            OnVisibilityChanged(GuControlDisplayEventArgs.Empty, this);
        }
        public virtual void Show()
        {
            if (visible)
            {
                return;
            }

            visible = true;

            OnVisibilityChanged(GuControlDisplayEventArgs.Empty, this);
        }

        public virtual void Disable()
        {
            if (!enabled)
            {
                return;
            }

            enabled = false;

            OnEnabledChanged(GuiEventArgs.Empty, this);
        }
        public virtual void Enable()
        {
            if (enabled)
            {
                return;
            }

            enabled = true;

            OnEnabledChanged(GuiEventArgs.Empty, this);
        }

        public virtual void SuspendLayout()
        {
            layoutSuspended = true;
        }
        public virtual void ResumeLayout()
        {
            layoutSuspended = false;
        }

        /// <summary>
        /// Päivittää kontrollin layoutin. Tarvitsee kutsua vain jos kontrollin 
        /// rakenteeseen kosketaan. 
        /// </summary>
        public virtual void UpdateLayout(GuiLayoutEventArgs guiLayoutEventArgs)
        {
            if (layoutSuspended)
            {
                return;
            }

            if (positioning == Gui.Positioning.Relative && parent != null)
            {
                CalculateSizeFromAlignment();
                CalculatePositionFromAlignment();
            }

            OnLayoutChanged(guiLayoutEventArgs, this);
        }
        #endregion

        #region Xna methods
        /// <summary>
        /// Kutsuu rendererin updatea. Basea ei tarvitse kutsua jos ylikirjoitetaan.
        /// </summary>
        protected virtual void OnUpdate(GameTime gameTime)
        {
            if (CanRender) 
            {
                renderer.Update(gameTime);
            }
        }
        /// <summary>
        /// Kutsuu rendererin drawia. Basea ei tarvitse kutsua jos ylikirjoitetaan.
        /// </summary>
        protected virtual void OnDraw(SpriteBatch spriteBatch)
        {
            if (CanRender)
            {
                renderer.Render(spriteBatch);
            }
        }

        public void HandleCursorMovement(CursorMovementArgs cursorMovementArgs)
        {
            if (!enabled)
            {
                return;
            }

            // Prosessoidaan hiiren liikkeet jos meillä on focus.
            if (containsFocus)
            {
                HandleMovement(cursorMovementArgs);

                oldMousePosition = cursorMovementArgs.MousePosition;
            }

            UpdateFocus(cursorMovementArgs);
        }
        public void HandleCursorInput(CursorInputArgs cursorInputArgs)
        {
            if (!enabled || !containsFocus)
            {
                return;
            }

            switch (cursorInputArgs.InputSource)
            {
                case InputSource.Gamepad:
                    HandleGamepadInput(cursorInputArgs.PressedGamePadButtons);
                    break;
                case InputSource.Mouse:
                    HandleMouseInput(cursorInputArgs.PressedMouseButtons);
                    break;
                case InputSource.Keyboard:
                    HandleKeyboardInput(cursorInputArgs.PressedKeys);
                    break;
                default:
                    throw new UnsupportedGuiOperationException(string.Format("Invalid input source in HandleCursorInput. Input type was {0}.", cursorInputArgs.InputSource));
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!enabled || disposed)
            {
                return;
            }

            OnUpdate(gameTime);

            invoker.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!visible || disposed)
            {
                return;
            }

            OnDraw(spriteBatch);
        }
        #endregion
            
        #region IDisposable members
        protected virtual void DisposeManagedResources()
        {
        }
        protected virtual void DisposeUnManagedResources()
        {
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Vapautetaan kaikki managoidut resurssit.
                if (controlRenderTarget != null)
                {
                    controlRenderTarget.Dispose();
                }

                eventHandlers.Dispose();

                if (renderer != null && !renderer.Disposed)
                {
                    renderer.Dispose();
                }

                GC.SuppressFinalize(this);

                DisposeManagedResources();
            }

            // Vapautetaan kaikki ei managoidut resurssit.
            DisposeUnManagedResources();

            disposed = true;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            OnDisposed(GuiEventArgs.Empty, this);

            Dispose(true);
        }
        #endregion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", this.GetType().Name, (string.IsNullOrEmpty(name) ? "No name" : name), this.GetType().Name);
        }

        ~Control()
        {
            if (!disposed)
            {
                Dispose(false);
            }
        }
    }
}
