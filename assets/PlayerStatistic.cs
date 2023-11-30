using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    public Color PlayerColor;
    public int GoblinsCaptured;
    public int BagsCollected;
    // Start is called before the first frame update
    void Start()
    {
        PlayerColor = gameObject.GetComponent<HFTGamepad>().Color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
