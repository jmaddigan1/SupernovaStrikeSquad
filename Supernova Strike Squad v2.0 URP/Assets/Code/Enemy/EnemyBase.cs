using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Supernova.Networking;

public class EnemyBase : NetworkBehaviour
{
    // Public Members
    // What type of enemy am I?
    public EnemyType EnemyType;

    // Delegates
    public OnEnemyDeath OnDeath;


    public List<Collider> myColliders = new List<Collider>();

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, PatrolDetectionRange);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, EscapeRange);
	}

    private void OnDestroy()
    {
        if (!CustomNetworkManager.ApplicationIsQuitting) {
            OnDeath?.Invoke();
        }
    }

    public void Shoot(Transform target)
	{

	}

    //[SerializeField]public GameObject shot = null;
    //public void Shoot(Transform target)
    //{
    //       GameObject go = Instantiate(shot, transform.position + transform.forward * 3, transform.rotation);

    //       //      if (go.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
    //       //      {
    //       //          Vector3 point = target.position + rigidbody.velocity.normalized * 2;
    //       //          go.transform.LookAt(point);
    //       //      }
    //       //else
    //       //{
    //       //          go.transform.LookAt(target);
    //       //      }

    //       go.transform.LookAt(target);

    //       // FIX COLLISION WITH OWNER
    //       foreach (Collider collider in myColliders)
    //	{
    //           Collider projectileColliders = go.GetComponentInChildren<Collider>();
    //           Collider playerCollider = collider;

    //           Physics.IgnoreCollision(projectileColliders, playerCollider);
    //       }

    //       NetworkServer.Spawn(go);
    //   }
}
