using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class ObjectChange : MonoBehaviour
{
    Controller controller;
    Frame frame;
    List<Hand> hands;
    IEnumerator startNext;
    float palmVelX;

    [Header("Dandelion")]
    public GameObject dandelion;

    [Header("Hand Palm")]
    public PalmDirectionDetector leftPalmD;

    // Start is called before the first frame update
    void Start()
    {
        startNext = getStart();
        addFuction();
    }

    // Update is called once per frame
    void Update()
    {
        controller = new Controller();
        frame = controller.Frame();
        hands = frame.Hands;
    }

    public void addFuction()
    {
        Debug.Log("addFuction!!");
        leftPalmD.OnActivate.AddListener(getnextObject);
        //LeftPalmD.OnActivate.RemoveListener(getnextObject);
    }

    public void getnextObject()
    {
        StartCoroutine(startNext);
    }

    IEnumerator getStart()
    {
        while (true)
        {
            if (hands.Count > 0)
            {
                //Debug.Log(hands.Count);
                //Debug.Log("coroutine!!");
                if (hands[0].IsLeft)
                {
                    palmVelX = hands[0].PalmVelocity.x;
                    //Debug.Log(palmVelX);
                }


                if (hands.Count > 1)
                {
                    if (hands[1].IsLeft)
                    {
                        palmVelX = hands[1].PalmVelocity.x;
                        //Debug.Log(palmVelX);
                    }
                }
            }

            if (palmVelX > 1000f)
            {
                dandelion.SetActive(true);
            }

            yield return null;
        }
    }
}
