using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Neva.BeatEmUp.Input.Trigger
{
    public sealed class ButtonTrigger : ITrigger
    {
        private readonly Buttons button;

        public ButtonTrigger(Buttons button)
        {
            this.button = button;
        }

        public int TriggerHash()
        {
            return (int) button;
        }

        public override bool Equals(object obj)
        {
            var trigger = obj as ButtonTrigger;
            if (trigger == null) return false;
            return trigger.button == button;
        }

        public override int GetHashCode()
        {
            return ("button" + TriggerHash()).GetHashCode();
        }
    }
}
