using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_FishLookingForFood", menuName = "Finite State Machines/FSM_FishLookingForFood", order = 1)]
public class FSM_FishLookingForFood : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    Fish_Blackboard blackboard;
    Arrive arrive;
    FlockingPlusAvoidance flocking;
    GameObject food;
    float elapsedTime;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<Fish_Blackboard>();
        arrive = GetComponent<Arrive>();
        flocking = GetComponent<FlockingPlusAvoidance>(); 

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

        State searchingForFood = new State("Searching For Food",
            () => {
                flocking.rotationalPolicy = SteeringBehaviour.RotationalPolicy.LWYGI;
                flocking.enabled = true;
            },
            () => { },
            () => { flocking.enabled = false; }
        );

        State reachFood = new State("Reach Food",
            () => {
                arrive.target = food;
                arrive.enabled = true;
            },
            () => { },
            () => {
                arrive.target = null; 
                arrive.enabled = false;  
            }
        );

        State eatingFood = new State("Eating Food",
            () => {
                elapsedTime = 0; 
            },
            () => { elapsedTime += Time.deltaTime;  },
            () => {
                //food.SetActive(false); 
                Destroy(food);
            }
        );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition foodDetected = new Transition("Food Detected",
            () => {
                food = SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackboard.foodTag, blackboard.foodDetectableRadius);
                return food != null; 
            }, 
            () => { }  
        );

        Transition foodEaten = new Transition("Food Eaten",
            () => {
                return food == null || food.Equals(null) || arrive.target == null;
            },
            () => { }
        );

        Transition foodReached = new Transition("Food Reached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, food) <= blackboard.foodEnoughRadius; 
            },
            () => { }
        );

        Transition timeOut = new Transition("Time Out",
            () => { return elapsedTime > blackboard.timeToEat; },
            () => { }
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(searchingForFood, reachFood, eatingFood);

        AddTransition(searchingForFood, foodDetected, reachFood);
        AddTransition(reachFood, foodReached, eatingFood);
        AddTransition(reachFood, foodEaten, searchingForFood); 
        AddTransition(eatingFood, timeOut, searchingForFood);
        AddTransition(eatingFood, foodEaten, searchingForFood);


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = searchingForFood; 

    }
}
