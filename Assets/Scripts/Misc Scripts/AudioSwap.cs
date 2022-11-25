using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwap : MonoBehaviour
{
    public AudioClip newTrack;
    public bool Permanent=false;
    public float timeToFade=0.25f;
    public float volume=1;
    private void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag("Player")) {
        AudioManager.instance.SwapTrack(newTrack, timeToFade, volume);
      }
    }


    private void OnTriggerExit2D(Collider2D other) {
      if (!Permanent) {
        if (other.CompareTag("Player")) {
          AudioManager.instance.ReturnToDefault();
        }
      }

    }
}
