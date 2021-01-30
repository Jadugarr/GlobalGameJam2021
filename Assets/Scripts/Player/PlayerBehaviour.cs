using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ2021.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float maxMovementSpeed;
        [SerializeField] private float accelerationSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float throwForce;

        [SerializeField] private bool canJump;
        [SerializeField] private bool canThrow;

        [SerializeField] private GameObject headRendererPrefab;
        [SerializeField] private GameObject legsRendererPrefab;
        [SerializeField] private GameObject armsRendererPrefab;

        [SerializeField] private GameObject legsCollectiblePrefab;
        [SerializeField] private GameObject armsCollectiblePrefab;

        [SerializeField] private ReticleBehaviour _reticleBehaviour;

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
            Vector2 movementDirection = callbackContext.ReadValue<Vector2>() * accelerationSpeed * Time.fixedDeltaTime;
            
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

        public void OnFire(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed && canThrow)
            {
                Vector2 reticlePosition = _reticleBehaviour.gameObject.transform.position;

                Vector2 direction = (reticlePosition - (Vector2)transform.position).normalized;
                _rigidbody2D.AddForce(direction * throwForce);
                
                Destroy(_boxCollider2D.gameObject);
                GameObject newRenderer = Instantiate(headRendererPrefab, transform);
                _boxCollider2D = newRenderer.GetComponent<BoxCollider2D>();

                canJump = false;
                canThrow = false;
                _reticleBehaviour.gameObject.SetActive(false);
                
                SpawnCollectible(armsCollectiblePrefab, Vector2.left);
                SpawnCollectible(legsCollectiblePrefab, Vector2.right);
            }
        }

        private void FixedUpdate()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            if (currentMovement == Vector3.zero)
            {
                return;
            }
            
            bool sameDirection = currentMovement.x < 0f && _rigidbody2D.velocity.x < 0f ||
                                 currentMovement.x > 0f && _rigidbody2D.velocity.x > 0f;

            if (sameDirection && Mathf.Abs(_rigidbody2D.velocity.x) >= maxMovementSpeed)
            {
                return;
            }
            
            _rigidbody2D.velocity += new Vector2(currentMovement.x, currentMovement.y);
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
                Destroy(other.gameObject.transform.parent.gameObject);
                
                Destroy(_boxCollider2D.gameObject);
                GameObject newRenderer = Instantiate(legsRendererPrefab, transform);
                _boxCollider2D = newRenderer.GetComponent<BoxCollider2D>();
            }

            if (other.CompareTag("Arms") && !canThrow && canJump)
            {
                canThrow = true;
                _reticleBehaviour.gameObject.SetActive(true);
                Destroy(other.gameObject.transform.parent.gameObject);
                
                Destroy(_boxCollider2D.gameObject);
                GameObject newRenderer = Instantiate(armsRendererPrefab, transform);
                _boxCollider2D = newRenderer.GetComponent<BoxCollider2D>();
            }
        }

        private void SpawnCollectible(GameObject collectible, Vector2 direction)
        {
            GameObject spawnedCollectible = Instantiate(collectible, (Vector2)transform.position + direction * 2, Quaternion.identity);
            spawnedCollectible.GetComponent<Rigidbody2D>().AddForce(direction * 300);
        }
    }
}