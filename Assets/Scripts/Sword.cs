using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class Sword : XRGrabInteractable
{	
	[SerializeField] CapsuleCollider _handle;
	[SerializeField] Rigidbody _rb;
	[SerializeField] Transform _balancePoint;
	[SerializeField] CapsuleCollider _blade;
	
	[Tooltip("Audio")]
	[SerializeField]
	private AudioSource metalHit;
	[SerializeField]
	private List<AudioClip> clips;
	//private Dictionary<string, AudioClip> audioclips;
	
	private Transform _grabbedObj;
	private ConfigurableJoint _joint;
	private Component _jointComp;
	
	private bool isGrabbed = false;
	
	private ActionBasedController grabbedController;

    void Start()
    {
		//set up rigidbody
		_rb.maxAngularVelocity = 20f;
		
		//audioclips = new Dictionary<string,AudioClip>();

		// sword meshes have different pivots,
		// so using empty transform to act as parent pivot
		transform.position = _handle.transform.position;
    }
	
	// if joint doesn't exist - usually before first grab and subsequent grabs
	private void SetupJoint(Rigidbody rb)
	{
		if(_joint == null)
		{
			_joint = gameObject.AddComponent<ConfigurableJoint>();
			
			_joint.connectedBody = rb;
			
			_joint.axis = new Vector3(1f,0f,0f);
			_joint.secondaryAxis = new Vector3(0f,1f,0f);
			
			_joint.autoConfigureConnectedAnchor = false;
			
			_joint.xMotion = ConfigurableJointMotion.Limited;
			_joint.yMotion = ConfigurableJointMotion.Limited;
			_joint.zMotion = ConfigurableJointMotion.Limited;
			_joint.angularXMotion = ConfigurableJointMotion.Free;
			_joint.angularYMotion = ConfigurableJointMotion.Free;
			_joint.angularZMotion = ConfigurableJointMotion.Free;
			
			var limit = _joint.linearLimit;
			
			limit.limit = 0f;
			limit.bounciness = 0f;
			limit.contactDistance = 0.1f;
			_joint.linearLimit = limit;
			
			/*var driveX = _joint.xDrive;
			var driveY = _joint.yDrive;
			var driveZ = _joint.zDrive;
			
			driveX.positionSpring = 10000f;
			driveX.positionDamper = 100f;
			driveY.positionSpring = 10000f;
			driveY.positionDamper = 100f;
			driveZ.positionSpring = 10000f;
			driveZ.positionDamper = 100f;
			
			_joint.xDrive = driveX;
			_joint.yDrive = driveY;
			_joint.zDrive = driveZ;*/
			
			_joint.rotationDriveMode = RotationDriveMode.Slerp;
			var slerp = _joint.slerpDrive;
			
			slerp.positionSpring = 10000f;
			slerp.positionDamper = 500f;
			_joint.slerpDrive = slerp;
			
			_joint.projectionMode = JointProjectionMode.PositionAndRotation;
			
			_joint.configuredInWorldSpace = true;
			
			_joint.breakTorque = 20000f;
		}
	}
	
	private void OnJointBreak(float force)
	{
		//TODO need to call events ??
		isGrabbed = false;
	}
	
	private void RemoveJoint()
	{
		Destroy(_joint);
	}
	
	
	
	List<ContactPoint> hits = new List<ContactPoint>();
	bool hasHit = false;
	Collider lastHitThing = null;
	Vector3 angVel = Vector3.zero;
	
	void Update()
	{
		angVel = _rb.angularVelocity;
	}
	
	void OnCollisionStay(Collision collision)
    {		
		// only make FX on hard swings
		if(isGrabbed && !hasHit)
		{// todo change for perf reasons
			hits.Clear();
			collision.GetContacts(hits);
			if(hits.Count == 0)
			{
				Debug.Log("no hit");
				return;
			}
			
			// heavy swings
			if(angVel.sqrMagnitude > 10f * 10f)
			{
				// change from contacts
				foreach (ContactPoint contact in collision.contacts)
				{
					if(contact.point == null || contact.otherCollider == null)
					{
						return;
					}
					
					var pos = contact.point;
					
					if(contact.otherCollider.sharedMaterial != null && 
					contact.otherCollider.sharedMaterial.name == "SwordMetal")
					{

						//play hit FX
						hasHit = true;
						SendHapticOnCollision(true);
						lastHitThing = contact.otherCollider;
						var go = Instantiate(Resources.Load("MetalEffect"), pos, Quaternion.identity);
						foreach(var c in clips)
						{
							if(c.name == "swordonmetal")
								metalHit.clip = c;
						}
						if(metalHit.clip != null)
							metalHit.Play();
					}
				}
			}
		}
    }
	
	private void SendHapticOnCollision(bool isHeavy)
	{
		if(grabbedController != null && grabbedController.enabled == true)
		{
			if(isHeavy)
			{				
				grabbedController.SendHapticImpulse( 1f, 0.3f);
			}
			else
			{
				grabbedController.SendHapticImpulse(0.5f, 0.1f);
			}
		}
	}
	
	private void SendHapticOnGrab()
	{
		grabbedController.SendHapticImpulse( 0.4f, 0.15f);
	}
	
	void OnCollisionExit(Collision collision)
	{// skip the thing we just hit
		if(collision.collider == lastHitThing)
		{
			hasHit = false;
		}
	}
	
	public void EnableHandleCollider()
	{
		_handle.enabled = true;
	}
	public void DisableHandCollider()
	{		
		_handle.enabled = false;
	}
	
	protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
		isGrabbed = true;
		_rb.constraints = RigidbodyConstraints.None;
		// get the hand grab transform when sword is picked up
		// not the best way but is only called once per event invocation
		var obj = args.interactorObject;		
		
		_grabbedObj = obj.transform.GetChild(0).GetChild(0).GetChild(0);
		grabbedController = obj.transform.GetComponent<ActionBasedController>();		
		SendHapticOnGrab();
		SetupJoint(_grabbedObj.gameObject.GetComponent<Rigidbody>());
        
		base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
		isGrabbed = false;
		RemoveJoint();
		_grabbedObj = null;
		grabbedController = null;
        base.OnSelectExiting(args);
    }
	
	
	// overriding XR rigidbody grabs to stop their functionality
	protected override void SetupRigidbodyGrab(Rigidbody rb)
	{
		
	}
	protected override void SetupRigidbodyDrop(Rigidbody rb)
	{
		
	}
	
}
