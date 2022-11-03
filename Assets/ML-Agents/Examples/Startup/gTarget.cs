using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gTarget : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "agent")
        {
            gRollerAgent ra = other.GetComponent<gRollerAgent>();
            if (null !=ra)
            {
                ra.EnteredTarget();
                this.gameObject.SetActive(false);
            }
        }
    }
   
}
