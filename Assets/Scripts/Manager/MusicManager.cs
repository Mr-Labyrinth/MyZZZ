using System.Collections;
using UnityEngine;

public class MusicManager : SingleMomoBase<MusicManager>
{
    [Header("Audio Source")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField]
    private AudioClip defaultBGM;// 默认背景音乐
    [Range(0f, 1f)]
    [SerializeField]
    private float defaultVolume = 0.5f;// 默认音量
    [SerializeField]
    private bool playOnStart = true;
    [SerializeField]
    private float defaultFadeDuration = 1f;// 默认淡入淡出时间

    private Coroutine fadeCoroutine;
    private float targetVolume;

    private const string VolumeKey = "MusicVolume";

    protected override void Awake()
    {
        base.Awake();
        // 设置音频源，获取组件
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        // 如果没有音频源，则添加一个新的音频源组件
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // 2D 音频

        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, defaultVolume);
        SetVolume(savedVolume, false);
    }

    private void Start()
    {
        if (playOnStart && defaultBGM != null)
            PlayMusic(defaultBGM);
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="clip">播放音乐</param>
    /// <param name="loop">音乐是否循环</param>
    /// <param name="fadeDuration">音乐是否淡入淡出</param>
    public void PlayMusic(AudioClip clip, bool loop = true, float fadeDuration = -1f)
    {
        Debug.Log("PlayMusic in");
        // 如果音乐为空，则直接返回
        if (clip == null) return;
        // 如果音乐正在播放，并且当前播放的音乐和要播放的音乐相同，则直接返回
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;
        // 如果淡入淡出时间小于0，则使用默认淡入淡出时间
        if (fadeDuration < 0)
            fadeDuration = defaultFadeDuration;

        // 协程如果不存在，则停止协程
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        // 开始协程切换音乐
        fadeCoroutine = StartCoroutine(SwitchMusicRoutine(clip, loop, fadeDuration));
        Debug.Log("PlayBGM");
    }

    private IEnumerator SwitchMusicRoutine(AudioClip clip, bool loop, float fadeDuration)
    {
        Debug.Log("SwitchMusicRoutine in");
        // 如果音乐正在播放，并且淡入淡出时间大于0，则淡出音乐
        if (audioSource.isPlaying && fadeDuration > 0f)
            yield return FadeTo(0f, fadeDuration);

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = 0f;
        audioSource.Play();

        // 如果淡入淡出时间大于0，则淡入音乐，否则直接设置音量为目标音量
        if (fadeDuration > 0f)
            yield return FadeTo(targetVolume, fadeDuration);
        else
            audioSource.volume = targetVolume;

        fadeCoroutine = null;
    }
    /// <summary>
    /// 停止音乐
    /// </summary>
    /// <param name="fadeDuration">淡出时间</param>
    public void StopMusic(float fadeDuration = -1f)
    {
        // 如果音乐没有播放，则直接返回
        if (!audioSource.isPlaying) return;

        // 如果淡入淡出时间小于0，则使用默认淡入淡出时间
        if (fadeDuration < 0)
            fadeDuration = defaultFadeDuration;
        // 协程如果存在，则停止协程
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        // 开始协程停止音乐
        fadeCoroutine = StartCoroutine(StopMusicRoutine(fadeDuration));
    }

    private IEnumerator StopMusicRoutine(float fadeDuration)
    {
        // 如果音乐正在播放，并且淡入淡出时间大于0，则淡出音乐
        if (fadeDuration > 0f)
            yield return FadeTo(0f, fadeDuration);

        audioSource.Stop();
        audioSource.clip = null;
        fadeCoroutine = null;
    }

    /// <summary>
    /// 停止音乐
    /// </summary>
    public void PauseMusic()
    {
        audioSource.Pause();
    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="volume">音量</param>
    /// <param name="save">是否保存</param>
    public void SetVolume(float volume, bool save = true)
    {
        targetVolume = Mathf.Clamp01(volume);
        audioSource.volume = targetVolume;

        if (save)
        {
            PlayerPrefs.SetFloat(VolumeKey, targetVolume);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 获取当前音量
    /// </summary>
    /// <returns>音量</returns>
    public float GetVolume()
    {
        return targetVolume;
    }

    private IEnumerator FadeTo(float target, float duration)
    {
        // 获取当前音量
        float start = audioSource.volume;
        float time = 0f;

        // 逐渐改变音量
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }
        audioSource.volume = target;
    }
}
