using UnityEngine;
using System.Collections;

public class InstantiateObject : MonoBehaviour
{

    [SerializeField]
    private GameObject prefabToInstantiate;

    [SerializeField]
    private bool preInstantiate;

    [SerializeField]
    private int preInstantiatePasses;

    [SerializeField]
    private int preInstantiateDepth;

    private int yMin;
    private int yMax;
    private int xMin;
    private int xMax;
    private int zMin;
    private int zMax;

    //how many frames there are between an object's spawn
    [SerializeField]
    private int framesBetweenSpawns;

    //maximum objects spawning per wave
    [SerializeField]
    private int maxObjectsAWave;

    //choose a random number of objects to spawn per wave
    //with maxObjectsAWave as the upper bound
    [SerializeField]
    private bool maxObjectsAWaveRandom;

    //objects will spawn in the precise centre of the spawner
    [SerializeField]
    private bool spawnFromCentre;

    //in development
    [SerializeField]
    private bool addScript;

    //in development
    [SerializeField]
    private MonoBehaviour[] scriptsToAdd;

    //percentage chance an object will be rescaled
    [SerializeField]
    [Range(0, 10)]
    private int chanceOfRescalingObject;

    //lower bound of an object's scale change
    [SerializeField]
    private float rescalingLowerBound;

    //upper bound of an object's scale change
    [SerializeField]
    private float rescalingUpperBound;

    //hide the spawner's renderer if you wish it to be invisible
    [SerializeField]
    private bool hideRenderer;

    //disable the spawner's collider if you want it to be collision-less
    [SerializeField]
    private bool disableCollider;

    //disable the spawner's collider for a period following spawn
    [SerializeField]
    private bool[] temporarilyDisableObjectCollider;

    //for how many frames the collider should be temporarily disabled
    [SerializeField]
    private int[] framesObjectColliderShouldBeDisabled;

    //the type of collider we'll be dealing with
    private enum ColliderType { Box, Sphere, Capsule, Cylinder };
    [SerializeField]
    private ColliderType[] typeOfCollider;

    //for handling different types of object colliders
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;
    private CapsuleCollider capsuleCollider; //note: cylinders and capsules both use capsule colliders

    //only start spawning when in rage of an object
    [SerializeField]
    private bool spawnInRangeOfObject;

    //the object to track a distance from
    [SerializeField]
    private GameObject objectToSpawnInRangeOf;

    //the distance the target object has to be in before the spawner will spawn
    [SerializeField]
    private int rangeToSpawnIn;

    //despawn the object after it moves a certain distance from an object
    [SerializeField]
    private bool despawnAfterDistance;

    //the object to track a distance from
    [SerializeField]
    private GameObject objectToDespawnAfterDistance;

    //the distance away from the target object the instianted object
    //should be before it despawns
    [SerializeField]
    private int distanceToDespawnFrom;

    //
    [SerializeField]
    private bool despawnOnCollisionWithObject;

    //
    [SerializeField]
    private GameObject objectToDespawnOnCollisionWith;

    private int counter = 0;
    private Vector3[] worldSpaceVertices = new Vector3[8];

    void Awake()
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        //Only need the first 8 vertices, the others are UV co-ordinates
        for (int i = 0; i < 8; i++)
        {
            //transform total co-ordinates to world space
            worldSpaceVertices[i] = transform.TransformPoint(vertices[i]);
            //Debug.Log("world space vert " + i + ": " + worldSpaceVertices[i]);
        }

        //take our mess of co-ordinates and find the min and max's
        sortVertexBounds();

        //disable the spawner's collider otherwise objects will be trapped inside it
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        collider.enabled = false;

