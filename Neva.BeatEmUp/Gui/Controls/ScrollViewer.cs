using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Gui.Controls.Components;
using Neva.BeatEmUp.Gui.Controls.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Neva.BeatEmUp.Gui.Controls
{
    internal sealed class ScrollViewer : ContentControl, IContainer
    {
        #region Event keys
        private readonly object EventViewChanged = new object();
        private readonly object EventViewMoved = new object();
        #endregion

        #region Vars
        private readonly SpriteBatch spriteBatch;
        private float topOverlapse;
        private float bottomOverlapse;
        private float rightOverlapse;
        private float leftOverlapse;
        #endregion

        #region Properties
        public float TopOverlapse
        {
            get
            {
                return topOverlapse;
            }
        }
        public float BottomOverlapse
        {
            get
            {
                return bottomOverlapse;
            }
        }
        public float RightOverlapse
        {
            get
            {
                return rightOverlapse;
            }
        }
        public float LeftOverlapse
        {
            get
            {
                return leftOverlapse;
            }
        }
        public float ViewWidth
        {
            get
            {
                return controlRenderTarget.Target.Width;
            }
        }
        public float ViewHeight
        {
            get
            {
                return controlRenderTarget.Target.Height;
            }
        }
        #endregion

        public ScrollViewer(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            ServiceFinder serviceFinder = new ServiceFinder(game);

            spriteBatch = serviceFinder.FindService<SpriteBatch>();

            renderer = new BasicScrollViewerRenderer(game, this);
        }

        private void CalculateOverlapses()
        {
            if (!HasContent)
            {
                return;
            }

            contentControl.Position = Position;

            leftOverlapse = contentControl.Area.Left - Area.Left;
            rightOverlapse = contentControl.Area.Right - Area.Right;
            topOverlapse = contentControl.Area.Top - Area.Top;
            bottomOverlapse = contentControl.Area.Bottom - Area.Bottom;
        }

        #region Event methods
        private void OnViewChanged(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHanlder = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventViewChanged];

            if (eventHanlder != null)
            {
                eventHanlder(e, sender);
            }
        }
        private void OnViewMoved(GuiLayoutEventArgs e, object sender)
        {
            GuiEventHandler<GuiLayoutEventArgs> eventHanlder = (GuiEventHandler<GuiLayoutEventArgs>)eventHandlers[EventViewMoved];

            if (eventHanlder != null)
            {
                eventHanlder(e, sender);
            }
        }
        protected override void OnLayoutChanged(GuiLayoutEventArgs e, object sender)
        {
            base.OnLayoutChanged(e, sender);

            OnViewChanged(e, sender);
        }
        #endregion

        protected override Positioning GetChildPositioning()
        {
            return Positioning.Absolute;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            Matrix transform = Matrix.CreateTranslation(-Position.X, -Position.Y, 0.0f) *
                               Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);

            controlRenderTarget.UpdateTarget();

            controlRenderTarget.BeginRendering();

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                transform);

            base.OnDraw(spriteBatch);

            spriteBatch.End();

            controlRenderTarget.EndRendering();
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(controlRenderTarget.Target, Position, Brush.Foreground);
        }

        public void MoveView(Vector2 amount)
        {
            if (!HasContent)
            {
                return;
            }

            contentControl.Position += amount;

            OnViewMoved(GuiLayoutEventArgs.Empty, this);
        }
        public void MoveViewTo(Vector2 position)
        {
            if (!HasContent)
            {
                return;
            }

            contentControl.Position = position;

            OnViewMoved(GuiLayoutEventArgs.Empty, this);
        }
        public override void UpdateLayout(GuiLayoutEventArgs guiLayoutEventArgs)
        {
            base.UpdateLayout(guiLayoutEventArgs);

            if (LayoutSuspended)
            {
                return;
            }

            CalculateOverlapses();
        }
        public IEnumerable<Control> Childs()
        {
            if (ContentIsContainer)
            {
                for (int i = 0; i < contentContainer.ChildsCount; i++)
                {
                    yield return contentContainer.ControlAtIndex(i);
                }
            }
            else
            {
                if (ContentIsControl)
                {
                    yield return contentControl;
                }
            }
        }
    }
}
