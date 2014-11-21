using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Input.Trigger
{
    public class MouseTrigger : ITrigger
    {
        private readonly MouseButtons button;

        public MouseTrigger(MouseButtons button)
        {
            this.button = button;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MouseTrigger);
        }

        public override int GetHashCode()
        {
            return ("mousebuttons" + button).GetHashCode();
        }

        public bool Equals(MouseTrigger t)
        {
            if (t == null) return false;


            return t.button == button;
        }

        public int TriggerHash()
        {
            return (int) button;
        }
    }
}
