using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFishSpawn : MonoBehaviour
{
    private basic _basic;

    [Header("Fish")]
    [SerializeField]
    private GameObject[] _fishPrefabs;
    [Header("Spawns")]
    [SerializeField]
    private float higherSpawningRateLowerValue;
    [SerializeField]
    private float lowerSpawningRateHigherValue;
    [SerializeField]
    private int minimumPercentChangeInDensity;
    [SerializeField]
    private float timeBeforeSpawnFertilityDegrade;
    [SerializeField]
    private float bufferSpaceUnderBoat;
    [SerializeField]
    private float _spawnWidth;
    [SerializeField]
    private Transform _leftSpawn;
    [SerializeField]
    private Transform _rightSpawn;
    private float _verticalSpawnFluctuation;
    private float _timePassed;
    private bool _valid;
    private float timeBetweenSpawns;
    private enum PossiblePolarities { Negative, Positive, Either, Niether }
    private PossiblePolarities possiblePolarities;
    private float timeCo;

    // Use this for initialization
    void Start()
    {
        possiblePolarities = PossiblePolarities.Niether;
        _basic = GetComponent<basic>();
        _verticalSpawnFluctuation = (_basic.GetSeaDepth() / 2);
        Vector3 leftSpawnPos = new Vector3(_leftSpawn.transform.position.x, GameObject.FindGameObjectWithTag("Boat").transform.position.y, _leftSpawn.transform.position.z);
        leftSpawnPos.y -= (_verticalSpawnFluctuation);
        _leftSpawn.transform.position = leftSpawnPos;

        Vector3 rightSpawnPos = new Vector3(_rightSpawn.transform.position.x, GameObject.FindGameObjectWithTag("Boat").transform.position.y, _rightSpawn.transform.position.z);
        rightSpawnPos.y -= (_verticalSpawnFluctuation);
        _rightSpawn.transform.position = rightSpawnPos;
        //Max our time to start
        timeBetweenSpawns = higherSpawningRateLowerValue;
        _timePassed = timeBetweenSpawns;
        _spawnWidth /= 2;

        //Is our game valid, if there is disparity between how many fish types we have and the levels of spawning
        //then mark that as invalid.
        _valid = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_valid == true)
        {
            MoveSpawnArea(basic.Boat.transform.position);
            //Always count down time
            _timePassed -= Time.deltaTime;
            //Once our spawn timer is up
            if (_timePassed <= 0)
            {
                _basic.AddFish(CreateFish(Random.Range(0, 2)));
                _timePassed = timeBetweenSpawns;
            }
        }
    }
    private fish CreateFish(int pPolarity)
    {
        //Choose a random fish and direction
        int randomFish = Random.Range(0, _fishPrefabs.Length);
        float verticalOffset = Random.Range(-_verticalSpawnFluctuation, _verticalSpawnFluctuation - bufferSpaceUnderBoat);

        GameObject newFish = Instantiate(_fishPrefabs[randomFish],
                                        (pPolarity == 0) ? _leftSpawn.position + new Vector3(0, verticalOffset, 0) : _rightSpawn.position + new Vector3(0, verticalOffset, 0),
                                        (pPolarity == 0) ? _leftSpawn.rotation : _rightSpawn.rotation);
        newFish.GetComponent<fish>().SetDirection(1.0f);
        newFish.GetComponent<fish>().SetFishType(randomFish);
        return newFish.GetComponent<fish>();
    }


    private void MoveSpawnArea(Vector3 pBoatPosition)
    {
        Vector3 differenceVector = pBoatPosition - new Vector3(0, 0.5f, 0);
        if (differenceVector.magnitude > 0)
        { 
                _leftSpawn.position = new Vector3(pBoatPosition.x - _spawnWidth, _leftSpawn.position.y, _leftSpawn.position.z);
                _rightSpawn.position = new Vector3(pBoatPosition.x + _spawnWidth, _rightSpawn.position.y, _rightSpawn.position.z);           
        }
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
        Debug.Log("Hit a spawning area. Current value: [" + timeBetweenSpawns + "]  |  totalRange: [" + totalRange + "]  |  posDiff: [" + positiveDiff + "]  |  negDiff: [" + negativeDiff + "]");

        //If the available range on the negative "axis" is greater than the min % of the total range
        if (negativeDiff > minimumPercentChangeInDensity * (lowerSpawningRateHigherValue / 100))
        {
            possiblePolarities = PossiblePolarities.Negative;
        }
        //If the available range on the positive "axis" is greater than the min % of the total range
        if (positiveDiff > minimumPercentChangeInDensity * (lowerSpawningRateHigherValue / 100))
        {
            possiblePolarities = PossiblePolarities.Positive;
        }
        //If the available range on both "axes" are greater than the min % of the total range
        if (positiveDiff > minimumPercentChangeInDensity * (lowerSpawningRateHigherValue / 100) && negativeDiff > minimumPercentChangeInDensity * (lowerSpawningRateHigherValue / 100))
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
                calculatePositiveDifference(positiveDiff);
                break;
            case PossiblePolarities.Negative:
                calculateNegativeDifference(negativeDiff);
                break;
            case PossiblePolarities.Either:
                int roll = Random.Range(0, 2);
                if (roll == 0)
                {
                    calculatePositiveDifference(positiveDiff);
                }
                else if (roll == 1)
                {
                    calculateNegativeDifference(negativeDiff);
                }
                break;
                timeCo = Time.time;
                Debug.Log("Calculated a new time of spawning which is: " + timeBetweenSpawns + "\nTime: " + timeCo);
                StartCoroutine(ReduceFertilityOfSpawnArea());
        }
    }

    private void calculateNegativeDifference(float pDiff)
    {
        /*FROM the minimum value minus the x% step (because we only have a difference figure), TO our current minus the x% step
        and the end minus the step*/
        timeBetweenSpawns = Random.Range(pDiff - minimumPercentChangeInDensity, timeBetweenSpawns - minimumPercentChangeInDensity);
        Debug.Log("Calculated Negative Difference.");
    }

    private void calculatePositiveDifference(float pDiff)
    {
        /*FROM our current number, PLUS a step of x% TO the amount of space between current 
        and the end minus the step (because we only have a difference figure)*/
        timeBetweenSpawns = Random.Range(timeBetweenSpawns + minimumPercentChangeInDensity, pDiff - minimumPercentChangeInDensity);
        Debug.Log("Calculated Positive Difference.");
    }

    IEnumerator ReduceFertilityOfSpawnArea()
    {
        //wait for an amount of time then make this spawn area infertile
        Debug.Log("Couting  " + timeBeforeSpawnFertilityDegrade + "seconds before making this area overfished.");
        yield return new WaitForSeconds(timeBeforeSpawnFertilityDegrade);
        timeBetweenSpawns = higherSpawningRateLowerValue;
        Debug.Log("TimeDiff: " + (timeCo - Time.time));
    }
}
