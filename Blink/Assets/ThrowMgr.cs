using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMgr : MonoBehaviour
{

    public GameObject starPrefab;
    private GameObject currentStar;    
    private bool hasStar = true;
    public Camera playerCamera;

    private Vector3 pullLocation;
    private bool pulling = false;

    private float pullTime = 0f;

    public GameObject curvePoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && hasStar && !pulling){
            currentStar = Instantiate(starPrefab);
            currentStar.transform.position = playerCamera.transform.position;
            currentStar.transform.forward = playerCamera.transform.forward;
            hasStar = false;
            
        } else if (Input.GetKeyDown(KeyCode.F) && !hasStar && !pulling){
            currentStar.GetComponent<Star>().SetStop();
            pullLocation = currentStar.transform.position;
            pulling = true; 
            pullTime = 0f;
        }

        if (pulling && pullTime < 1) {
            currentStar.transform.position = GetQuadraticCurvePoint(pullTime, pullLocation, curvePoint.transform.position, this.transform.position);
            pullTime += Time.deltaTime;
        } else if (pulling){
            Destroy(currentStar);
            pulling = false;
            hasStar = true; 
        }
    }

    Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}
