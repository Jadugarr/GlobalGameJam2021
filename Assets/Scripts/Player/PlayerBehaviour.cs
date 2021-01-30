using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ2021.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpForce;

        [SerializeField] private bool canJump;
        [SerializeField] private bool canThrow;

        [SerializeField] private GameObject headRendererPrefab;
        [SerializeField] private GameObject legsRendererPrefab;
        [SerializeField] private GameObject armsRendererPrefab;

        private Vector3 currentMovement = Vector3.zero;
        private ContactFilter2D _contactFilter2D;
        private BoxCollider2D _boxCollider2D;

        private void Start()
        {
            _contactFilter2D = new ContactFilter2D();
            _contactFilter2D.SetLayerMask(LayerMask.GetMask($"Ground"));
            _boxCollider2D = GetComponentInChildren<BoxCollider2D>();
        }

        public void OnMovement(InputAction.CallbackContext callbackContext)
        {
            Vector2 movementDirection = callbackContext.ReadValue<Vector2>() * movementSpeed * Time.fixedDeltaTime;
            
            currentMovement = new Vector3(movementDirection.x, movementDirection.y, 0f);
        }

        public void OnJump(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed && canJump && IsOnGround())
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

        private bool IsOnGround()
        {
            List<RaycastHit2D> hits = new List<RaycastHit2D>();

            return _boxCollider2D.Cast(Vector2.down, _contactFilter2D, hits, 0.1f) > 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Legs") && !canJump && !canThrow)
            {
                canJump = true;
                Destroy(other.gameObject);
                
                Destroy(_boxCollider2D.gameObject);
                GameObject newRenderer = Instantiate(legsRendererPrefab, transform);
                _boxCollider2D = newRenderer.GetComponent<BoxCollider2D>();
            }

            if (other.CompareTag("Arms") && !canThrow && canJump)
            {
                canThrow = true;
                Destroy(other.gameObject);
                
                Destroy(_boxCollider2D.gameObject);
                GameObject newRenderer = Instantiate(armsRendererPrefab, transform);
                _boxCollider2D = newRenderer.GetComponent<BoxCollider2D>();
            }
        }
    }
}