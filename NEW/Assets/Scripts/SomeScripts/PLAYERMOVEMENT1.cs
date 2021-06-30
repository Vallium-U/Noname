
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using EZCameraShake;


namespace Vm
{
    public class PLAYERMOVEMENT1 : MonoBehaviour
    {
        //Assingables
        [Header("Assingables: ")]
        public Transform playerCam;
        public Transform orientation;
        //public Transform WeaponPos;

        public Rigidbody rb;
        //public GameObject Gun;
        public Camera cam;
        public float FOV;

        //Rotation and look
        [Header("Sensivity: ")]
        private float xRotation;
        public float sensitivity = 100f;
        public float sensMultiplier = 5f;

        //Movement
        [Header("Movement parameters: ")]
        public float moveSpeed = 450;
        public float maxSpeed = 25;
        public float jumpPower = 20f;
        //public float jumpCount = 2;
        public bool canJump;
        public float extraGravity = 1f;
        public float maxSlopeAngle = 35f;
        public float counterMovement = 0.175f;
        [Range(0.0f, 10.0f)]
        public float AirCounterMovementMultiplier;
        private float threshold = 0.01f;
        public LayerMask whatIsGround;
        //Sliding
        private Vector3 normalVector = Vector3.up;
        private Vector3 wallNormalVector;
        //Crouch & Slide
        [Header("Crouch&Slide: ")]
        private Vector3 crouchScale = new Vector3(1.2f, 0.45f, 1.2f);
        private Vector3 playerScale = new Vector3(1.2f, 1.75f, 1.2f);
        public float slideForce = 400;
        public float slideCounterMovement = 0.2f;
        //dash
        [Header("Dash: ")]
        public float dashForce;// = 200f;
        public float afterDashForce; //= 20f;
        public float dashDuration;// = 0.2f;
        public float dashCooldown;
        public float dashCDTimer;// = 5f;
        //cameraShake
        [Header("Camera shake: ")] 
        public GameObject CameraHolder;
        //
        float bodyRotationX;
        float camRotationY;
        Vector3 directionIntentX;
        Vector3 directionIntentY;
        float speed;

        //Vector3 defaultPos;
        //Vector3 posOffset;
        //float fallSpeed = 40f;
        //Vector3 posVel;
        //float posSpeed = 40f;
        [Header("Checkers: ")]
        public bool grounded;
        public bool crouched;
        [Header("AudioSources: ")]
        public AudioSource stepSound;
        public AudioSource wing;
        public AudioSource slide;
        public AudioSource jump;
        //BulletTime
        [Header("BulletTime: ")] 
        public float BulletTimeCoef;
        private float BulletTimeDef=1.0f;
        public PostProcessVolume PostProcessVolume;
        private Vignette _vignette;
        [Range(15.0f, 150.0f)]
        public float VignetteSpeed;
        //camerashake
        [Header("CameraShake: ")]
        public float landMagFx;
        public float landRougFx;
        public float landFadeInFx;
        public float landFadeOutFx;
        public Vector3 Yvec= new Vector3(0f, 5f, 0f);
        //Our saved shake instance.
        private CameraShakeInstance _shakeInstance;
        
        //public Transform CAMERASFORSHAKE;
        //private Vector3 currentRotation;
        //private Vector3 Rot;

        //[Header("SizeShake: ")]
        //public Vector3 ShakeRotation = new Vector3();//2 2 2 
        //public float rotationSpeed;//6
        //public float returnSpeed; // 25
        //[Header("RunShake: ")]
        //public Vector3 ShakeRotationrun = new Vector3(0, 2, 2);//2 2 2 
        //public float rotationSpeedrun = 5;//6
        //public float returnSpeedrun = 10;

        //private Vector3 ShakeRotationfix = new Vector3(0, 0, 0);//2 2 2 
        //private float rotationSpeedfix = 1;//6
        //private float returnSpeedfix = 30;

        public ParticleSystem VFX;
        ParticleSystem.EmissionModule emissionFX;

