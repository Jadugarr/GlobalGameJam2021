using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class TutorialGraveBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text tutorialTextfield;

        private void Start()
        {
            HideText();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ShowText();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                HideText();
            }
        }

        private void ShowText()
        {
            tutorialTextfield.enabled = true;
            tutorialTextfield.alpha = 1f;
        }

        private void HideText()
        {
            
            tutorialTextfield.enabled = false;
            tutorialTextfield.alpha = 0f;
        }
    }
}