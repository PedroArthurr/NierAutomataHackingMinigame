namespace UnityEngine.Audio
{
    using System;

    public static class AudioExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mixer"></param>
        /// <param name="exposedName">The name of 'The Exposed to Script' variable</param>
        /// <param name="value">value must be between 0 and 1</param>
        public static void SetVolume(this AudioMixer mixer, string exposedName, float value)
        {
            mixer.SetFloat(exposedName, Mathf.Lerp(-60.0f, 0.0f, Mathf.Clamp01(value)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mixer"></param>
        /// <param name="exposedName">The name of 'The Exposed to Script' variable</param>
        /// <returns></returns>
        public static float GetVolume(this AudioMixer mixer, string exposedName)
        {
            if (mixer.GetFloat(exposedName, out float volume))
            {
                return Mathf.InverseLerp(-60.0f, 0.0f, volume);
            }

            return 0f;
        }
        private static readonly string onCompleteMethodName = "OnAudioComplete";

        public static void AddOnCompleteEvent(this AudioSource source, Action onCompleteAction)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onCompleteAction == null)
            {
                throw new ArgumentNullException(nameof(onCompleteAction));
            }

            source.Stop();

            var listener = source.gameObject.GetComponent<AudioCompletionListener>();
            if (listener == null)
            {
                listener = source.gameObject.AddComponent<AudioCompletionListener>();
                listener.OnAudioComplete += onCompleteAction;
            }
            else
            {
                listener.OnAudioComplete -= onCompleteAction;
                listener.OnAudioComplete += onCompleteAction;
            }

            source.Play();
        }

        public static void RemoveOnCompleteEvent(this AudioSource source, Action onCompleteAction)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onCompleteAction == null)
            {
                throw new ArgumentNullException(nameof(onCompleteAction));
            }

            var listener = source.gameObject.GetComponent<AudioCompletionListener>();
            if (listener != null)
            {
                listener.OnAudioComplete -= onCompleteAction;
            }
        }

        private class AudioCompletionListener : MonoBehaviour
        {
            public event Action OnAudioComplete;

            private void Update()
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    OnAudioComplete?.Invoke();
                    Destroy(this);
                }
            }
        }
    }
}

//https://answers.unity.com/questions/966255/mute-audiomixergroup-through-script.html