using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public static Compass Instance;

    [SerializeField]
    private Camera compassCamera = null;

    [SerializeField]
    private Transform targetPrefab = null;

    private List<Transform> targets = new List<Transform>();
    private List<Transform> targetImages = new List<Transform>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
    }

    void Start()
    {
        foreach (PlayerShipController ship in FindObjectsOfType<PlayerShipController>())
        {
            AddTarget(ship.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: check is the target is behind us

		for (int index = 0; index < targets.Count; index++)
		{
            if (targets[index] == null) { continue; }
            else
            {
                targetImages[index].position = compassCamera.WorldToScreenPoint(targets[index].position);

                Vector2 t = new Vector2(targetImages[index].localPosition.x, targetImages[index].localPosition.y);

                if (t.magnitude > 400)
                {
                    Vector2 direction = t.normalized;
                    targetImages[index].localPosition = direction * 400;
                }
            }
        }  
    }

	public void AddTarget(Transform newTarget)
    {
        targetImages.Add(Instantiate(targetPrefab, transform));

        targets.Add(newTarget);
    }

    public void Clear(Transform toRemove)
    {
        // TODO
    }
}
