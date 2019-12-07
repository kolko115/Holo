using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterflyScript : MonoBehaviour
{
    [Header("Butterfly")]
	public GameObject Twig;
	public GameObject AL;
	public GameObject Caterpillar;
	public GameObject Cocoon01;
	public GameObject Butterfly;

	int clickCnt = 0;

	bool trigger = false;
	private float preTime = -1.0f;

	bool triggerChange = false;
	float blendRange = 0.0f;

	// Start is called before the first frame update
	void Start()
    {
		Cocoon01.GetComponent<MeshRenderer>().material.SetFloat("_Blend", blendRange);
	}

	// Update is called once per frame
	void Update()
	{
		// 클릭 마다 visible 변경
		if (clickCnt == 0)
		{
			setVisible(AL, true);
			setVisibleSkined(Caterpillar, false);
			setVisible(Cocoon01, false);
			setVisibleSkined(Butterfly, false);
		}
		else if (clickCnt == 1)
		{
			setVisible(AL, false);
			setVisibleSkined(Caterpillar, true);
			setVisible(Cocoon01, false);
			setVisibleSkined(Butterfly, false);
		}
		else if (clickCnt == 2 && !Caterpillar.GetComponent<Animation>().isPlaying)
		{
			setVisible(AL, false);
			setVisibleSkined(Caterpillar, false);
			setVisible(Cocoon01, true);
			setVisibleSkined(Butterfly, false);
		}
		else if (clickCnt == 3 || clickCnt == 4)
		{
			setVisible(AL, false);
			setVisibleSkined(Caterpillar, false);
			setVisible(Cocoon01, false);
			setVisibleSkined(Butterfly, true);
		}

		// 처음 시작
		if (clickCnt == 0)
		{
			Caterpillar.GetComponent<Animation>().Play();
			Debug.Log("may_caterpillar Play");
			trigger = true;
		}
		else if (!Caterpillar.GetComponent<Animation>().isPlaying && clickCnt == 1)
		{
			Caterpillar.GetComponent<Animation>()["may_caterpillar"].time = 0.0f;
			Caterpillar.GetComponent<Animation>().Stop();
			triggerChange = true;
			blendRange = 0.0f;
		}
		else if (clickCnt == 2 && blendRange >= 1.0f && !triggerChange)
		{
			Butterfly.GetComponent<Animation>().Play("may_butterfly_stay");
		}
		else if (clickCnt == 3)
		{
			clickCnt = 4;
			Butterfly.GetComponent<Animation>().Play("may_butterfly_fly");
			trigger = true;
		}

		if (clickCnt == 2 && triggerChange)
		{
			blendRange = blendRange + 0.005f; //change speed

			Cocoon01.GetComponent<MeshRenderer>().material.SetFloat("_Blend", blendRange);

			if (blendRange >= 1.0f)
			{
				triggerChange = false;
				Cocoon01.GetComponent<MeshRenderer>().material.SetFloat("_Blend", 1);
				print("changeComplete");
			}
		}
		
		if (trigger)
		{
			if (clickCnt == 4)
			{
				float curTime = Butterfly.GetComponent<Animation>()["may_butterfly_fly"].time;
				//Debug.Log("Butterfly: " + preTime + " " + curTime);

				if (preTime > curTime)
				{
					Butterfly.GetComponent<Animation>().Stop();
					Butterfly.GetComponent<Animation>()["may_butterfly_fly"].time = 0.0f;
					preTime = -1.0f;
					trigger = false;
					clickCnt = 0;
					//Debug.Log("may_butterfly_fly Stop");
				}
				else
				{
					//Debug.Log(preTime + " " + curTime);
					preTime = curTime;
				}
			}
		}
    }

    public void startAction()
    {
        if (clickCnt == 0)
        {
            clickCnt = 1;
            Debug.Log("1");
        }
        else if (clickCnt == 1 && !Caterpillar.GetComponent<Animation>().isPlaying)
        {
            clickCnt = 2;
            Debug.Log("2");
        }
        else if (clickCnt == 2 && blendRange >= 1.0f && !triggerChange)
        {
            clickCnt = 3;
            Debug.Log("3");
        }
    }


	void setVisible(GameObject obj, bool flag)
	{

		MeshRenderer[] children = obj.GetComponentsInChildren<MeshRenderer>();

		foreach (MeshRenderer child in children)
		{
			child.enabled = flag;
		}

		MeshCollider[] children_mesh = obj.GetComponentsInChildren<MeshCollider>();

		foreach (MeshCollider child in children_mesh)
		{
			child.enabled = flag;
		}

		BoxCollider[] children_box = obj.GetComponentsInChildren<BoxCollider>();

		foreach (BoxCollider child in children_box)
		{
			child.enabled = flag;
		}

	}

	void setVisibleSkined(GameObject obj, bool flag)
	{

		SkinnedMeshRenderer[] children = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

		foreach (SkinnedMeshRenderer child in children)
		{
			child.enabled = flag;
		}

		MeshCollider[] children_mesh = obj.GetComponentsInChildren<MeshCollider>();

		foreach (MeshCollider child in children_mesh)
		{
			child.enabled = flag;
		}

		BoxCollider[] children_box = obj.GetComponentsInChildren<BoxCollider>();

		foreach (BoxCollider child in children_box)
		{
			child.enabled = flag;
		}

	}
}
