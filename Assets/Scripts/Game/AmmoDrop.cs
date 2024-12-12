using Scripts.UtilityBase;
using TMPro;
using UnityEngine;

namespace Scripts.Game.Game
{
    public class AmmoDrop : MonoBehaviour
    {
        [SerializeField] private Vector2Int rangeAmmo = new Vector2Int(5, 10);
        [SerializeField] private TextMeshPro title;

        private int Ammo { get; set; }

        public int GetAmmo => Ammo;

        public void StartReset(Vector3 position)
        {
            transform.position = position;
            Ammo = rangeAmmo.RandomRange();
            title.text = Ammo.ToString();
        }
    }
}
