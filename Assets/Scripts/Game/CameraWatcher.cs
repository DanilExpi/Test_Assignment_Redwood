using Scripts.Game.Game;
using System.Collections;
using UnityEngine;

namespace Scripts.Game
{
    public class CameraWatcher : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;

        private float yPosition;
        private float zPosition;
        private Coroutine followCor;

        private float SetXPosition
        {
            set
            {
                transform.position = new Vector3
                {
                    x = value,
                    y = yPosition,
                    z = zPosition
                };
            }
        }
        private Transform TargetFollow { get; set; }

        public void StartReset()
        {
            yPosition = transform.position.y;
            zPosition = transform.position.z;
        }

        public void StopFollow()
        {
            if (followCor != null)
            {
                StopCoroutine(followCor);
                followCor = null;
            }
            TargetFollow = null;
        }

        public void ResetWatcher(Player player)
        {
            StopFollow();
            TargetFollow = player.transform;
            followCor = StartCoroutine(FollowCor());
        }

        private IEnumerator FollowCor()
        {
            while (true)
            {
                var newX = TargetFollow.position.x;
                if (newX < gameManager.XLeftRangeCamera)
                {
                    newX = gameManager.XLeftRangeCamera;
                }
                if (newX > gameManager.XRightRangeCamera)
                {
                    newX = gameManager.XRightRangeCamera;
                }
                SetXPosition = newX;
                yield return null;
            }
        }
    }
}