using UnityEngine;

namespace Steerings
{

    public class LeaderFollowingBlended : SteeringBehaviour
    {
        
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        public float wlr = 0.5f;

        public override GameObject GetTarget()
        {
            return target;
        }
      
        
        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            return LeaderFollowingBlended.GetLinearAcceleration(Context, target, requiredDistance, requiredAngle, wlr);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float distance, float angle, float wlr)
        {
            /*
             Compute both steerings
                lr = LinearRepulsion.GetLinearAcceleration(...)
                kp = KeepPosition...
             - if lr is zero return kp
             - else return the blending of lr and kp
             */
            /* COMPLETE */
            Vector3 linear_Repulsion = LinearRepulsion.GetLinearAcceleration(me);
            Vector3 keep_Position = KeepPosition.GetLinearAcceleration(me, target, distance, angle);

            if (linear_Repulsion == Vector3.zero)
            {
                Debug.Log("KEEP POSITION");
                return keep_Position;
            }
            else
            {
                //return linear_Repulsion;
                Debug.Log("REPULSION");
                return linear_Repulsion * wlr + (1 - wlr) * keep_Position; 
            }


        }
    }
}