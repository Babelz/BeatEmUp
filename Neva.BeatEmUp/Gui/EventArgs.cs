using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Neva.BeatEmUp.Gui.Controls;
using Neva.BeatEmUp.Gui.Cursor;
using Neva.BeatEmUp.Gui.Cursor.Args;

namespace Neva.BeatEmUp.Gui
{
    internal class GuiEventArgs : EventArgs
    {
        new public static readonly GuiEventArgs Empty;

        static GuiEventArgs()
        {
            Empty = new GuiEventArgs();
        }

        public GuiEventArgs()
            : base()
        {
        }
    }

    internal sealed class GuiGamePadInputEventArgs : GuiEventArgs
    {
        #region Properties
        public Buttons[] PressedButtons
        {
            get;
            private set;
        }
        #endregion

        public GuiGamePadInputEventArgs(Buttons[] pressedButtons)
            : base()
        {
            PressedButtons = pressedButtons;
        }
    }

    internal sealed class GuiKeyboardInputEventArgs : GuiEventArgs
    {
        #region Properties
        public Keys[] PressedKeys
        {
            get;
            private set;
        }
        #endregion

        public GuiKeyboardInputEventArgs(Keys[] pressedKeys)
        {
            PressedKeys = pressedKeys;
        }
    }

    internal sealed class GuiCursorInputEventArgs : GuiEventArgs
    {
        #region Properties
        public MouseButtons[] PressedButtons
        {
            get;
            private set;
        }
        #endregion

        public GuiCursorInputEventArgs(MouseButtons[] pressedButtons)
            : base()
        {
            PressedButtons = pressedButtons;
        }
    }

    internal sealed class GuiCursorMovementEventArgs : GuiEventArgs
    {
        #region Properties
        public CursorMovementArgs CursorMovementArgs
        {
            get;
            private set;
        }
        #endregion

        public GuiCursorMovementEventArgs(CursorMovementArgs cursorMovementArgs)
            : base()
        {
            CursorMovementArgs = cursorMovementArgs;
        }
    }

    internal sealed class GuiLayoutEventArgs : GuiEventArgs
    {
        new public static readonly GuiLayoutEventArgs Empty;

        #region Properties
        public GuiContentEventArgs GuiContentEventArgs
        {
            get;
            private set;
        }
        public Horizontal? OldHorizontalAlignment
        {
            get;
            private set;
        }
        public Horizontal? NewHorizontalAlingment
        {
            get;
            private set;
        }
        public Vertical? VerticalAlignment
        {
            get;
            private set;
        }
        public Vertical? NewVerticalAlingment
        {
            get;
            private set;
        }
        public Control Control
        {
            get;
            private set;
        }
        public Vector2? OldPosition
        {
            get;
            private set;
        }
        public Vector2? NewPosition
        {
            get;
            private set;
        }
        public Vector2 OldSize
        {
            get;
            private set;
        }
        public Vector2? NewSize
        {
            get;
            private set;
        }
        public FRectangle? NewArea
        {
            get;
            private set;
        }
        public bool ParentChanged
        {
            get;
            private set;
        }
        #endregion

        static GuiLayoutEventArgs()
        {
            Empty = new GuiLayoutEventArgs();
        }
        
        public GuiLayoutEventArgs(Vector2 oldSize, Vector2 newSize, FRectangle newArea)
            : base()
        {
            OldSize = oldSize;
            NewSize = newSize;
            NewArea = newArea;
        }
        public GuiLayoutEventArgs(Horizontal oldAlignment, Horizontal newAlignment)
            : base()
        {
            OldHorizontalAlignment = oldAlignment;
            NewHorizontalAlingment = newAlignment;
        }
        public GuiLayoutEventArgs(Vertical oldAlingment, Vertical newAlignment)
            : base()
        {
            VerticalAlignment = oldAlingment;
            NewVerticalAlingment = newAlignment;
        }
        public GuiLayoutEventArgs(Vector2 position, Vector2 newPosition)
            : base()
        {
            OldPosition = position;
            NewPosition = newPosition;
        }
        public GuiLayoutEventArgs(Control control, bool parentChanged = false)
        {
            Control = control;
            ParentChanged = parentChanged;
        }
        public GuiLayoutEventArgs(GuiContentEventArgs guiContentEventArgs)
        {
            GuiContentEventArgs = guiContentEventArgs;
        }
        public GuiLayoutEventArgs()
            : base()
        {
        }
    }

