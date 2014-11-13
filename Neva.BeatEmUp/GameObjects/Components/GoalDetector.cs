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
        private Conditional conditional;
        private ComparisonMethod comparisonMethod;

        private Func<float, float, float, float, bool> conditionalFunc;
        
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
                comparisonMethod = value;
            }
        }
        /// <summary>
        /// Conditionaali joka määrää onko goaliin saavutettu. Vakio on Bigger.
        /// </summary>
        public Conditional Conditional
        {
            get
            {
                return conditional;
            }
            set
            {
                switch (conditional)
                {
                    case Conditional.Bigger:
                        conditionalFunc = Bigger;
                        break;
                    case Conditional.Smaller:
                        conditionalFunc = Smaller;
                        break;
                    case Conditional.Equal:
                        conditionalFunc = Equal;
                        break;
                    default:
                        break;
                }

                conditional = value;
            }
        }
        #endregion

        public GoalDetector(GameObject owner, Vector2 goal)
            : base(owner, false)
        {
            this.goal = goal;

            ComparisonMethod = ComparisonMethod.RoundToInts;
            Conditional = Components.Conditional.Bigger;

            AtGoal += delegate { };
        }

        private void RoundToInts(ref float mX, ref float mY, ref float gX, ref float gY)
        {
            mX = (int)position.X;
            mY = (int)position.Y;

            gX = (int)goal.X;
            gY = (int)goal.Y;
        }

        #region Conditionals
        private bool Bigger(float mX, float mY, float gX, float gY)
        {
            return mX > gX && mY > gY;
        }
        private bool Smaller(float mX, float mY, float gX, float gY)
        {
            return mX < gX && mY < gY;
        }
        private bool Equal(float mX, float mY, float gX, float gY)
        {
            return mX == gX && mY == gY;
        }
        private bool ExecuteConditional()
        {
            float mX = position.X;
            float mY = position.Y;

            float gX = goal.X;
            float gY = goal.Y;

            if (comparisonMethod == Components.ComparisonMethod.RoundToInts)
            {
                RoundToInts(ref mX, ref mY, ref gX, ref gY);
            }

            return conditionalFunc(mX, mY, gX, gY);
        }
        #endregion

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (ExecuteConditional() && !calledAtGoal)
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
