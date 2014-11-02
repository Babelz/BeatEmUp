using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components.AI
{
    public sealed class FiniteStateMachine : GameObjectComponent
    {
        #region Vars
        private readonly Stack<Action> states;
        #endregion

        #region Properties
        public bool HasStates
        {
            get
            {
                return states.Count > 0;
            }
        }
        #endregion

        public FiniteStateMachine(GameObject owner)
            : base(owner, false)
        {
            states = new Stack<Action>();
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            if (HasStates)
            {
                states.Peek()();
            }

            return new ComponentUpdateResults(this, true);
        }

        public void PushState(Action state)
        {
            states.Push(state);
        }
        public Action PopState()
        {
            if (HasStates)
            {
                return states.Pop();
            }

            return null;
        }
    }
}
