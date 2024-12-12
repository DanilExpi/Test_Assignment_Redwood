using TMPro;
using UnityEngine;

namespace Scripts.Game.EnemiesManagerBase
{
    public class HealthBar_Enemy : MonoBehaviour
    {
        [SerializeField] private Transform healthBody;
        [SerializeField] TextMeshPro title;

        private Vector3 HealthBodyScale
        {
            get => healthBody.localScale;
            set => healthBody.localScale = value;
        }
        private Vector3 FullScale { get; set; }
        private Vector3 ZeroScale { get; set; }

        public void StartReset(int health, int maxHealth)
        {
            FullScale = HealthBodyScale;
            ZeroScale = new Vector3
            {
                x = 0f,
                y = HealthBodyScale.y,
                z = HealthBodyScale.z
            };

            var parent = transform.parent;
            transform.SetParent(null);
            transform.localScale = new Vector3()
            {
                x = Mathf.Abs(transform.localScale.x),
                y = transform.localScale.y,
                z = transform.localScale.z,
            };
            transform.SetParent(parent);

            SetHealthRatio(health, maxHealth);
        }

        public void SetHealthRatio(int health, int maxHealth)
        {
            var ratioHealth = (float)health / maxHealth;
            HealthBodyScale = Vector3.Lerp(ZeroScale, FullScale, ratioHealth);
            title.text = $"{health}/{maxHealth}";
        }
    }
}