        private bool IsLanding= false ;
 
        
        // Start is called before the first frame update
        void Start()
        {
            //We make a single shake instance that we will fade in and fade out when the player enters and leaves the trigger area.
            _shakeInstance = CameraShaker.Instance.StartShake(1, 1, 5);

            //Immediately make the shake inactive.  
            _shakeInstance.StartFadeOut(0);
         

            //We don't want our shake to delete itself once it stops shaking.
            _shakeInstance.DeleteOnInactive = false;
            
            FindObjectOfType<AudioManager>().Play("MainTheme");//MainTheme music play
            rb = GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cam.fieldOfView = FOV;

            //VFX = GetComponent<ParticleSystem>();
            emissionFX = VFX.emission;
            emissionFX.rateOverTime = 0;
            VFX.Play();
            wing.volume = 0;
            wing.Play();
        }
        private void FixedUpdate()
        {
            ExtraGravity();
            /*if (!grounded)
            {
                currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
                Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
                CAMERASFORSHAKE.transform.localRotation = Quaternion.Euler(Rot);
            }
            else
            {
                currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeedfix * Time.deltaTime);
                Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeedfix * Time.fixedDeltaTime);
                CAMERASFORSHAKE.transform.localRotation = Quaternion.Euler(Rot);
            }
            if(grounded){
                currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeedrun * Time.deltaTime);
                Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeedrun * Time.fixedDeltaTime);
                CAMERASFORSHAKE.transform.localRotation = Quaternion.Euler(Rot);
            }*/
        }
        // Update is called once per frame
        void Update()
        {
            var vel = rb.velocity;
            var main = VFX.main;
            Look();
            Movement();
            //GroundCheck();
            Jump();
            if (grounded)
            {
                Landing = true;
                //jumpCount = 0;
                //wing.Stop();
            }
            else
            {
                Landing = false;
            }
            //BulletTime
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                BulletTime();
                BulletTimePost();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Time.timeScale = BulletTimeDef;
                BulletTimePostDef();
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                BulletTimePost();
            }
            else
            {
                BulletTimePostDef();
            }
            //WingFX
            if (vel.magnitude > 1 && !grounded)
            {
                wing.volume = vel.magnitude / 250;
                //wing.pitch = Random.Range(0.5f, 1.1f);
                //wing.Play();

            }
            else if (vel.magnitude > 20 && grounded)
            {
                wing.volume = vel.magnitude / 300;
                //wing.pitch = Random.Range(0.5f, 1.1f);
                //wing.Play();

            }
            else
            {
                wing.volume = 0;
            }
            if (dashCDTimer > 0)
            {
                dashCDTimer -= Time.deltaTime;
            }
            if (dashCDTimer < 0)
            {
                dashCDTimer = 0;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCrouch();
                crouched = true;
                if (vel.magnitude > 15 && slide.isPlaying == false)
                {
                    SlideSoundFX();
                }
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                StopCrouch();
                crouched = false;
            }
            if (!grounded && dashCDTimer == 0 && Input.GetKeyDown(KeyCode.LeftShift))
            {
                CameraShaker.Instance.ShakeOnce(2f, 2f, 0.2f, 1.2f);
                StartCoroutine(Dash());
                dashCDTimer = dashCooldown;
            }
            if (vel.magnitude > 15 && grounded && stepSound.isPlaying == false)
            {
                StepSoundFX();
            }
            if (vel.magnitude > 120 && !grounded)
            {
                _shakeInstance.Magnitude = 0.5f;
                _shakeInstance.Roughness = 1.2f;
                
                _shakeInstance.StartFadeIn(3f);
                //rotationSpeed = vel.magnitude / 50;
                //CameraHighSpeedShake();
            }
            else if(vel.magnitude>40 && grounded){
                _shakeInstance.Magnitude = 0.35f;
                _shakeInstance.Roughness = 0.75f;
                
                _shakeInstance.StartFadeIn(3f);
                //ShakeRun();
            }
            if(vel.magnitude<30)
            {
                _shakeInstance.Magnitude = 0.5f;
                _shakeInstance.Roughness = 0.5f;
                _shakeInstance.StartFadeOut(5f);  
                //ShakeFix();
            }
            
            //particle speed fx
            if (vel.magnitude > 30)
            {
                //ShakeRun();
                main.maxParticles = (int)vel.magnitude / 10;
                emissionFX.rateOverTime = vel.magnitude / 10;
                //VFX.speed = vel.magnitude / 10;
            }
            else
            {
                emissionFX.rateOverTime = 0;
                main.maxParticles = 0;
            }
        }
        public bool Landing
        {
            get { return IsLanding ; }
            set
            {
                if( value == IsLanding )
                    return ;
 
                IsLanding = value ;
                if( IsLanding )
                {
                    //_shakeInstance.PositionInfluence.x = 0f;
                    //_shakeInstance.PositionInfluence.y = -2.5f;
                    //_shakeInstance.PositionInfluence.z = 0f;
                    //_shakeInstance.Magnitude = 0.5f;
                    //_shakeInstance.Roughness = 0.5f;

                    // var position = CameraHolder.transform.position;
                    // Vector3 CurPos = new Vector3(position.x,position.y - 10f,position.z);
                    // CameraHolder.transform.position = Vector3.Lerp(CurPos, position , 0.5f)* Time.deltaTime;
                    

                    CameraShaker.Instance.ShakeOnce(landMagFx, landRougFx,landFadeInFx, landFadeOutFx, Yvec, CameraShaker.Instance.RestRotationOffset);
                    // Play sound
                }    
            }    
        }
        private float desiredX;
        public void Look()
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

