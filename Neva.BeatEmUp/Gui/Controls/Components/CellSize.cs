using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.Controls.Components
{
    public sealed class CellSize
    {
        #region Vars
        private readonly int index;
        private readonly float percents;
        #endregion

        #region Properties
        public int Index
        {
            get
            {
                return index;
            }
        }
        public float Percents
        {
            get
            {
                return percents;
            }
        }
        #endregion

        public CellSize(int index, float percents)
        {
            this.index = index;
            this.percents = percents;
        }
    }
}
