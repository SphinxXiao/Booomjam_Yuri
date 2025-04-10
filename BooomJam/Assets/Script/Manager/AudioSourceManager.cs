using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceManager : MonoBehaviour
{
/// <summary>
/// 单个储存的音频信息
/// </summary>
    [System.Serializable]
        public class Sound
    {
        [Header("声音片段")]
        public AudioClip clip;

        [Header("音频分类")]
        public AudioMixerGroup outputGroup;

        [Header("音量控制")]
        [Range(0,1)]
        public float volume = 1;

        [Header("全局播放")]
        public bool playOnAwake;

        [Header("循环播放声音")]
        public bool loop;
    }
/// <summary>
/// 储存所有的音频信息
/// </summary>
    public List<Sound> sounds;

/// <summary>
/// 每一个声音片段名称仅对应一个音频组件
/// </summary>
    private Dictionary<string, AudioSource> audiosDic;

    private void Awake()
    {
        instance = this;
        audiosDic = new Dictionary<string, AudioSource>();
    }

    private static AudioSourceManager instance;

    private void Start()
    {
        ///单次音频处理
        foreach(var sound in sounds)
        {
            GameObject obj = new GameObject(sound.clip.name);
            obj.transform.SetParent(transform);

            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.outputAudioMixerGroup = sound.outputGroup;
            source.volume = sound.volume;
            source.playOnAwake = sound.playOnAwake;
            source.loop = sound.loop;

            if(sound.playOnAwake)
                source.Play();

            audiosDic.Add(sound.clip.name, source);

        }
    }
/// <summary>
/// 开始播放
/// </summary>
/// <param name="name"></param>
/// <param name="pause"></param>
    public static void PlayAudio(string name, bool pause = false)
    { 
        if(!instance.audiosDic.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}音频不存在");
            return;
        }
        if(pause)
        {
            if(!instance.audiosDic[name].isPlaying)
                instance.audiosDic[name].Play();
        }
        else
            instance.audiosDic[name].Play();
    }
/// <summary>
/// 停止播放
/// </summary>
/// <param name="name"></param>

    public static void StopAudio(string name)
    {
        if(!instance.audiosDic.ContainsKey(name))
           {
            Debug.LogWarning($"名为{name}音频不存在");
            return;
        }
        instance.audiosDic[name].Stop();
    }

}
