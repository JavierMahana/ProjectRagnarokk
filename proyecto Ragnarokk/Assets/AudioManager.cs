using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public Sound[] ButtonSounds;
	public Sound[] WeaponSounds;
	public Sound[] EnemySounds;
	public Sound[] OtherSounds;

	public Sound[] Music;

	public Sound[] Ambience;

	private string CurrentMusic;

	public List<Sound> sounds = new List<Sound>();

	public void CheckMusic()
	{
		if (GameManager.Instance.GameState == GAME_STATE.PREGAME)
		{
			foreach (Sound music in sounds)
            {
				if (music.SoundName == Music[0].SoundName) { SetMusic(music); }
            }
		}
		else if (GameManager.Instance.GameState == GAME_STATE.EXPLORATION)
		{
			foreach (Sound music in sounds)
			{			
				if (music.SoundName == Music[5].SoundName) { SetMusic(music); }
			}
		}
		else if(GameManager.Instance.GameState == GAME_STATE.COMBAT)
        {
			int i = 2;
			var cb = FindObjectOfType<CombatManager>();
			if(cb.EnemyFighters[0].Name == "Progenitor" || cb.EnemyFighters[0].Name == "Parasito") {i++; }
			foreach (Sound music in sounds)
			{
				if (music.SoundName == Music[i].SoundName) { SetMusic(music); }
			}
		}
		else if(GameManager.Instance.GameState == GAME_STATE.SHOP)
		{
			foreach (Sound music in sounds)
			{
				
				if (music.SoundName == Music[1].SoundName) { SetMusic(music); }
			}
		}
		else if(GameManager.Instance.GameState == GAME_STATE.CREDITS)
		{
			foreach (Sound music in sounds)
			{
				if (music.SoundName == Music[4].SoundName) { SetMusic(music); }
			}
		}
        else
        {
			Debug.Log("No game state for music");
        }
	}

	public void SetMusic(Sound music)
	{
		foreach (Sound s in sounds)
		{ 
			if (s.SoundName == music.SoundName) 
			{ 
				if(music.SoundName != CurrentMusic)
                {
					Play(music.SoundName);
					CurrentMusic = music.SoundName;
				}
				
			}

			else
			{ 
				for (int i = 0; i < Music.Length; i++)
				{
					if (CurrentMusic != Music[i].SoundName)
						Stop(s.SoundName);
				}
			}
		}
		
		
	}

    void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		//añadir todos los sonidos
		foreach (Sound s in ButtonSounds) { sounds.Add(s); }
		foreach (Sound s in WeaponSounds) { sounds.Add(s); }
		foreach (Sound s in OtherSounds) { sounds.Add(s); }
		foreach (Sound s in EnemySounds) { sounds.Add(s); }
		foreach (Sound s in Music) { sounds.Add(s); }
		foreach (Sound s in Ambience) { sounds.Add(s); }

		UpdateAudioParameters();
		
	}

	public void UpdateAudioParameters()
    {
		Debug.Log(sounds.Count);

		var generalFactor = PlayerPrefs.GetFloat("audioGeneral")/100f;
		var sfxFactor = PlayerPrefs.GetFloat("audioSFX") / 100f;
		var musicFactor = PlayerPrefs.GetFloat("audioMusic") / 100f;
		var ambientFactor = PlayerPrefs.GetFloat("audioAmbient") / 100f;

		#region sounds
		foreach (Sound s in ButtonSounds)
		{
			if(s.source == null) { s.source = gameObject.AddComponent<AudioSource>(); }
			s.source.clip = s.clip;
			s.source.outputAudioMixerGroup = s.mixer;
			s.source.volume = sfxFactor * generalFactor;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

		foreach (Sound s in WeaponSounds)
		{
			if (s.source == null) { s.source = gameObject.AddComponent<AudioSource>(); }
			s.source.clip = s.clip;
			s.source.outputAudioMixerGroup = s.mixer;
			s.source.volume = sfxFactor * generalFactor;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

		foreach (Sound s in OtherSounds)
		{
			if (s.source == null) { s.source = gameObject.AddComponent<AudioSource>(); }
			s.source.clip = s.clip;
			s.source.outputAudioMixerGroup = s.mixer;
			s.source.volume = sfxFactor * generalFactor;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

		foreach (Sound s in EnemySounds)
		{
			if (s.source == null) { s.source = gameObject.AddComponent<AudioSource>(); }
			s.source.clip = s.clip;
			s.source.outputAudioMixerGroup = s.mixer;
			s.source.volume = sfxFactor * generalFactor;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

		#endregion


		#region music

		foreach (Sound s in Music)
		{
			if (s.source == null) { s.source = gameObject.AddComponent<AudioSource>(); }
			s.source.clip = s.clip;
			s.source.outputAudioMixerGroup = s.mixer;
			s.source.volume = musicFactor * generalFactor;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
		#endregion

		#region ambient
		foreach (Sound s in Ambience)
		{
			if (s.source == null) { s.source = gameObject.AddComponent<AudioSource>(); }
			s.source.clip = s.clip;
			s.source.outputAudioMixerGroup = s.mixer;
			s.source.volume = ambientFactor * generalFactor;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
		#endregion
	}

    public void Play(string sound)
	{
		Sound s = null;
		foreach (Sound a in sounds)
        {
			if(a.SoundName == sound) {s = a; }
        }
        if (s != null) { s.source.Play(); }
		else { Debug.Log($" The Sound was not found by name : {sound}"); }
		
	}
	public void Stop(string sound)
	{
		Sound s = null;
		foreach (Sound a in sounds)
		{
			if (a.SoundName == sound) { s = a; }
		}
		if (s != null) { s.source.Stop(); }
		else { Debug.Log($" The Sound was not found by name : {s.SoundName}"); }
	}

	public void PlayButtonSound()
    {
		var size = ButtonSounds.Length - 1;
		int i = Random.Range(0, size);
		Play(ButtonSounds[i].SoundName);
    }


}
