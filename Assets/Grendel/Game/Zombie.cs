using UnityEngine;
using System.Collections;

public class Zombie : Entity {
	
	//public float Health = 10;
	public float WanderSpeed = 1f;
	public float HostileSpeed = 5f;
	public float SearchRadius = 25;
	public float SearchTime = 0.25f;
	public float StepTime = 1;
	public float StepCooldown = 1;
	public float StepCoolMin = 1;
	public float StepCoolMax = 2;
	
	public float HostileStepCoolMin = 0.15f;
	public float HostileStepCoolMax = 0.35f;
	
	public float AttackDistance = 0.5f;
	public float AttackTime = 2;
	public int Damage = 25;
	public float WanderTime;
	public float SpawnAmt = 3;
	public float HeightOffset = 0.5f;
	public GameObject BloodParticles;
	public GameObject ZombiePrefab;
	public ParticleEmitter ZombieBloodParticles;
	public bool Spawned = false;
	public float ItemDropForce = 4f;
	public Material NeutralMaterial;
	public Material HostileMaterial;
	
	public enum STATES {NEUTRAL, HOSTILE};
	public STATES State = STATES.NEUTRAL;
	
	[System.Serializable]
	public class ItemDrop
	{
		public GameObject ItemToDrop;
		public int DropCountMin;
		public int DropCountMax;		
	}
	
	public ItemDrop[] ItemDrops;
	
	private float _currentSpeed = 1f;
	//private float _currentStepTime = 0;
	//private float _currentStepCooldown = 0;
	//private float _currentWanderTime = 0;
	private float _currentAttackTime = 0;
	private float _currentSearchTime = 0;
	//private float _myTimeDelta = 0;
	private float _myTimeSinceStartup = 0;
	
	private GameObject _playerObject;
	private Transform _playerTransform;
	private Player _playerScript;
	
	//private GameObject _gameObject;
	//private Transform _transform;
	private CharacterController _controller;
	
	private Vector3 _wanderDirection;
	private Vector3 _moveDirection;
	private Vector3 _calculatedMoveAmt;
	private float _distanceToPlayer;
	private Vector3 _directionToPlayer;
	
	private bool _canStep = false;
	
	
	
	// Use this for initialization
	protected override void Start () {
		
		_playerObject = EntityManager.PlayerReference.gameObject;
		_playerTransform = _playerObject.transform;
		_playerScript = EntityManager.PlayerReference;
		
		_currentSpeed = WanderSpeed;
		
		_controller = GetComponent<CharacterController>();
		
		//_gameObject = gameObject;
		//_transform = gameObject.transform;
		
		StepCooldown = Random.Range(StepCoolMin, StepCoolMax);
		
		NewWanderTime();
		SearchTime = Random.Range(0, SearchTime);
		
		if (Spawned)
		{
			SetAsSpawn();
		}
		
		StartCoroutine( StepCooldownMethod() );
		
		base.Start();	
		
	}
	
	//Cooldown between steps
	IEnumerator StepCooldownMethod()
	{		    
		//Debug.Log("Cooling");
		yield return new WaitForSeconds( State == STATES.NEUTRAL ? Random.Range(StepCoolMin, StepCoolMax) : Random.Range(HostileStepCoolMin, HostileStepCoolMax) );
		StartCoroutine( StepTimeMethod() );	//begin stepping	
	}
	
	//Start stepping
	IEnumerator StepTimeMethod()
	{
		//Debug.Log("Steping");
		_canStep = true;
		StartCoroutine( ZombieMovement() );
		yield return new WaitForSeconds(StepTime); //hold this function
	    _canStep = false;
		StartCoroutine( StepCooldownMethod() ); //begin step cooldown
		
	}
	
