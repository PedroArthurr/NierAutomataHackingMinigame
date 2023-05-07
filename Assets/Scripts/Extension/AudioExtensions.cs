namespace UnityEngine.Audio
{
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
    }
}

//https://answers.unity.com/questions/966255/mute-audiomixergroup-through-script.html