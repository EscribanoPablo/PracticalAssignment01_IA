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

    public float distanceBetweenPartners;
    public float cohesionBetweenPartners;
    float repulsion;
    float cohesion;

    public float timeToEat = 3;

    public GameObject theShark;
    public string foodString = "FISHFOOD";

    SteeringContext fish;

    private void Awake()
    {
        repulsion = fish.GetComponent<SteeringContext>().repulsionThreshold;
        cohesion = fish.GetComponent<SteeringContext>().cohesionThreshold;
    }

    void SetCohesionAndDistacnePartners()
    {
        repulsion = distanceBetweenPartners;
        cohesion = cohesionBetweenPartners;
    }

}
