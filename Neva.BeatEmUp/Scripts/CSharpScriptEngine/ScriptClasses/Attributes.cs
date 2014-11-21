using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses
{
    public class ScriptAttribute : Attribute
    {
        #region Vars
        private readonly bool ispublic;
        #endregion

        #region Properties
        /// <summary>
        /// Näkyykö skriptin fieldit ja metodit ulospäin. 
        /// Jos tämä flägi on true, skripti voidaan kästätä
        /// olemassa olevaksi tyypiksi. Hidastaa kääntämis prosessia joten
        /// on suositeltavaa että tätä flägiä käytetään vain kun on tarve.
        /// </summary>
        public bool Ispublic
        {
            get
            {
                return ispublic;
            }
        }
        #endregion

        public ScriptAttribute(bool ispublic)
            : base()
        {
            this.ispublic = ispublic;
        }
    }
}
