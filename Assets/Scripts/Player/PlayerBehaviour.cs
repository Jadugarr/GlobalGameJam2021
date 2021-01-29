using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ2021.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpForce;

        private Vector3 currentMovement = Vector3.zero;

        public void OnMovement(InputAction.CallbackContext callbackContext)
        {
            Vector2 movementDirection = callbackContext.ReadValue<Vector2>() * movementSpeed * Time.fixedDeltaTime;
            
            currentMovement = new Vector3(movementDirection.x, movementDirection.y, 0f);
        }

        public void OnJump(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
            {
                _rigidbody2D.AddForce(transform.up * jumpForce);
                //_rigidbody2D.velocity = new Vector2(0f, jumpForce);
            }
        }

        private void FixedUpdate()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            transform.Translate(currentMovement.x, currentMovement.y, 0f);
        }
    }
}