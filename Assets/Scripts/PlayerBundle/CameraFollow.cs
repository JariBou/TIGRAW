using System;
using UnityEngine;

namespace PlayerBundle
{
    public class CameraFollow : MonoBehaviour
    {

        public GameObject playerObj;
        private Transform _player;
        public Vector3 offset = new(0f, 0f, -10f);
        public float smoothTime = 0.25f;
    
        private Vector3 _velocity = Vector3.zero;

        public bool playerIsTeleporting;

        // Start is called before the first frame update
        void Awake()
        {
            _player = playerObj.GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerIsTeleporting) {return;}
        
            Vector3 targetPosition = _player.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
        }

    }
}
