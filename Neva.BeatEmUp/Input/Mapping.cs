using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Input.Listener;
using Neva.BeatEmUp.Input.Trigger;

namespace Neva.BeatEmUp.Input
{
    public sealed class Mapping
    {
        public string MappingName { get; private set; }
        public InputEvent Callback { get; private set; }
        public List<ITrigger> Triggers { get; private set; }

        public Mapping(string mappingName, InputEvent callback)
        {
            MappingName = mappingName;
            Callback = callback;
            Triggers = new List<ITrigger>();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Mapping);
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int res = MappingName.GetHashCode();
            foreach (var trigger in Triggers)
            {
                res = res * prime + trigger.GetHashCode();
            }
            res += Callback.GetHashCode();
            return res;
        }

        public bool Equals(Mapping other)
        {
            if (other == null) return false;

            return MappingName == other.MappingName && Callback == other.Callback && Triggers.SequenceEqual(other.Triggers);
        }
    }
}
