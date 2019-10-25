using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundsManager : MonoBehaviour
{
    [SerializeField] List<Collider> listObjectCollider;
    [SerializeField] List<bool> listObjectInSight;

    public Material[] listMaterial;

    Vector3 middlePointObjects = Vector3.zero;

    private CameraViewAllObjectController cameraController;

    private void Awake()
    {
        listObjectCollider = new List<Collider>();
        listObjectInSight = new List<bool>();

        cameraController = Camera.main.GetComponent<CameraViewAllObjectController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Récupère tous les objets de la scène
        GameObject[] listGameObject = GameObject.FindGameObjectsWithTag("ObjectToDisplay");
        for(int i=0; i<listGameObject.Length; i++)
        {
            //L'objet a besoin d'un collider. S'il n'en a pas il n'est pas valide on ne va pas le prendre en compte
            Collider tmp = listGameObject[i].GetComponent<Collider>();
            if (tmp != null)
            {

                //Ajoute le collider à la liste et détermine si l'objet est déjà visible ou pas
                listObjectCollider.Add(tmp);
                listObjectInSight.Add(IsObjectInSight(tmp));

                middlePointObjects += tmp.transform.position;
            }
        }
        middlePointObjects /= listObjectCollider.Count;

        cameraController.CalculateTargetPoint(middlePointObjects);
    }


    // Update is called once per frame
    void Update()
    {
        //Met à jour la liste des objets visibles et affiche les 
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
        cameraController.allObjectsInSight = AllObjectInSight();
    }

    //Test si l'objet est dans le champs de vision de la caméra, même s'il est derrière un autre objet.
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
