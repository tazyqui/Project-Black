using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMusic : MonoBehaviour
{
    public AudioClip newTrack;
    public float timeToFade=0.25f;
    public float volume=1;
    private void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag("Player")) {
        AudioManager.instance.SwapMusic(newTrack, timeToFade, volume);
      }
    }
    /*
    private void OnTriggerExit2D(Collider2D other) {
      if (other.CompareTag("Player")) {
        AudioManager.instance.ReturnToDefault();
      }
    }
    */


}