            //Find current look rotation
            Vector3 rot = playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;

            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Perform the rotations
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }
        public void Movement()
        {
            //Directiont
            directionIntentX = playerCam.right;
            directionIntentX.y = 0;
            directionIntentX.Normalize();

            directionIntentY = playerCam.forward;
            directionIntentY.y = 0;
            directionIntentY.Normalize();

            float X = Input.GetAxisRaw("Horizontal");
            float Y = Input.GetAxisRaw("Vertical");

            Vector2 mag = FindVelRelativeToLook();
            float _xMag = mag.x, _yMag = mag.y;

            CounterMovement(X, Y, mag);
            // if (!grounded) return;
            //Set max speed
            float maxSpeed = this.maxSpeed;

            // If sliding down a ramp, add force down so player stays grounded and also builds speed
            if (crouched && grounded)
            {
                rb.AddForce(Vector3.down * Time.deltaTime * 30000);
                return;
            }

            //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
            if (X > 0 && _xMag > maxSpeed) X = 0;
            if (X < 0 && _xMag < -maxSpeed) X = 0;
            if (Y > 0 && _yMag > maxSpeed) Y = 0;
            if (Y < 0 && _yMag < -maxSpeed) Y = 0;

            //Some multipliers
            float multiplier = 1f, multiplierV = 1f;

            // Movement in air
            if (!grounded)
            {
                multiplier = AirCounterMovementMultiplier;
                multiplierV = AirCounterMovementMultiplier;
            }

            // Movement while sliding
            if (grounded && crouched) multiplierV = 0.6f;

            //Apply forces to move player
            rb.AddForce(orientation.transform.forward * Y * moveSpeed * 100 * Time.deltaTime * multiplier * multiplierV);
            rb.AddForce(orientation.transform.right * X * moveSpeed * 100 * Time.deltaTime * multiplier * multiplierV);

            //Control speed based movement state
        }
        public void CounterMovement(float X, float Y, Vector2 mag)
        {
            if (!grounded) return;

            //float multiplier = 100.5f;
            //Slow down sliding
            if (crouched)
            {
                rb.AddForce(moveSpeed * Time.deltaTime * rb.velocity.normalized * slideForce);
                rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
                return;
            }

            //Counter movement
            if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(X) < 0.05f || (mag.x < -threshold && X > 0) || (mag.x > threshold && X < 0))
            {
                rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
            }
            if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(Y) < 0.05f || (mag.y < -threshold && Y > 0) || (mag.y > threshold && Y < 0))
            {
                rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
            }

            //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
            if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
            {
                float fallspeed = rb.velocity.y;
                Vector3 n = rb.velocity.normalized * maxSpeed;
                rb.velocity = new Vector3(n.x, fallspeed, n.z);
            }
        }
        public Vector2 FindVelRelativeToLook()
        {
            float lookAngle = orientation.transform.eulerAngles.y;
            float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

            float u = Mathf.DeltaAngle(lookAngle, moveAngle);
            float v = 90 - u;

            float magnitue = rb.velocity.magnitude;
            float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
            float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

            return new Vector2(xMag, yMag);
        }

        public void StartCrouch()
        {
            grounded = false;
            float slideForce = 15f;
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            if (rb.velocity.magnitude > 0.5f)
            {
                if (grounded)
                {
                    rb.AddForce(orientation.transform.forward * slideForce);
                }
            }
        }

        public void StopCrouch()
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }

        public void ExtraGravity()
        {
            rb.AddForce(Vector3.down * extraGravity);
        }
        // void GroundCheck()
        // {
        //     RaycastHit groundHit;
        //     grounded = Physics.Raycast(transform.position, -transform.up, out groundHit, 2f);
        // }

        // private bool IsFloor(Vector3 v)
        // {
        //     float angle = Vector3.Angle(Vector3.up, v);
        //     return angle < maxSlopeAngle;
        // }
        public bool OnSlope(Vector3 s)
        {
            float angle = Vector3.Angle(Vector3.up, s);
            if (angle > 10 && angle < maxSlopeAngle && !crouched && grounded)
            {
                rb.AddForce(Vector3.up * 50);
            }
            else
            {

            }

            return angle < maxSlopeAngle;
        }

        private bool cancellingGrounded;

        // Handle ground detection
        private void OnCollisionStay(Collision other)
        {
            //Make sure we are only checking for walkable layers
            int layer = other.gameObject.layer;
            if (whatIsGround != (whatIsGround | (1 << layer))) return;

            //Iterate through every collision in a physics update
            for (int i = 0; i < other.contactCount; i++)
            {
                Vector3 normal = other.contacts[i].normal;
                //FLOOR
                if (OnSlope(normal))
                {
                    grounded = true;

                    cancellingGrounded = false;
                    normalVector = normal;
                    CancelInvoke(nameof(StopGrounded));
                }
            }

            //Invoke ground/wall cancel, since we can't check normals with CollisionExit
            float delay = 3f;
            if (!cancellingGrounded)
            {
                cancellingGrounded = true;
                Invoke(nameof(StopGrounded), Time.deltaTime * delay);
            }
        }

        public void StopGrounded()
        {
            grounded = false;
        }
        public void Jump()
        {
            if (grounded)
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
            if (Input.GetKeyDown(KeyCode.Space) && grounded && canJump)
            {
                var vel = rb.velocity;

                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                if (jump.isPlaying == false)
                {
                    JumpSoundFX();
                }
                // if (jumpCount == 0)
                // {
                //     rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                //     if (grounded)
                //     {
                //         jumpCount = 1;
                //     }
                //     else
                //     {
                //         jumpCount = 2;
                //     }
                // }
                // else if (jumpCount == 1)
                // {
                //     rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                //     jumpCount = 2;
                // }
            }


            //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        public IEnumerator Dash()
        {

            rb.AddForce(cam.transform.forward * dashForce, ForceMode.Impulse);
            yield return new WaitForSeconds(dashDuration);
            rb.velocity = Vector3.zero;
            rb.AddForce(cam.transform.forward * (afterDashForce), ForceMode.Impulse);

        }
        
        
        //FXes
        void StepSoundFX()
        {
            stepSound.volume = Random.Range(0.025f, 0.25f);
            stepSound.pitch = Random.Range(0.8f, 1.2f);
            stepSound.Play();
        }
        void WingSoundFX()
        {
            // wing.volume = Random.Range(0.5f, 0.75f);
            wing.pitch = Random.Range(0.8f, 1.1f);
            wing.Play();

        }
        void SlideSoundFX()
        {
            slide.volume = Random.Range(0.025f, 0.25f);
            slide.pitch = Random.Range(0.8f, 1.2f);
            slide.Play();
        }
        void JumpSoundFX()
        {
            jump.volume = Random.Range(0.025f, 0.25f);
            jump.pitch = Random.Range(0.8f, 1.2f);
            jump.Play();
        }

        /*void CameraHighSpeedShake()
        {
            currentRotation += new Vector3(-ShakeRotation.x, Random.Range(-ShakeRotation.y, ShakeRotation.y), Random.Range(-ShakeRotation.z, ShakeRotation.z));
        }
        void ShakeFix()
        {
            currentRotation += new Vector3(-ShakeRotationfix.x, Random.Range(-ShakeRotationfix.y, ShakeRotationfix.y), Random.Range(-ShakeRotationfix.z, ShakeRotationfix.z));

        }
        void ShakeRun()
        {
            
            //currentRotation += new Vector3(-ShakeRotationrun.x, Random.Range(-ShakeRotationrun.y, ShakeRotationrun.y), Random.Range(-ShakeRotationrun.z, ShakeRotationrun.z));

        }*/

        public void BulletTime()
        {
            Time.timeScale = BulletTimeCoef;
        }

        public void BulletTimePost()
        {
            PostProcessVolume.profile.TryGetSettings(out _vignette);
            _vignette.roundness.value = Mathf.Lerp(_vignette.roundness.value, 0.9f, VignetteSpeed * Time.fixedDeltaTime);
        }
        public void BulletTimePostDef()
        {
            PostProcessVolume.profile.TryGetSettings(out _vignette);
            _vignette.roundness.value = Mathf.Lerp(_vignette.roundness.value, 0, VignetteSpeed * Time.fixedDeltaTime);
        }
    }
}