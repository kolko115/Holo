using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class changeScript : MonoBehaviour
{
    Controller controller;
    Frame frame;
    List<Hand> hands;

    IEnumerator startNext;
    protected float palmVelX;

    [Header("Objects")]
    public GameObject pineconeScene;
    public GameObject dandelionScene;
    public GameObject spiderScene;
    public GameObject butterflyScene;

    [Header("Hands")]
    public PalmDirectionDetector leftPalmD;
    public PalmDirectionDetector rightPalmD;

    int numScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        //pineconeScene = transform.GetChild(0).gameObject;
        //dandelionScene = transform.GetChild(1).gameObject;
        //spiderScene = transform.GetChild(2).gameObject;
        //butterflyScene = transform.GetChild(3).gameObject;

        //leftPalmD = GameObject.Find("Hands").transform.GetChild(0).GetComponent<PalmDirectionDetector>();
        //rightPalmD = GameObject.Find("Hands").transform.GetChild(1).GetComponent<PalmDirectionDetector>();

        startNext = getStart();

        leftPalmD.OnActivate.AddListener(getnextObject);
        leftPalmD.OnDeactivate.AddListener(stopnextObject);

        pineconeScene.SetActive(true);
        dandelionScene.SetActive(false);
        spiderScene.SetActive(false);
        butterflyScene.SetActive(false);
    }

    // Update is called once per frame
    protected void Update()
    {
        controller = new Controller();
        frame = controller.Frame();
        hands = frame.Hands;
    }

    public void getnextObject()
    {
        Debug.Log("do!");

        StartCoroutine(startNext);
    }

    public void stopnextObject()
    {
        Debug.Log("stop!");

        StopCoroutine(startNext);
    }

    IEnumerator getStart()
    {
        if (dandelionScene.activeInHierarchy)
        {
            
        }

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
            
            if (palmVelX > 1500f)
            {
                if (pineconeScene.activeInHierarchy)// && !dandelionScene.activeInHierarchy && !spiderScene.activeInHierarchy && !butterflyScene.activeInHierarchy)
                {
                    pineconeScene.SetActive(false);
                    rightPalmD.PointingDirection = Vector3.left;


                    if (!pineconeScene.activeInHierarchy)
                    {
                        Debug.Log("check1");
                        yield return new WaitForSeconds(3.0f);
                        Debug.Log("check2");
                        dandelionScene.SetActive(true);
                    }
                    
                }
                else if (dandelionScene.activeInHierarchy)// && !pineconeScene.activeInHierarchy && !spiderScene.activeInHierarchy && !butterflyScene.activeInHierarchy)
                {
                    dandelionScene.SetActive(false);
                    rightPalmD.PointingDirection = Vector3.zero;

                    if (!dandelionScene.activeInHierarchy)
                    {
                        yield return new WaitForSeconds(3.0f);
                        spiderScene.SetActive(true);
                    }
                    

                }
                else if (spiderScene.activeInHierarchy)// && !pineconeScene.activeInHierarchy && !dandelionScene.activeInHierarchy && !butterflyScene.activeInHierarchy)
                {
                    spiderScene.SetActive(false);
                    
                    if (!spiderScene.activeInHierarchy)
                    {
                        yield return new WaitForSeconds(3.0f);
                        butterflyScene.SetActive(true);
                    }

                }
                else if (butterflyScene.activeInHierarchy)// && !pineconeScene.activeInHierarchy && !dandelionScene.activeInHierarchy && !spiderScene.activeInHierarchy)
                {
                    butterflyScene.SetActive(false);
                    rightPalmD.PointingDirection = Vector3.down;
                    
                    if (!butterflyScene.activeInHierarchy)
                    {
                        yield return new WaitForSeconds(3.0f);
                        pineconeScene.SetActive(true);
                    }
                }

            }

            yield return null;
        }
    }

}
