using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ2021.Player
{
    public class ReticleBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject playerObject;
        [SerializeField] private float maxDistance;
        [SerializeField] private SpriteRenderer reticleRenderer;

        private void Start()
        {
            reticleRenderer.enabled = false;
        }

        public void OnLook(InputAction.CallbackContext callbackContext)
        {
            reticleRenderer.enabled = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (Vector2.Distance(playerObject.transform.position, mousePosition) <= maxDistance)
            {
                transform.position = mousePosition;
            }
            else
            {
                Vector2 dir = (mousePosition - (Vector2)playerObject.transform.position).normalized;
                transform.position = (Vector2)playerObject.transform.position + dir * maxDistance;
            }
        }

        public void OnGamePadMove(InputAction.CallbackContext callbackContext)
        {
            Vector2 dir = callbackContext.ReadValue<Vector2>();

            reticleRenderer.enabled = dir != Vector2.zero;
            transform.position = (Vector2)playerObject.transform.position + dir * maxDistance;
        }
    }
}