using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vm
{
    public class EnemyMissle : MonoBehaviour
    {

        public Transform target;
        Vector3 direction;

        public float speed = 20f;
        public float rotationSpeed = 5f;

        ShootProjectiles dmg;
        public float damage;
        public float radius;
        public float explForce;
        public GameObject destroySound;
        public float bulletLifeTime;
        public GameObject destroyFX;
        // public AudioSource rocketSound;


        void Start()
        {

            target = GameObject.Find("Player").transform;
            //rocketSound.Play();
        }

        void Update()
        {
            //Movement missle
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            //Rotation
            if (target != null)
            {
                direction = target.position - transform.position;

                direction = direction.normalized;
                var rot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            }
        }
        private void OnCollisionEnter(Collision other)
        {
            GameObject holdSound;
            if (destroySound)
            {
                holdSound = Instantiate(destroySound, other.transform.position, other.transform.rotation);
            }
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explForce, transform.position, radius);
                }
            }
            Instantiate(destroyFX, transform.position, Quaternion.identity);

            //rocketSound.Stop();

            Destroy(gameObject);
        }


    }
}