using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [Range(0.1f,10)]
        [SerializeField] private float gravoty;
        [SerializeField] private float jumpDistance;
        
        private float rotationPower = 3f;
        private float rotationLerp = 0.5f;
        private Vector2 smoothDeltaPosition = Vector2.zero;
        private Vector2 move = Vector2.zero;
        private Vector2 look = Vector2.zero;
        private Vector3 nextPosition;
        private Vector2 velocity;
        private CharacterController controller;
        public GameObject followTransform;
        private Quaternion nextRotation;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnMouse(InputValue value)
        {
            look = value.Get<Vector2>();
        }

        private void Update()
        {
            #region Follow Transform Rotation

            //Rotate the Follow Target transform based on the input
            followTransform.transform.rotation *= Quaternion.AngleAxis(look.x * rotationPower, Vector3.up);
            followTransform.transform.rotation *= Quaternion.AngleAxis(look.y * rotationPower, Vector3.right);

            var angles = followTransform.transform.localEulerAngles;
            angles.z = 0;

            var angle = followTransform.transform.localEulerAngles.x;

            //Clamp the Up/Down rotation
            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if(angle < 180 && angle > 40)
            {
                angles.x = 40;
            }


            followTransform.transform.localEulerAngles = angles;
            #endregion
            
            nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

            if (move.x == 0 && move.y == 0)
            {
                nextPosition = transform.position;
                return; 
            }

            var moveSpeed = speed / 100f;
            var position = (transform.forward * move.y * moveSpeed) + (transform.right * move.x * moveSpeed);
            nextPosition = transform.position + position;        
        

            //Set the player rotation based on the look transform
            transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
            //reset the y rotation of the look transform
            followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        }

        private void FixedUpdate()
        {
            var worldDeltaPosition = nextPosition - transform.position;
            
            //Map to local space
            var dX = Vector3.Dot(transform.right, worldDeltaPosition);
            var dY = Vector3.Dot(transform.forward, worldDeltaPosition);
            var deltaPosition = new Vector2(dX, dY);
            
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            if (Time.deltaTime > 1e-5f)
            {
                velocity = smoothDeltaPosition / Time.deltaTime;
            }

            transform.position = nextPosition;
        }
    }
}
