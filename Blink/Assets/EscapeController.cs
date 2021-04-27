using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeController : MonoBehaviour
{
    public GameObject insideDoor;
    public GameObject outsideDoor;

    public GameObject car;

    public GameObject enemyParent;

    public bool objectiveRetrieved = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectiveCollected()
    {
        insideDoor.SetActive(false);
        outsideDoor.SetActive(false);
        objectiveRetrieved = true;
        enemyParent.SetActive(true);
        //displayUI here
    }

    public void StartCar()
    {
        car.SetActive(true);
        BlinkMgr.Instance.BlinkTimer = 3.5f;
    }
}
