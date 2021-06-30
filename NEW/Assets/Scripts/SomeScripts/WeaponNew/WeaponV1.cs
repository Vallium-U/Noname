
using System;
using UnityEngine;
using Vm;
using Random = UnityEngine.Random;

public class WeaponV1 : MonoBehaviour
{
   [Header("Throwing")]
    public float throwForce;
    public float throwExtraForce;
    public float rotationForce;

    [Header("Pickup")]
    public float animTime;

   [Header("Data")]
    public int weaponGfxLayer;
    public GameObject[] weaponGfxs;
    public Collider[] gfxColliders;
    public GameObject HELDER;
    public GameObject HandsOff;

    private float _rotationTime;
    private float _time;
    private bool _held;
    private bool _scoping;
    private bool _reloading;
    private bool _shooting;
    private int _ammo;
    private Rigidbody _rb;
    private Transform _playerCamera;
    private Vector3 _startPosition;
    private Quaternion _startRotation;


   private void Start()
   {
      _rb = gameObject.AddComponent<Rigidbody>();
      _rb.mass = 0.1f;
   }

   private void Update()
   {
      if (!_held) return;

        if (_time < animTime) {
            _time += Time.deltaTime;
            _time = Mathf.Clamp(_time, 0f, animTime);
            var delta = -(Mathf.Cos(Mathf.PI * (_time / animTime)) - 1f) / 2f;
            transform.localPosition = Vector3.Lerp(_startPosition, Vector3.zero, delta);
            transform.localRotation = Quaternion.Lerp(_startRotation, Quaternion.identity, delta);
        }else {
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
        }

   }

   public void PickUp(Transform weaponHolder, Transform playerCamera)
   {
      HELDER.SetActive(true);
      HandsOff.SetActive(false);
      gameObject.GetComponent<MeshCollider>().enabled=false;
      gameObject.GetComponent<MeshRenderer>().enabled=false;
      
      if (_held) return;
        Destroy(_rb);
        _time = 0f;
        transform.parent = weaponHolder;
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        foreach (var col in gfxColliders) {
            col.enabled = false;
        }
        foreach (var gfx in weaponGfxs) {
            gfx.layer = weaponGfxLayer;
        }
        _held = true;
        _playerCamera = playerCamera;
        _scoping = false;
   }

   public void Drop(Transform playerCamera)
   {
      if (!_held) return;
      HELDER.SetActive(false);
      HandsOff.SetActive(true);
      gameObject.GetComponent<MeshCollider>().enabled=true;
      gameObject.GetComponent<MeshRenderer>().enabled=true;
      
       if (!_held) return;
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.mass = 0.1f;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        var forward = playerCamera.forward;
        forward.y = 0f;
        _rb.velocity = forward * throwForce;
        _rb.velocity += Vector3.up * throwExtraForce;
        _rb.angularVelocity = Random.onUnitSphere * rotationForce;
        foreach (var col in gfxColliders) {
            col.enabled = true;
        }
        foreach (var gfx in weaponGfxs) {
            gfx.layer = 0;
        }
        transform.parent = null;
        _held = false;
   }

   public void HeldCheck()
   {
      if (_held)
      {
         HELDER.SetActive(true);
      }
      else
      {
         HELDER.SetActive(false);
      }
   }
   
   
}

