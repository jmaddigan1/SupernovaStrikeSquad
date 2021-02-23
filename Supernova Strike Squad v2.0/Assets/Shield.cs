using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private Transform shield = null;

    // Start is called before the first frame update
    void Start()
    {
        shield.transform.localScale = Vector3.one * 2;
    }

    void FixedUpdate()
	{
        if (shield.TryGetComponent<Renderer>(out Renderer renderer)) {
            renderer.material.color = new Color(0, 0, 0, 0.2f);
        }
    }
}
