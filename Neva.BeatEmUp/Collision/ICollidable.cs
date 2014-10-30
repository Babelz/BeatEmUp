using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Collision.Broadphase;
using Neva.BeatEmUp.Collision.Dynamics;

namespace Neva.BeatEmUp.Collision
{
    internal interface ICollidable
    {
        Body Body
        {
            get;
        }
        //BroadphaseProxy BroadphaseProxy { get; set; }
    }
}
