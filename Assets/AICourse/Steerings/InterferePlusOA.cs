using UnityEngine;

namespace Steerings
{

    public class InterferePlusOA : SteeringBehaviour
    {
        public GameObject target;
        public float requiredDistance;
        
        public override GameObject GetTarget()
        {
            return target;
        }
        
        
        public override Vector3 GetLinearAcceleration()
        {
            return InterferePlusOA.GetLinearAcceleration(Context, target, requiredDistance);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
            Vector3 avoidanceAcceleration = ObstacleAvoidance.GetLinearAcceleration(me);

            if (avoidanceAcceleration.Equals(Vector3.zero))
            {
                Debug.Log("interfere");
                return Interfere.GetLinearAcceleration(me, target, requiredDistance); 
            }
            else
            {
                return avoidanceAcceleration;
            }
        }

    }
}