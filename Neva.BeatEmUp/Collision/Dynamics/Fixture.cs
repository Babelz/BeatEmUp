using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neva.BeatEmUp.GameObjects;

namespace Neva.BeatEmUp.Collision.Dynamics
{
    /// <summary>
    /// Yhdistää bodyn ja shapen
    /// TODO: Tähän vaihdetaan joskus
    /// </summary>
    public class Fixture
    {
        #region Vars

        private static int FixtureIdCounter;

        #endregion

        #region Callback

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
        /// </summary>
        public AfterCollisionEventHandler AfterCollision;

        /// <summary>
        /// Kutsutaan kun kaksi otusta ovat törmänneet ja niiden kontakti on poistettu
        /// </summary>
        public OnSeparatioinEventHandler OpSeparation;

        #endregion

        #region Properties

        public Body Body
        {
            get;
            private set;
        }

        public IShape Shape
        {
            get;
            private set;
        }

        /// <summary>
        /// Uniikki id
        /// </summary>
        public int FixtureId
        {
            get;
            private set;
        }

        #endregion

        public Fixture(Body body, IShape shape, GameObject owner)
        {
            Body = body;
            Shape = shape;
            FixtureId = FixtureIdCounter++;
        }
    }
}
