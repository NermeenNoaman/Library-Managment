// In HRSystem.Infrastructure/Utilities/HaversineHelper.cs
using System;

namespace HRSystem.Infrastructure.Utilities
{
    public static class HaversineHelper
    {
        // Earth's mean radius in meters
        private const double EarthRadiusInMeters = 6371000.0;

        /// <summary>
        /// Calculates the distance between two geographical points using the Haversine formula.
        /// </summary>
        /// <returns>Distance in meters.</returns>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusInMeters * c;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}