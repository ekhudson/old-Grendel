using UnityEngine;
using System.Collections;

public class Player : Entity
{

    public CharacterController ctrl;
    public Cannon pCannon;  
	public GameObject DamageParticles;
	public SearchRadius MySearchRadius;

    public float playerMoveSpeed = 1f;
    public float pJumpSpeed = 10f;
	public float pCooldown = 1f;
	public float BulletCooldown = 0.01f;
	private float _currentCooldown = 1f;
	private float _currentBulletCooldown = 0.05f;
    //float pCurrentJumpSpeed = 0f;
    //public float pMaxJumpHeight = 64f;
    Vector3 pFallSpeed = Vector3.zero;	
	private Vector3 _tempRotation;
   	private Vector3 _move;
	private bool _isPounding = false;
	private Zombie _tempZombie;
	
	
	private static Player instance;	
	
	public static Player Instance
	{
		get { return instance; }
	}
	
	// Use this for initialization
    void Awake()
    {		
		base.Awake();
		instance = this;
		GameManager.PlayerRef = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (ctrl)
        {            
             _tempRotation = new Vector3(0f, Input.GetAxis("Horizontal") * GameManager.UserInputRef.MouseSensitivityHorizontal, 0f); //player rotation
             _move = Vector3.zero;


            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _move += _transform.forward * playerMoveSpeed;

            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _move += -_transform.forward * playerMoveSpeed;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _move += _transform.right * playerMoveSpeed;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _move += -_transform.right * playerMoveSpeed;
            }

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                _move *= 1.45f;
            }

            if (Input.GetKeyDown(KeyCode.Space) && ctrl.isGrounded)
            {
                pFallSpeed.y = pJumpSpeed;
                //pCurrentJumpSpeed = pJumpSpeed;
                //pFallSpeed = Vector3.zero;
            }
            else if (ctrl.isGrounded == false)
            {
                if (Input.GetKeyDown(KeyCode.Space) && _isPounding == false)
				{
					pFallSpeed += Vector3.Scale(Physics.gravity, new Vector3(4,4,4));
					_isPounding = true;
					
				}
				
				pFallSpeed += Physics.gravity * Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E))
            {
               if (_currentCooldown >= pCooldown)
				{
					_currentCooldown = 0;
				pCannon.Fire();
				}
            }
			
			if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Q))
			{
				if(_currentBulletCooldown >= BulletCooldown)
				{
					_currentBulletCooldown = 0;
				pCannon.BulletFire();
				}
			}
			
			
			if (_isPounding && ctrl.isGrounded)
			{
				
				_isPounding = false;				
				
				foreach(Collider collider in MySearchRadius.ObjectList)
				{
					if (collider != null && collider.tag != tag)
					{
						//find this collider in the entity dictionary, and tell them they are being damaged
						try
						{
						EntityManager.EntityDictionary[collider.gameObject.GetInstanceID()].TakeDamage(100);
						}
						catch
						{
							Debug.LogWarning("Entity does not exist in the dictionary.");
						}
						
					}
				}
				
				
			}

           	_transform.Rotate(_tempRotation); //apply rotation

            _currentCooldown += Time.deltaTime; //increment cooldown
			_currentBulletCooldown += Time.deltaTime; // increment bullet cooldown

            _move += pFallSpeed;

            ctrl.Move( _move  * Time.deltaTime);           
        }

    }//end update
	
	public override void CalledUpdate()
	{
		//do nothing
	}
	
	public override int TakeDamage(int amount)
	{		
		GameObject cb = (GameObject)Instantiate(DamageParticles, _transform.position, _transform.rotation);		
		
		return base.TakeDamage(amount);
	}	
	
}//end class
