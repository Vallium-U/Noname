using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ENEMYSCRIPT : MonoBehaviour
{
    PlayerMANAGERscript player;
    public GameObject Player;
    public Transform weaponPos;
    public Transform headPos;
    NavMeshAgent agent;
    public float AttacDistance = 10f;
    public float FollowDistance = 20f;
    public float followSpeed;
    public Transform shootPos;
    public GameObject bullet;
    public Rigidbody bulletRb;
    public float bulletSpeed;
    public float fireRate;
    int fireController = 0;
    // //patrolling
    // public enum State
    // {
    //     PATROL,
    //     CHASE,
    //     INVESTIGATE
    // }
    // public State state;
    // private bool alive;
    // public GameObject[] waypoints;
    // private int waypointInd;
    // public float patrolSpeed = 0.5f;
    // //Chasing
    // public float chaseSpeed = 1f;
    // public GameObject target;

    // //Investigating
    // private Vector3 investigateSpot;
    // private float timer = 0;
    // public float investigateWait = 10;
    // //Sight
    // public float heighMultiplier;
    // public float sightDist = 10;

    // [Range(0.0f, 1.0f)]
    // public float AttacProbability = 0.5f;

    // [Range(0.0f, 1.0f)]
    // public float HitAccuracy = 0.5f;

    public float Damage = 2f;
    public AudioClip GunSound = null;
    private void Start()
    {
        player = FindObjectOfType<PlayerMANAGERscript>();
        agent = GetComponent<NavMeshAgent>();
        bulletRb = GetComponent<Rigidbody>();
        // agent.updatePosition = true;
        // agent.updateRotation = false;

        // waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        // waypointInd = Random.Range(0, waypoints.Length);
        // state = ENEMYSCRIPT.State.PATROL;
        // alive = true;
        // heighMultiplier = 1.36f;
        // StartCoroutine("FSM");

    }
    // IEnumerator FSM()
    // {
    //     while (alive)
    //     {
    //         switch (state)
    //         {
    //             case State.PATROL:
    //                 Patrol();
    //                 break;
    //             case State.CHASE:
    //                 Chase();
    //                 break;
    //             case State.INVESTIGATE:
    //                 Investigate();
    //                 break;
    //         }
    //         yield return null;
    //     }
    // }

    // void Patrol()
    // {
    //     agent.speed = patrolSpeed;
    //     if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2)
    //     {
    //         agent.SetDestination(waypoints[waypointInd].transform.position);
    //         // ? character.Move(agent.desiredVelocity, false, false);
    //     }
    //     else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
    //     {
    //         waypointInd = Random.Range(0, waypoints.Length);
    //     }
    //     else
    //     {
    //         // ? character.Move(Vector3.zero, false, false);
    //     }
    // }

    // void Chase()
    // {
    //     agent.speed = chaseSpeed;
    //     agent.SetDestination(target.transform.position);
    //     // ? character.Move(agent.desiredVelocity, false, false);
    // }
    // void Investigate()
    // {
    //     timer += Time.deltaTime;
    //     RaycastHit hit;
    //     Debug.DrawRay(transform.position + Vector3.up * heighMultiplier, transform.forward * sightDist, Color.green);
    //     Debug.DrawRay(transform.position + Vector3.up * heighMultiplier, (transform.forward + transform.right).normalized * sightDist, Color.green);
    //     Debug.DrawRay(transform.position + Vector3.up * heighMultiplier, (transform.forward - transform.right).normalized * sightDist, Color.green);
    //     if (Physics.Raycast(transform.position + Vector3.up * heighMultiplier, transform.forward, out hit, sightDist))
    //     {
    //         if (hit.collider.gameObject.tag == "Player")
    //         {
    //             state = ENEMYSCRIPT.State.CHASE;
    //             target = hit.collider.gameObject;
    //         }
    //     }
    //     if (Physics.Raycast(transform.position + Vector3.up * heighMultiplier, (transform.forward + transform.right).normalized, out hit, sightDist))
    //     {
    //         if (hit.collider.gameObject.tag == "Player")
    //         {
    //             state = ENEMYSCRIPT.State.CHASE;
    //             target = hit.collider.gameObject;
    //         }
    //     }
    //     if (Physics.Raycast(transform.position + Vector3.up * heighMultiplier, (transform.forward - transform.right).normalized, out hit, sightDist))
    //     {
    //         if (hit.collider.gameObject.tag == "Player")
    //         {
    //             state = ENEMYSCRIPT.State.CHASE;
    //             target = hit.collider.gameObject;
    //         }
    //     }
    //     agent.SetDestination(this.transform.position);
    //     // ? character.Move(Vector3.zero, false, false);
    //     transform.LookAt(investigateSpot);
    //     if (timer >= investigateWait)
    //     {
    //         state = ENEMYSCRIPT.State.PATROL;
    //         timer = 0;
    //     }

    // }
    // private void OnTriggerEnter(Collider coll)
    // {
    //     if (coll.tag == "Player")
    //     {
    //         state = ENEMYSCRIPT.State.INVESTIGATE;
    //         investigateSpot = coll.gameObject.transform.position;
    //     }
    // }

    private void FixedUpdate()
    {

        agent.speed = followSpeed;
        agent.SetDestination(Player.transform.position);
        if (agent.enabled)
        {
            float dist = Vector3.Distance(transform.position, Player.transform.position);
            bool shoot = false;
            bool follow = (dist < FollowDistance);
            weaponPos.transform.LookAt(Player.transform);
            headPos.transform.LookAt(Player.transform);
            if (follow)
            {
                //float random = Random.Range(0.0f, 1.0f);
                if (dist < AttacDistance)
                {
                    shoot = true;
                }
            }
            if (follow)
            {
                agent.SetDestination(Player.transform.position);
            }
            if (shoot)
            {
                agent.SetDestination(transform.position);
                fireController++;
                if (fireController > fireRate)
                {

                    Shoot();
                }

            }
        }
    }
    public void Shoot()
    {
        shootPos.transform.LookAt(Player.transform);
        // weaponPos.transform.LookAt(Player.transform);
        // agent.transform.LookAt(Player.transform);
        var spawnedBullet = Instantiate(bullet, shootPos.position, shootPos.rotation);
        bulletRb = spawnedBullet.gameObject.AddComponent<Rigidbody>();
        bulletRb.useGravity = false;
        bulletRb.AddForce(shootPos.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(spawnedBullet, 5);
        fireController = 0;

    }
}
