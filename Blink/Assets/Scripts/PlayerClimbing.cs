using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    public LayerMask whatIsGround;
    [SerializeField] private Transform vaultHeight;
    [SerializeField] private Transform climbHeight;
    [SerializeField] private Camera cam;

    private bool canVault = false;
    private bool canClimb = false;

    [SerializeField] Vector3 vaultTo;
    [SerializeField] Vector3 climbTo;

    void FixedUpdate()
    {
        if (canVault)
        {
            print("vaulting");
            transform.position += vaultTo;
            canVault = false;
        }
        
        if (canClimb)
        {
            print("climbing");
            transform.position += climbTo;
            canClimb = false;
        }
    }

    private void CheckRays()
    {
        RaycastHit hit;
        RaycastHit obstructionHit;
        Vector3 dir = cam.transform.forward;
        Vector3 pos = climbHeight.position;
        Vector3 obstruction_pos = new Vector3(pos.x, pos.y + .5f, pos.z);
        dir.y = 0;
        Debug.DrawLine(pos, pos + dir * 1f, Color.red);
        if (Physics.Raycast(pos, dir, out hit, 1f, whatIsGround) && !Physics.Raycast(obstruction_pos, dir, out obstructionHit, 1f, whatIsGround))
        {
            print("hit at climb height");
            canClimb = true;
            return;
        }
        pos = vaultHeight.position;
        obstruction_pos = new Vector3(pos.x, pos.y + .5f, pos.z);
        if (!canClimb && Physics.Raycast(pos, dir, out hit, 1f, whatIsGround) && !Physics.Raycast(obstruction_pos, dir, out obstructionHit, 1f, whatIsGround))
        {
            print("hit at vault height");
            canVault = true;
            return;
        }
    }
}

