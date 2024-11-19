namespace RadarLibrary
{
    public class Target
    {
        public int Id { get; set; }
        public double Distance { get; set; }
        public double Angle { get; set; }
        public double Velocity { get; set; }

        public Target(int id, double distance, double angle, double velocity)
        {
            Id = id;
            Distance = distance;
            Angle = angle;
            Velocity = velocity;
        }

        public override string ToString()
        {
            return $"Target ID: {Id}, Distance: {Distance:F2}, Angle: {Angle:F2}, Velocity: {Velocity:F2}";
        }
    }
}
