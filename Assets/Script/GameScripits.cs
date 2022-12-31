using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScripits : MonoBehaviour
{
    public bool newRound = true;
    public GameObject spawnwalls;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(roundClock());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator roundClock()
    {
        yield return new WaitForSeconds(3f);
        spawnwalls.SetActive(false);
        yield return new WaitForSeconds(15f);
        spawnwalls.SetActive(true);
    }
}
