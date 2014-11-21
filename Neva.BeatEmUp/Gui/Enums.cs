using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui
{
    public enum InputSource
    {
        Gamepad,
        Mouse,
        Keyboard
    }

    public enum SizeValueType
    {
        /// <summary>
        /// Koko tulee asettaa pikseleinä.
        /// </summary>
        Fixed,
        /// <summary>
        /// Koko tulee asettaa prosentteina (vakioarvo)
        /// </summary>
        Percents
    }

    /// <summary>
    /// Kontrollin asettelu horisontaalisesti (X)
    /// </summary>
    public enum Horizontal
    {
        /// <summary>
        /// Asettelu vasemmalta.
        /// </summary>
        Left,
        /// <summary>
        /// Keskitys.
        /// </summary>
        Center,
        /// <summary>
        /// Asettelu oikealta.
        /// </summary>
        Right,
        /// <summary>
        /// Venyttää kontrollin (vakioarvo)
        /// </summary>
        Stretch
    }

    /// <summary>
    /// Kontrollin asettelu vertikaalisesti (Y)
    /// </summary>
    public enum Vertical
    {
        /// <summary>
        /// Asettelu ylhäältä alas.
        /// </summary>
        Top,
        /// <summary>
        /// Keskitys.
        /// </summary>
        Center,
        /// <summary>
        /// Asettelu alhaalta ylös.
        /// </summary>
        Bottom,
        /// <summary>
        /// Venyttää kontrollin (vakioarvo)
        /// </summary>
        Stretch
    }

    /// <summary>
    /// Kontrollin sijainnin tyyppi.
    /// </summary>
    public enum Positioning
    {
        /// <summary>
        /// Absoluuttinen, sijanti voidaan määritellä itse.
        /// </summary>
        Absolute,
        /// <summary>
        /// Relatiivinen, sijainti lasketaan parentista.
        /// </summary>
        Relative
    }

    /// <summary>
    /// Voiko kontrollin kokoa muokata jos sijainnin määrittely
    /// on relatiivinen.
    /// </summary>
    public enum SizeBehaviour
    {
        /// <summary>
        /// Leveys voidaan ylikirjoitetaan.
        /// </summary>
        OverwriteWidth,
        /// <summary>
        /// Korkeus voidaan ylikirjoitetaan.
        /// </summary>
        OverwriteHeight,
        /// <summary>
        /// Molemmat arvot voidaan ylikirjoitetaan (vakioarvo)
        /// </summary>
        OverwriteBoth,
        /// <summary>
        /// Arvoja ei voi ylikirjoiteta.
        /// </summary>
        NoOverwrites 
    }

    public enum ScrollbarType
    {
        Horizontal,
        Vertical
    }

    public enum MouseButtons
    {
        LeftButton = 0,
        RightButton,
        MiddleButton,
        XButton1,
        XButton2
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
