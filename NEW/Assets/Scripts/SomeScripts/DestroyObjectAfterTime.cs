using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterTime : MonoBehaviour
{
    public float delay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DestroyBulletAfterTime());
    }
    private IEnumerator DestroyBulletAfterTime()
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
