using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        public void StartReset(UnityAction onPlayButtonClick, UnityAction onExitButtonClick)
        {
            playButton.onClick.AddListener(onPlayButtonClick);
            exitButton.onClick.AddListener(onExitButtonClick);
        }        
    }
}