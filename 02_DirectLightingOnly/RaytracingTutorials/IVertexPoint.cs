using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    interface IVertexPoint
    {
        Vector Position { get; }
        Vector Normal { get; }
    }
}
