using UnityEngine;

namespace Scripts.Sound
{
    public class SFX : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        public void StartReset(AudioClip clip, float volume)
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
            if (clip == null)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, clip.length + 1f);
            }
        }
    }
}