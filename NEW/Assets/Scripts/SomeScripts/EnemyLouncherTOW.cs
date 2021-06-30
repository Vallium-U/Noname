using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLouncherTOW : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject missle;
    public Transform target;
    public Transform aim;
    public Transform head;
    public float reloadTime = 1f;
    public float rotationSpeed = 5f;
    public float firePauseTime = 0.25f;
    public float range = 10f;
    public bool canSee = false;
    public GameObject lounchSound;
    public GameObject lounchFX;

    private float nextFireTime, nextMoveTime;
    //int index = 0;


    void Start()
    {
        //Invoke("SpawnMissle", 5f);


    }
    void Update()
    {
        AimTOW();
        Tracking();
    }
    // void SpawnMissle()
    // {
    //     GameObject newMissle = Instantiate(missle, spawnPoint.position, spawnPoint.rotation) as GameObject;

    //     Invoke("SpawnMissle", 2f);

    // }

    void AimTOW()
    {
        if (target != null)
        {
            if (Time.time >= nextMoveTime)
            {
                aim.LookAt(target);
                aim.eulerAngles = new Vector3(aim.eulerAngles.x, aim.eulerAngles.y, aim.eulerAngles.z);
                head.rotation = Quaternion.Lerp(head.rotation, aim.rotation, Time.deltaTime * rotationSpeed);
            }
            if (Time.time >= nextFireTime && canSee == true)
            {
                Fire();

            }
        }
    }
    void Fire()
    {
        //lounch fixed
        Instantiate(lounchFX, transform.position, Quaternion.identity);
        
        nextFireTime = Time.time + reloadTime;
        nextMoveTime = Time.time + firePauseTime;
        //audiofx
        GameObject newMissle = Instantiate(missle, spawnPoint.position, spawnPoint.rotation) as GameObject;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!target)
        {
            if (other.CompareTag("Player"))
            {
                nextFireTime = Time.time + (reloadTime * 0.5f);
                target = other.gameObject.transform;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == target)
        {
            target = null;
        }
    }
    void Tracking()
    {
        Vector3 fwd = spawnPoint.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Debug.DrawRay(spawnPoint.position, fwd * range, Color.green);

        if (Physics.Raycast(spawnPoint.position, fwd, out hit, range))
        {
            if (hit.collider.CompareTag("Player"))
            {
                canSee = true;
            }
        }
        else
        {
            canSee = false;
        }
    }


}
