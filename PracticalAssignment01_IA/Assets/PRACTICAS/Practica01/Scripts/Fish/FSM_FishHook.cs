using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_FishHook", menuName = "Finite State Machines/FSM_FishHook", order = 1)]
public class FSM_FishHook : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private Arrive arrive;
    private GameObject fishHook;
    private Fish_Blackboard blackboard;
    private SteeringContext steeringContext;
    private float elapsedTime;
    private PlayerController player;

    public bool CanFish => canFish;
    private bool canFish;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<Fish_Blackboard>();
        canFish = false;
        player = FindAnyObjectByType<PlayerController>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        if (currentState.Name.Equals("Reach Fish Hook") || currentState.Name.Equals("Eating FishHook's worm"))
        {
            currentState.OnExit();
        }

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
        FiniteStateMachine lookingForFood = ScriptableObject.CreateInstance<FSM_FishLookingForFood>();


        State reachFishHook = new State("Reach Fish Hook",
            () => {
                arrive.target = fishHook;
                arrive.enabled = true;
            }, 
            () => { }, 
            () => {
                arrive.target = null;
                arrive.enabled = false;
            }  
        );

        State eat = new State("Eating FishHook's worm",
            () => {
                canFish = true;
                elapsedTime = 0;
            },
            () => { elapsedTime += Time.deltaTime; },
            () => {
                fishHook.SetActive(false);
            }
        );

        State die = new State("Die",
            () => {
                canFish = false;
                Destroy(this.gameObject); },
            () => {  },
            () => {  }
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition fishHookDetected = new Transition("Fish Hook Detected",
            () => {
                fishHook = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.fishHookTag, blackboard.fishHookDetectableRadius);
                return fishHook != null;
            }, 
            () => { } 
        );

        Transition fishHookReached = new Transition("Fish Hook Reached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, fishHook) <= blackboard.fishHookEnoughRadius; 
            },
            () => { fishHook.tag = "FISHHOOKSELECTED"; }
        );

        Transition timeOut = new Transition("Fish Eats FishHook's worm",
            () => {
                return elapsedTime >= blackboard.timeToEat; 
            },
            () => { canFish = false; }
        );

        Transition fishHookTagChanged = new Transition("Fish Hook Tag Changed",
            () => {
                return fishHook.tag != blackboard.fishHookTag; 
            },
            () => { }
        );

        Transition fishFished = new Transition("Fish Fished",
            () => {
                return player.FishFished;
            },
            () => { }
        );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(lookingForFood, reachFishHook, eat, die);

        AddTransition(lookingForFood, fishHookDetected, reachFishHook);
        AddTransition(reachFishHook, fishHookTagChanged, lookingForFood);
        AddTransition(reachFishHook, fishHookReached, eat);
        AddTransition(eat, timeOut, lookingForFood);
        AddTransition(eat, fishFished, die);


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

        initialState = lookingForFood;

    }
}
