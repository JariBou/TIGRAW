using UnityEngine;
using UnityEngine.Serialization;

namespace PathFinding
{
    public enum Direction
    {
        Right,
        Left,
        Top,
        Bottom,
        BottomLeft,
        BottomRight,
        TopLeft,
        TopRight,  
    }

    public class Grid : MonoBehaviour
    {
        public const float NodeDistance = 1f;

        [HideInInspector]
        public int width;
        [HideInInspector]
        public int height;

        private static Vector2 _originStartOffset = Vector2.zero;

        public Node[,] NodesGrid;
        public CompositeCollider2D floorCollider;

        [FormerlySerializedAs("DEBUG_MODE")] public bool debugMode;
    
        public int right => width;
        public int top => height;

        public static Grid Instance;


        // Start is called before the first frame update
        private void Start()
        {
            Instance = this;
        
            Transform transform1 = transform;
            Vector3 localScale = transform1.localScale;
            width = (int)((((int)localScale.x) * 2 + 2)* (1/NodeDistance));
            height = (int)((((int)localScale.y) * 2 + 2)* (1/NodeDistance));

            Vector3 position = transform1.position;
            _originStartOffset = new Vector2(width / (4f* (1/NodeDistance)) - position.x, -(height / (4f* (1/NodeDistance)) + position.y - 1));

            NodesGrid = new Node[width, height];
        

            for (int x=0; x < (width / 2); x++) {
                for (int y=0; y < height; y++) {
                
                    //float ptx = x;
                    float ptx = x * NodeDistance;
                    //float pty = -(y/2) + (NodeDistance/2f);
                    float pty = -(y/2) * NodeDistance + (NodeDistance/2f);
                    int offsetx = 0;

                    if (y % 2 == 0)
                    {
                        //ptx = x + NodeDistance/2f;
                        ptx = x * NodeDistance + NodeDistance/2f;
                        offsetx = 1;
                    }	
                    else
                    {
                        //pty = -(y/2);
                        pty = -(y/2) * NodeDistance;
                    }
                
                    Vector2 pos = new Vector2(ptx - _originStartOffset.x, pty - _originStartOffset.y);
                    Node node = new Node(x * 2 + offsetx, y, pos, floorCollider, gameObject);
                    node.Helper.isInbounds = true;
                    NodesGrid[x*2 + offsetx, y] = node;
                }
            }
        
            // Once grid is initialized, create connections between nodes
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (NodesGrid[x,y] == null) continue;				
                    NodesGrid[x, y].InitializeConnections(this);
                }
            }	
        
