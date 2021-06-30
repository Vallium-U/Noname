using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Vm
{
    public class GrapplingGunScript : MonoBehaviour
    {
        public Camera cam;
        public GameObject Player;
        public Rigidbody rb;
        public Transform Gun;
        private Vector3 grapplePoint;
        private SpringJoint joint;
        public LineRenderer lr;
        public float grappleDistanceMax;
        public float grappleDistanceMin;
        public float jointsprinf;
        public float jointdamperf;
        public float jointmassscalef;
        public float gunRotateSpeed;
        public float maxGrappleDistance;
        public AudioSource RopeSound;

        private void Start()
        {

        }
        private void Update()
        {
            CHCanGrab();
            CHMotion();

            if (Input.GetKeyDown(KeyCode.E) && CanGrab)
            {
                StartGrapple();


            }
            if (Input.GetKeyUp(KeyCode.E) || Input.GetKey(KeyCode.Q))
            {
                StopGrapple();
            }
            DrawGripple();
            GunMove();
        }
        private void LateUpdate()
        {
            UpdateGrapple();
        }
        private void StartGrapple()
        {
            RaycastHit[] raycastHit = Physics.RaycastAll(cam.transform.position, cam.transform.forward, maxGrappleDistance);
            if (raycastHit.Length < 1) return;
            //do not catch triggers colliders
            if (raycastHit[0].collider.tag == "EnemyMissle")
            {
                if (raycastHit[1].collider.tag == "EnemyMissle")
                    return;
                else
                {
                    grapplePoint = raycastHit[1].point;
                }
            }
            //do not catch triggers colliders
            grapplePoint = raycastHit[0].point;
            joint = Player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            joint.maxDistance = grappleDistanceMax; //8
            joint.minDistance = grappleDistanceMin; //1
            joint.spring = jointsprinf; //2
            joint.damper = jointdamperf; //2.5
            joint.massScale = jointmassscalef; //2

            Grapple(pos.position, grapplePoint);

            RopeSoundFx();

        }
        private void StopGrapple()
        {
            Destroy(joint);
            UnGrapple();
        }
        private void DrawGripple()
        {
            if (grapplePoint == Vector3.zero || joint == null)
            {
                lr.positionCount = 0;
                return;
            }
            lr.positionCount = 2;
            //lr.startWidth = 0.1f;
            // lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.numCapVertices = 10;
            lr.numCornerVertices = 1;
            lr.SetPosition(0, pos.position);
            lr.SetPosition(1, grapplePoint);

            UpdateStart(pos.position);
        }

        [Header("Values")]
        public AnimationCurve effectOverTime;
        public AnimationCurve curve;
        public AnimationCurve curveEffectOverDistance;
        public float curveSize;
        public float scrollSpeed;
        public float segments;
        public float animSpeed;
        public Transform pos;

        [Header("Data")]
        public LineRenderer lineRenderer;

        private Vector3 _start;
        private Vector3 _end;
        private float _time;
        private bool _active;

        public void UpdateGrapple()
        {
            lineRenderer.enabled = _active;

            if (_active)
                ProcessBounce();
        }

        private void ProcessBounce()
        {
            var vectors = new List<Vector3>();

            _time = Mathf.MoveTowards(_time, 1f,
                Mathf.Max(Mathf.Lerp(_time, 1f, animSpeed * Time.deltaTime) - _time, 0.2f * Time.deltaTime));

            vectors.Add(pos.position);

            var forward = Quaternion.LookRotation(grapplePoint - pos.position);
            var up = forward * Vector3.up;

            for (var i = 1; i < segments + 1; i++)
            {
                var delta = 1f / segments * i;
                var realDelta = delta * curveSize;
                while (realDelta > 1f) realDelta -= 1f;
                var calcTime = realDelta + -scrollSpeed * _time;
                while (calcTime < 0f) calcTime += 1f;

                var defaultPos = GetPos(delta);
                var effect = Eval(effectOverTime, _time) * Eval(curveEffectOverDistance, delta) * Eval(curve, calcTime);

                vectors.Add(defaultPos + up * effect);
            }

            lineRenderer.positionCount = vectors.Count;
            lineRenderer.SetPositions(vectors.ToArray());
        }

        private Vector3 GetPos(float d)
        {
            return Vector3.Lerp(pos.position, grapplePoint, d);
        }

        private static float Eval(AnimationCurve ac, float t)
        {
            return ac.Evaluate(t * ac.keys.Select(k => k.time).Max());
        }

        public void Grapple(Vector3 start, Vector3 end)
        {
            _active = true;
            _time = 0f;

            _start = start;
            _end = end;
        }

        public void UnGrapple()
        {
            _active = false;
        }

        public void UpdateStart(Vector3 start)
        {
            _start = pos.position;
        }

        public bool Grappling => _active;


        private void GunMove()
        {
            Vector3 GP = grapplePoint;
            Vector3 dir = (grapplePoint - transform.position).normalized;
            Quaternion lookdir = Quaternion.LookRotation(dir);

            if (grapplePoint == Vector3.zero || joint == null)
            {
                lookdir = transform.parent.rotation;
            }
            else {
            lookdir = Quaternion.LookRotation(grapplePoint - transform.position);
            }
                 transform.rotation = Quaternion.Lerp(transform.rotation, lookdir, Time.deltaTime * gunRotateSpeed);
        }
        private void RopeSoundFx()
        {

            RopeSound.volume = Random.Range(0.25f, 0.75f);
            RopeSound.pitch = Random.Range(0.8f, 1.2f);
            RopeSound.Play();

        }

        //CROSSHAIR
        public GameObject CH;
        public bool CanGrab;
        private float firingTimeStamp;
        private float accumulatedRecoil;
        private float recoveringRecoilTimeStamp;
        private const float exponentialAlpha = 0.8f;
        private float rPrevious;
        private void CHCanGrab()
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, maxGrappleDistance))
            {
                RaycastHit[] raycastHit = Physics.RaycastAll(cam.transform.position, cam.transform.forward, maxGrappleDistance);
                if (raycastHit.Length < 1) return;
                //do not catch triggers colliders
                if (raycastHit[0].collider.tag == "EnemyMissle")
                {
                    CanGrab = false;
                }
                else

                    CanGrab = true;
            }
            else
            {
                CanGrab = false;
            }
            if (CanGrab == true)
            {
                CH.SetActive(true);

            }
            else
            {
                CH.SetActive(false);
            }
        }

        public void CHMotion()
        {
            // firing
            firingTimeStamp = Mathf.Max(0, firingTimeStamp - Time.deltaTime);
            accumulatedRecoil = Mathf.Max(0, accumulatedRecoil - (Time.deltaTime * 3));
            recoveringRecoilTimeStamp += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.E) && firingTimeStamp <= 0)
            {
                // firingTimeStamp = ShootRate;
                accumulatedRecoil = 1f;
                recoveringRecoilTimeStamp = 0;
            }
            float rEffectiveRecoil = (accumulatedRecoil * exponentialAlpha) + (rPrevious * (1 - exponentialAlpha)); // exponential moving average
            rPrevious = rEffectiveRecoil;

            //CrosshairLibs.SetRecoil(rEffectiveRecoil);
        }
       public bool IsGrappling(){
        return joint==null;
        } 
    }
    
}