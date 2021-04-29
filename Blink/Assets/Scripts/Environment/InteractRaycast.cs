using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractRaycast : MonoBehaviour
{
    private float raycastLength = 5f;
    [SerializeField] private LayerMask whatIsInteract;

    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Debug.DrawLine(transform.position, transform.position + forward * raycastLength, Color.green);
        if(Physics.Raycast(transform.position, forward, out RaycastHit hit, raycastLength, whatIsInteract))
        {
            Interactable interactObj = hit.transform.gameObject.GetComponent<Interactable>();
            if (interactObj)
            {
                interactObj.Hover();
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    interactObj.Interact();
                }
            }
        }
    }

}
