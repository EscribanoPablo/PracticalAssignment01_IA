using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;


public class Fish_Blackboard : MonoBehaviour
{
    public float foodDetectableRadius = 50;
    public float fishHookDetectableRadius = 70;
    public float sharkDetectableRadius = 70;
    public float foodEnoughRadius = 5; 

    public float distanceBetweenPartners = 40;
    public float cohesionBetweenPartners = 50;

    public float timeToEat = 3;

    public GameObject theShark;
    public string foodTag = "FISHFOOD";

    SteeringContext fish;

    private void Awake()
    {
        SetCohesionAndDistacnePartners();
    }

    private void SetCohesionAndDistacnePartners()
    {
        fish = GetComponent<SteeringContext>();

        fish.repulsionThreshold = distanceBetweenPartners;
        fish.cohesionThreshold = cohesionBetweenPartners;
    }


}
