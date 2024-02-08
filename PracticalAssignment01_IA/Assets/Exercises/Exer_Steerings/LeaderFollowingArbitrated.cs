using UnityEngine;

// Leader following combines Keep position with linear repulsion
// (linear respulsion prevents the agent from colliding against the leader 
// and against other agents following the same leader)

namespace Steerings
{

    public class LeaderFollowingArbitrated : SteeringBehaviour
    {

        
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        public override GameObject GetTarget()
        {
            return target;
        }
      
        
        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            return LeaderFollowingArbitrated.GetLinearAcceleration(Context, target, requiredDistance, requiredAngle); 
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float distance, float angle)
        {
            // Give priority to linear repulsion
            // (if linear repulsion is not Vector3.Zero return linear repulsion
            // else return Keep Position)
            /* COMPLETE */
            Vector3 linear_Repulsion = LinearRepulsion.GetLinearAcceleration(me);
            Vector3 keep_Position = KeepPosition.GetLinearAcceleration(me, target, distance, angle);

            if (!linear_Repulsion.Equals(Vector3.zero))
            {
                return linear_Repulsion; 
            }
            else
            {
                return keep_Position;
            }
        }

    }
}