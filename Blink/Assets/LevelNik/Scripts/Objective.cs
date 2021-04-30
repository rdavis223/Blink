using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public GameObject objective;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PromptText());
    }

    IEnumerator PromptText()
    {
        objective.SetActive(true);
        yield return new WaitForSeconds(3);
        objective.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
