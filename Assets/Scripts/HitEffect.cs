using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
	private ParticleSystem p;
    private float liveTime = 2f;
	
	// Start is called before the first frame update
    void Start()
    {
        p = GetComponent<ParticleSystem>();
    }

	
	
    // Update is called once per frame
    void Update()
    {
        liveTime -= Time.deltaTime;
        if(liveTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
