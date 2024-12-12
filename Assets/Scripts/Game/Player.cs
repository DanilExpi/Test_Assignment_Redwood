using Scripts.Sound;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;

namespace Scripts.Game.Game
{
    public class Player : MonoBehaviour
    {
        private enum StateValue
        {
            None,
            Idle,
            Run,
            Fire
        }

        [SerializeField] private float speedMove;
        [SerializeField] private Rigidbody2D rig;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform aim;
        [SerializeField] private Bullet_Player bulletPrefab;
        [SerializeField] private float timeToNextFire = 0.3f;
        [SerializeField] private int ammo;
        [SerializeField] private AudioClip clipFire;
        [SerializeField] private float volumeFire = 0.5f;
        [SerializeField] private AudioClip clipDeath;
        [SerializeField] private float volumeDeath = 0.75f;
        [SerializeField] private AudioClip clipPickUp;
        [SerializeField] private float volumePickUp = 0.75f;

        private Coroutine fireCor;

        private const string RunKeyAnimator = "IsRun";
        private const string FireKeyAnimator = "IsFire";

        private Vector3 ScaleDirectionToRight { get; set; }
        private Vector3 ScaleDirectionToLeft { get; set; }
        private StateValue State { get; set; }
        private bool IsRightDirection { get; set; } = true;
        private GameManager GameManager { get; set; }
                
        private Vector3 Scale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }
        private bool IsLive { get; set; }

        public Vector3 Position
        {
            get => transform.position;
            private set => transform.position = value;
        }

        public void StartReset(GameManager gameManager, Vector3 startPosition)
        {
            GameManager = gameManager;
            Position = startPosition;
            ScaleDirectionToRight = Scale;
            ScaleDirectionToLeft = new Vector3()
            {
                x = -Scale.x,
                y = Scale.y,
                z = Scale.z
            };
            IsLive = true;

            gameManager.SetAmmoInUI(ammo);
            IdleAnimation();

            GameManager.OnPause += StopAnimations;
        }

        private void OnDestroy()
        {
            GameManager.OnPause -= StopAnimations;
        }

        private void Update() 
        {
            if (!IsLive) return;
            if (GameManager.InPause) return;

            if(Input.GetMouseButton(0))
            {
                PlayFire();
                return;
            }

            StopFire();
            var horVelocity = Input.GetAxis("Horizontal");
            if(horVelocity != 0f)
            {
                RunAnimation();
                rig.velocity = new Vector2(horVelocity * speedMove, rig.velocity.y);
                if(horVelocity > 0f)
                {
                    Scale = ScaleDirectionToRight;
                    IsRightDirection = true;
                }
                else
                {
                    Scale = ScaleDirectionToLeft;
                    IsRightDirection = false;
                }
                CheckPosition();
            }
            else
            {
                IdleAnimation();
            }            
        }

        private void CheckPosition()
        {
            var position = Position;
            if (position.x < GameManager.XLeftRangePlayer)
            {
                position.x = GameManager.XLeftRangePlayer;
            }
            if (position.x > GameManager.XRightRangePlayer)
            {
                position.x = GameManager.XRightRangePlayer;
            }
            Position = position;
        }

        private void RunAnimation()
        {
            if(State != StateValue.Run)
            {
                State = StateValue.Run;
                animator.SetBool(RunKeyAnimator, true);
            }
        }

        private void IdleAnimation()
        {
            if (State != StateValue.Idle)
            {
                State = StateValue.Idle;
                animator.SetBool(RunKeyAnimator, false);
                DropVelocity();
            }
        }

        private void PlayFire()
        {
            if (State != StateValue.Fire)
            {
                State = StateValue.Fire;
                animator.SetBool(RunKeyAnimator, false);
                animator.SetBool(FireKeyAnimator, true);
                DropVelocity();

                if (fireCor == null)
                {
                    fireCor = StartCoroutine(FireCor());
                }
            }
        }

        private void StopFire()
        {
            if (State == StateValue.Fire)
            {
                State = StateValue.Idle;
                animator.SetBool(FireKeyAnimator, false);
                if(fireCor != null)
                {
                    StopCoroutine(fireCor);
                    fireCor = null;
                }
            }
        }

        private void CreateBullet()
        {
            if (!IsLive) return;
            if (ammo <= 0) return;

            var bullet = Instantiate(bulletPrefab);
            bullet.StartReset(aim.position, IsRightDirection);
            ammo--;
            GameManager.SetAmmoInUI(ammo);
            if(ammo <= 0)
            {
                Death();
            }
            SoundManager.SetSound(clipFire, volumeFire);
        }
    
        private IEnumerator FireCor()
        {
            CreateBullet();
            while(true)
            {
                yield return new WaitForSeconds(timeToNextFire);
                CreateBullet();
            }
        }

        public void Death()
        {
            IsLive = false;
            StopAnimations();
            GameManager.EndGame();
            SoundManager.SetSound(clipDeath, volumeDeath);
        }

        private void StopAnimations()
        {
            StopFire();
            DropVelocity();
            IdleAnimation();
        }

        private void DropVelocity()
        {
            rig.velocity = Vector2.zero;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collider = collision.collider;
            var ammo = collider.GetComponent<AmmoDrop>();
            if (ammo != null)
            {
                AddAmmo(ammo.GetAmmo);
                Destroy(ammo.gameObject);
            }
        }
        private void AddAmmo(int add)
        {
            ammo += add;
            GameManager.SetAmmoInUI(ammo);
            SoundManager.SetSound(clipPickUp, volumePickUp);
        }
    }
}