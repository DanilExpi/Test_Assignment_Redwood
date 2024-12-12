using Scripts.UtilityBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.EnemiesManagerBase
{
    public class EnemiesManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Enemy[] enemiesPrefabs;
        [SerializeField] private Transform parentEnemies;
        [SerializeField] private Vector2 timeRangeCreateEnemy = new Vector2(1f, 10f);
        [SerializeField] private Camera gameCamera;
        [SerializeField] private float addOffsetEnemy = 2f;

        private Coroutine createEnemiesCor;

        private float GetWidthScreen =>
           Vector3.Distance(ViewportToWorldPoint(0f, 0f), ViewportToWorldPoint(1f, 0f));

        private float GetNextTime => timeRangeCreateEnemy.RandomRange();

        private Vector3 ViewportToWorldPoint(float x, float y)
        {
            return gameCamera.ViewportToWorldPoint(new Vector3(x, y, gameCamera.nearClipPlane));
        }
        private Enemy GetEnemyPrefab => enemiesPrefabs.RandomRange();


        public void ResetEnemies()
        {
            StopCreate();
            ClearParent();
            createEnemiesCor = StartCoroutine(CreateEnemiesCor());
        }

        public void StopCreate()
        {
            if (createEnemiesCor != null)
            {
                StopCoroutine(createEnemiesCor);
                createEnemiesCor = null;
            }
        }

        private void ClearParent()
        {
            for (int i = 0; i < parentEnemies.childCount; i++)
            {
                Destroy(parentEnemies.GetChild(i).gameObject);
            }
        }

        private IEnumerator CreateEnemiesCor()
        {
            while (true)
            {
                var timeWait = GetNextTime;
                while (timeWait > 0f)
                {
                    if (!gameManager.InPause)
                    {
                        timeWait -= Time.deltaTime;
                    }
                    yield return null;
                }

                var enemyPrefab = GetEnemyPrefab;
                var enemy = Instantiate(enemyPrefab, parentEnemies);
                var enemyPosition = GetPositionForEnemy();
                enemy.StartReset(gameManager.GetPlayer, enemyPosition);
            }
        }

        private Vector3 GetPositionForEnemy()
        {
            var playerPosition = gameManager.GetPlayer.Position;
            var widthScreen = GetWidthScreen;

            List<Vector3> variantPosition = new List<Vector3>();
            var leftPosition = playerPosition;
            leftPosition.x -= widthScreen / 2f + addOffsetEnemy;
            if (leftPosition.x > gameManager.XLeftRangeEnemy)
            {
                variantPosition.Add(leftPosition);
            }

            var rightPosition = playerPosition;
            rightPosition.x += widthScreen / 2f + addOffsetEnemy;
            if (rightPosition.x < gameManager.XRightRangeEnemy)
            {
                variantPosition.Add(rightPosition);
            }

            var getPosition = variantPosition.RandomRange();
            getPosition.y = parentEnemies.position.y;
            return getPosition;
        }
    }
}