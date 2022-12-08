using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour {
  public AudioClip defaultAmbience;


  //track 01 = default ambience, track 02 = ambience to swap to, track 03/04 = music tracks
  private AudioSource track01, track02, track03, track04;
  private bool isPlayingTrack01, isPlayingTrack03;

  public static AudioManager instance;

  private void Awake() {
    if (instance==null) {
      instance = this;
    }
  }

  private void Start() {
    track01 = gameObject.AddComponent<AudioSource>();
    track01.loop = true;
    track02 = gameObject.AddComponent<AudioSource>();
    track02.loop = true;
    isPlayingTrack01 = true;

    track03 = gameObject.AddComponent<AudioSource>();
    track03.loop = true;
    isPlayingTrack03 = false;

    track04 = gameObject.AddComponent<AudioSource>();
    track04.loop = true;

    SwapTrack(defaultAmbience);

  }



 public void SwapMusic(AudioClip newClip, float timeToFade=0.25f, float volume=1) {
   StopAllCoroutines();
   StartCoroutine(FadeMusic(newClip, timeToFade, volume));

   isPlayingTrack03 = !isPlayingTrack03;
 }

 private IEnumerator FadeMusic(AudioClip newClip, float timeToFade=0.25f, float volume=1) {
   //float timeToFade = 0.25f;
   float timeElapsed = 0;
   if (isPlayingTrack03) {
     track04.clip = newClip;
     track04.Play();
     while(timeElapsed < timeToFade) {
       track04.volume = Mathf.Lerp(0, volume, timeElapsed/timeToFade);
       track03.volume = Mathf.Lerp(volume, 0, timeElapsed/timeToFade);
       timeElapsed += Time.deltaTime;
       yield return null;
     }
     //track03.Stop();

   }
   else {
     track03.clip = newClip;
     track03.Play();
     while(timeElapsed < timeToFade) {
       track03.volume = Mathf.Lerp(0, volume, timeElapsed/timeToFade);
       track04.volume = Mathf.Lerp(volume, 0, timeElapsed/timeToFade);
       timeElapsed += Time.deltaTime;
       yield return null;
     }
     isPlayingTrack03 = true;
     //track04.Stop();
   }
 }

public void SwapTrack(AudioClip newClip, float timeToFade=0.25f, float volume=1) {
  StopAllCoroutines();

  StartCoroutine(FadeTrack(newClip, timeToFade, volume));

  isPlayingTrack01 = !isPlayingTrack01;

}
  public void ReturnToDefault() {
    SwapTrack(defaultAmbience);
  }

  private IEnumerator FadeTrack(AudioClip newClip, float timeToFade=0.25f, float volume=1) {
    //float timeToFade = 0.25f;
    float timeElapsed = 0;
    if (isPlayingTrack01) {
      track02.clip = newClip;
      track02.Play();
      while(timeElapsed < timeToFade) {
        track02.volume = Mathf.Lerp(0, volume, timeElapsed/timeToFade);
        track01.volume = Mathf.Lerp(volume, 0, timeElapsed/timeToFade);
        timeElapsed += Time.deltaTime;
        yield return null;
      }
      track01.Stop();

    }
    else {
      track01.clip = newClip;
      track01.Play();
      while(timeElapsed < timeToFade) {
        track01.volume = Mathf.Lerp(0, volume, timeElapsed/timeToFade);
        track02.volume = Mathf.Lerp(volume, 0, timeElapsed/timeToFade);
        timeElapsed += Time.deltaTime;
        isPlayingTrack01 = true;
        yield return null;
      }
      track02.Stop();
    }
  }




}
