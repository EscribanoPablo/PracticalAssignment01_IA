using UnityEngine;

namespace Steerings
{

    public class Interfere : SteeringBehaviour
    {
        public GameObject target;
        public float requireDistance;

        public override GameObject GetTarget()
        {
            return target;
        }
        
        public override Vector3 GetLinearAcceleration()
        {
            return Interfere.GetLinearAcceleration(Context, target, requireDistance);
        }
        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float distance)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
            Vector3 l_TargetForwardMovement = target.GetComponent<SteeringContext>().velocity.normalized * distance;
            Vector3 l_desiredPosition = target.transform.position + l_TargetForwardMovement;

            SURROGATE_TARGET.transform.position = l_desiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET); 
        }

    }
}