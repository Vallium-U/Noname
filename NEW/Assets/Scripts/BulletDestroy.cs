using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

namespace Vm
{
    public class BulletDestroy : MonoBehaviour
    {
        ShootProjectiles dmg;
        public float damage;
        public float radius;
        public float explForce;
        public GameObject destroySound;
        public float bulletLifeTime;
        public GameObject destroyFXx;
        public ParticleSystem destroyFX;
        private void Update()
        {
            StartCoroutine(DestroyBulletAfterTime(this.gameObject, bulletLifeTime));
        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.tag == "Player" && transform.tag != "EnemyBullet")
            {

            }
            else
            {
                GameObject holdSound;
                Target target = other.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
                if (destroySound)
                {
                    holdSound = Instantiate(destroySound, gameObject.transform.position, gameObject.transform.rotation);
                }
                Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
                foreach (Collider nearbyObject in colliders)
                {
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explForce, transform.position, radius);
                    }
                }

                if (destroyFXx != null)
                {
                    Instantiate(destroyFXx, transform.position, Quaternion.identity);
                    CameraShaker.Instance.ShakeOnce(2.5f, 1f, 0.1f, 1f);
                    Destroy(this.gameObject);
                }
                else
                {
                    Instantiate(destroyFX, transform.position, Quaternion.identity);
                }
                Destroy(this.gameObject);
            }
        }
        private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(bullet);
        }
    }
}
