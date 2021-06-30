using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vm
{
    public class Shooting : MonoBehaviour
    {
        //Gun scriptableG;
        public float damage;
        //public float fireRate;
        public float range;
        public Camera fpsCam;

        private void Start()
        {

        }
        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        void Shoot()
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }
        }
    }
}