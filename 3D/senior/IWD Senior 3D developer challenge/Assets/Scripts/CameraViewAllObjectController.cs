using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewAllObjectController : MonoBehaviour
{
    //"Encadre" le champs de vision de la caméra
    private Plane[] planes;
    public Plane[] Planes { get { return planes; } }

    //Référence à la caméra
    private Camera thisCamera;

    //Détermine quand mettre à jour les planes
    private Vector3 previousPos;

    //Permet de se déplacer de façon plus lissée
    private Vector3 targetPoint;
    public Vector3 TargetPoint { get { return targetPoint; } set { targetPoint = value; } }


    public bool allObjectsInSight = false;
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
    
    public void CalculateTargetPoint(Vector3 middleBounds)
    {
        targetPoint = middleBounds;
        
        //Rejoint la position moyenne des objets dans un premier temps
        //pour pouvoir reculer derrière
        StartCoroutine("GoToMiddlePoint");
    }

    //Déplace la caméra jusqu'au point central pour la reculer ensuite
    IEnumerator GoToMiddlePoint()
    {
        while (Vector3.SqrMagnitude(transform.position - targetPoint) > 0.35f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, 3*lerpSpeed*Time.deltaTime);
            yield return null;
        }
        StartCoroutine("GoToTargetPoint");
    }

    //Recule la caméra jusqu'à ce que tous les objets soient en vue.
    IEnumerator GoToTargetPoint()
    {
        while(!allObjectsInSight)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position- transform.TransformDirection(Vector3.forward)*5, Time.deltaTime*lerpSpeed);
            yield return null;
        }

        //Simule l'inertie et assure que l'objet limite soit entièrement dans la caméra
        targetPoint = transform.position - transform.TransformDirection(Vector3.forward) * 4;
        while(Vector3.SqrMagnitude(transform.position-targetPoint)>0.3)
        {
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * lerpSpeed);
            yield return null;
        }
    }
}
