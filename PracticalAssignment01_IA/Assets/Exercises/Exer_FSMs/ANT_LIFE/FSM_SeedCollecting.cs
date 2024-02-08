using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_SeedCollecting", menuName = "Finite State Machines/FSM_SeedCollecting", order = 1)]
public class FSM_SeedCollecting : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private SteeringContext steeringContext;
    private ANT_Blackboard blackboard;
    private Arrive arrive;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<ANT_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();
        arrive = GetComponent<Arrive>();

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
        FiniteStateMachine twoPointWandering = ScriptableObject.CreateInstance<FSM_TwoPointWandering>();

        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */
        State goingToSeed = new State("Going To Seed",
           () => {/* COMPLETE */
               //arrive.target = theSeed
               arrive.target = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "SEED", blackboard.seedDetectionRadius);
               arrive.enabled = true;
           },
           () => { },
           () => { /* COMPLETE */
               arrive.enabled = false;
           }
       );

        State transportingSeedToNest = new State("Transporting Seed",
           () => {/* COMPLETE */
               arrive.target.transform.SetParent(gameObject.transform);
               arrive.enabled = true;
           },
           () => { },
           () => { /* COMPLETE */
               arrive.enabled = false;
               arrive.target.transform.SetParent(null);
               //theSeed.tag = "NO_SEED";
           }
       );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        Transition NearbySeedDetected = new Transition("Nearby Seed detected",
           () => {
               return arrive.target != null;
           },
           () => {

           }
           );

        Transition SeedReached = new Transition("Seed Reached",
            () => {
                return SensingUtils.FindRandomInstanceWithinRadius(gameObject, "SEED", blackboard.seedReachedRadius);
            },
            () => {

            }
            );

        Transition NestReached = new Transition("Nest Reached",
           () => {
               return SensingUtils.DistanceToTarget(gameObject, blackboard.nest) < blackboard.nestReachedRadius;
           },
           () => {

           }
           );

        Transition seedTakenByOther = new Transition("Nest Reached",
           () => {
               return true;//theSeed.tag != "SEED";
           },
           () => {

           }
           );
        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(goingToSeed, transportingSeedToNest);
        AddTransition(twoPointWandering, NearbySeedDetected, goingToSeed);
        AddTransition(goingToSeed, SeedReached, transportingSeedToNest);
        AddTransition(transportingSeedToNest, NestReached, twoPointWandering);
        AddTransition(goingToSeed, seedTakenByOther, twoPointWandering);    

        /* STAGE 4: set the initial state
         
        initialState = ... 
         */

        initialState = twoPointWandering; 

    }
}
