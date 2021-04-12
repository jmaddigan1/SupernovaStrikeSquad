using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Vector3 pos;

    // Start is called before the first frame update
    IEnumerator Start()
    {
		while (true)
		{
            float x = Random.Range(-1f, 1);
            float y = Random.Range(-1f, 1);
            float z = Random.Range(-1f, 1);

            float distance = Random.Range(0, 200);

            pos = new Vector3(x, y, z) * distance;


            yield return new WaitForSeconds(2.5f);
        }
    }

	private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * 0.5f);
    }
}
