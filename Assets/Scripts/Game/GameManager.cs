using Scripts.Game.EnemiesManagerBase;
using Scripts.Game.Game;
using Scripts.Sound;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        public static Action OnRestart;
        public static Action OnPause;
        public static Action OnUnPause;
        public static Action OnDeathPlayer;

        [SerializeField] private SoundManager soundManager;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Transform playerCreatePosition;
        [SerializeField] private EnemiesManager enemiesManager;
        [SerializeField] private CameraWatcher cameraWatcher;
        [SerializeField] private UI_GameManager ui;
        [SerializeField] Transform leftRangePlayer, rightRangePlayer, leftRangeCamera, rightRangeCamera, leftRangeEnemy, rightRangeEnemy;


        private Player CurrentPlayers { get; set; }

        private Vector3 PlayerCreatePosition => playerCreatePosition.position;
        private bool IsEndGame { get; set; }

        public float XLeftRangePlayer => leftRangePlayer.position.x;
        public float XRightRangePlayer => rightRangePlayer.position.x;
        public float XLeftRangeCamera => leftRangeCamera.position.x;
        public float XRightRangeCamera => rightRangeCamera.position.x;
        public float XLeftRangeEnemy => leftRangeEnemy.position.x;
        public float XRightRangeEnemy => rightRangeEnemy.position.x;
        public Player GetPlayer => CurrentPlayers;
        public bool InPause { get; private set; }
        
        private void Start()
        {
            soundManager.StartReset();
            cameraWatcher.StartReset();
            InPause = false;
            ui.StartReset(ResetGame, Exit, UnPause);
            ResetGame();
        }

        private void Exit()
        {
            SceneManager.LoadScene("MainScene");
        }

        private void ResetGame()
        {
            IsEndGame = false;
            InPause = false;
            OnRestart?.Invoke();

            cameraWatcher.StopFollow();
            ui.UnPause();

            if (CurrentPlayers != null)
            {
                Destroy(CurrentPlayers.gameObject);
                CurrentPlayers = null;
            }

            CurrentPlayers = Instantiate(playerPrefab);
            CurrentPlayers.StartReset(this, PlayerCreatePosition);
            enemiesManager.ResetEnemies();

            cameraWatcher.ResetWatcher(CurrentPlayers);
        }

        public void SetAmmoInUI(int valueAmmo)
        {
            ui.SetValueAmmo(valueAmmo);
        }

        public void EndGame()
        {
            IsEndGame = true;
            OnDeathPlayer?.Invoke();
            Pause(false);
        }

        private void Pause(bool thisCloseButton)
        {
            InPause = true;
            ui.Pause(thisCloseButton);
            OnPause?.Invoke();
        }

        private void UnPause()
        {
            InPause = false;
            ui.UnPause();
            OnUnPause?.Invoke();
        }

        private void Update()
        {
            if (IsEndGame) return;
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                InPause = !InPause;
                if(InPause)
                {
                    ui.Pause(true);
                    OnPause?.Invoke();
                }
                else
                {
                    ui.UnPause();
                    OnUnPause?.Invoke();
                }
            }
        }
    }
}