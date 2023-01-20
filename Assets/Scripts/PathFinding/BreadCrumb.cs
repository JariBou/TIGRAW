using System;

// IComparable<BreadCrumb> To be able to override the default equals C# method
// A BreadCrumb represents the path taken to get to where it is (in the grid in this case)
namespace PathFinding
{
    public class BreadCrumb : IComparable<BreadCrumb>
    {	
        public readonly Point Position;
        public BreadCrumb Prev;
        public BreadCrumb Next;
        public int Cost = Int32.MaxValue;
        public bool OnClosedList = false;
        public bool OnOpenList = false;

        public BreadCrumb(Point position)
        {
            Position = position;
        }
		
        //Overrides default Equals so we check on position instead of object memory location
        public override bool Equals(object obj)
        {
            return (obj is BreadCrumb crumb) && crumb.Position.X == Position.X && crumb.Position.Y == Position.Y;
        }
	
        //Faster Equals for if we know something is a BreadCrumb
        public bool Equals(BreadCrumb breadcrumb)
        {
            return breadcrumb.Position.X == Position.X && breadcrumb.Position.Y == Position.Y;
        }
	
        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
	
        #region IComparable<> interface
        public int CompareTo(BreadCrumb other)
        {
            return Cost.CompareTo(other.Cost);
        }
        #endregion
    }
}