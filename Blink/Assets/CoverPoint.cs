using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{

    public GameObject occupyingEnemy;
    public bool occupied = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isFree()
    {
        return !occupied;
    }

    public void setOccupied(bool occ)
    {
        occupied = occ;
    }
}
