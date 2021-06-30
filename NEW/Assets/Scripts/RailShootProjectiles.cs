using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailShootProjectiles : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPos;
    public float bulletSpeed;
    public float bulletLifeTime;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }
    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.position = bulletSpawnPos.position;

        Vector3 rotation = bullet.transform.rotation.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPos.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
