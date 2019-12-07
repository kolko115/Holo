using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class SpiderScript : MonoBehaviour
{
    Controller controller;
    Frame frame;
    List<Hand> hands;

    RectTransform spider_web_rt;

    public GameObject spider_web;
    public GameObject spider;

	bool trigger = true;
	int playClip = 1;
	private float preTime = -0.1f;
	private float waitTime;
	private float currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
		spider_web_rt = spider_web.GetComponent<RectTransform>();
        spider_web_rt.sizeDelta = new Vector2(10, 10);

        spider.GetComponent<Animation>().Play("spider_ani0");

		spider_web.GetComponent<CanvasRenderer>().SetAlpha(0.0f);

    }

	// Update is called once per frame
	void Update()
	{
        controller = new Controller();
        frame = controller.Frame();
        hands = frame.Hands;

        if (playClip == 8 || playClip == 9) currentTime += Time.deltaTime;

		if (trigger)
		{
			Debug.Log(playClip);
			if (playClip == 1)
			{
				float curTime = spider.GetComponent<Animation>()["spider_ani1"].time;
				Debug.Log("curTime: " + curTime);
				if (preTime >= curTime)
				{
                    spider.GetComponent<Animation>().Stop();
                    spider.GetComponent<Animation>()["spider_ani1"].time = 0.0f;
                    spider.GetComponent<Animation>().Play("spider_ani2");
					preTime = -1.0f;
					trigger = false;
					playClip = 2;
					Debug.Log("Stop");

				}
				else
				{
					Debug.Log(preTime + " " + curTime);
					preTime = curTime;
				}
			}
			else if (playClip == 3)
			{
				float curTime = spider.GetComponent<Animation>()["spider_ani3"].time;
				if (preTime >= curTime)
				{
                    spider.GetComponent<Animation>().Stop();
                    spider.GetComponent<Animation>()["spider_ani3"].time = 0.0f;
                    spider.GetComponent<Animation>().Play("spider_ani4");
					preTime = -1.0f;
					trigger = false;
					playClip = 4;
					Debug.Log("Stop");

				}
				else
				{
					Debug.Log(preTime + " " + curTime);
					preTime = curTime;
				}
			}
			else if (playClip == 5)
			{
				float curTime = spider.GetComponent<Animation>()["spider_ani5"].time;
				if (preTime >= curTime)
				{
                    spider.GetComponent<Animation>().Stop();
                    spider.GetComponent<Animation>()["spider_ani5"].time = 0.0f;
                    spider.GetComponent<Animation>().Play("spider_ani6");
					preTime = -1.0f;
					trigger = false;
					playClip = 6;
					Debug.Log("spider_ani5 Stop");

				}
				else
				{
					Debug.Log(preTime + " " + curTime);
					preTime = curTime;
				}
			}
			else if (playClip == 7)
			{
				float curTime = spider.GetComponent<Animation>()["spider_ani7"].time;
				if (preTime >= curTime)
				{
                    spider.GetComponent<Animation>().Stop();
					currentTime = 0.0f;
					waitTime = currentTime;
					preTime = -1.0f;
					trigger = false;
					playClip = 8;
					Debug.Log("spider_ani7 Stop");

				}
				else
				{
					Debug.Log(preTime + " " + curTime);
					preTime = curTime;
				}
			}
			else if (playClip == 9)
			{
				float curTime = spider.GetComponent<Animation>()["spider_ani8"].time;
				if (preTime >= curTime)
				{
					Debug.Log("spider_ani8 Stop " + curTime + " and " + preTime);

                    spider.GetComponent<Animation>().Stop();
                    spider.GetComponent<Animation>()["spider_ani8"].time = 0.0f;
                    spider.GetComponent<Animation>().Play("spider_ani0");
					preTime = -1.0f;
					trigger = false;
					playClip = 0;

					spider_web.GetComponent<CanvasRenderer>().SetAlpha(0.0f);

					Debug.Log("spider_ani8 End");


				}
				else
				{
					Debug.Log(preTime + " " + curTime);
					preTime = curTime;
				}

				if (playClip == 9)
				{
					float second = (currentTime - waitTime);
					float alpha = 1.0f - (second / 1.5f);
					if (alpha > 1.0f) alpha = 1.0f;
					else if (alpha < 0.0f) alpha = 0.0f;
					spider_web.GetComponent<CanvasRenderer>().SetAlpha(alpha);
					Debug.Log("spider_ani8 Alpha: " + alpha);
				}


			}

		}
		if (playClip == 8)
		{
			float second = (currentTime - waitTime);

			if (second > 0.5f)
			{
                spider.GetComponent<Animation>()["spider_ani8"].time = 0.0f;
                spider.GetComponent<Animation>().Play("spider_ani8");

				trigger = true;
				playClip = 9;
				preTime = -1.0f;
				spider_web.GetComponent<CanvasRenderer>().SetAlpha(255.0f);
				waitTime = currentTime;
				Debug.Log("spider_ani5 stop and spider_ani8 play");
			}
			else
			{
				//GetComponent<Animation> ().Stop();
				float alpha = second * 8.0f;
				if (alpha > 1.0f) alpha = 1.0f;
				spider_web.GetComponent<CanvasRenderer>().SetAlpha(alpha);
				Debug.Log("spider_ani5 Alpha: " + alpha);

			}

			Debug.Log(second + " = " + currentTime + " - " + waitTime);
		}
		if (Input.GetKeyDown("d"))
		{
			Debug.Log("click d");
			trigger = true;
		}
	}

    public void moveSpider()
    {
        Debug.Log("touch!!");
        if (playClip == 0)
        {
            spider.GetComponent<Animation>().Play("spider_ani1");
            trigger = true;
            playClip = 1;
        }
        else if (playClip == 2)
        {
            spider.GetComponent<Animation>().Play("spider_ani3");
            trigger = true;
            playClip = 3;
        }
        else if (playClip == 4)
        {
            spider.GetComponent<Animation>().Play("spider_ani5");
            trigger = true;
            playClip = 5;
        }
        else if (playClip == 6)
        {
            spider.GetComponent<Animation>().Play("spider_ani7");
            trigger = true;
            playClip = 7;
        }
    }

}
