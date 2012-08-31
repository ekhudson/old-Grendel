using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
   
    int counter;
	
	public float TargetRange = 50f;
    public float rateOfFire = 1f;
    public float fireForce = 150f;
    private float nextFire = 0f;

    public GameObject firePosition;
	public GameObject MuzzleFlash;
	public GameObject P_MuzzleFlash;
	public Light MuzzleFlash_Light;
    public GameObject cannonBall;
	public GameObject bullet;
	public float bulletFireForce = 1000f;
	public AudioClip BulletFireSound;
	public AudioClip GrenadeFireSound;
	public GameObject CaseEjector;
	
	//private Vector3 targetPos;
	private Ray _ray;
	private RaycastHit _rayHit;
	
	public Vector3 ReticulePos = Vector3.zero;

    // Use this for initialization
    void Start()
    {
      	
		
    }

    // Update is called once per frame
    void Update()
    {
        nextFire -= Time.deltaTime;

        if (nextFire <= 0f)
        {
            if (cannonBall != null)
            {               
                
            }

            nextFire = rateOfFire;
        }
		
		TraceToReticule();
        ++counter; 
		
		if (MuzzleFlash_Light.intensity > 0)
		{
			MuzzleFlash_Light.intensity -= 1f * Time.deltaTime;
		}
    }

    public void Fire()
    {
        AudioSource.PlayClipAtPoint(GrenadeFireSound, Vector3.zero);		
		P_MuzzleFlash.particleEmitter.Emit();		
		GameObject cb = (GameObject)Instantiate(cannonBall, firePosition.transform.position, firePosition.transform.rotation);
        cb.transform.LookAt(ReticulePos);
		cb.rigidbody.AddForce((ReticulePos - transform.position).normalized * bulletFireForce);
    }
	
	public void BulletFire()
	{		
		AudioSource.PlayClipAtPoint(BulletFireSound, Vector3.zero);		
		MuzzleFlash_Light.intensity = 1f;
		P_MuzzleFlash.particleEmitter.Emit();
		P_MuzzleFlash = (GameObject)Instantiate(CaseEjector, firePosition.transform.position, firePosition.transform.rotation);
		P_MuzzleFlash.transform.parent = transform;		
		GameObject cb = (GameObject)Instantiate(bullet, firePosition.transform.position, firePosition.transform.rotation);
        cb.transform.LookAt(ReticulePos);
		cb.rigidbody.AddForce((ReticulePos - transform.position).normalized * bulletFireForce);
	}	
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(_rayHit.point, 0.1f);		
	}
	
	public void TraceToReticule()
	{
		
		Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0f);
		_ray = Camera.main.ViewportPointToRay(screenCenter);
		_rayHit = new RaycastHit();	
		LayerMask layerMask = 1 << LayerMask.NameToLayer("PlayerLayer") | 1 << LayerMask.NameToLayer("PlayerSearchLayer") | 1 << LayerMask.NameToLayer("ZombieSearchLayer") | 1 << LayerMask.NameToLayer("TriggerLayer");
		
		if(Physics.Raycast(_ray, out _rayHit, TargetRange, ~layerMask) && _rayHit.transform != Player.Instance.transform )
		{
			ReticulePos = _rayHit.point;
			//targetPos = ReticulePos;
			//Debug.DrawRay(transform.position, _ray.direction * TargetRange, Color.red);
			//Debug.DrawLine(transform.position, _rayHit.point, Color.red);
		}
		else
		{
			//Debug.DrawRay(transform.position, _ray.direction * TargetRange, Color.blue);	
			ReticulePos = _ray.direction * TargetRange;
			//targetPos = ReticulePos;			
		}
		
		
		
		//ReticulePos = ray.GetPoint(100);
		
		//ReticulePos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
		
	}
}
