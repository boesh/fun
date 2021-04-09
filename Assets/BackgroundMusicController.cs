using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BackgroundMusicController : MonoBehaviour
    {

        private static BackgroundMusicController instance;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private List<AudioClip> audioClips;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        private void LateUpdate()
        {


            if (PlayerController.GetKillsCount() <= 20)
            {

                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioClips[0];
                    audioSource.Play();
                }
                else if (audioSource.isPlaying && audioSource.clip != audioClips[0])
                {
                    audioSource.Stop();
                }
            }
            if (PlayerController.GetKillsCount() > 20)
            {

                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioClips[1];
                    audioSource.Play();
                }
                else if (audioSource.isPlaying && audioSource.clip != audioClips[1])
                {
                    audioSource.Stop();
                }
            }
        }
    }
}
