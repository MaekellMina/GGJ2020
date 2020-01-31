using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioListener))]
public class AudioManager : MonoBehaviour
{
    //public bool isSingleton = true;
    public bool sfxOn = true;
    public bool musicOn = true;
    public float musicVol = 1;
    public float SFXVol = 1;
    private bool mute = false;

    public AudioClip[] audioClipList;

    private AudioSource localAudioSource;

    private static AudioManager _instance;

    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("An instance of " + typeof(AudioManager) + " is needed in the scene, but there is none.");
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        Init();
    }

    AudioSource[] audioSourcePool = new AudioSource[10];
    public AudioSource bgAudioSource;

    int audioIndex = 0;

    void Init()
    {
        GameObject blankObj = new GameObject();
        for (int i = 0; i < audioSourcePool.Length; i++)
        {
            GameObject tmp = Instantiate(blankObj, Vector3.zero, Quaternion.identity) as GameObject;
            tmp.name = "AudioSource_" + i;
            tmp.AddComponent<AudioSource>();
            tmp.GetComponent<AudioSource>().playOnAwake = false;
            tmp.GetComponent<AudioSource>().loop = false;
            tmp.transform.SetParent(this.transform);
            tmp.transform.position = Vector3.zero;
            audioSourcePool[i] = tmp.GetComponent<AudioSource>();
        }

        GameObject bg = Instantiate(blankObj, Vector3.zero, Quaternion.identity) as GameObject;
        bg.name = "BGAudioSource";
        bg.AddComponent<AudioSource>();
        bg.GetComponent<AudioSource>().playOnAwake = false;
        bg.GetComponent<AudioSource>().loop = true;
        bg.transform.SetParent(this.transform);
        bg.transform.position = Vector3.zero;
        bgAudioSource = bg.GetComponent<AudioSource>();
        Destroy(blankObj);
    }

    public void PlayAudioClip(AudioClip clip, bool loop = false, float delay = 0)
    {
        if (mute) return;
        StartCoroutine(PlayCoroutine(clip, loop, delay));
    }

    public void PlayAudioClip(int index = 0, bool loop = false, float delay = 0)
    {
        if (mute) return;
        if (index > audioClipList.Length)
        {
            Debug.Log("index is out of range");
            return;
        }
        StartCoroutine(PlayCoroutine(audioClipList[index], loop, delay));
    }

    IEnumerator PlayCoroutine(AudioClip c, bool loop, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (sfxOn)
        {
            if (!loop)
            {
                audioSourcePool[audioIndex].volume = SFXVol;
                audioSourcePool[audioIndex].loop = false;
                audioSourcePool[audioIndex].PlayOneShot(c, SFXVol);
                if (audioIndex < audioSourcePool.Length - 1)
                    audioIndex++;
                else
                    audioIndex = 0;
            }
            else
            {

                audioSourcePool[audioIndex].volume = SFXVol;
                audioSourcePool[audioIndex].clip = c;
                audioSourcePool[audioIndex].loop = true;
                audioSourcePool[audioIndex].Play();
                if (audioIndex < audioSourcePool.Length - 1)
                    audioIndex++;
                else
                    audioIndex = 0;
            }
        }
    }


    // -- Stops the looped audio clip --//
    public void StopAudioClipLoop(AudioClip clip)
    {
        for (int i = 0; i < audioSourcePool.Length; i++)
        {
            if (audioSourcePool[i].clip == clip)
                audioSourcePool[i].Stop();
        }
    }

    // -- Overload for stopping looped audio clip -- //
    public void StopAudioClipLoop(int index)
    {
        if (index > audioClipList.Length) return;
        for (int i = 0; i < audioSourcePool.Length; i++)
        {
            if (audioSourcePool[i].clip == audioClipList[index])
                audioSourcePool[i].Stop();
        }
    }

    public void PlayBGMusic(AudioClip music)
    {
        if (mute) return;
        if (musicOn)
        {
            bgAudioSource.clip = music;
            bgAudioSource.loop = true;
            bgAudioSource.volume = musicVol;
            bgAudioSource.Play();
        }
    }

    public void StopBgMusic()
    {
        bgAudioSource.Stop();
    }

    public void Mute(bool state)
    {
        mute = state;
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < audioSourcePool.Length; i++)
        {
            audioSourcePool[i].Stop();
        }
        bgAudioSource.Stop();
    }
    public void StopAllSFX()
    {
        for (int i = 0; i < audioSourcePool.Length; i++)
        {
            audioSourcePool[i].Stop();
        }
    }
    public void SetMusicVolume(float value)
    {
        musicVol = value;
        bgAudioSource.volume = musicVol;

        if (value == 0)
            musicOn = false;
        else
            musicOn = true;
    }

    public void SetSFXVolume(float value)
    {
        SFXVol = value;
        if (value <= 0)
            sfxOn = false;
        else
            sfxOn = true;
    }

    // -- WIP -- //
    IEnumerator AudioFade(AudioSource audioSource, float time, float start, float end)
    {
        yield return null;
    }

    /*
	#region Testing
	void Update() {
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.G)) {
			PlayAudioClip(0,false,1.5f);
		}	
		if (Input.GetKeyDown(KeyCode.H))
		{
			PlayAudioClip(0,true,0);
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			StopAudioClipLoop(0);
		}
#endif
	}
	#endregion
	*/
}