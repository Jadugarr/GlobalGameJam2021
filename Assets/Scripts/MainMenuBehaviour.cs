using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button quitGameButton;
        [SerializeField] private Button creditsButton;

        [SerializeField] private GameObject creditsDialog;

        private void Start()
        {
            startGameButton.onClick.AddListener(OnStartGameClicked);
            quitGameButton.onClick.AddListener(OnQuitGameClicked);
            creditsButton.onClick.AddListener(OnCreditsClicked);
        }

        private void OnCreditsClicked()
        {
            creditsDialog.SetActive(true);
            gameObject.SetActive(false);
        }

        private void OnQuitGameClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnStartGameClicked()
        {
            SceneManager.LoadScene("Tutorial");
        }
        
        
    }
}