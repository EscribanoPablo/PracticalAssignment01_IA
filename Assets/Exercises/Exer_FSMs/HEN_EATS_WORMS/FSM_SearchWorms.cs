using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_SearchWorms", menuName = "Finite State Machines/FSM_SearchWorms", order = 1)]
public class FSM_SearchWorms : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private HEN_Blackboard blackboard;
    private WanderAround wanderAround;
    private Arrive arrive;
    private AudioSource audioSource;
    private GameObject theWorm;
    private float elapsedTime;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        /* COMPLETE */
        blackboard = GetComponent<HEN_Blackboard>();
        wanderAround = GetComponent<WanderAround>();
        arrive = GetComponent<Arrive>();
        audioSource = GetComponent<AudioSource>();
        theWorm = blackboard.attractor; 


        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        /* COMPLETE */
        audioSource.Stop(); 
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* COMPLETE */

        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

        State wander_State = new State("Wander State", 
            () => { 
                wanderAround.enabled = true;
                audioSource.clip = blackboard.angrySound;
                audioSource.Play();

            }, 
            () => {
            },
            () => {
                wanderAround.enabled = false;
                audioSource.Stop();

            }
        );

        State reachWorm_State = new State("Reach Worm",
           () => {
               arrive.enabled = true;
               arrive.target = theWorm;

           },
           () => { },
           () => {
               arrive.enabled = false;
           }
       );

        State eat_State = new State("Eat",
           () => {
               elapsedTime = 0;
               audioSource.clip = blackboard.eatingSound;
               audioSource.Play(); 
           },
           () => {
               elapsedTime += Time.deltaTime;
           },
           () => {
               GameObject.Destroy(theWorm);
               audioSource.Stop();
           }
       );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition wormDetected_Transition = new Transition("Worm Detected",
            () => { 
                //return SensingUtils.DistanceToTarget(gameObject, theWorm) < blackboard.wormDetectableRadius;
                theWorm = SensingUtils.FindInstanceWithinRadius(gameObject, "WORM", blackboard.wormDetectableRadius);
                return theWorm != null;
            }, 
            () => { } 
        );

        Transition wormReached_Transition = new Transition("Worm Reached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theWorm) < blackboard.wormReachedRadius;
            },
            () => { }
        );

        Transition wormVanished_Transition = new Transition("Worm Vanished",
            () => {
                return theWorm == null || theWorm.Equals(null);// hay que comprobarlo en este orden "si esta destruido || se esta destruyendo"; 
            },
            () => { }
        );

        Transition timeOut_Transition = new Transition("Time Out",
            () => {
                return elapsedTime > blackboard.timeToEatWorm;
            },
            () => { }
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(wander_State, reachWorm_State, eat_State);

        AddTransition(wander_State, wormDetected_Transition, reachWorm_State);
        AddTransition(reachWorm_State, wormReached_Transition, eat_State);
        AddTransition(eat_State, timeOut_Transition, wander_State);
        AddTransition(eat_State, wormVanished_Transition, wander_State);



        /* STAGE 4: set the initial state
         
        initialState = ... 
        

         */
        initialState = wander_State;
    }
}
