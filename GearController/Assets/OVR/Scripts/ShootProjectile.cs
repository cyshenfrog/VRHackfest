using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour {

    public GameObject projectilePrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            shootProjectile();
        }
    }

    public void shootProjectile()
    {
        GameObject projectile = GameObject.Instantiate(projectilePrefab);
        projectile.transform.position = this.transform.position;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 look = this.transform.forward;
        Vector3 force = look * 10;
        rb.velocity = look;
        //        rb.AddForce(force);

    }
}
