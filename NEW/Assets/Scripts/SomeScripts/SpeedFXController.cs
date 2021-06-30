// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Vm
// {
//     public class SpeedFXController : MonoBehaviour
//     {
//         public ParticleSystem VFX;
//         PLAYERMOVEMENT1 rb;

//         // Start is called before the first frame update
//         void Start()
//         {

//         }

//         // Update is called once per frame
//         void Update()
//         {
//             var vel = rb.rb.velocity;
//             if (vel.magnitude > 30)
//             {
//                 VFX.startSize = vel.magnitude / 50;

//             }
//         }
//     }
// }