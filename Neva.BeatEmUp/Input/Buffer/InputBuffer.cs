using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Input.Buffer
{
    public abstract class InputBuffer
    {
        #region Properties

        public Dictionary<string, Mapping> ReleasedMappings
        {
            get;
            protected set;
        }

        public Dictionary<string, Mapping> PressedMappings
        {
            get;
            protected set;
        }

        public Dictionary<string, Mapping> DownMappings
        {
            get;
            protected set;
        }

        #endregion

        #region Ctor

        protected InputBuffer()
        {
            ReleasedMappings = new Dictionary<string, Mapping>();
            PressedMappings = new Dictionary<string, Mapping>();
            DownMappings = new Dictionary<string, Mapping>();
        }

        #endregion

        #region Abstract
        public abstract void Update();
        #endregion
    }
}
