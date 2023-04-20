using UnityEngine;

namespace PathFinding
{
	public static class PathFinder
	{     
		// Returns the BreadCrumb Path
		public static BreadCrumb FindPath(Grid world, Point start, Point end)
		{        
			BreadCrumb bc = FindPathReversed(world, start, end);
			BreadCrumb[] temp = new BreadCrumb[256];

			if (bc != null)
			{
				int index = 0;
				while (bc != null)
				{
					temp[index] = bc;
					bc = bc.Next;
					index++;
				}

				index -= 2;

				BreadCrumb current = new BreadCrumb(start);
				BreadCrumb head = current;

				while (index >= 0)
				{                
					current.Next = new BreadCrumb(temp[index].Position);
					current = current.Next;
					index--;              
				}
				return head;
			}

			return null;
		}
	
		// Since we are going from the starting point in a FILO way we get the path reversed  
		private static BreadCrumb FindPathReversed(Grid world, Point start, Point end)
		{
			MinHeap<BreadCrumb> openList = new MinHeap<BreadCrumb>(256);
			// Create a version of the nodeGrid with BreadCrumbs instead that will contain the best possible path for their pos
			// I mean for now it's empty, but that's kind of the end goal, like that's how the algorithm works lol
			BreadCrumb[,] brWorld = new BreadCrumb[world.right, world.top];
			BreadCrumb node;
			Point tmp;
			int cost;
			int diff;
	
			BreadCrumb current = new BreadCrumb(start)
			{
				Cost = 0
			};

			BreadCrumb finish = new BreadCrumb(end);
			brWorld[current.Position.X, current.Position.Y] = current;
			openList.Add(current);
	
			while (openList.count > 0)
			{
				// Find best item and switch it to the closedList
				// closedList means that the best path for that Spot has been found
				current = openList.ExtractFirst();
				current.OnClosedList = true;
	
				// Find neighbours
				foreach (Point point in Surrounding)
				{
					tmp = new Point(current.Position.X + point.X, current.Position.Y + point.Y);
				
					// If The neighbour's position is valid (like if it has at least one valid connection)
					if (!world.ConnectionIsValid(current.Position, tmp))
					{
						continue;
					}
		        
					// Check if node is occupied
					if (world.NodesGrid[tmp.X, tmp.Y].IsOccupied)
					{
						continue;
					}
		        
					// Check if we've already examined the neighbour (if it has already a BreadCrumb), if not create new node
					if (brWorld[tmp.X, tmp.Y] == null)
					{
						node = new BreadCrumb(tmp);
						brWorld[tmp.X, tmp.Y] = node;
					}
					else
					{
						node = brWorld[tmp.X, tmp.Y];
					}

					// If the node is on the 'closedList' pass because we know that the best path is already assigned to it
					if (node.OnClosedList) continue;
		        
					// If the node is not on the 'closedList' check if new path is faster than the old one
					diff = 0;
					if (current.Position.X != node.Position.X)
					{
						diff += 1;
					}
					if (current.Position.Y != node.Position.Y)
					{
						diff += 1;
					}
		        
					// Calculate distance from destination
					int distance = (int)Mathf.Pow(Mathf.Max(Mathf.Abs (end.X - node.Position.X), Mathf.Abs(end.Y - node.Position.Y)), 2);
					cost = current.Cost + diff + distance;
				
					// If cost is less than before, reassign it and assign it's path next node to the current one
					if (cost < node.Cost)
					{
						node.Cost = cost;
						node.Next = current;
					}

					//If the node is already on OpenList continue
					if (node.OnOpenList) continue;
		        
					// If the node wasn't on the openList yet, add it 
		        
					// Check to see if we're done
					if (node.Equals(finish))
					{
						node.Next = current;
						return node; // Return BreadCrumb Path
					}
					node.OnOpenList = true;
					openList.Add(node);
				}
			}
			return null; //no path found
		}
	
		// The diamond pattern offsets top/bottom/left/right by 2 instead of 1 (Same case than Raycast2D)
		// Returns surrounding Points
		private static readonly Point[] Surrounding = {                         
			new Point(0, 2), new Point(-2, 0), new Point(2, 0), new Point(0,-2),	
			new Point(-1, 1), new Point(-1, -1), new Point(1, 1), new Point(1, -1)
		};
	}


// Point class created to hold the grid positions (should theoretically improve performance since not using Vector2
// that has a shit tons of other methods and all)
	public class Point
	{
		public int X, Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}
    
		public Point(Vector2 position)
		{
			X = (int)position.x;
			Y = (int)position.y;
		}
    
		public static implicit operator Vector2(Point point) => new Vector2(point.X, point.Y);
		public static implicit operator Vector3(Point point) => new Vector3(point.X, point.Y, 0);
	}
}