// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Vm
// {
//     public class camerashakerspeed : MonoBehaviour
//     {
//         public Rigidbody rb;
//         //public bool grounded;
//         //camerashake
//         PLAYERMOVEMENT1 p;
//         [Header("CameraShake: ")]
//         public Transform CAMERASFORSHAKE;
//         public Transform CAMERASFORSHAKEx2;
//         private Vector3 currentRotation;
//         private Vector3 Rot;
//         private Vector3 currentRotationx2;
//         private Vector3 Rotx2;
//         [Header("SizeShake: ")]
//         public Vector3 ShakeRotation = new Vector3();//2 2 2 
//         public float rotationSpeed;//6
//         public float returnSpeed; // 25
//         private Vector3 ShakeRotationx2 = new Vector3();//2 2 2 
//         private float rotationSpeedx2;//6
//         private float returnSpeedx2;

//         void Start()
//         {
//             rotationSpeedx2 = 8;
//             ShakeRotationx2 = new Vector3(0, 4, 4);//2 2 2 
//             returnSpeedx2 = 26;
//         }
//         private void FixedUpdate()
//         {
//             if (p.grounded.Equals(true))
//             {
//                 currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
//                 Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
//                 CAMERASFORSHAKE.transform.localRotation = Quaternion.Euler(Rot);
//             }
//             else
//             {
//                 currentRotationx2 = Vector3.Lerp(currentRotationx2, Vector3.zero, returnSpeedx2 * Time.deltaTime);
//                 Rotx2 = Vector3.Slerp(Rotx2, currentRotationx2, rotationSpeedx2 * Time.fixedDeltaTime);
//                 CAMERASFORSHAKEx2.transform.localRotation = Quaternion.Euler(Rotx2);
//             }
//         }
//         // Update is called once per frame
//         void Update()
//         {

//             var vel = rb.velocity;
//             if (vel.magnitude > 40)
//             {
//                 //rotationSpeed = vel.magnitude / 25;
//                 CameraHighSpeedShake();
//             }

//             if (vel.magnitude > 125 && p.grounded.Equals(false))
//             {
//                 rotationSpeedx2 = vel.magnitude / 35;
//                 CameraHighSpeedShakex2();
//             }
//             else
//             {
//                 rotationSpeedx2 = 8;
//             }
//             if (vel.magnitude > 40 && vel.magnitude < 125)
//             {
//                 //rotationSpeed = vel.magnitude / 25;
//                 CameraHighSpeedShake();
//             }
//         }
//         void CameraHighSpeedShake()
//         {
//             currentRotation += new Vector3(-ShakeRotation.x, Random.Range(-ShakeRotation.y, ShakeRotation.y), Random.Range(-ShakeRotation.z, ShakeRotation.z));

//         }
//         void CameraHighSpeedShakex2()
//         {
//             currentRotationx2 += new Vector3(-ShakeRotationx2.x, Random.Range(-ShakeRotationx2.y, ShakeRotationx2.y), Random.Range(-ShakeRotationx2.z, ShakeRotationx2.z));

//         }
//     }
// }