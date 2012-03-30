using UnityEngine;
using System.Collections;

public class Item : BaseObject {
	
	//Define item types
	public enum ItemTypes
	{
		COIN, 
		EXPERIENCE,
		AMMO_PISTOL,
		AMMO_RPG
	};
	
	public ItemTypes ItemType;
	public AudioClip[] PickupSounds;
	
	protected float _timeToSleep = 4f; //time this item takes to remove its rigid body
		
	//void Awake()
	//{
		
	//}
	
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		StartCoroutine( TimeOut() );	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
		{			
			PickUp();
		}		
	}
	
	void PickUp()
	{
		
		AudioSource.PlayClipAtPoint(PickupSounds[Random.Range(0, PickupSounds.Length - 1)], _transform.position, 0.1f);
		Destroy(gameObject);
		
	}
	
	IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(_timeToSleep);
		Destroy(_rigidbody);		
	}
}
