using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        col.gameObject.transform.SetParent(gameObject.transform, true);
    }

    void OnCollisionExit(Collision col)
    {
        col.gameObject.transform.parent = null;
    }
}
