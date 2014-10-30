using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Collision.Broadphase;

namespace Neva.BeatEmUp.Collision
{
    [DebuggerDisplay("ProxyA {ProxyA.ProxyID}, ProxyB {ProxyB.ProxyID}")]
    internal struct CollisionPair : IEquatable<CollisionPair>, IComparable<CollisionPair>
    {
        public BroadphaseProxy ProxyA
        {
            get;
            private set;
        }

        public BroadphaseProxy ProxyB
        {
            get;
            private set;
        }
        public CollisionPair(BroadphaseProxy proxyA, BroadphaseProxy proxyB) : this()
        {
            ProxyA = proxyA;
            ProxyB = proxyB;
        }

        public bool Equals(CollisionPair other)
        {
            return (ProxyA == other.ProxyA && ProxyB == other.ProxyB);
        }

        public int CompareTo(CollisionPair other)
        {
            // jotta voidaan poistaa duplikaatit, helposti?

            if (ProxyA.ProxyID < other.ProxyA.ProxyID)
            {
                return -1;
            }
            if (ProxyA.ProxyID == other.ProxyA.ProxyID)
            {
                if (ProxyB.ProxyID < other.ProxyB.ProxyID)
                {
                    return -1;
                }
                if (ProxyB.ProxyID == other.ProxyB.ProxyID)
                {
                    return 0;
                }
            }

            return 1;
        }

        public override bool Equals(object obj)
        {
            bool r = false;
            if (obj is CollisionPair)
                r = Equals((CollisionPair) obj);
            return r;
        }

        public override int GetHashCode()
        {
            return ProxyA.GetHashCode() + ProxyB.GetHashCode();
        }
    }
}
