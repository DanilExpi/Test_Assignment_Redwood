using Scripts.Game.EnemiesManagerBase;
using Scripts.UtilityBase;
using System.Collections;
using UnityEngine;

namespace Scripts.Game
{
    public class Bullet_Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rig;
        [SerializeField] private float speedMove = 6f;
        [SerializeField] private Vector2Int rangeDamage = new Vector2Int(1,3);

        private float timeToDeath = 5f;
        private Coroutine waitDeathCor;

        private Vector2 Velocity { get; set; }

        public void StartReset(Vector3 startPosition, bool directionIsRight)
        {
            transform.position = startPosition;
            CheckScale(directionIsRight);

            Vector2 direction = directionIsRight ? Vector2.right : Vector2.left;
            var velocity = direction * speedMove;

            rig.velocity = velocity;
            Velocity = velocity;
            WaitDeath();

            GameManager.OnRestart += OnRestart;
            GameManager.OnPause += OnPauseOrDeathPlayer;
            GameManager.OnUnPause += OnUnPause;
            GameManager.OnDeathPlayer += OnPauseOrDeathPlayer;
        }

        private void CheckScale(bool directionIsRight)
        {
            if(!directionIsRight)
            {
                var scale = transform.localScale;
                scale.x = -scale.x;
                transform.localScale = scale;
            }
        }

        private void OnDestroy()
        {
            GameManager.OnRestart -= OnRestart;
            GameManager.OnPause -= OnPauseOrDeathPlayer;
            GameManager.OnUnPause -= OnUnPause;
            GameManager.OnDeathPlayer -= OnPauseOrDeathPlayer;
        }

        private void OnRestart()
        {
            StopWaitDeath();
            Destroy(gameObject);
        }

        private void OnPauseOrDeathPlayer()
        {
            rig.velocity = Vector2.zero;
            StopWaitDeath();
        }

        private void OnUnPause()
        {
            rig.velocity = Velocity;
            WaitDeath();
        }

        private void WaitDeath()
        {
            if(waitDeathCor == null)
            {
                waitDeathCor = StartCoroutine(WaitDeathCor());
            }
        }

        private void StopWaitDeath()
        {
            if(waitDeathCor!= null)
            {
                StopCoroutine(waitDeathCor);
                waitDeathCor = null;
            }
        }

        private IEnumerator WaitDeathCor()
        {
            while(timeToDeath > 0f)
            {
                timeToDeath -= Time.deltaTime;
                yield return null;
            }
            StopWaitDeath();
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collider = collision.collider;
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                var damage = rangeDamage.RandomRange();
                enemy.SetDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}