using UnityEngine;

// ReSharper disable Unity.UnknownTag

namespace PathFinding
{
	public class Node
	{
		private GameObject _self;
		private readonly int _x;
		private readonly int _y;

		public Vector2 Position;
		public bool BadNode;
		public bool IsOccupied = false;
		public Vector2 GridPos;
    
		// Variables Holding the 8 Connections between nodes
		public NodeConnection Top;
		public NodeConnection Left;
		public NodeConnection Bottom;
		public NodeConnection Right;
		public NodeConnection TopLeft;
		public NodeConnection TopRight;
		public NodeConnection BottomLeft;	
		public NodeConnection BottomRight;

		private CompositeCollider2D floor;
		// DEBUG PURPOSES
		public NodeHelper Helper;
    
		public Node(int x, int y, Vector2 pos, CompositeCollider2D floorCollider, GameObject parent)
		{
			_x = x;
			_y = y;
			GridPos = new Vector2(x, y);
			Position = new Vector3(pos.x, pos.y);
			floor = floorCollider;


			Create(pos, parent);
		}

		public void SetColor(Color color)
		{
			_self.GetComponent<SpriteRenderer>().color = color;
		}

		private void Create(Vector3 pos, GameObject parent)
		{
			_self = Object.Instantiate(Resources.Load ("Prefabs/Node"), parent.transform, instantiateInWorldSpace:true) as GameObject;

			_self!.GetComponent<SpriteRenderer>().enabled = Grid.Instance.DEBUG_MODE;
	    
			// _self = Object.Instantiate(Resources.Load ("Prefabs/Node")) as GameObject;
			if (_self != null) _self.transform.position = pos;
			Helper = _self.GetComponent<NodeHelper>();
			Helper.Position = pos;
			Helper.X = _x;
			Helper.Y = _y;

		}
    
		public void InitializeConnections(Grid grid)
		{
			bool valid = true;
			RaycastHit2D hit;
		
			// Distance for diagonal raycast
			float diagonalDistance = Mathf.Sqrt(Mathf.Pow(Grid.NodeDistance/2f, 2) + Mathf.Pow(Grid.NodeDistance/2f, 2));
		
			// If Not first nodes from left
			if (_x > 1)
			{	
				// Left
				hit = Physics2D.Raycast(Position, new Vector2(-1, 0), Grid.NodeDistance);
				if (hit.collider != null && hit.collider.CompareTag("Wall"))
				{
					valid = false;
				}
				// x - 2 because horizontaly between to nodes there is a blank space (diamond shaped pattern in a 2D matrix)
				Left = new NodeConnection(this, grid.NodesGrid[_x - 2, _y], valid);

				// TopLeft
				if (_y > 0)
				{
					valid = true;
					hit = Physics2D.Raycast(Position, new Vector2(-1, 1), diagonalDistance);
					if (hit.collider != null && hit.collider.CompareTag("Wall"))
					{
						valid = false;
					}
					TopLeft = new NodeConnection(this, grid.NodesGrid[_x - 1, _y - 1], valid);
				}

				// BottomLeft
				if (_y < grid.height - 1)
				{
					valid = true;
					hit = Physics2D.Raycast(Position, new Vector2(-1, -1), diagonalDistance);
					if (hit.collider != null && hit.collider.CompareTag("Wall"))
					{
						valid = false;
					}			
					BottomLeft = new NodeConnection(this, grid.NodesGrid[_x - 1, _y + 1], valid);

				}
			}

			// If not last nodes from left
			if (_x < grid.width - 2)
			{
				// Right
				valid = true;
				hit = Physics2D.Raycast(Position, new Vector2(1, 0), Grid.NodeDistance);
				if (hit.collider != null && hit.collider.CompareTag("Wall"))
				{
					valid = false;
				}
				Right = new NodeConnection(this, grid.NodesGrid[_x + 2, _y], valid);

				// TopRight
				if (_y > 0)
				{
					valid = true;
					hit = Physics2D.Raycast(Position, new Vector2(1, 1), diagonalDistance);
					if (hit.collider != null && hit.collider.CompareTag("Wall"))
					{
						valid = false;
					}
					TopRight = new NodeConnection(this, grid.NodesGrid[_x + 1, _y - 1], valid);
				}

				// BottomRight
				if (_y < grid.height - 1)
				{
					valid = true;
					hit = Physics2D.Raycast(Position, new Vector2(1, -1), diagonalDistance);
					if (hit.collider != null && hit.collider.CompareTag("Wall"))
					{
						valid = false;
					}
					BottomRight = new NodeConnection(this, grid.NodesGrid[_x + 1, _y + 1], valid);
				}

			}
		
			// If not last notes from bottom
			if (_y - 1 > 0)
			{
				// Top
				valid = true;
				hit = Physics2D.Raycast(Position, new Vector2(0, 1), Grid.NodeDistance);
				if (hit.collider != null && hit.collider.CompareTag("Wall"))
				{
					valid = false;
				}		
				Top = new NodeConnection(this, grid.NodesGrid[_x, _y - 2], valid);

			}

		
			// If not first notes from bottom
			if (_y < grid.height - 2)
			{
				// Bottom
				valid = true;
				hit = Physics2D.Raycast(Position, new Vector2(0, -1), Grid.NodeDistance);
				if (hit.collider != null && hit.collider.CompareTag("Wall"))
				{
					valid = false;
				}	
				Bottom = new NodeConnection(this, grid.NodesGrid[_x, _y + 2], valid);

			}


		
		}

