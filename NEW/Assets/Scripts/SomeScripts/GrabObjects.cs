using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vm
{
    public class GrabObjects : MonoBehaviour
    {
        private SpringJoint grabJoint;
        public Vector3 grabPoint;
        private Vector3 previousLookdir;
        public LineRenderer grabLr;
        public Rigidbody objectGrabbing;
        public Camera cam;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Alpha1) && !Input.GetKey(KeyCode.Alpha2) && !Input.GetKey(KeyCode.Alpha3) && !Input.GetKeyUp(KeyCode.Alpha4) && !Input.GetKey(KeyCode.Alpha5))
            {
                GrabObject();
                //HoldGrab();

            }
            else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5))
            {
                StopGrab();
            }
            // if (Input.GetKeyUp(KeyCode.Mouse1))
            // {
            //     StopGrab();
            // }
            HoldGrab();
        }

        private void GrabObject()
        {
            if (objectGrabbing == null)
                StartGrab();
            else if (Input.GetKey(KeyCode.Mouse1) || !Input.GetKey(KeyCode.Alpha1) || !Input.GetKey(KeyCode.Alpha2) || !Input.GetKey(KeyCode.Alpha3) || !Input.GetKeyUp(KeyCode.Alpha4) || !Input.GetKey(KeyCode.Alpha5))
            {
                HoldGrab();
            }
            else StopGrab();
        }
        private void StartGrab()
        {
            RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, 5f);
            if (hits.Length < 1) return;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Object"))
                {
                    objectGrabbing = hits[i].transform.GetComponent<Rigidbody>();
                    if (objectGrabbing == null) return;
                    grabPoint = hits[i].point;
                    grabJoint = objectGrabbing.gameObject.AddComponent<SpringJoint>();
                    grabJoint.autoConfigureConnectedAnchor = false;
                    grabJoint.minDistance = 0f;
                    grabJoint.maxDistance = 0f;
                    grabJoint.damper = 4f;
                    grabJoint.spring = 40f;
                    grabJoint.massScale = 5f;

                    objectGrabbing.angularDrag = 5f;
                    objectGrabbing.drag = 1f;

                    previousLookdir = cam.transform.forward;

                    grabLr = objectGrabbing.gameObject.AddComponent<LineRenderer>();
                    grabLr.positionCount = 2;
                    grabLr.startWidth = 0.1f;
                    grabLr.material = new Material(Shader.Find("Sprites/Default"));
                    grabLr.numCapVertices = 10;
                    grabLr.numCornerVertices = 10;
                    return;
                }
            }
        }
        private void HoldGrab()
        {
            if (objectGrabbing == null) return;
            grabJoint.connectedAnchor = cam.transform.position + (cam.transform.forward * 6.5f);
            grabLr.SetPosition(0, objectGrabbing.position);
            grabLr.SetPosition(1, grabJoint.connectedAnchor);
            previousLookdir = cam.transform.forward;
            return;
        }
        private void StopGrab()
        {
            Destroy(grabJoint);
            Destroy(grabLr);
            if (objectGrabbing == null) return;
            objectGrabbing.angularDrag = 0.05f;
            objectGrabbing.drag = 0.0f;
            objectGrabbing = null;
            return;
        }
    }
}