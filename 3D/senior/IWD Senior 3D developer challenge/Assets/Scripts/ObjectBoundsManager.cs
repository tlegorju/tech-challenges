using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundsManager : MonoBehaviour
{
    [SerializeField] List<Collider> listObjectCollider;
    [SerializeField] List<bool> listObjectInSight;

    CameraViewAllObjectController cameraController;

    public Material[] listMaterial;

    Vector3 middlePointObjects = Vector3.zero;
    Vector3 minBounds, maxBounds;

    private void Awake()
    {
        listObjectCollider = new List<Collider>();
        listObjectInSight = new List<bool>();

        cameraController = Camera.main.GetComponent<CameraViewAllObjectController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] listGameObject = GameObject.FindGameObjectsWithTag("ObjectToDisplay");
        for(int i=0; i<listGameObject.Length; i++)
        {
            Collider tmp = listGameObject[i].GetComponent<Collider>();
            if (tmp != null)
            {
                if(i==0)
                {
                    minBounds = maxBounds = tmp.transform.position;
                }
                else
                {
                    UpdateObjectsBounds(tmp.transform);
                }

                listObjectCollider.Add(tmp);
                listObjectInSight.Add(IsObjectInSight(tmp));

                //middlePointObjects += listObjectCollider[i].transform.position;
            }
        }
        middlePointObjects /= listObjectCollider.Count;

        cameraController.CalculateTargetPoint(minBounds, maxBounds);
    }

    void UpdateObjectsBounds(Transform obj)
    {
        if (minBounds.x > obj.position.x)
            minBounds.x = obj.position.x;
        if (minBounds.y > obj.position.y)
            minBounds.y = obj.position.y;
        if (minBounds.z > obj.position.z)
            minBounds.z = obj.position.z;
        if (maxBounds.x < obj.position.x)
            maxBounds.x = obj.position.x;
        if (maxBounds.y < obj.position.y)
            maxBounds.y = obj.position.y;
        if (maxBounds.z < obj.position.z)
            maxBounds.z = obj.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        ///TMP
        
        for(int i=0; i<listObjectCollider.Count; i++)
        {

            listObjectInSight[i] = IsObjectInSight(listObjectCollider[i]);
            if (listObjectInSight[i])
            {
                listObjectCollider[i].GetComponent<MeshRenderer>().material = listMaterial[0];
            }
            else
            {
                listObjectCollider[i].GetComponent<MeshRenderer>().material = listMaterial[1];
            }
        }


        //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, middlePointObjects, 1 * Time.deltaTime);

        ///TMP
    }

    public bool IsObjectInSight(Collider objCollider)
    {
        return GeometryUtility.TestPlanesAABB(cameraController.Planes, objCollider.bounds);
    }

    public bool AllObjectInSight()
    {
        for (int i = 0; i < listObjectInSight.Count; i++)
            if (!listObjectInSight[i])
                return false;

        return true;
    }
}
