using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public enum ComparisonMethod
    {
        /// <summary>
        /// Suora vertaus vektorien x ja y komponenttien välillä.
        /// </summary>
        Floats,

        /// <summary>
        /// Pyöristetään vektorien x ja y komponentit inteiksi ja verrataan.
        /// </summary>
        RoundToInts
    }

    public enum Conditional
    {
        /// <summary>
        /// Sijainnin tule olla suurempi kuin goali.
        /// </summary>
        Bigger,

        /// <summary>
        /// Sijainnin tulee olla pienempi kuin goali.
        /// </summary>
        Smaller,

        /// <summary>
        /// Sijainnin tulee olla sama kuin goalin.
        /// </summary>
        Equal
    }
}
