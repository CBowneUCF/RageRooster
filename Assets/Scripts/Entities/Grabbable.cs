using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour
{
    #region Config

    public bool twoHanded;
    public float weight;
    public float wiggleFreeTime;
    public int maxHealthToGrab;
    public UnityEvent<bool> GrabStateEvent;

    #endregion
    #region Data

    private Grabber grabber;
    public bool grabbed => grabber != null;

    public new Collider collider {  get; private set; }
    public Rigidbody rb { get; private set; }
    public EnemyHealth health { get; private set; }

    #endregion

    private void Awake()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<EnemyHealth>();
    }

    public static bool Grab(GameObject target, out Grabbable result) => target.TryGetComponent(out result) && result.enabled && result.UnderThreshold() ? true : false;

    public Grabbable Grab(Grabber grabber)
    {
        this.grabber = grabber;
        GrabStateEvent?.Invoke(true);


        if(rb) rb.isKinematic = true;
        collider.enabled = false;

        if (wiggleFreeTime > 0) StartCoroutine(WiggleEnum(wiggleFreeTime));

        return this;
    }
    private IEnumerator WiggleEnum(float wiggleTime)
    {
        yield return new WaitForSeconds(wiggleTime);
        Release();
    }

    public void Release()
    {
        if (!grabbed) return;
        grabber = null;
        GrabStateEvent?.Invoke(false);

        if(rb) rb.isKinematic = false;
        collider.enabled = true;
    }

    public bool UnderThreshold() => !health || maxHealthToGrab < 15 || health.GetCurrentHealth() <= maxHealthToGrab;

}
