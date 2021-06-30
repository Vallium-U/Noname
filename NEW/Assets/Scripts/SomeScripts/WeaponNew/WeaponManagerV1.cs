using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vm
{
    

public class WeaponManagerV1 : MonoBehaviour
{
    public float pickupRange;
    public float pickupRadius;

    public int weaponLayer;

    public Transform weaponHolder;
    public Transform playerCamera;

    private bool _isWeaponHeld;
    private WeaponV1 _heldWeapon;

    private void Update()
    {
        if (_isWeaponHeld)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                _heldWeapon.Drop(playerCamera);
                _heldWeapon = null;
                _isWeaponHeld = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            var hitList = new RaycastHit[256];
            var hitNumber = Physics.CapsuleCastNonAlloc(playerCamera.position,
                playerCamera.position + playerCamera.forward * pickupRange, pickupRadius, playerCamera.forward, hitList);
            var realList = new List<RaycastHit>();
            for (var i = 0; i < hitNumber; i++)
            {
                var hit = hitList[i];
                if(hit.transform.gameObject.layer != weaponLayer) continue;
                if (hit.point == Vector3.zero)
                {
                   realList.Add(hit); 
                }
                else if (Physics.Raycast(playerCamera.position,hit.point - playerCamera.position,out var hitInfo, hit.distance+0.1f)&& hitInfo.transform == hit.transform)
                {
                    realList.Add(hit);
                }
            }

            if (realList.Count == 0) return;
            
            realList.Sort((hit1, hit2) =>
            {
                var dist1 = GetDistanceTo(hit1);
                var dist2 = GetDistanceTo(hit2);
                return Mathf.Abs(dist1 - dist2) < 0.001f ? 0 : dist1 < dist2 ? -1 : 1;
            });

            _isWeaponHeld = true;
            _heldWeapon = realList[0].transform.GetComponent<WeaponV1>();
            _heldWeapon.PickUp(weaponHolder, playerCamera);

        }
    }

    private float GetDistanceTo(RaycastHit hit)
    {
        return Vector3.Distance(playerCamera.position, hit.point == Vector3.zero ? hit.transform.position : hit.point);
    }
    
}
}