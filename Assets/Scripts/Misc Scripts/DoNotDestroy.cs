using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    private void Awake() {
      //GameObject[] persObj = GameObject.FindGameObjectWithTag("Persistent");
      DontDestroyOnLoad(this.gameObject);
    }
}
