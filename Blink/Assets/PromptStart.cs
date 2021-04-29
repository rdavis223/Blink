using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptStart : MonoBehaviour
{

    public GameObject prompt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<PromptTrigger>().openPrompt();
        GetComponent<PromptStart>().enabled = false;
    }
}
