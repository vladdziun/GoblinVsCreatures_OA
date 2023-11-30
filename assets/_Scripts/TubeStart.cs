using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeStart : MonoBehaviour
{
    public GameObject end;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerTeam1" || col.gameObject.tag == "PlayerTeam0")
        {
                col.transform.position = end.transform.position + new Vector3(0, 0, 0);
        }
    }
}