		public void CheckIfBadNode()
		{
			Helper.isInbounds = false;
			if (!floor.bounds.Contains(Position)) 
			{
				// Debug.Log("IS INBOUNDS");
				Helper.isInbounds = true;
				BadNode = true;
			}
	    
			int connections = 0;

			if (Top is { Valid: true })
				connections++;
			if (Bottom is { Valid: true })
				connections++;
			if (Left is { Valid: true })
				connections++;
			if (Right is { Valid: true })
				connections++;
			if (TopLeft is { Valid: true })
				connections++;
			if (TopRight is { Valid: true })
				connections++;
			if (BottomLeft is { Valid: true })
				connections++;
			if (BottomRight is { Valid: true })
				connections++;

			//If not at least 3 valid connection points - disable node
			if (connections < 3)
			{
				BadNode = true;
			}

			Helper.BadNode = BadNode;
			_self.GetComponent<SpriteRenderer>().color = BadNode ? Color.red : Color.green;
		}

		public void DrawConnections()
		{
			Top?.DrawLine ();  // Null Propagation
			Bottom?.DrawLine ();  //  = if (Bottom != null) Bottom.DrawLine()
			Left?.DrawLine ();
			Right?.DrawLine ();
			BottomLeft?.DrawLine ();
			BottomRight?.DrawLine ();
			TopRight?.DrawLine ();
			TopLeft?.DrawLine ();
		}
    
		//Remove connections that connect to bad nodes
		public void RemoveBadConnections()
		{
			if (Top != null && ((Top.Node != null && Top.Node.BadNode) || BadNode))
				Top.Valid = false;
			if (Bottom != null && ((Bottom.Node != null && Bottom.Node.BadNode) || BadNode))
				Bottom.Valid = false;
			if (Left != null && ((Left.Node != null && Left.Node.BadNode) || BadNode))
				Left.Valid = false;
			if (Right != null && ((Right.Node != null && Right.Node.BadNode) || BadNode))
				Right.Valid = false;
			if (TopLeft != null && ((TopLeft.Node != null && TopLeft.Node.BadNode) || BadNode))
				TopLeft.Valid = false;
			if (TopRight != null && ((TopRight.Node != null && TopRight.Node.BadNode) || BadNode))
				TopRight.Valid = false;
			if (BottomLeft != null && ((BottomLeft.Node != null && BottomLeft.Node.BadNode) || BadNode))
				BottomLeft.Valid = false;
			if (BottomRight != null && ((BottomRight.Node != null && BottomRight.Node.BadNode) || BadNode))
				BottomRight.Valid = false;
		}
    
    
	}
}

    



