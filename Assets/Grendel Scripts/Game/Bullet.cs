using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public GameObject explosionObject;
	public float Lifetime = 4f;
	private Collider[] FoundColliders;
	private Transform _myTransform;
	private GameObject _myGameObject;
	public AudioClip ImpactSound;
	
	
	void Awake()
	{
		_myTransform = gameObject.transform;
		_myGameObject = gameObject;		
	}
	
	// Use this for initialization
	void Start () {		
		
		StartCoroutine( CountLiftime() ); //begin our lifetime countdown	
	}
	
	//waits for the duration of the projectile's lifetime, then destroys it
	IEnumerator CountLiftime()
	{
		yield return new WaitForSeconds(Lifetime);
		
		DestroyProjectile();		
	}
	
	void OnCollisionEnter(Collision collision)
    {
        
		if (collision.gameObject.tag == "Scrap")
		{
			
		}
		else
		{
			if (collision.gameObject.tag == "Zombie")
			{
				try
				{					
					EntityManager.EntityDictionary[collision.gameObject.GetInstanceID()].TakeDamage(25);
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
    GameObject ex = (GameObject)Instantiate(explosionObject, _myTransform.position, Quaternion.identity);
	
	AudioSource.PlayClipAtPoint(ImpactSound,transform.position);
	
	Destroy(_myGameObject);
	}
	
	public void DestroyProjectile()
	{
		Destroy(_myGameObject);
	}
}

