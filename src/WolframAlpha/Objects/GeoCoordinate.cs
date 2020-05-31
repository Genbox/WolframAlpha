namespace Genbox.WolframAlpha.Objects
{
    public class GeoCoordinate
    {
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public override string ToString()
        {
            return Latitude + "," + Longitude;
        }
    }
}