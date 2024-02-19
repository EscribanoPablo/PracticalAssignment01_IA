using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "Scare_a_Nerd", menuName = "Finite State Machines/Scare_a_Nerd", order = 1)]
public class Scare_a_Nerd : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private GHOST_Blackboard blackboard;
    private Arrive arrive;
    private SteeringContext steeringContext;
    private float elapsedTime;
    private GameObject nerdTarget;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<GHOST_Blackboard>();
        arrive = GetComponent<Arrive>();
        steeringContext = GetComponent<SteeringContext>();


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
        State goCastle_State = new State("Go Castle",
            () => {
                arrive.target = blackboard.castle;
                arrive.enabled = true;
                steeringContext.maxSpeed *= 4;
            }, 
            () => { }, 
            () => { arrive.enabled = false; arrive.target = null; steeringContext.maxSpeed /= 4; }   
        );

        State hide_State = new State("Hide",
            () => {
                elapsedTime = 0;
            },
            () => { elapsedTime += Time.deltaTime; },
            () => { }
        );

        State selectTarget_State = new State("Select Target",
            () => {
                //nerdTarget = SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackboard.victimLabel, blackboard.nerdDetectionRadius); 
            },
            () => { },
            () => { }
        );

        State aproach_State = new State("Aproach",
            () => {
                arrive.target = nerdTarget;
                arrive.enabled = true;
            },
            () => { },
            () => { arrive.enabled = false; }
        );

        State CryBoo_State = new State("Cry Boo",
            () => {
                arrive.target = nerdTarget;
                arrive.enabled = true;
                blackboard.CryBoo(true);
                elapsedTime = 0;
                //pursue.enabled = true;
                //pursue.target = nerdTarget;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => { 
                arrive.enabled = false;
                blackboard.CryBoo(false);
            }
        );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        Transition CastleReached_Tarnsition = new Transition("Castle Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.castle) < blackboard.castleReachedRadius; }, 
            () => { }  
        );

        Transition TimeOutHide_Tarnsition = new Transition("Time Out Hide",
            () => { return elapsedTime > blackboard.hideTime; },
            () => { }
        );

        Transition nerdDetected_Tarnsition = new Transition("Nerd Detected",
            () => {
                nerdTarget = SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackboard.victimLabel, blackboard.nerdDetectionRadius);
                //return nerdTarget < blackboard.nerdDetectionRadius; 
                return nerdTarget != null;
            },
            () => { }
        );

        Transition nerdIsClose_Tarnsition = new Transition("Nerd is Close",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, nerdTarget) < blackboard.closeEnoughToScare; 
            },
            () => { }
        );

        Transition TimeOutBoo_Tarnsition = new Transition("Time Out Boo",
            () => { return elapsedTime > blackboard.booDuration; },
            () => { }
        );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(goCastle_State, hide_State, selectTarget_State, aproach_State, CryBoo_State);

        AddTransition(goCastle_State, CastleReached_Tarnsition, hide_State);
        AddTransition(hide_State, TimeOutHide_Tarnsition, selectTarget_State);
        AddTransition(selectTarget_State, nerdDetected_Tarnsition, aproach_State);
        AddTransition(aproach_State, nerdIsClose_Tarnsition, CryBoo_State);
        AddTransition(CryBoo_State, TimeOutBoo_Tarnsition, goCastle_State);
        /* STAGE 4: set the initial state
         
        initialState = ... 
         */
        initialState = goCastle_State; 

    }
}
