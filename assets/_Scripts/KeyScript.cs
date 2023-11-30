using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{ 
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerTeam1" || col.gameObject.tag == "PlayerTeam0")
        {
            col.gameObject.GetComponent<UpgradeManager>().isKey = true;
            Destroy(gameObject);
        }
    }
}
