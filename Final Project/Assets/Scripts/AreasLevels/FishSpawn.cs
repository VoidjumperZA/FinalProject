using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawn : MonoBehaviour
{
    private basic _basic { get { return GetComponent<basic>(); } }

    [Header("Fish")]
    [SerializeField]
    private GameObject[] _fishPrefabs;
    [Header("Spawning Rates")]
    [SerializeField]
    private float higherSpawningRateLowerValue;
    [SerializeField]
    private float lowerSpawningRateHigherValue;
    [SerializeField]
    private int minimumPercentChangeInDensity;
    [SerializeField]
    private float timeBeforeSpawnFertilityDegrade;
    [Header("Misc Values")]
    [SerializeField]
    private int maximumNumberOfOnscreenFish;
    private float _timePassed;
    private bool _valid;
    [HideInInspector] public bool _boatSetUp = false;
    private float timeBetweenSpawns;
    private enum PossiblePolarities { Negative, Positive, Either, Niether }
    private PossiblePolarities possiblePolarities;
    private float timeCo;
    private int totalNumberOfSpawnedFish;

    [SerializeField] private ShoppingList _shoppingList;
    private Dictionary<int, List<int>> _fishInfo = new Dictionary<int, List<int>>();
    private List<int> _fishPerTypeSpawned = new List<int>() { 0, 0, 0 };

    public static List<fish> SpawnedFish = new List<fish>();
    [SerializeField] private Transform _leftSpawner;
    [SerializeField] private Transform _rightSpawner;
    [SerializeField] private Transform _leftDespawner;
    [SerializeField] private Transform _rightDespawner;

    private int _amountToSpawn = 0;
    private int _totalSpawned = 0;
    private int _typeToSpawn = 0;
    public void TakeShoppingListValues()
    {
        _fishInfo = _shoppingList.FishInfo;
        Initialize();
    }
    // Use this for initialization
    private void Initialize()
    {
        foreach (fish pFish in SpawnedFish) Destroy(pFish.gameObject);
        SpawnedFish.Clear();

        possiblePolarities = PossiblePolarities.Niether;
        //_basic = GetComponent<basic>();
        /*_verticalSpawnFluctuation = (-1.0f * (basic.GetSeaDepth() / 2));

        Vector3 leftSpawnPos = new Vector3(_leftSpawn.transform.position.x, basic.Boat.transform.position.y, _leftSpawn.transform.position.z);
        leftSpawnPos.y += (_verticalSpawnFluctuation);
        _leftSpawn.transform.position = leftSpawnPos;

        Debug.Log("Vertical Spawn Fluctuation: " + _verticalSpawnFluctuation);

        Vector3 rightSpawnPos = new Vector3(_rightSpawn.transform.position.x, basic.Boat.transform.position.y, _rightSpawn.transform.position.z);
        rightSpawnPos.y += (_verticalSpawnFluctuation);
        _rightSpawn.transform.position = rightSpawnPos;*/
        //Max our time to start
        _totalSpawned = 0;
        _amountToSpawn = 0;
        for (int i = 0; i < _shoppingList.AmountOfTypes; i++)
        {
            _amountToSpawn += _fishInfo[i][2];
        }


        timeBetweenSpawns = higherSpawningRateLowerValue;
        _timePassed = timeBetweenSpawns;

        totalNumberOfSpawnedFish = 0;

        //Is our game valid, if there is disparity between how many fish types we have and the levels of spawning
        //then mark that as invalid.
        _valid = true;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_valid == true)
        {
            //Always count down time
            _timePassed -= Time.deltaTime;
            //Once our spawn timer is up
            if (_timePassed <= 0)
            {
                CreateFish(Random.Range(0, 2));
                _timePassed = timeBetweenSpawns;
            }
            if (Input.GetKeyDown(KeyCode.A)) Debug.Log(SpawnedFish.Count);
        }
    }

    public void StartFertilityDegradeCoroutine()
    {
        StartCoroutine(ReduceFertilityOfSpawnArea());
    }
    public void QueueFishAgain(fish pFish, bool pQueueAgain, bool pRemoveFromList, bool pDestroyNow)
    {
        if (!pFish) return;

        int pType = (int)pFish.GetFishType();
        Debug.Log(_fishPerTypeSpawned[pType] + " Spawned");
        if (pQueueAgain && _fishPerTypeSpawned[pType] > 0)
        {
            _totalSpawned -= 1;
            _fishPerTypeSpawned[pType] -= 1;
            Debug.Log(_fishPerTypeSpawned[pType] + " After Requed");
        }
        if (pRemoveFromList) SpawnedFish.Remove(pFish);
        if (pDestroyNow && pFish.gameObject) Destroy(pFish.gameObject);
    }
    public void CollectFish(fish pFish)
    {
        SpawnedFish.Remove(pFish);
    }
    private void CreateFish(int pPolarity)
    {
        // Fish Info IDs
        int FISHTYPE = 0;
        int TOCOLLECT = 2;
        

        List<int> alreadyChosen = new List<int>();
        int rnd = -1;
        if (_totalSpawned < _amountToSpawn)
        {
            //Debug.Log(_totalSpawned + " / " + _amountToSpawn);
            bool again = true;
            while (again)
            {
                rnd = Random.Range(0, _shoppingList.AmountOfTypes);
                if (!alreadyChosen.Contains(rnd))
                {
                    int type = _fishInfo[rnd][FISHTYPE];
                    if (_fishPerTypeSpawned[type] < _fishInfo[rnd][TOCOLLECT])
                    {
                        _totalSpawned += 1;
                        _fishPerTypeSpawned[type] += 1;
                        Vector3 spawnPos = (pPolarity == 0) ? _leftSpawner.position : _rightSpawner.position;
                        spawnPos.y -= Random.Range(0, (pPolarity == 0) ? _rightDespawner.transform.lossyScale.y : _leftDespawner.transform.lossyScale.y);
                        GameObject newFish = Instantiate(_fishPrefabs[type], spawnPos, Quaternion.LookRotation((pPolarity == 0) ? Vector3.right : -Vector3.right));
                        again = false;
                        SpawnedFish.Add(newFish.GetComponent<fish>());
                    }
                    else alreadyChosen.Add(rnd);

                }
                else again = (_totalSpawned < _amountToSpawn) ? true : false;
            }
        }
        
    }

    /// <summary>
    /// Removes one from the total number of fish spawned, to allow space for another.
    /// </summary>
    public void RemoveOneFishFromTracked()
    {
        totalNumberOfSpawnedFish--;
    }
    public void CalculateNewSpawnDensity()
    {
        /*If I forget what the hell maths I was using here, here is a helpful image:
        http://imgur.com/a/C8Vn7
        We want to generate a new time between fish spawns. The time between spawns basically
        controls the density of the fish spawning. Because we are using floats, our new value could
        be so close there is no noticable difference in feel. To combat this we're making sure 
        we choose a new value that is at least x% larger or smaller than our old value.
        But first we need to make sure we can make a step that big in either direction*/

        float negativeDiff = timeBetweenSpawns - higherSpawningRateLowerValue;
        float positiveDiff = lowerSpawningRateHigherValue - timeBetweenSpawns;
        float totalRange = lowerSpawningRateHigherValue - higherSpawningRateLowerValue;
        float minPercentOfRange = minimumPercentChangeInDensity * (totalRange / 100);
        Debug.Log("Hit a spawning area. Current value: [" + timeBetweenSpawns + "]  |  totalRange: [" + totalRange + "]  |  posDiff: [" + positiveDiff + "]  |  negDiff: [" + negativeDiff + "]");

        //If the available range on the negative "axis" is greater than the min % of the total range
        if (negativeDiff > minPercentOfRange) // lowerspawningratehighervalue / 100
        {
            possiblePolarities = PossiblePolarities.Negative;
        }
        //If the available range on the positive "axis" is greater than the min % of the total range
        if (positiveDiff > minPercentOfRange)
        {
            possiblePolarities = PossiblePolarities.Positive;
        }
        //If the available range on both "axes" are greater than the min % of the total range
        if (positiveDiff > minPercentOfRange && negativeDiff > minPercentOfRange)
        {
            possiblePolarities = PossiblePolarities.Either;
        }
        Debug.Log("possiblePol: " + possiblePolarities.ToString());
        //
        switch (possiblePolarities)
        {
            case PossiblePolarities.Niether:
                Debug.Log("ERROR: No space to change spawn density to a difference either positive or negative. Incorrect maths applied.");
                break;
            case PossiblePolarities.Positive:
                calculatePositiveDifference(positiveDiff, minPercentOfRange);
                break;
            case PossiblePolarities.Negative:
                calculateNegativeDifference(negativeDiff, minPercentOfRange);
                break;
            case PossiblePolarities.Either:
                int roll = Random.Range(0, 2);
                if (roll == 0)
                {
                    calculatePositiveDifference(positiveDiff, minPercentOfRange);
                }
                else if (roll == 1)
                {
                    calculateNegativeDifference(negativeDiff, minPercentOfRange);
                }
                break;
        }
                timeCo = Time.time;
                Debug.Log("Calculated a new time of spawning which is: " + timeBetweenSpawns + "\nTime: " + timeCo);
                StartCoroutine(ReduceFertilityOfSpawnArea());
    }

    private void calculateNegativeDifference(float pDiff, float minPercentOfRange)
    {
        /*FROM the minimum value minus the x% step (because we only have a difference figure), TO our current minus the x% step
        and the end minus the step*/
        timeBetweenSpawns = Random.Range(pDiff - minPercentOfRange, timeBetweenSpawns - minPercentOfRange);
        Debug.Log("Calculated Negative Difference.\nNew density: " + timeBetweenSpawns);
    }

    private void calculatePositiveDifference(float pDiff, float minPercentOfRange)
    {
        /*FROM our current number, PLUS a step of x% TO the amount of space between current 
        and the end minus the step (because we only have a difference figure)*/
        timeBetweenSpawns = Random.Range(timeBetweenSpawns + minPercentOfRange, pDiff - minPercentOfRange);
        Debug.Log("Calculated Positive Difference.\nNew density: " + timeBetweenSpawns);
    }

    IEnumerator ReduceFertilityOfSpawnArea()
    {
        //wait for an amount of time then make this spawn area infertile
        Debug.Log("Counting  " + timeBeforeSpawnFertilityDegrade + "seconds before making this area overfished.");
        yield return new WaitForSeconds(timeBeforeSpawnFertilityDegrade);
        timeBetweenSpawns = lowerSpawningRateHigherValue;
        Debug.Log("TimeDiff: " + (timeCo - Time.time) + "This fishing area is now overfished, and has the minimal spawn rate (" + timeBetweenSpawns + ")");
    }
}
