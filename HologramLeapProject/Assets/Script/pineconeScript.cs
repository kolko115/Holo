using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class pineconeScript : MonoBehaviour
{
    public GameObject pinecone;
    changeScript changescript;
    Animation pineconeAni;
    Controller controller;
    Frame frame;
    List<Hand> hands;

    IEnumerator rainRoutine;
    IEnumerator sunRoutine;

    public ParticleSystem rain;
	public ParticleSystem sun;

	public int delayTime = 0;
	public int stopTime = -1;

	bool spreadPinecone = true;
    float palmVelY = 0f;

	// Start is called before the first frame update
	void Start()
	{
        changescript = GameObject.Find("SceneChange").GetComponent<changeScript>();
        //changescript.rightPalmD.OnActivate.AddListener(rainning);

        rainRoutine = startRain();
        sunRoutine = startShine();

        pineconeAni = pinecone.GetComponent<Animation>();
	}

	// Update is called once per frame
	void Update()
	{
        controller = new Controller();
        frame = controller.Frame();
        hands = frame.Hands;

        //Debug.Log(hands.Count);
        // rain -> pinecone play
        if (rain.isPlaying)
		{
            //StopCoroutine(rainRoutine);
			//Debug.Log("rain");
			if (delayTime == 150)
			{
				//Debug.Log("ani");
                pineconeAni["ani_0"].speed = 1;
                pineconeAni["ani_0"].time = 0;
                pineconeAni.Play("ani_0");
				stopTime = -1;
			}
			else if (delayTime > 150 && !pineconeAni.isPlaying && stopTime == -1)
			{
				stopTime = delayTime;
			}
			else if (delayTime > 400 && !pineconeAni.isPlaying && (delayTime - stopTime) > 10 && stopTime != -1)
			{
				ParticleSystem.EmissionModule m_rain = rain.emission;
				m_rain.enabled = false;
				rain.Stop();
				rain.Clear();
                spreadPinecone = false;
                StopCoroutine(rainRoutine);
                //Debug.Log("rain stop");
			}
			delayTime++;
		}

		// sun -> pinecone reverse play
		if (sun.isPlaying)
		{
            //StopCoroutine(sunRoutine);
			//Debug.Log("sun");
			if (delayTime == 150)
			{
				//Debug.Log("reverse ani");
                pineconeAni["ani_0"].speed = -1;
                pineconeAni["ani_0"].time = pineconeAni["ani_0"].length;
                pineconeAni.Play("ani_0");
				stopTime = -1;
			}
			else if (delayTime > 150 && !pineconeAni.isPlaying && stopTime == -1)
			{
				stopTime = delayTime;
			}
			else if (delayTime > 300 && !pineconeAni.isPlaying && (delayTime - stopTime) > 10 && stopTime != -1)
			{
				ParticleSystem.EmissionModule m_sun = sun.emission;
				m_sun.enabled = false;
				sun.Stop();
				sun.Clear();
                spreadPinecone = true;
                StopCoroutine(sunRoutine);
                //Debug.Log("sun stop");
			}
			delayTime++;
		}

		if (!rain.isPlaying && !sun.isPlaying)
		{
			delayTime = 0;
		}
	}

    public void rainning()
    {
        if (pinecone.activeInHierarchy)
        {
            StartCoroutine(rainRoutine);
        }
        //Debug.Log("rain");
    }

    public void dontrain()
    {
        if (pinecone.activeInHierarchy)
        {
            StopCoroutine(rainRoutine);
        }      
    }

    public void sunshine()
    {
        if (pinecone.activeInHierarchy)
        {
            StartCoroutine(sunRoutine);
        }     
    }

    public void dontshine()
    {
        if (pinecone.activeInHierarchy)
        {
            StopCoroutine(sunRoutine);
        }
    }

    IEnumerator startRain()
    {
        while (true)
        {
            //Debug.Log(hands.Count);
            if (hands.Count > 0)
            {
                //Debug.Log(hands[0]);
                if (hands[0].IsRight)    // 왼손 오른손 구분 가능
                {
                    palmVelY = hands[0].PalmVelocity.y;
                    //Debug.Log(palmVelY);
                }

                if (hands.Count > 1)
                {
                    if (hands[1].IsRight)
                    {
                        palmVelY = hands[1].PalmVelocity.y;
                        //Debug.Log(palmVelY);
                    }
                }
            }

            if (spreadPinecone == true && palmVelY < -1000f)
            {
                //Debug.Log("doing!!");
                //pointingDirection 방향 위로 바꾸기
                changescript.rightPalmD.PointingDirection = Vector3.up;

                //비 재생
                ParticleSystem.EmissionModule m_rain = rain.emission;
                m_rain.enabled = true;
                rain.Play();

            }

            yield return null;
        }
    }

    IEnumerator startShine()
    {
        while (true)
        {
            if (hands.Count > 0)
            {
                if (hands[0].IsRight)    // 왼손 오른손 구분 가능
                {
                    palmVelY = hands[0].PalmVelocity.y;
                    //Debug.Log(palmVelY);
                }
                if (hands.Count > 1)
                {
                    if (hands[1].IsRight)
                    {
                        palmVelY = hands[1].PalmVelocity.y;
                    }
                }
                
            }

            if (spreadPinecone == false && palmVelY > 1000f)
            {
                //pointingDirection 방향 아래로 바꾸기
                changescript.rightPalmD.PointingDirection = Vector3.down;

                //햇빛 재생
                //Debug.Log("sunshine!!!!");

                ParticleSystem.EmissionModule m_sun = sun.emission;
                m_sun.enabled = true;
                sun.Play();

            }

            yield return null;
        }
    }
}
