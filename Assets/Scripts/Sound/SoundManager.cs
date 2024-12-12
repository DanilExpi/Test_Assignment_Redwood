using UnityEngine;

namespace Scripts.Sound
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager Instance;
        [SerializeField] private SFX sfxPrefab;

        public void StartReset()
        {
            Instance = this;
        }

        public static void SetSound(AudioClip clip, float volume = 1f)
        {
            if (Instance == null) return;
            var sfx = Instantiate(Instance.sfxPrefab);
            sfx.StartReset(clip, volume);
        }
    }
}