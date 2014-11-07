using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Collision.Broadphase;
using Neva.BeatEmUp.GameObjects;

namespace Neva.BeatEmUp.Collision.Dynamics
{
    [Flags]
    public enum CollisionFlags
    {
        /// <summary>
        /// Ei ota response vectoria collisionista vastaan, aka työnnä pois objectista
        /// </summary>
        Solid,
        /// <summary>
        /// Ottaa response vektorin ja työntyy poiis
        /// </summary>
        Response,
        /// <summary>
        /// Ei katsota broadphasella ollenkaan
        /// TODO: ei käytetä vielä
        /// </summary>
        Ignore,
        /// <summary>
        /// Tähän ei voi collidata mutta triggeröi eventit
        /// Hyvä käyttää esim. powerupeissa
        /// </summary>
        Sensor
    }

    [Flags]
    public enum BodyType
    {
        /// <summary>
        /// Tämä body ei liiku
        /// TODO ei käytetä
        /// </summary>
        Static,
        /// <summary>
        /// Tämä body voi liiikkua
        /// TODO ei käytetä
        /// </summary>
        Dynamic
    }
    public class Body
    {
        #region Dirty Hackzz
        #region Callbacks

        /// <summary>
        /// Kutsutaan kun oliot ovat törmäämässä broadphasella, ei niin tarkka koska AABB
        /// </summary>
        public BeforeCollisionEventHandler BeforeCollision;

        /// <summary>
        /// Kutsutaan kun kaksi oliota törmäävät 
        /// </summary>
        public OnCollisionEventHanlder OnCollision;

        /// <summary>
        /// Kutsutaan kun kaksi oliota ovat törmänneet ja niiden törmäys on käsitelty (a.k.a solved)
        /// TODO howto
        /// </summary>
        public AfterCollisionEventHandler AfterCollision;

        /// <summary>
        /// Kutsutaan kun kaksi otusta ovat törmänneet ja niiden kontakti on poistettu
        /// TODO: howto
        /// </summary>
        public OnSeparatioinEventHandler OpSeparation;

        #endregion
        #endregion

        #region Vars
        private Transform transform;
        
        #endregion

        #region Properties

        public GameObject Owner
        {
            get; private set;
        }

        /// <summary>
        /// TODO: kommentoi paremmin 
        /// Tähän ei voi collidata mutta triggeröi eventit
        /// </summary>
        public bool IsSensor
        {
            get
            {
                return (CollisionFlags & CollisionFlags.Sensor) == CollisionFlags.Sensor;
            }
        }

        /// <summary>
        /// Proxy, viite vittu mikä lie, mitä käytetään broadphasessa etsimään collisioneja ja pareja
        /// </summary>
        public BroadphaseProxy BroadphaseProxy
        {
            get;
            set;
        }

        public BodyType BodyType
        {
            get;
            set;
        }

        public CollisionFlags CollisionFlags
        {
            get;
            set;
        }

        public IShape Shape
        {
            get;
            private set;
        }
        public float Rotation
        {
            get
            {
                return transform.Rotation.Angle;
            }
            set
            {
                transform.Rotation.Set(value);
            }
        }

        /// <summary>
        /// Onko liikkunut tällä framella
        /// TODO: not implemnted
        /// </summary>
        public bool IsAwake
        {
            get;
            set;
        }

        /// <summary>
        /// Nykyinen vauhti
        /// </summary>
        public Vector2 Velocity
        {
            get;
            set;
        }

        /// <summary>
        /// Rotation velocity
        /// TODO not implemented
        /// </summary>
        public float AngularVelocity
        {
            get;
            set;
        }
        public Vector2 Position
        {
            get
            {
                return transform.Position;
            }
            set
            {
                transform.Position = value;
            }
        }
        public Transform Transform
        {
            get
            {
                return transform;
            }
        }
        #endregion

        /// <summary>
        /// Alustaa uuden bodyn dynaamisena bodyna
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="shape"></param>
        /// <param name="position"></param>
        public Body(GameObject owner, IShape shape, Vector2 position)
        {
            Owner = owner;
            Shape = shape;
            BodyType = BodyType.Dynamic;
            CollisionFlags = CollisionFlags.Response;
            transform = new Transform();
            transform.Set(position, 0);
        }

        #region Methods

        public void OnAdded()
        {

        }

        public void OnRemoved()
        {
            // clean up
        }

        public void SetCollisionGroup(CollisionGroup group)
        {
            BroadphaseProxy.CollisionGroup = group;
        }

        public void CollidesWithGroup(CollisionGroup mask)
        {
            BroadphaseProxy.CollisionFilterGroup = mask;
        }

        public bool BelongsToGroup(CollisionGroup group)
        {
            return (BroadphaseProxy.CollisionFilterGroup & group) != CollisionGroup.None;
        }

        public AABB GetAABB()
        {
            AABB aabb;
            Shape.ComputeAaab(ref transform, out aabb);
            return aabb;
        }

        #endregion
    }
}
