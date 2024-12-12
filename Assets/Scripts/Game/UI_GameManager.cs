using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class UI_GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseBody;
        [SerializeField] private Button restartButton, exitButton, closeButton;
        [SerializeField] private TextMeshProUGUI titlelAmmo;

        public void StartReset(UnityAction restartAction, UnityAction exitAction, UnityAction closeAction)
        {
            restartButton.onClick.AddListener(restartAction);
            exitButton.onClick.AddListener(exitAction);
            closeButton.onClick.AddListener(closeAction);

            UnPause();
        }

        public void Pause(bool thisClose)
        {
            pauseBody.SetActive(true);
            closeButton.gameObject.SetActive(thisClose);
        }

        public void UnPause()
        {
            pauseBody.SetActive(false);
        }

        public void SetValueAmmo(int valueAmmo)
        {
            titlelAmmo.text = valueAmmo.ToString();
        }
    }
}