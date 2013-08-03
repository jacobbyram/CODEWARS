
namespace CruiseControl.Models
{
	public class Coordinate
	{
		public int X { get; set; }
		public int Y { get; set; }

	    public override bool Equals(object obj)
	    {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            var otherCoordinate = (Coordinate) obj;
            return otherCoordinate.X == this.X && otherCoordinate.Y == this.Y;
	    }

	    public override int GetHashCode()
	    {
	        return X.GetHashCode() ^ Y.GetHashCode();
	    }
	}
}