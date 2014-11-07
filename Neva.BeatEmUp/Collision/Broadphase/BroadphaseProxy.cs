using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.Collision.Dynamics;

namespace Neva.BeatEmUp.Collision.Broadphase
{
    /// <summary>
    /// Proxy joka toimii broadphase stagella collision detectionin kanssa 
    /// ja wrappaa paskaa itseensä, toimii ns. cachena
    /// </summary>
    [DebuggerDisplay("ID = {ProxyID}")]
    public class BroadphaseProxy
    {
        // TODO DIRTY DIRTY DIRTY HACK
        public static int ProxyCounter = 0;
        public int ProxyID;
        /// <summary>
        /// Broadphasen AABB, tavallaan sama kuin gameObject.Body.GetAABB()
        /// </summary>
        public AABB AABB;

        /// <summary>
        /// Mihin collision ryhmään tämä proxy kuuluu
        /// </summary>
        public CollisionGroup CollisionGroup;

        /// <summary>
        /// Kenen kanssa collision otetaan huomioon
        /// </summary>
        public CollisionGroup CollisionFilterGroup;


        /// <summary>
        /// Kenen proxy tämä on, yleensä body
        /// </summary>
        public Body Client;
        

        /// <summary>
        /// Voiko törmätä toiseen otukseen
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True jos voi, false muuten</returns>
        public bool ShouldCollide(BroadphaseProxy other)
        {
            return (CollisionGroup & other.CollisionFilterGroup) != CollisionGroup.None;
        }

        public static BroadphaseProxy Create(Body client)
        {
            return Create(client, CollisionGroup.Group1, CollisionGroup.All);
        }

        public static BroadphaseProxy Create(Body client, CollisionGroup group, CollisionGroup mask)
        {
            BroadphaseProxy proxy = new BroadphaseProxy
            {
                Client = client,
                CollisionFilterGroup = mask,
                CollisionGroup = group,
                ProxyID = ProxyCounter++
            };
            // ei tiedetä AABB vielä, broadphasen tehtävä selvittää se
            return proxy;
        }


    }
}
