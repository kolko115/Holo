using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

// c클릭하면 민들레 날림 (끝나면 원 상태로 돌아감)
public class dandelionScript : MonoBehaviour
{
    public GameObject dandelion;
    Animation dandelionAni;
    Controller controller;
    Frame frame;
    List<Hand> hands;
    IEnumerator windAction;

    private float preTime;
    string input;
	bool ResetState = false;
    int count = 0;
    float palmVelX = 0;

    // Start is called before the first frame update
    void Start()
	{
		//Debug.Log("start!!");
        dandelionAni = dandelion.GetComponent<Animation>();

        windAction = handswing();

        preTime = -0.1f;

        changeScript changescript = GameObject.Find("SceneChange").GetComponent<changeScript>();

        // 흩날리는 ani1 속도 조정, 민들레 흔들리는 ani0 play
        dandelionAni["dandelion_ani1"].speed = 1.2f;
        dandelionAni.Play("dandelion_ani0");
	}

	// Update is called once per frame
	void Update()
	{
        controller = new Controller();
        frame = controller.Frame();
        hands = frame.Hands;

        if (dandelion.activeInHierarchy)
        {
            dandelionAction();
        }

	}

    private void dandelionAction()
    {
        if (!ResetState)
        {
            // 흩날리는 ani1 시간
            float curTime = dandelionAni["dandelion_ani1"].time;
            //Debug.Log("ani1 curTime: " + curTime + "    preTime: " + preTime);

            // 흩날리는 ani1 끝나면 흔들리는 ani0 실행
            if (preTime >= curTime)
            {
                //Debug.Log("ani2");
                dandelionAni.Stop();
                dandelionAni["dandelion_ani1"].time = 0.0f;
                dandelionAni.Play("dandelion_ani0");
                preTime = -1.0f;
                ResetState = true;

                //Debug.Log("ani1 Stop");

            }
            else
            {
                preTime = curTime;
            }
        }

    }

    public void HandWind()
    {
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(windAction);
        }
    }

    public void WindStop()
    {
        if (this.gameObject.activeInHierarchy)
        {
            StopCoroutine(windAction);
        }
    }

    // 바람불고 민들레 날아감
    IEnumerator handswing()
    {
        while (true)
        {
            //첫번째 손 
            if (hands.Count > 0)
            {
                if (hands[0].IsRight)
                {
                    palmVelX = hands[0].PalmVelocity.x;

                    if (palmVelX > 1500f || palmVelX < -1500f)
                    {
                        if (dandelion.activeInHierarchy)
                        {
                            Debug.Log(count);
                            count++;
                        }
                    }
                }

                //두번째 손
                if (hands.Count > 1)
                {
                    if (hands[1].IsRight)
                    {
                        palmVelX = hands[1].PalmVelocity.x;

                        if (palmVelX > 1500f || palmVelX < -1500f)
                        {
                            if (dandelion.activeInHierarchy)
                            {
                                Debug.Log(count);
                                count++;
                            }
                        }
                    }
                }
            }

            if (ResetState && count >= 20)
            {
                dandelionAni.Stop();
                dandelionAni["dandelion_ani2"].time = 0.0f;
                dandelionAni.Play("dandelion_ani1");
                ResetState = false;
                count = 0;
            }

            yield return null;
        }
    }
}