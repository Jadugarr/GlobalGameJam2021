using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CreditsDialogBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;

        [SerializeField] private Button backButton;

        private void Start()
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnCancelPressed(InputAction.CallbackContext callbackContext)
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}