using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vm
{
    public class ShootProjectiles : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform bulletSpawnPos;
        public float bulletSpeed;
        public float bulletLifeTime;
        public float currentCooldown;
        public float damage;
        public float fireRate;
        public GameObject bulletSound;
        //public float bloom;
        public Camera mainCam;
        public Camera weaponCam;
        private float defaultZoom;
        private float defaultZoomw;
        public int aimingZoom;
        public float smoothZoom;
        //private bool isZoomed = false;
        public float kikback;
        public float recoil;
        public float range;
        public float aimingSpeed;
        public int maxAmmo;
        private int currentAmmo;
        public float reloadTime;
        private bool isReloading;
        [Header("Recoil Settings: ")]
        public float rotationSpeed;//6
        public float returnSpeed; // 25

        [Header("Hipfire: ")]
        public Vector3 RecoilRotation = new Vector3();//2 2 2 

        [Header("Aiming: ")]
        public Vector3 RecoilRotationAiming = new Vector3();//0.5 0.5 1.5

        [Header("State")]
        public bool aiming;

        private Vector3 currentRotation;
        private Vector3 Rot;
        //private Vector3 Rotw;

        public Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            //if (currentAmmo == -1)
            currentAmmo = maxAmmo;
            defaultZoom = mainCam.fieldOfView;
            defaultZoomw = weaponCam.fieldOfView;
        }

        private void Awake()
        {
            currentAmmo = maxAmmo;
            defaultZoom = mainCam.fieldOfView;
            defaultZoomw = weaponCam.fieldOfView;
            
            currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
            Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
            mainCam.transform.localRotation = Quaternion.Euler(Rot);
            weaponCam.transform.localRotation = Quaternion.Euler(Rot);
        }

        private void FixedUpdate()
        {
            currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
            Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
            mainCam.transform.localRotation = Quaternion.Euler(Rot);
            weaponCam.transform.localRotation = Quaternion.Euler(Rot);
        }
        void OnEnable()
        {
            isReloading = false;
            animator.SetBool("Reloading", false);
        }
        // Update is called once per frame
        void Update()
        {
            GameObject holdSound;

            if (isReloading)
            {
                Aim(Input.GetMouseButton(1), Input.GetMouseButton(1));
                return;
            }

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            Aim(Input.GetMouseButton(1), Input.GetMouseButton(1));
            if (Input.GetKey(KeyCode.Mouse0) && currentCooldown <= 0)
            {
                Fire();
                if (bulletSound)
                {
                    holdSound = Instantiate(bulletSound, bulletSpawnPos.transform.position, bulletSpawnPos.transform.rotation);
                }
            }

            //weapon position elasticity
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 4);
            //cooldown
            if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        }
        IEnumerator Reload()
        {

            isReloading = true;
            Debug.Log("Reloading...");
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(reloadTime - 0.25f);
            animator.SetBool("Reloading", false);
            yield return new WaitForSeconds(0.25f);
            currentAmmo = maxAmmo;
            isReloading = false;
        }
        public void Fire()
        {
            // Transform t_spawn = bulletSpawnPos.transform;
            // //bloom
            // Vector3 t_bloom = transform.position + t_spawn.forward * 1000f;
            // t_bloom += Random.Range(-bloom, bloom) * t_spawn.up;
            // t_bloom += Random.Range(-bloom, bloom) * t_spawn.right;
            // t_bloom -= t_spawn.position;
            // //t_bloom.Normalize();
            currentAmmo--;

            GameObject bullet = Instantiate(bulletPrefab);

            bullet.transform.position = bulletSpawnPos.position;

            Vector3 rotation = bullet.transform.rotation.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPos.forward * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));

            //gun fx
            transform.Rotate(-recoil, 0, 0);
            transform.position -= transform.forward * kikback;
            //cooldown
            currentCooldown = fireRate;
            if (aiming)
            {
                currentRotation += new Vector3(-RecoilRotationAiming.x, Random.Range(-RecoilRotationAiming.y, RecoilRotationAiming.y), Random.Range(-RecoilRotationAiming.z, RecoilRotationAiming.z));
            }
            else
            {
                currentRotation += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
            }
        }
        private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(bullet);
        }

        void Aim(bool p_isAiming, bool isZoomed)
        {
            Transform t_anchor = transform.GetChild(0);
            Transform t_state_ads = transform.GetChild(1).GetChild(1);
            Transform t_state_hip = transform.GetChild(1).GetChild(0);
            if (p_isAiming)
            {
                //aim
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * aimingSpeed);
                aiming = true;

            }
            else
            {
                //hip
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * aimingSpeed);
                aiming = false;
            }
            if (isZoomed)
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, aimingZoom, smoothZoom * Time.deltaTime);
                weaponCam.fieldOfView = Mathf.Lerp(weaponCam.fieldOfView, aimingZoom, smoothZoom * Time.deltaTime);
            }
            else
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, defaultZoom, smoothZoom * Time.deltaTime);
                weaponCam.fieldOfView = Mathf.Lerp(weaponCam.fieldOfView, defaultZoom, smoothZoom * Time.deltaTime);
            }
        }
    }
}