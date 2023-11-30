using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HappyFunTimes;

public class UpgradeManager : MonoBehaviour
{
    public bool isKey;
    public bool isShovel;
    public bool isSpeedUp;
    public SpriteRenderer keySprite;
    public SpriteRenderer shovelSprite;
    public SpriteRenderer speedUpSprite;
    public TrailRenderer speedUpTrail;

    private HFTGamepad m_gamepad;
    private HFTInput m_hftInput;

    private bool checkForDoorOpenClose;
    private Collider2D doorObject;

    void Start()
    {
        m_gamepad = GetComponent<HFTGamepad>();
        m_hftInput = GetComponent<HFTInput>();
    }

    void Update()
    {
        if (isKey)
            keySprite.enabled = true;
        if (isShovel && shovelSprite.enabled ==false)
        {
            shovelSprite.enabled = true;
            StartCoroutine(Shovel());
        }
        if (isSpeedUp && speedUpSprite.enabled == false)
        {
            speedUpSprite.enabled = true;
            speedUpTrail.enabled = true;
            StartCoroutine(SpeedUp());
        }
        if (!isKey)
        {
            if (keySprite == null)
                return;
            keySprite.enabled = false;
        }

        if (checkForDoorOpenClose)
        {
            OpenCloseDoor();
        }
            
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Door")
        {
            checkForDoorOpenClose = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "DestWall" && isShovel)
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Door")
        {
            doorObject = col;
            checkForDoorOpenClose = true;
        }
    }

    void OpenCloseDoor()
    {
        if (m_hftInput.GetButtonDown("fire1") || Input.GetKeyDown("space"))
        {
            Debug.Log("Trying open the door");
            if (isKey && doorObject != null)
            {
                if (gameObject.GetComponent<Player>().team == 1)
                {
                    doorObject.gameObject.GetComponent<DoorScript>().OpenDoor();
                }
                else
                {
                    doorObject.gameObject.GetComponent<DoorScript>().CloseDoor();
                }
                isKey = false;
            }
        }
    }

    IEnumerator Shovel()
    {
        yield return new WaitForSeconds(10);
        shovelSprite.enabled = false;
        isShovel = false;
    }

    IEnumerator SpeedUp()
    {
        var defaultSpeed = gameObject.GetComponent<BirdScript>().maxSpeed;
        gameObject.GetComponent<BirdScript>().maxSpeed = defaultSpeed * 1.5f;
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<BirdScript>().maxSpeed = defaultSpeed;
        speedUpSprite.enabled = false;
        isSpeedUp = false;
        yield return new WaitForSeconds(1);
        speedUpTrail.enabled = false;
    }
}
