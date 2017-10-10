using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    interface IPoint
    {
        float this[int dimension] { get; }
    }
}
