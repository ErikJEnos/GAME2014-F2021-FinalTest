//********************************************************************************************
// Erik Enos 100994107
//
// MovingPlatformContoller 
//
// Added float platfroms to the script. 2021-12-17
//
//******************************************************************************************8

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovingPlatformController : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Transform tempTrans; 
    public Vector3 startingScale;
    public bool isActive;
    public bool floating; 
    public float platformTimer;
    public float threshold;

    public PlayerBehaviour player;
    public Transform[] childern;


    public AudioClip shrinkClip; 
    public AudioClip growClip;
    public AudioSource audio;

    private Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        startingScale = transform.localScale;
        tempTrans = gameObject.transform.parent;
        player = FindObjectOfType<PlayerBehaviour>();
        platformTimer = 0.1f;
        platformTimer = 0;
        isActive = false;
        distance = end.position - start.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive == false)
        {
            ReturnSize(); //returns object to the original size
        }

        if (isActive)
        {
            platformTimer += Time.deltaTime;
            _Move();

            if (floating == true) //If the Platfrom is marked for floating it will shrink in size 
            {
                tempTrans = gameObject.transform.parent;
                childern = gameObject.GetComponentsInChildren<Transform>();
                player.transform.parent = null; //have to remove the player for localScale to not shrink player too 
                transform.localScale -= new Vector3(0.1F, 0.1f, 0.1f) * Time.smoothDeltaTime;
                player.transform.parent = tempTrans;//adds player back after scaling 

            }
        }
        else
        {
            if (Vector3.Distance(player.transform.position, start.position) <
                Vector3.Distance(player.transform.position, end.position))
            {
                if (!(Vector3.Distance(transform.position, start.position) < threshold))
                {
                    platformTimer += Time.deltaTime;
                    _Move();
                }
            }
            else
            {
                if (!(Vector3.Distance(transform.position, end.position) < threshold))
                {
                    platformTimer += Time.deltaTime;
                    _Move();
                }
            }
        }
    
       
    }

   
    private void ReturnSize()
    {
        if(transform.localScale.x <  startingScale.x)
        {
                transform.localScale += new Vector3(0.1F, 0.1f, 0.1f) * Time.smoothDeltaTime;
        }
    }


   

    void OnCollisionExit2D(Collision2D collision) //plays audio when player hops off 
    {
        Debug.Log("Off");
        audio.clip = growClip;
        audio.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)//plays audio when player hops on 
    {
        audio.clip = shrinkClip;
        audio.Play();
        Debug.Log("On");
    }

    private void _Move()
    {
        var distanceX = (distance.x > 0) ? start.position.x + Mathf.PingPong(platformTimer, distance.x) : start.position.x;
        var distanceY = (distance.y > 0) ? start.position.y + Mathf.PingPong(platformTimer, distance.y) : start.position.y;

        transform.position = new Vector3(distanceX, distanceY, 0.0f);
    }

    public void Reset()
    {
        transform.position = start.position;
        platformTimer = 0;
    }
}
