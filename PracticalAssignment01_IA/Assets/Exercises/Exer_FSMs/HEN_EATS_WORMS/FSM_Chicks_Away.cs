using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Chicks_Away", menuName = "Finite State Machines/FSM_Chicks_Away", order = 1)]
public class FSM_Chicks_Away : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private Arrive arrive;
    private HEN_Blackboard blackboard;
    private GameObject chick;
    private SteeringContext steeringContext;
    private AudioSource audioSource; 


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<HEN_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();
        audioSource = GetComponent<AudioSource>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        if (currentState.Name.Equals("ANGRY"))
        {
            currentState.OnExit(); 
        }
        
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        FiniteStateMachine searchWorms = ScriptableObject.CreateInstance<FSM_SearchWorms>();

        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */


        State Chick_Away = new State("ANGRY",
            () => {
                arrive.target = chick;
                arrive.enabled = true;
                steeringContext.maxSpeed *= 2;
                gameObject.transform.localScale *= 1.4f;
                steeringContext.maxAcceleration *= 2;
                audioSource.clip = blackboard.angrySound;
                audioSource.Play();
            },
            () => {
            },
            () => {
                gameObject.transform.localScale /= 1.4f;
                steeringContext.maxSpeed /= 2;
                steeringContext.maxAcceleration /= 2;
                arrive.enabled = false;
            }
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        Transition chickTooClose_Transition = new Transition("Chick Too Close",
            () => {
                chick = SensingUtils.FindInstanceWithinRadius(gameObject, "CHICK", blackboard.chickDetectionRadius);
                return chick != null;
            },
            () => { }
        );

        Transition chickFarEnough_Transition = new Transition("Chick Far Enough",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, chick) >= blackboard.chickFarEnoughRadius;
            },
            () => { }
        );
        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(Chick_Away, searchWorms);
        AddTransition(searchWorms, chickTooClose_Transition, Chick_Away);
        AddTransition(Chick_Away, chickFarEnough_Transition, searchWorms);


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = searchWorms;

    }
}
