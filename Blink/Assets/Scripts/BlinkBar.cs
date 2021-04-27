using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkBar : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = BlinkMgr.Instance.BlinkTimer;
    }
}
