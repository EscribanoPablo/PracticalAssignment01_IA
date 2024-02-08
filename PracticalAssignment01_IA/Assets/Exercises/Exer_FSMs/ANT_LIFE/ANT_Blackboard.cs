
using UnityEngine;

public class ANT_Blackboard : MonoBehaviour
{
    //[Header("Two point wandering")]
    public GameObject pointA;
    public GameObject pointB;

    public float timeBetweenConsecutiveTimeOuts;
    public float initialSeekWeight;
    public float incrementOfSeek;
    public float locationReachedRadius;

    //[Header("Seed colecting")]

    public GameObject nest;
    public float seedDetectionRadius;
    public float seedReachedRadius;
    public float nestReachedRadius;

    //[Header("Peril Fleeing")]


    void Start()
    {
       
    }

   
}
