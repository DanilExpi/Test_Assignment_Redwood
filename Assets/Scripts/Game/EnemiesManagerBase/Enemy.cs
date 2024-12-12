using Scripts.Game.Game;
using Scripts.Sound;
using Scripts.UtilityBase;
using UnityEngine;

namespace Scripts.Game.EnemiesManagerBase
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speedMove = 4f;
        [SerializeField] private Rigidbody2D rig;
        [SerializeField] private Vector2Int rangeHealth = new Vector2Int(3, 8);
        [SerializeField] private HealthBar_Enemy healthBar;
        [SerializeField] private Animator animator;
        [SerializeField] private AmmoDrop ammoDropPrefab;
        [SerializeField] private AudioClip[] voices;
        [SerializeField] private AudioClip clipDeath;
        [SerializeField] private float volumeVoice = 0.5f;

        private const string KeySpeedAnimation = "SpeedAnimation";

        private Player Player { get; set; }
        private Vector3 Scale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }
        private Vector3 DefaultScale { get; set; }
        private bool IsPlayerInRightDirection
        {
            get => transform.position.x < Player.Position.x;
        }
        private int MaxHealth { get; set; }
        private int Health { get; set; }
        private Vector2 Velocity { get; set; }

        public void StartReset(Player player, Vector3 startPosition)
        {
            Player = player;
            DefaultScale = Scale;
            transform.position = startPosition;

            CheckDirectionMovement();
            StartResetHealth();
            PlayStartVoice();

            GameManager.OnRestart += OnRestart;
            GameManager.OnPause += OnPause;
            GameManager.OnUnPause += OnUnPause;
            GameManager.OnDeathPlayer += OnDeathPlayer;
        }

        private void PlayStartVoice()
        {
            var clip = voices.RandomRange();
            SoundManager.SetSound(clip, volumeVoice);
        }

        private void OnDestroy()
        {
            GameManager.OnRestart -= OnRestart;
            GameManager.OnPause -= OnPause;
            GameManager.OnUnPause -= OnUnPause;
            GameManager.OnDeathPlayer -= OnDeathPlayer;
        }

        private void OnRestart()
        {
            Destroy(gameObject);
        }

        private void OnPause()
        {
            StopAnimation();
        }

        private void OnDeathPlayer()
        {
            StopAnimation();
        }

        private void OnUnPause()
        {
            animator.SetFloat(KeySpeedAnimation, 1f);
            rig.velocity = Velocity;
        }

        private void StartResetHealth()
        {
            Health = rangeHealth.RandomRange();
            MaxHealth = Health;
            healthBar.StartReset(Health, MaxHealth);
        }

        private void CheckDirectionMovement()
        {
            if (IsPlayerInRightDirection)
            {
                Scale = DefaultScale;
                rig.velocity = Vector2.right * speedMove;
            }
            else
            {
                Scale = new Vector3
                {
                    x = -DefaultScale.x,
                    y = DefaultScale.y,
                    z = DefaultScale.z,
                };
                rig.velocity = Vector2.left * speedMove;
            }
            Velocity = rig.velocity;
        }

        private void StopAnimation()
        {
            animator.SetFloat(KeySpeedAnimation, 0f);
            rig.velocity = Vector2.zero;
        }

        public void SetDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Death();
            }
            else
            {
                healthBar.SetHealthRatio(Health, MaxHealth);
            }
            rig.velocity = Velocity;
        }

        private void Death()
        {
            var ammo = Instantiate(ammoDropPrefab, transform.parent);
            ammo.StartReset(transform.position);

            SoundManager.SetSound(clipDeath, volumeVoice);

            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collider = collision.collider;
            var player = collider.GetComponent<Player>();
            if (player != null)
            {
                player.Death();
            }
        }
    }
}