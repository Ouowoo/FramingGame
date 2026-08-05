using UnityEngine;

namespace Farm.Gameplay
{
    /// <summary>
    /// 脚步声接收器：响应 body 动画里埋的 AnimationEventPlayFootstepSound 事件。
    /// 为什么只有一个 AudioSource 但有两个音量：走路和跑步只切换音量，不换音频源，
    /// 两个 source 反而可能在状态切换时重叠播放，单一 source + Play() 自然互斥。
    /// 为什么用 Play() 而不是 PlayOneShot：脚步声只有 0.12s，Player 只有一个
    /// 脚步声源，直接 Play 不会被长音效截断，且能通过 source.volume 统一调音。
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class FootstepAudio : MonoBehaviour
    {
        [Header("音频源")]
        [SerializeField]
        private AudioSource audioSource;

        // 为什么在 Awake 处理 null：Inspector 留空时自动补，减少挂载出错的可能。
        private void Awake()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        /// <summary>
        /// 走路脚步声：由动画事件 AnimationEventPlayFootstepSound 触发。
        /// 为什么动画事件的 float 参数默认 0 时用 walkVolume：参数为 0 表示
        /// 动画师未设音量值（floatParameter 默认 0），此时走 Inspector 可调参数。
        /// </summary>
        public void AnimationEventPlayFootstepSound(float volume = 0f)
        {
            audioSource.Play();
        }

    }
}
