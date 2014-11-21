using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.Gui.Transitions
{
    public sealed class ModernTransition : Transition
    {
        #region Constants
        private const float VELOCITY_MODIFIER = 1f;
        private const int FRAMES_TO_BREAK = 20;
        private const float MAX_VELOCITY = 50f;
        #endregion

        #region Vars
        private readonly Direction direction;
        private readonly Action accelerate;
        private readonly Action brake;

        private Action velocityFunc;
        private float velocity;
        #endregion

        public ModernTransition(Direction direction)
            : base()
        {
            this.direction = direction;

            brake = Brake;
            accelerate = Accelerate;
        }

        private void Brake()
        {
            if (velocity > 1f)
            {
                velocity -= VELOCITY_MODIFIER;
            }
        }
        private void Accelerate()
        {
            if (velocity < MAX_VELOCITY)
            {
                velocity += VELOCITY_MODIFIER;
            }
        }

        private void UpdateVelocityFunc()
        {
            switch (direction)
            {
                case Direction.Left:
                    velocityFunc = Next.Position.X <= -(velocity * FRAMES_TO_BREAK) - 1f ? accelerate : brake;
                    break;
                case Direction.Right:
                    velocityFunc = Next.Position.X >= velocity * FRAMES_TO_BREAK ? accelerate : brake;
                    break;
                case Direction.Up:
                    velocityFunc = Next.Position.Y <= -(velocity * FRAMES_TO_BREAK) - 1f ? accelerate : brake;
                    break;
                case Direction.Down:
                    velocityFunc = Next.Position.Y >= velocity * FRAMES_TO_BREAK ? accelerate : brake;
                    break;
                default:
                    break;
            }
        }
        private void ApplyVelocity()
        {
            switch (direction)
            {
                case Direction.Left:
                    Next.Position = new Vector2(Next.Position.X + velocity, Next.Position.Y);
                    Current.Position = new Vector2(Current.Position.X + velocity, Current.Position.Y);
                    break;
                case Direction.Right:
                    Next.Position = new Vector2(Next.Position.X - velocity, Next.Position.Y);
                    Current.Position = new Vector2(Current.Position.X - velocity, Current.Position.Y);
                    break;
                case Direction.Up:
                    Next.Position = new Vector2(Next.Position.X, Next.Position.Y + velocity);
                    Current.Position = new Vector2(Current.Position.X + velocity, Current.Position.Y + velocity);
                    break;
                case Direction.Down:
                    Next.Position = new Vector2(Next.Position.X, Next.Position.Y - velocity);
                    Current.Position = new Vector2(Current.Position.X + velocity, Current.Position.Y - velocity);
                    break;
                default:
                    break;
            }
        }
        private bool IsFinished()
        {
            if(direction == Direction.Up || direction == Direction.Down)
            {
                return Next.Position.Y == 0.0f;
            }
            else 
            {
                return Next.Position.X == 0.0f;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateVelocityFunc();

            velocityFunc();

            ApplyVelocity();

            if (IsFinished())
            {
                Finish();
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            Next.Draw(spriteBatch);
            Current.Draw(spriteBatch);
        }

        public override void Start()
        {
            // Asetetaan aloitus sijainti seuraavalle ikkunalle.
            switch (direction)
            {
                case Direction.Left:
                    Next.Position = new Vector2(-Current.SizeInPixels.X, 0.0f);
                    break;
                case Direction.Right:
                    Next.Position = new Vector2(Current.SizeInPixels.X, 0.0f);
                    break;
                case Direction.Up:
                    Next.Position = new Vector2(Current.Position.X, -Current.SizeInPixels.Y);
                    break;
                case Direction.Down:
                    Next.Position = new Vector2(Current.Position.X, Current.SizeInPixels.Y);
                    break;
                default:
                    break;
            }

            Current.UpdateLayout(null);
            Next.UpdateLayout(null);

            Current.Disable();
            Next.Disable();
        }
    }
}
