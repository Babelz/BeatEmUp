using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public sealed class GoalDetector : GameObjectComponent
    {
        #region Vars
        private ComparisonMethod comparisonMethod;
        private Func<bool> comparisonFunc;
        
        private Vector2 position;
        private Vector2 goal;

        private bool calledAtGoal;
        #endregion

        #region Events
        public event GameObjectComponentEventHandler<GameObjectComponentEventArgs> AtGoal;
        #endregion

        #region Properties
        /// <summary>
        /// Miten goalin ja sijainnin vertailu tehdään. Default on RoundToInts.
        /// </summary>
        public ComparisonMethod ComparisonMethod
        {
            get
            {
                return comparisonMethod;
            }
            set
            {
                switch (value)
                {
                    case ComparisonMethod.Floats:
                        comparisonFunc = CompareFloats;
                        break;
                    case ComparisonMethod.RoundToInts:
                        comparisonFunc = CompareInts;
                        break;
                    default:
                        break;
                }

                comparisonMethod = value;
            }
        }
        #endregion

        public GoalDetector(GameObject owner, Vector2 goal)
            : base(owner, false)
        {
            this.goal = goal;

            ComparisonMethod = ComparisonMethod.RoundToInts;

            AtGoal += delegate { };
        }

        private bool CompareFloats()
        {
            return position.X == goal.X && position.Y == goal.Y;
        }
        private bool CompareInts()
        {
            int mX = (int)position.X;
            int mY = (int)position.Y;

            int gX = (int)goal.X;
            int gY = (int)goal.Y;

            return mX == gX && mY == gY;
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (comparisonFunc() && !calledAtGoal)
            {
                AtGoal(this, new GameObjectComponentEventArgs());

                calledAtGoal = true;
            }

            return new ComponentUpdateResults(this, true);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
        public void MoveBy(float x, float y)
        {
            position.X += x;
            position.Y += y;
        }

        public void Reset(Vector2 newGoal)
        {
            calledAtGoal = false;

            goal = newGoal;
        }
    }
}
