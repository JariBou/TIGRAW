using UnityEngine;

namespace PathFinding
{
    public class NodeConnection
    {

        public bool Valid;
        public readonly Node Node;
        private readonly Node _parent;
        private GameObject _line;
    
    
        public NodeConnection(Node parentNode, Node linkedNode, bool isValid)
        {
            Valid = isValid;
            Node = linkedNode;	
            _parent = parentNode;
        
            if (linkedNode is { BadNode: true }) Valid = false;
            if (parentNode is { BadNode: true }) Valid = false;
            //if (linkedNode.Position.Equals(new Vector2(-0.5f, 11))) Debug.Log($"{Valid}");
        }
    
        public void DrawLine()
        {
            if (!Valid) return;
            if (_parent == null || Node == null) return;
        
            // ReSharper disable once Unity.UnknownResource
            _line = Object.Instantiate (Resources.Load ("Prefabs/Line"), _parent.Self.transform) as GameObject;
            if (_line == null) return;
            LineRenderer lineRenderer = _line.GetComponent<LineRenderer> ();
				
            lineRenderer.SetPosition (0, _parent.Position);
            lineRenderer.SetPosition (1, Node.Position);
//        Debug.Log($"node_pos={_node.position}");
            lineRenderer.startWidth = 0.06f;
            lineRenderer.endWidth = 0f;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }

    }
}
