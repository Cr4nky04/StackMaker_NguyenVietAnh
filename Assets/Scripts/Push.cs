using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    public LayerMask mask;
    Vector3 dir;
    private Player player;
    public bool isPush=false;

    private void Update()
    {
        CheckPush();
    }
    private void CheckPush()
    {
        
        if (Physics.Raycast(player.transform.position, Vector3.down*1000f, 1000f, mask))
        {
            isPush = true;
        }
        
    }
}
