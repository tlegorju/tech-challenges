using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewAllObjectController : MonoBehaviour
{
    private Plane[] planes;
    public Plane[] Planes { get { return planes; } }

    [SerializeField] private Camera thisCamera;

    private Vector3 previousPos;
    private Vector3 targetPoint;
    public Vector3 TargetPoint { get { return targetPoint; } set { targetPoint = value; } }

    [SerializeField] ObjectBoundsManager objectBoundsManager;
    [SerializeField] float lerpSpeed = 5.0f;

    private void Awake()
    {
        thisCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousPos = transform.position;
        UpdatePlanes();
    }

    // Update is called once per frame
    void Update()
    {
        if(previousPos != transform.position)
        {
            previousPos = transform.position;
            UpdatePlanes();
        }
    }

    public void UpdatePlanes()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(thisCamera);
    }

    public void CalculateTargetPoint(Vector3 minBounds, Vector3 maxBounds)
    {
        targetPoint.z = (minBounds.z + maxBounds.z) / 2; //minBounds.z-10;
        targetPoint.x = (minBounds.x + maxBounds.x) / 2;
        targetPoint.y = (minBounds.y + maxBounds.y) / 2;
        Debug.Log(targetPoint);
        StartCoroutine("GoToMiddlePoint");
    }

    IEnumerator GoToMiddlePoint()
    {
        while (Vector3.SqrMagnitude(transform.position - targetPoint) > 0.35f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime);
            yield return null;
        }
        StartCoroutine("GoToTargetPoint");
    }

    IEnumerator GoToTargetPoint()
    {
        while(!objectBoundsManager.AllObjectInSight())
        {
            transform.position = Vector3.Lerp(transform.position, transform.position- transform.TransformDirection(Vector3.forward)*3, Time.deltaTime*lerpSpeed);
            yield return null;
        }
        targetPoint = transform.position - transform.TransformDirection(Vector3.forward) * 3;

        while(Vector3.SqrMagnitude(transform.position-targetPoint)>0.3)
        {
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * lerpSpeed);
            yield return null;
        }
    }
}