        if (hideRenderer == true)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
        }

        if (disableCollider == true)
        {
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }
    }
    // Use this for initialization
    void Start()
    {
        if (preInstantiate == true)
        {
            for (int i = 0; i < preInstantiatePasses; i++)
            {
                generateObject();
            }
        }      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //for spawning
        counter++;
        if (counter == 1000)
        { counter = 0; }

        //only spawn on a spesified frame
        if (counter % framesBetweenSpawns == 0 && counter != 0)
        {
            //if we're only spawning in range of an object and that object is inside that range
            if (spawnInRangeOfObject == true && Vector3.Distance(objectToSpawnInRangeOf.transform.position, transform.position) < rangeToSpawnIn)
            {
                generateObject();
            }
            if (spawnInRangeOfObject == false)
            {
                generateObject();
            }
        }
    }

    private void generateObject()
    {
        //set how many objects are spawning and if it's random
        int numObjectsSpawning = maxObjectsAWave;
        if (maxObjectsAWaveRandom)
        {
            numObjectsSpawning = Random.Range(0, maxObjectsAWave);
        }

        for (int i = 0; i < numObjectsSpawning; i++)
        {
            //create a new object and instantiate it
            GameObject newObject;
            newObject = Instantiate(prefabToInstantiate);

            //for reenabling colliders and despawning after a distance - custom written for Y2.0 project
            newObject.AddComponent<ReenableColliders>();
            ReenableColliders reenableColliders = newObject.GetComponent<ReenableColliders>();
            reenableColliders.SetNumberOfColliders(typeOfCollider.Length);
            for (int j = 0; j < typeOfCollider.Length; j++)
            {                
                reenableColliders.AssignColliderTime(j, framesObjectColliderShouldBeDisabled[j]);
                reenableColliders.AssignColliderType(j, (int)typeOfCollider[j]);
            }
            

            //if we want our objects to despawn when out of sight
            if (despawnAfterDistance == true)
            {
                //set the script
                newObject.AddComponent<DespawnAfterDistance>();
                
                DespawnAfterDistance despawnAfterDist = newObject.GetComponent<DespawnAfterDistance>();
                //assign the tracked object and the distance figure
                despawnAfterDist.SetTargetObject(objectToDespawnAfterDistance);
                despawnAfterDist.SetTargetDistance(distanceToDespawnFrom);
            }

            if (despawnOnCollisionWithObject == true)
            {
                newObject.AddComponent<DespawnOnTouch>();
                DespawnOnTouch despawnOnTouch = newObject.GetComponent<DespawnOnTouch>();
                //assign the tracked object and the distance figure
                despawnOnTouch.SetTargetObject(objectToDespawnOnCollisionWith);
            }

            for (int k = 0; k < typeOfCollider.Length; k++)
            {
                //disable the right collider type
                if (temporarilyDisableObjectCollider[k] == true)
                {
                    toggleColliders(newObject, false);
                }
            }
          

            //if we want to add a script, do so
            if (addScript == true)
            {
                //add all the scripts in our array
                for (i = 0; i < scriptsToAdd.Length; i++)
                {
                    newObject.AddComponent(scriptsToAdd[i].GetType());
                }
            }

            //create the x, y and z positions our objects will spawn at
            int fieldX;
            int fieldY;
            int fieldZ;

            //objects will spawn in the centre of the spawner
            if (spawnFromCentre == true)
            {
                fieldX = (int)transform.position.x;
                fieldY = (int)transform.position.y;
                fieldZ = (int)transform.position.z;
            }
            //objects spawn anywhere in the spawner's bounds
            else
            {
                fieldX = Random.Range(xMin, xMax);
                fieldY = Random.Range(yMax, yMin); //y is inverted because it works top down
                fieldZ = Random.Range(zMin, zMax);
            }

            newObject.transform.position = new Vector3(fieldX, fieldY, fieldZ);

            //chance of rescaling, based on user input
            int rescaleInt = Random.Range(0, 10);
            if (rescaleInt <= chanceOfRescalingObject)
            {
                float rescaleFloat = Random.Range(rescalingLowerBound, rescalingUpperBound);
                //make a vector of the rescale
                Vector3 rescale = new Vector3(rescaleFloat, rescaleFloat, rescaleFloat);
                //save the objects's scale
                Vector3 scaleStorage = newObject.transform.localScale;
                //scale the clone
                scaleStorage.Scale(rescale);
                //reapply the scale
                newObject.transform.localScale = scaleStorage;
            }

        }
    }

    //find the min and max vertex bounds
    private void sortVertexBounds()
    {
        //you shouldn't be building your level more than this far from your scene's origin anyways
        yMax = -999999999;
        yMin = 999999999;

        xMax = -999999999;
        xMin = 999999999;

        zMax = -999999999;
        zMin = 999999999;

        //sort
        for (int i = 0; i < worldSpaceVertices.Length; i++)
        {
            if (worldSpaceVertices[i].y > yMax) { yMax = (int)worldSpaceVertices[i].y; }
            if (worldSpaceVertices[i].y < yMin) { yMin = (int)worldSpaceVertices[i].y; }

            if (worldSpaceVertices[i].x > xMax) { xMax = (int)worldSpaceVertices[i].x; }
            if (worldSpaceVertices[i].x < xMin) { xMin = (int)worldSpaceVertices[i].x; }

            if (worldSpaceVertices[i].z > zMax) { zMax = (int)worldSpaceVertices[i].z; }
            if (worldSpaceVertices[i].z < zMin) { zMin = (int)worldSpaceVertices[i].z; }
        }
    }

    //use the right collider, and toggle it either way
    private void toggleColliders(GameObject pNewObject, bool pIncomingState)
    {
        for (int i = 0; i < typeOfCollider.Length; i++)
        {
            switch ((int)typeOfCollider[i])
            {
                case 0:
                    boxCollider = pNewObject.GetComponent<BoxCollider>();
                    boxCollider.enabled = pIncomingState;
                    break;
                case 1:
                    sphereCollider = pNewObject.GetComponent<SphereCollider>();
                    sphereCollider.enabled = pIncomingState;
                    break;
                case 2:
                    capsuleCollider = pNewObject.GetComponent<CapsuleCollider>();
                    capsuleCollider.enabled = pIncomingState;
                    break;
                case 3:
                    capsuleCollider = pNewObject.GetComponent<CapsuleCollider>();
                    capsuleCollider.enabled = pIncomingState;
                    break;
            }
        }
     
    }
}
