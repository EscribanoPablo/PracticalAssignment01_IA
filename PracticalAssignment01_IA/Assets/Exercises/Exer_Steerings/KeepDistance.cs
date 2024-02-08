using UnityEngine;

namespace Steerings
{

    public class KeepDistance : SteeringBehaviour
    {
   
        public GameObject target;
        public float requiredDistance;

        public override GameObject GetTarget()
        {
            return target;
        }
     
        
        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            //return Vector3.zero; // remove this line when exercise completed
            return KeepDistance.GetLinearAcceleration(Context, target, requiredDistance);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance)
        {

            /* COMPLETE */
            Vector3 l_directionFromTarget = me.transform.position - target.transform.position;
            Vector3 l_displacementFromTarget = l_directionFromTarget.normalized * requiredDistance;
            Vector3 l_desiredPosition = target.transform.position + l_displacementFromTarget;

            SURROGATE_TARGET.transform.position = l_desiredPosition;

            // return Seek.GetLinearAcceleration(me, SURROGATE_TARGET);
            // In the agent's SteeringContext, parameters for arrive should be set to  1, 20, 0.1f
            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        
        }

    }
}