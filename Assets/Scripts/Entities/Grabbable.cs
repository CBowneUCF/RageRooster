using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour
{
    #region Config

    public bool twoHanded;
    public float weight;
    public float wiggleFreeTime;
    private UnityEvent<bool> GrabStateEvent;

    #endregion
    #region Data

    private Coroutine wiggleEnum;
    private Grabber grabber;
    public bool grabbed => grabber != null;

    public new Collider collider {  get; private set; }
    public Rigidbody rb { get; private set; }

    #endregion

    private void Awake()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public Grabbable Grab(Grabber grabber)
    {
        this.grabber = grabber;
        GrabStateEvent?.Invoke(true);

        rb.isKinematic = true;
        collider.enabled = false;

        if (wiggleFreeTime > 0) wiggleEnum = StartCoroutine(WiggleEnum(wiggleFreeTime));

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

        rb.isKinematic = false;
        collider.enabled = true;
    }




}

public abstract class Grabber : MonoBehaviour
{
    public Grabbable currentGrabbed;
    public UnityEvent<bool> GrabStateEvent;

    public bool grabbing => currentGrabbed != null;

    protected bool AttemptGrab(GameObject target)
    {
        if (target.TryGetComponent(out Grabbable result) && result.enabled)
        {
            BeginGrab(result);
            return true;
        }
        else return false;
    }

    public void BeginGrab(Grabbable target)
    {
        currentGrabbed = target;
        currentGrabbed.Grab(this);
        this.OnGrab();
        GrabStateEvent?.Invoke(true);
    }

    public void Release()
    {
        currentGrabbed.Release();
        this.OnRelease();
        currentGrabbed = null;
        GrabStateEvent?.Invoke(false);
    }

    protected virtual void OnGrab() { }
    protected virtual void OnRelease() { }
}