            // Check if Node is Valid for connections
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (NodesGrid[x,y] == null) continue;				
                    NodesGrid[x, y].CheckIfBadNode();
                }
            }	
        
            // Remove connections to bad nodes
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (NodesGrid[x,y] == null) continue;				
                    NodesGrid[x, y].RemoveBadConnections();
                }
            }	
        
            // Then draw connections between nodes
            /*for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (NodesGrid[x,y] == null) continue;				
                NodesGrid[x, y].DrawConnections();
            }
        }*/
        
            //Debug.Log($"{WorldToGrid(GridToWorld(NodesGrid[10, 5].GridPos))}");
            //TestWorldToGrid(100);

        }

        // Transforms grid Coordinates to world Coordinates, Done, Working Easy part 
        public static Vector2 GridToWorld(Vector2 gridPosition)
        {
            Vector2 world = new Vector2(gridPosition.x / 2f, -(gridPosition.y / 2f - 0.5f));

            return world - _originStartOffset;
        }
    
        // Now onto the hard part:
        // Convert world coordinates to the grid pos of the nearest node (That has a node)
        public static Vector2 WorldToGrid(Vector2 worldPosition)
        {
            Vector2 gridPosition = new Vector2(worldPosition.x + _originStartOffset.x,
                -(worldPosition.y + _originStartOffset.y - 0.5f)) * 2f;


            //adjust to the nearest integer
            float rx = gridPosition.x % 1;
            if (rx < 0.5f)
                gridPosition.x -= rx;
            else
                gridPosition.x += (1 - rx);
		
            float ry = gridPosition.y % 1;
            if (ry < 0.5f)
                gridPosition.y -= ry;
            else
                gridPosition.y += (1 - ry);
        
            int x = (int)gridPosition.x;
            int y = (int)gridPosition.y;
            Node node = Instance.NodesGrid[x, y];
        
            //We calculated a spot between nodes'
            //Find nearest neighbor
            if ((node != null)) return gridPosition;
        
            float mag = 1000;

            // Bug here somewhere, WTF IS THIS SHITE??? nvm... just be careful of prefabs loading
        
            if (x < Instance.width && !Instance.NodesGrid[x + 1, y].BadNode)
            {
                float mag1 = (Instance.NodesGrid[x + 1, y].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Instance.NodesGrid[x + 1, y];
                }
            }
            if (y < Instance.height - 1 && !Instance.NodesGrid[x, y + 1].BadNode)
            {
                float mag1 = (Instance.NodesGrid[x, y + 1].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Instance.NodesGrid[x, y + 1];
                }
            }
            if (x > 0 && !Instance.NodesGrid[x - 1, y].BadNode)
            {
                float mag1 = (Instance.NodesGrid[x - 1, y].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Instance.NodesGrid[x - 1, y];
                }
            }
            if (y > 0 && !Instance.NodesGrid[x, y - 1].BadNode)
            {
                float mag1 = (Instance.NodesGrid[x, y - 1].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    node = Instance.NodesGrid[x, y - 1];
                }
            }


            if (node != null) return node.GridPos;
            return new Vector2(2, 1); // To implement: search of nearest node that is not bad
        }

        private void TestWorldToGrid(int number)
        {
            for (int i = 0; i < number; i++)
            {
                int x = Random.Range(0, width / 2);
                int y = Random.Range(0, width / 2);

                Vector2 worldPos = GridToWorld(new Vector2(x, y));
                Vector2 gridPos = WorldToGrid(worldPos);
                Node node = NodesGrid[(int)gridPos.x, (int)gridPos.y];
            
                if (gridPos.x == x && gridPos.y == y)
                {
                    Debug.Log($"Test: PASSED  ||  {NodesGrid[(int)gridPos.x, (int)gridPos.y]}");
                }
                else
                {
                    if (node != null)
                    {
                        Debug.Log($"Test: PASSED  ||  {NodesGrid[(int)gridPos.x, (int)gridPos.y]}");

                    } else
                    {
                        Debug.Log(
                            $"Test {i}: FAILED \nDETAILS: [x, y] = [{x}, {y}]  ||  output: [{gridPos.x}, {gridPos.y}");
                    }
                }
            


            }
        }
    
        public bool ConnectionIsValid(Point point1, Point point2)
        {
            // Comparing same point, return false
            if (point1.X == point2.X && point1.Y == point2.Y)
                return false;
		
            if (NodesGrid[point1.X, point1.Y] == null)
                return false;
		
            // Determine direction from point1 to point2
            Direction direction = Direction.Bottom;

            if (point1.X == point2.X)
            {
                if (point1.Y < point2.Y)
                    direction = Direction.Bottom;
                else if (point1.Y > point2.Y)
                    direction = Direction.Top;
            }
            else if (point1.Y == point2.Y)
            {
                if (point1.X < point2.X)
                    direction = Direction.Right;
                else if (point1.X > point2.X)
                    direction = Direction.Left;
            }
            else if (point1.X < point2.X)
            {
                if (point1.Y > point2.Y)
                    direction = Direction.TopRight;
                else if (point1.Y < point2.Y)
                    direction = Direction.BottomRight;
            }
            else if (point1.X > point2.X)
            {
                if (point1.Y > point2.Y)
                    direction = Direction.TopLeft;
                else if (point1.Y < point2.Y)
                    direction = Direction.BottomLeft;
            }

            // Check connections, If at least one connection is good then direction is valid
            return direction switch
            {
                Direction.Bottom => NodesGrid[point1.X, point1.Y].Bottom is { Valid: true },
                Direction.Top => NodesGrid[point1.X, point1.Y].Top is { Valid: true },
                Direction.Right => NodesGrid[point1.X, point1.Y].Right is { Valid: true },
                Direction.Left => NodesGrid[point1.X, point1.Y].Left is { Valid: true },
                Direction.BottomLeft => NodesGrid[point1.X, point1.Y].BottomLeft is { Valid: true },
                Direction.BottomRight => NodesGrid[point1.X, point1.Y].BottomRight is { Valid: true },
                Direction.TopLeft => NodesGrid[point1.X, point1.Y].TopLeft is { Valid: true },
                Direction.TopRight => NodesGrid[point1.X, point1.Y].TopRight is { Valid: true },
                // ReSharper disable once UnreachableSwitchArmDueToIntegerAnalysis
                _ => false,
            };
        }
    
        /*
    // DEBUG
    public Transform player;
    public LineRenderer lr;
    public Transform ennemy;

    [SerializeField]
    private float timer;

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= 1)
        {
            timer = 0;
            //Convert player point to grid coordinates
            Vector2 playerPos = WorldToGrid(Player.instance.GetPosition());

            //Find path from player to clicked position
            //BreadCrumb bc = PathFinder.FindPath(this,new Point(WorldToGrid(ennemy.transform.position)), new Point(playerPos));
            BreadCrumb bc = PathFinder.FindPath(this,new Point(WorldToGrid(ennemy.transform.position)), new Point(playerPos));
            //ennemy.transform.position = GridToWorld(next.Position);
            //bc = next; 
            
            int count = 0;
            lr.SetVertexCount(100); //Need a higher number than 2, or crashes out
            lr.SetWidth(0.1f, 0.11f);
            lr.SetColors(Color.yellow, Color.yellow);

            //Draw out our path
            while (bc != null)
            {
                Vector3 pos = GridToWorld(bc.Position);
                lr.SetPosition(count, pos);
                bc = bc.Next;
                count += 1;
            }

            lr.SetVertexCount(count);
        }
        
    } */

    }
}