    internal sealed class GuiParentEventArgs : GuiEventArgs
    {
        #region Properties
        public Control NextParent
        {
            get;
            private set;
        }
        public Control TargetControl
        {
            get;
            private set;
        }
        public Control ControlAdded
        {
            get;
            private set;
        }
        #endregion

        public GuiParentEventArgs(Control nextParent, Control targetControl)
            : base()
        {
            NextParent = nextParent;
            TargetControl = targetControl;
        }
        public GuiParentEventArgs(Control controlAdded)
            : this(null, null)
        {
            ControlAdded = controlAdded;
        }
    }

    internal sealed class GuControlDisplayEventArgs : GuiEventArgs
    {
        new public static readonly GuControlDisplayEventArgs Empty;

        static GuControlDisplayEventArgs()
        {
            Empty = new GuControlDisplayEventArgs();
        }

        public GuControlDisplayEventArgs()
            : base()
        {
        }
    }

    internal sealed class GuiFocusEventArgs : GuiEventArgs
    {
        new public static readonly GuiFocusEventArgs Empty;

        #region Properties
        public Control CurrentFocused
        {
            get;
            private set;
        }
        public Control NextInFocus
        {
            get;
            private set;
        }
        #endregion

        static GuiFocusEventArgs()
        {
            Empty = new GuiFocusEventArgs();
        }

        public GuiFocusEventArgs(Control currentFocused, Control nextInFocus)
        {
            CurrentFocused = currentFocused;
            NextInFocus = nextInFocus;
        }
        public GuiFocusEventArgs()
            : this(null, null)
        {
        }
    }

    internal sealed class GuiContentEventArgs : GuiEventArgs
    {
        new public static readonly GuiContentEventArgs Empty;

        #region Properties
        public object ContentBeingReleased
        {
            get;
            private set;
        }
        public object CurrentContent
        {
            get;
            private set;
        }
        public object NewContent
        {
            get;
            private set;
        }
        #endregion

        static GuiContentEventArgs()
        {
            Empty = new GuiContentEventArgs(null, null);
        }

        public GuiContentEventArgs(object contentBeingReleased)
            : this(null, null)
        {
            ContentBeingReleased = contentBeingReleased;
        }
        public GuiContentEventArgs(object currentContent, object newContent)
            : base()
        {
            CurrentContent = currentContent;
            NewContent = newContent;
        }
    }

    internal sealed class GuiButtonEventArgs : GuiEventArgs
    {
        #region Properties
        public bool Pressed
        {
            get;
            private set;
        }
        public bool Down
        {
            get;
            private set;
        }
        public bool Released
        {
            get;
            private set;
        }
        public TimeSpan TimeDown
        {
            get;
            private set;
        }
        #endregion

        public GuiButtonEventArgs(bool pressed, bool down, bool released, TimeSpan timeDown)
        {
            Pressed = pressed;
            Down = down;
            Released = released;
            TimeDown = timeDown;
        }
    }

    internal sealed class GuiScrollEventArgs : GuiEventArgs
    {
        #region Properties
        public int Value
        {
            get;
            private set;
        }
        public int OldValue
        {
            get;
            private set;
        }
        #endregion

        public GuiScrollEventArgs(int value, int oldValue)
            : base()
        {
            Value = value;
            OldValue = oldValue;
        }
        public GuiScrollEventArgs(int value)
            : this(value, value)
        {
            Value = value;
        }
    }

    internal class GuiComponentEventArgs : GuiEventArgs
    {
        public GuiComponentEventArgs()
            : base()
        {
        }
    }

    internal sealed class GuiChildComponentEventArgs : GuiComponentEventArgs
    {
        #region Properties
        public Control Child
        {
            get;
            private set;
        }
        #endregion

        public GuiChildComponentEventArgs(Control child)
            : base()
        {
            Child = child;
        }
    }

    internal class GuiWindowManagerEventArgs : GuiEventArgs
    {
        public GuiWindowManagerEventArgs()
            : base()
        {
        }
    }

    internal sealed class GuiWindowChangingEventArgs : GuiWindowManagerEventArgs
    {
        #region Properties
        public Window Current
        {
            get;
            private set;
        }
        public Window Next
        {
            get;
            private set;
        }
        public bool Cancel
        {
            get;
            set;
        }
        #endregion

        public GuiWindowChangingEventArgs(Window current, Window next)
            : base()
        {
        }
    }
}
