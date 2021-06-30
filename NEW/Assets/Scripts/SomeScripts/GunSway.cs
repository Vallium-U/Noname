using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vm
{
    public class GunSway : MonoBehaviour
    {
        // public float intensity;
        // public float smooth;
        // private Quaternion origin_rotation;

        // private void Start()
        // {
        //     origin_rotation = transform.localRotation;
        //     UpdateSwayHip();
        // }
        // void Update()
        // {

        //     if (Input.GetMouseButton(1))
        //     {
        //         UpdateSwayAim();
        //     }
        //     else
        //         UpdateSwayHip();
        // }

        // private void UpdateSwayHip()
        // {
        //     //controls
        //     float t_x_mouse = Input.GetAxisRaw("Mouse X");
        //     float t_y_mouse = Input.GetAxisRaw("Mouse Y");

        //     //target rotation
        //     Quaternion t_x_adj = Quaternion.AngleAxis(-intensity * t_x_mouse, Vector3.up);
        //     Quaternion t_y_adj = Quaternion.AngleAxis(intensity * t_y_mouse, Vector3.right);
        //     Quaternion target_rotation = origin_rotation * t_x_adj * t_y_adj;
        //     //rotate towards target rotation
        //     transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
        // }
        // private void UpdateSwayAim()
        // {
        //     //controls
        //     float t_x_mouse = Input.GetAxisRaw("Mouse X");
        //     float t_y_mouse = Input.GetAxisRaw("Mouse Y");

        //     //target rotation
        //     Quaternion t_x_adj = Quaternion.AngleAxis(-(intensity / 2) * t_x_mouse, Vector3.up);
        //     Quaternion t_y_adj = Quaternion.AngleAxis((intensity / 2) * t_y_mouse, Vector3.right);
        //     Quaternion target_rotation = origin_rotation * t_x_adj * t_y_adj;
        //     //rotate towards target rotation
        //     transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * (smooth / 2));
        // }

        public float amount;
        public float maxAmount;
        public float smoothAmount;
        private Vector3 initPos;
        private Vector3 finalPos;
        private Quaternion origin_rotation;

        private void Start()
        {
            initPos = transform.localPosition;
            origin_rotation = transform.localRotation;
        }

        private void Awake()
        {
            initPos = transform.localPosition;
            origin_rotation = transform.localRotation;
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                UpdateSwayAim();
            }
            else
                UpdateSwayHip();
        }

        public void UpdateSwayHip()
        {
            float moveX = -Input.GetAxisRaw("Mouse X") * amount;
            float moveY = -Input.GetAxisRaw("Mouse Y") * amount;
            moveX = Mathf.Clamp(moveX, -maxAmount, maxAmount);
            moveY = Mathf.Clamp(moveY, -maxAmount, maxAmount);

            finalPos = new Vector3(moveX, moveY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initPos, Time.deltaTime * smoothAmount);
            //rotate towards target rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, origin_rotation, Time.deltaTime * (smoothAmount * 4));
        }
        private void UpdateSwayAim()
        {
            float moveX = -Input.GetAxisRaw("Mouse X") * amount / 3;
            float moveY = -Input.GetAxisRaw("Mouse Y") * amount / 3;
            moveX = Mathf.Clamp(moveX, -maxAmount / 3, maxAmount / 3);
            moveY = Mathf.Clamp(moveY, -maxAmount / 3, maxAmount / 3);

            Vector3 finalPos = new Vector3(moveX, moveY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initPos, Time.deltaTime * smoothAmount);
            //rotate towards target rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, origin_rotation, Time.deltaTime * (smoothAmount * 8));
        }
    }
}