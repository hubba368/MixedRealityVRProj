using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CuratorTrigger : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;
	
	[SerializeField]
	private Transform spawn;
	[SerializeField]
	private CuratorState state;
	private GameObject reference;
	
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	
	void OnTriggerEnter(Collider coll)
	{
		if(coll.gameObject.tag == "Player")
		{
			reference = Instantiate(prefab, spawn);
			reference.GetComponent<Curator>().SetAlive(state);
		}
	}
	
	void OnTriggerExit(Collider coll)
	{
		if(coll.gameObject.tag == "Player")
		{
			if(reference != null)
			{
				Destroy(reference.gameObject);
			}
		}
	}
}
