using PathFinding;
using PlayerBundle;
using UnityEngine;
using Grid = PathFinding.Grid;

namespace Enemies.EnemiesAI
{
    public class AIFlyingScript : AIDad
    {
        private Vector2 _targetPos;
        private Vector2 _previousPos;

        private Vector3 _currentPosition;
        [SerializeField]
        private float updateFrameCount;
        
        public Rigidbody2D rb;
        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _enemyInstance = GetComponent<EnemyInterface>();
            targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _animator = _enemyInstance.animator;
            var position = transform.position;
            _targetPos = position;
            _previousPos = position;
        }
    
        void Update()
        {
            if (_enemyInstance.isInRange) {     // Leave it here because ranged and melee will not behave the same       
                _previousPos = transform.position;
                
                
                AttackPlayer();
                
                return;
            }
            updateFrameCount += 1;
            if (updateFrameCount >= updateRate)
            {
                UpdatePath();
                updateFrameCount = 0;
            }
        }

        private void AttackPlayer()
        {
            if (_enemyInstance.DmgInteractionTimer > 0)
            {
                return;
            }
            else
            {
                Debug.Log("ATTACKING FUCKER");
                _animator.SetTrigger(Attack);
                targetPlayer.Damage(_enemyInstance.attack);
                _enemyInstance.InitInteractionTimer();
            }

        }

        void FixedUpdate()
        {
            
        

        
        
        
            // Crashes Unity basically....
            /*Point start = new Point(Grid.WorldToGrid(transform.position));
        Point end = new Point(
            Grid.WorldToGrid(targetEntity.transform.position));
        _path = PathFinder.FindPath(Grid.Instance, start, end);
        _targetPos = Grid.GridToWorld(_path.Next.Position);
        Vector2 direction = (_targetPos - new Vector2(transform.position.x, transform.position.y));
        direction /= direction.magnitude;

        rb.velocity = direction * speed;
        _previousPos = transform.position;
        _path = _path.Next;
        
        while ((_targetPos - new Vector2(transform.position.x, transform.position.y)).magnitude > 0.2)
        {
            
        }*/
        
            /*transform.position = new Vector2(Mathf.Lerp(_previousPos.x,_targetPos.x, timer/(10 / speed)),
            Mathf.Lerp(_previousPos.y,_targetPos.y, timer/(10 / speed)));
        timer += 1/speed;
        if (timer >= 10/speed)
        {
            timer = 0;
            Point start = new Point(Grid.WorldToGrid(transform.position));
            Point end = new Point(
                Grid.WorldToGrid(targetEntity.transform.position - new Vector3(0, -0.88f, 0))); /* Try and make it less... Arbitrary #1#
            _path = PathFinder.FindPath(Grid.Instance, start, end);
            _targetPos = Grid.GridToWorld(_path.Next.Position);
            
            
            
            // MAKE NODE CLOSEST TO PLAYER UN-OCCUPIABLE ELSE THEM BOYS GO CRAZY
            if (Grid.Instance.NodesGrid[_path.Next.Position.X, _path.Next.Position.Y] != Grid.Instance.NodesGrid[end.X, end.Y])
            {
                Grid.Instance.NodesGrid[_path.Next.Position.X, _path.Next.Position.Y].IsOccupied = true;
            }
            Grid.Instance.NodesGrid[start.X, start.Y].IsOccupied = false;
            _previousPos = transform.position;
            _path = _path.Next;
            // TODO: Implement when ennemy arrives at player (raises error and ennemy just goes crazy)
        }*/

        




        }

        private void UpdatePath()
        {
            Vector2 direction = (targetPlayer.transform.position - transform.position);
            rb.velocity = direction.normalized * _enemyInstance.Speed;
            _enemyInstance.renderer.flipX = direction.x < 0;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Enemy")) {return;}

            if (col.gameObject.CompareTag("Player"))
            {
                rb.AddForce(Vector2.MoveTowards(transform.position, col.transform.position, 1f));
            }
            UpdatePath();
            updateFrameCount = 0;
        }
    }
}
