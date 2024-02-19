using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_TwoPointWandering", menuName = "Finite State Machines/FSM_TwoPointWandering", order = 1)]
public class FSM_TwoPointWandering : FiniteStateMachine
{
    

    private WanderAround wanderAround;
    private SteeringContext steeringContext;
    private ANT_Blackboard blackboard;
    private Arrive arrive;

    private float elapsedTime = 0;


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is executed every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        /* COMPLETE */
        blackboard = GetComponent<ANT_Blackboard>();
        wanderAround = GetComponent<WanderAround>();
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

        /* COMPLETE */
        base.DisableAllSteerings();
        steeringContext.seekWeight = blackboard.initialSeekWeight; 
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         */

        FiniteStateMachine twoPointWandering = ScriptableObject.CreateInstance<FSM_TwoPointWandering>(); 

        State goingA = new State("Going_A",
           () => { /* COMPLETE */
               wanderAround.attractor = blackboard.pointA;
               wanderAround.enabled = true;
               elapsedTime = 0;
           },
           () => { elapsedTime += Time.deltaTime;}, 
           () => {/* COMPLETE */
               wanderAround.enabled = false;
           }
       );

        State goingB = new State("Going_B",
           () => {/* COMPLETE */
               wanderAround.attractor = blackboard.pointB;
               wanderAround.enabled = true; 
               elapsedTime = 0;
           },
           () => { elapsedTime += Time.deltaTime; },
           () => { /* COMPLETE */
               wanderAround.enabled = false; 
           }
       );
        //////////////////////////////////////////////////////////////////////////////
        


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------
        */

        /*
        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        */

        /* COMPLETE, create the transitions */
        Transition locationAreached = new Transition("Location A reached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.pointA) < blackboard.locationReachedRadius;
            },
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }
            );

        Transition locationBreached = new Transition("Location B reached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.pointB) < blackboard.locationReachedRadius;
            },
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }
            );

        Transition TimeOut = new Transition("TimeOut",
            () => {
                return elapsedTime > blackboard.timeBetweenConsecutiveTimeOuts;
            },
            () => {
                float sk = Mathf.Min(1, steeringContext.seekWeight + blackboard.incrementOfSeek);
                steeringContext.seekWeight = sk; 
                //steeringContext.seekWeight += blackboard.incrementOfSeek;
                elapsedTime = 0; 
            }
            );
        //////////////////////////////////////////////////////////////////////////
        

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
         */

        AddStates(goingA, goingB);

        /* COMPLETE, add the transitions */

        AddTransition(goingA, locationAreached, goingB);
        AddTransition(goingB, locationBreached, goingA);
        AddTransition(goingA, TimeOut, goingA);
        AddTransition(goingB, TimeOut, goingB);

        



        /* STAGE 4: set the initial state */

        initialState = goingA;
    }
}
