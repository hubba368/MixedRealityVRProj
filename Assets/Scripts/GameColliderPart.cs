using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameColliderPart : MonoBehaviour
{
	
	[SerializeField]
	private MeshRenderer mesh;
	[SerializeField]
	private float velocityTarget = 2f;
	
	[SerializeField]
	private CapsuleCollider mainColl;
	[SerializeField]
	private CapsuleCollider WrongColl;
	
	
	public int roundNum = 0;
	public GameHandler Parent;
	public bool wasHit = false;
	
	public UnityEvent<int> OnGameHit;
	
    // Start is called before the first frame update
    void Start()
    {
		Parent = transform.parent.transform.parent.gameObject.GetComponent<GameHandler>();
		roundNum = int.Parse(transform.parent.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void InitPart(GameHandler _parent)
	{
		Parent = _parent;
	}
	
	public void SetRoundIndex(int num)
	{
		roundNum = num;
	}
	
	public void ResetMesh()
	{
		
	}

	public void ShowColor(Color newCol)
	{
		wasHit = false;
		mesh.material.color = newCol;
	}
	
	void OnCollisionEnter(Collision other)
	{
		 // TODO check for correct layer?
		 // on heavy hit disable collider, hide mesh and create effect and sound?
		 if(other.gameObject.tag == "Weapon" && !wasHit)
		 {
			 wasHit = true;
			if(other.relativeVelocity.magnitude > velocityTarget)
			 {
				OnGameHit.Invoke(roundNum);
				Debug.Log("hit hard");
			}
			else
			{
				Debug.Log("hit slow");
			} 
		 }		 
	}
	
	void OnCollisionExit(Collision other)
	{
		if(other.gameObject.tag == "Weapon" && wasHit)
		{
			wasHit = false;
		}
	}
}
