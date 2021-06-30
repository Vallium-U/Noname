using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vm
{
    public class GrapplingGunScripts : MonoBehaviour
    {
        public Camera cam;
        public GameObject Player;
        public GameObject GrapplingGun;
        public Rigidbody rb;
        public float intensity;
        public float smooth;
        private Vector3 origin_position;
        private Vector3 grapplePoint;
        private SpringJoint joint;
        public LineRenderer lr;
        public float grappleDistanceMax = 8f;
        public float grappleDistanceMin = 1f;
        public float jointsprinf = 2.5f;
        public float jointdamperf = 2f;
        public float jointmassscalef = 3f;
        float speed;
        PLAYERMOVEMENT1 PMVM;

        private void Start()
        {
            origin_position = transform.localPosition;
            PMVM = GetComponent<PLAYERMOVEMENT1>();
        }
        private void Update()
        {
            UpdateSway();
            TakeOn();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartGrapple();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopGrapple();
            }
            DrawGripple();
        }
        private void TakeOn()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GrapplingGun.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                GrapplingGun.SetActive(false);
            }
        }
        private void StartGrapple()
        {
            RaycastHit[] raycastHit = Physics.RaycastAll(cam.transform.position, cam.transform.forward, 100f);
            if (raycastHit.Length < 1) return;
            grapplePoint = raycastHit[0].point;
            joint = Player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            joint.maxDistance = grappleDistanceMax; //8
            joint.minDistance = grappleDistanceMin; //1
            joint.spring = jointsprinf; //2
            joint.damper = jointdamperf; //2.5
            joint.massScale = jointmassscalef; //2

            //joint.spring = 3.5f; // 3.5
            //joint.damper = 0.25f; //0.25
            // //joint.massScale = 15.5f;
            //PMVM.grounded = false;
        }

        private void StopGrapple()
        {
            Destroy(joint);
            //lr.enabled = false;
            //PMVM.Gun.transform.position = PMVM.WeaponPos.position;
        }


        private void DrawGripple()
        {
            if (grapplePoint == Vector3.zero || joint == null)
            {
                lr.positionCount = 0;
                return;
            }
            lr.positionCount = 2;
            lr.startWidth = 0.1f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.numCapVertices = 10;
            lr.numCornerVertices = 1;
            lr.SetPosition(0, transform.GetChild(0).GetChild(0).GetChild(2).position);
            lr.SetPosition(1, grapplePoint);
            // Vector3 grapplePoint1 = grapplePoint;
            // Vector3 dir = (grapplePoint1 - PMVM.Gun.transform.GetChild(0).GetChild(0).GetChild(2).transform.position).normalized;
            // Quaternion lookdir = Quaternion.LookRotation(dir);

            // if (grapplePoint == Vector3.zero)
            // {
            //     lookdir = PMVM.Gun.transform.GetChild(0).GetChild(0).GetChild(2).transform.rotation;
            // }
            // PMVM.Gun.transform.GetChild(0).GetChild(0).transform.rotation = Quaternion.Slerp(PMVM.Gun.transform.GetChild(0).GetChild(0).GetChild(2).transform.rotation, lookdir, Time.deltaTime * speed);
        }
        private void GunMove()
        {

            // PMVM.Gun.transform.position = PMVM.WeaponPos.position;
            // PMVM.Gun.transform.rotation = PMVM.WeaponPos.rotation;
            //Gun.transform.position = defaultPos;
            //Gun.transform.rotation = Quaternion.EulerRotation(0.0f, 0.0f, 0.0f);
        }
        private void UpdateSway()
        {
            //controls
            float t_x_mouse = Input.GetAxisRaw("Mouse X");
            float t_y_mouse = Input.GetAxisRaw("Mouse Y");

            //target rotation
            Quaternion t_x_adj = Quaternion.AngleAxis(intensity * t_x_mouse, Vector3.up);
            Quaternion t_y_adj = Quaternion.AngleAxis(intensity * t_y_mouse, Vector3.right);
            Vector3 target_position = origin_position; //* t_x_adj * t_y_adj;
                                                       //rotate towards target rotation
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);//(transform.localPosition, target_rotation, Time.deltaTime * smooth);
        }
    }
}