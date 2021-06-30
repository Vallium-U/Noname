using System.Collections;
using UnityEngine;

namespace Vm
{
    public class WallMovement : MonoBehaviour
    {
        public bool isWallR = false;
        public bool isWallL = false;
        public float JumpFromWallHeigh;//45
        public float SideJumpForce;//40
        public float GravityModifire;//12
        public float SideForce;//250
        private RaycastHit hitR;
        private RaycastHit hitL;
        private int jumpCount = 0;
        public Rigidbody rb;
        public PLAYERMOVEMENT1 cc;
        public Transform cameraEffect;
        public Animator CamAnim;
        public Animator InvAnim;
        public bool canJump;
        public AudioSource stepSound;
        public AudioSource jump;

        private int check = 0;
        public float impulseUP;



        // Use this for initialization
        void Start()
        {

        }

        void FixedUpdate()
        {
            if (!cc.grounded)
            {
                if (Physics.Raycast(transform.position, transform.right, out hitR, 1.5f))
                {
                    if (hitR.transform.tag == "Wall")
                    {

                        if (check == 0)
                        {
                            Impulse();
                            check = 1;
                        }
                        ExtraWallGrav();
                    }
                }
                if (Physics.Raycast(transform.position, -transform.right, out hitL, 1.5f))
                {
                    if (hitL.transform.tag == "Wall")
                    {
                        if (check == 0)
                        {
                            Impulse();
                            check = 1;
                        }
                        ExtraWallGrav();
                    }
                }

            }

        }
        // Update is called once per frame
        void Update()
        {
            if (isWallL == false && isWallR == false)
            {
                check = 0;
            }
            else
            {
                check = 1;
            }
            var vel = rb.velocity;
            // you have to add your own is grounded stuff
            if (cc.grounded)
            {

                jumpCount = 0;
                isWallL = false;
                isWallR = false;
            }
            if (isWallR == true && isWallL == false)
            {

                CamAnim.SetBool("RightWallrun", true);
                InvAnim.SetBool("RightWallrun", true);
            }
            if (isWallR == false)
            {

                CamAnim.SetBool("RightWallrun", false);
                InvAnim.SetBool("RightWallrun", false);
            }
            if (isWallL == false)
            {

                CamAnim.SetBool("LeftWallrun", false);
                InvAnim.SetBool("LeftWallrun", false);
            }
            if (isWallR == false && isWallL == true)
            {

                CamAnim.SetBool("LeftWallrun", true);
                InvAnim.SetBool("LeftWallrun", true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isWallL && Input.GetKeyDown(KeyCode.Space) && canJump == true)
                {
                    rb.AddForce(Vector3.up * JumpFromWallHeigh, ForceMode.Impulse);
                    JumpSoundFX();
                }
                if (isWallR && Input.GetKeyDown(KeyCode.Space) && canJump == true)
                {
                    rb.AddForce(Vector3.up * JumpFromWallHeigh, ForceMode.Impulse);
                    JumpSoundFX();
                }
                //rb.AddForce(Vector3.up * 45, ForceMode.Impulse);
                if (isWallL == true)
                {
                    rb.AddForce(Vector3.up * 20, ForceMode.Impulse);

                    Vector3 force = this.transform.right * SideJumpForce;
                    rb.AddForceAtPosition(force, this.transform.position, ForceMode.Impulse);
                }
                if (isWallR == true)
                {
                    rb.AddForce(Vector3.up * 20, ForceMode.Impulse);

                    Vector3 force = -this.transform.right * SideJumpForce;
                    rb.AddForceAtPosition(force, this.transform.position, ForceMode.Impulse);
                }
            }

            if (!cc.grounded)
            {
                if (Physics.Raycast(transform.position, transform.right, out hitR, 1.5f))
                {
                    if (hitR.transform.tag == "Wall")
                    {
                        canJump = true;
                        cc.canJump = false;
                        isWallR = true;
                        isWallL = false;
                        jumpCount += 1;

                        rb.AddForce(transform.right * SideForce);
                        if (vel.magnitude > 15 && stepSound.isPlaying == false)
                        {
                            StepSoundFX();
                        }
                        //ExtraWallGrav();

                        rb.useGravity = false;
                    }
                }
                if (!Physics.Raycast(transform.position, transform.right, out hitR, 1.5f))
                {

                    isWallR = false;
                    jumpCount += 1;
                    if (isWallL == false)
                    {
                        canJump = false;
                        rb.useGravity = true;
                    }
                }
                if (Physics.Raycast(transform.position, -transform.right, out hitL, 1.5f))
                {
                    if (hitL.transform.tag == "Wall")
                    {
                        canJump = true;
                        isWallL = true;
                        jumpCount += 1;

                        rb.AddForce(-transform.right * SideForce);
                        if (vel.magnitude > 15 && stepSound.isPlaying == false)
                        {
                            StepSoundFX();
                        }
                        //ExtraWallGrav();

                        rb.useGravity = false;
                    }
                }
                if (!Physics.Raycast(transform.position, -transform.right, out hitL, 1.5f))
                {

                    isWallL = false;
                    jumpCount += 1;
                    if (isWallR == false)
                    {
                        canJump = false;
                        rb.useGravity = true;
                    }
                }
            }
        }
        public void ExtraWallGrav()
        {
            rb.AddForce(Vector3.down * -cc.extraGravity * GravityModifire);

        }
        private void Impulse()
        {
            cc.rb.velocity = new Vector3(cc.rb.velocity.x, 0f, cc.rb.velocity.z);

            rb.AddForce(Vector3.up * impulseUP, ForceMode.Impulse);



        }
        void StepSoundFX()
        {
            stepSound.volume = Random.Range(0.025f, 0.25f);
            stepSound.pitch = Random.Range(0.8f, 1.2f);
            stepSound.Play();
        }
        void JumpSoundFX()
        {
            jump.volume = Random.Range(0.025f, 0.25f);
            jump.pitch = Random.Range(0.8f, 1.2f);
            jump.Play();
        }
    }
}