	IEnumerator ZombieMovement()
	{
		while(_canStep)
		{
			_moveDirection.y = 0f; //force 0
		
			//Apply the move
			//Note: we multiply by the bool, _canStep, which cancels out the move if
			//it is false
			_controller.Move((_moveDirection * _currentSpeed) * Time.deltaTime) ;
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
	
	// Update is called once per frame
	//void Update () 
	//{
		
	//	if (_canStep) { ZombieMovement(); }
		
//		_currentStepCooldown += Time.deltaTime;
//		
//		
//		if (_currentStepCooldown > StepCooldown && _distanceToPlayer > AttackDistance * AttackDistance)
//		{
//			if (_currentStepTime > StepTime)
//			{
//				_currentStepTime = 0;
//				_currentStepCooldown = 0;
//				StepCooldown = Random.Range(StepCoolMin, StepCoolMax);
//			}
//			else
//			{
//				_currentStepTime += Time.deltaTime;
//				_moveDirection.y = 0.5f; //force 0
//				_controller.Move(Vector3.Scale(_moveDirection, new Vector3(Speed, 0, Speed)) * Time.deltaTime);
//			}
//		}
//		
//		_currentWanderTime += Time.deltaTime;
//		
//		if (_currentWanderTime > WanderTime)
//		{
//			_currentWanderTime= 0;
//			NewWanderTime();
//		}	
	
	//}
	
	public override void CalledUpdate()
	{		
		//if (!_gameObject.active) { return; }			
		Search();		
	}
	
	void OnDrawGizmos()
	{
		
		//if (transform.position.y != 0.5f) { transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z) ; }
	}
	
	void Search()
	{
		//Debug.Log("Searching");
		//check if player is close or not
		if (_currentSearchTime >= SearchTime)
		{		
			_distanceToPlayer = (_playerTransform.position - _transform.position).sqrMagnitude;
			_currentSearchTime = 0;
			if ( (_distanceToPlayer < SearchRadius * SearchRadius)  )
			{
				_directionToPlayer = (_playerTransform.position - _transform.position).normalized;
				_currentAttackTime += Time.deltaTime;
				
				if (_distanceToPlayer > AttackDistance * AttackDistance)
				{
					_moveDirection = _directionToPlayer;
					_currentSpeed = HostileSpeed;
					
					_renderer.material = HostileMaterial;
					State = STATES.HOSTILE;
					
				}
				else
				{
					//attack player
					if (_currentAttackTime > AttackTime)
					{
						_currentAttackTime = 0;
						_playerScript.TakeDamage(Damage);
					}
					else
					{
						//do nothing, still cooling
					}
				}			
				
			}
			else
			{
			
				_renderer.material = NeutralMaterial;
				State = STATES.HOSTILE;
				_moveDirection = _wanderDirection;
				_currentSpeed = WanderSpeed;
				
			}
		}
		else
		{
			_currentSearchTime += (Time.realtimeSinceStartup - _myTimeSinceStartup);
			_myTimeSinceStartup = Time.realtimeSinceStartup;
		}
		
		
	}
	
	public void DestroyZombie()
	{
		Destroy(_gameObject);
	}
	
	void NewWanderTime()
	{		
		_wanderDirection = new Vector3( Random.Range(-1,1), 0, Random.Range(-1,1));
		WanderTime = Random.Range(1,3);
	}
	
		
	public override int ModifyHealth(int amt)
	{		
		DropItems(); //TODO: Make entities drop a percentage of their items based on the percent of the damage / total health;						
		
		return base.ModifyHealth(amt);
	}
	
	public override void OnDeath()
	{
		if (!Spawned) //check if this is a spawned zombie. If so, don't spawn more
		{
			Spawn();
		}
	}
	
	void DropItems()
	{
		
		foreach(ItemDrop drop in ItemDrops)
		{			
			for (int i = 0; i < Random.Range(drop.DropCountMin, drop.DropCountMax); i++)
			{
				
				GameObject item = (GameObject)Instantiate(drop.ItemToDrop, _transform.position + new Vector3( Random.Range(-1, 1), Random.Range(1, 2), Random.Range(-1, 1) ), Quaternion.identity);	
				
				item.rigidbody.AddForce( Random.Range(-ItemDropForce, ItemDropForce), Random.Range(ItemDropForce * 0.5f, ItemDropForce * 2f), Random.Range(-ItemDropForce, ItemDropForce), ForceMode.Force );
			}
		}
	}
	
	void Spawn()
	{
		for (int i = 0; i < SpawnAmt; i++)
		{
			Vector3 position = _transform.position;
			position.x += Random.Range(-2,2);
			position.y = 0.25f;
			position.z += Random.Range(-2,2);
			
			Instantiate(ZombiePrefab, position, Quaternion.identity);
			
		}
		
	}
	
	public void SetAsSpawn()
	{
		//Speed *= 3;
		StepCoolMax *= 0.5f;
		StepCoolMin *= 0.5f;
		Damage /= 2;
		HeightOffset = 0.25f;
		
	}
	
	
}
