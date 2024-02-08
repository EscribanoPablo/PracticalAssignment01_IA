using UnityEngine;

namespace Steerings
{

    public class KeepPosition : SteeringBehaviour
    {

        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        /* COMPLETE */ 

        

        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            return KeepPosition.GetLinearAcceleration(Context, target, requiredDistance, requiredAngle);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target,
                                                     float distance, float angle)
        {
            /* COMPLETE */
            //Vector3 l_DirectionToTarget = (target.transform.position - me.transform.position).normalized;
            float l_DesiredAngle = target.transform.rotation.eulerAngles.z + angle; 
                
            Vector3 l_directionFromTarget = Utils.OrientationToVector(l_DesiredAngle);
            Vector3 l_displaçement = l_directionFromTarget * distance;
            Vector3 l_DesiredPosition = target.transform.position + l_displaçement;

            

            SURROGATE_TARGET.transform.position = l_DesiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        }

    }
}