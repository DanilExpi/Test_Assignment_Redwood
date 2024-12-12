using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] MainMenuButtons mainMenuButtons;

        private void Start()
        {
            mainMenuButtons.StartReset(OnPlayClick, OnClickExit);
        }

        private void OnPlayClick()
        {
            SceneManager.LoadScene("GamePlayScene");
        }
        private void OnClickExit()
        {
            Application.Quit();
        }
    }
}