using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableAfterSeconds : MonoBehaviour
{
    public void startDisabling(int secs){
        StartCoroutine(disabling(secs));
    }
    IEnumerator disabling(int secs){
        yield return new WaitForSeconds(5);
        this.gameObject.SetActive(false);
    }
}
