using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordwoosh : MonoBehaviour
{
	[SerializeField]
	private Transform _wooshPoint;
	[SerializeField]
	private Rigidbody _rb;
	[SerializeField]
	private float currentPitch = 1f;
	[SerializeField]
	private float wooshSpeedReq = 1f;
	[SerializeField]
	private AudioSource wooshSource;
	
    // Start is called before the first frame update
    void Start()
    {
        if(_rb == null)
			GetComponent<Rigidbody>();
		if(wooshSource == null)
			GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_rb.angularVelocity.sqrMagnitude > wooshSpeedReq * wooshSpeedReq)
		{
			if(!wooshSource.isPlaying)
			{
				ChangePitch();
				PlayWoosh();
			}
		}
    }
	
	public void PlayWoosh()
	{
		wooshSource.Play();
	}
	
	public void ChangePitch()
	{
		currentPitch = mapValue(currentPitch, 0f, 5f, .5f, 2f);
	}
	
	float mapValue(float mainValue, float inValueMin, float inValueMax, float outValueMin, float outValueMax)
	{
		return (mainValue - inValueMin) * (outValueMax - outValueMin) / (inValueMax - inValueMin) + outValueMin;
	}
}
