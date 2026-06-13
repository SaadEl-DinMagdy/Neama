using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.BranchSpecification
{
    public static class LocationHelper
    {
        private static readonly GeometryFactory _geometryFactory =
        NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        public static Point GetUserPoint(double lng, double lat)
        {
            var userLocation = _geometryFactory.CreatePoint(new Coordinate(lng, lat));
            return userLocation;

        }
    }
}
