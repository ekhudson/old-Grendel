using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

    public GameObject explosionObject;
	private Collider[] _foundColliders;
	private Transform _myTransform;
	private GameObject _myGameObject;

    // Use this for initialization
	void Start () {
		
		_myTransform = gameObject.transform;
		_myGameObject = gameObject;
	
	}

    void OnCollisionEnter(Collision collision)
    {
        _foundColliders = Physics.OverlapSphere(_myTransform.position,4f);
		
		foreach(Collider collider in _foundColliders)
		{				
			if (collider.gameObject.tag == "Zombie")
			{				
				try
				{
					EntityManager.EntityDictionary[collider.gameObject.GetInstanceID()].TakeDamage(100);
				}
				catch
				{
					
				}
			}
		}
			
        createExplode();

    }

    public void createExplode()
    {        
		GameObject ex = (GameObject)Instantiate(explosionObject, transform.position, Quaternion.identity);		
		Destroy(_myGameObject);
    }
}
