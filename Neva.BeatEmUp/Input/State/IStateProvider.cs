﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Input.State
{
    public interface IStateProvider
    {
        /// <summary>
        /// Päivittää statea
        /// </summary>
        void Update();
    }
}
