using UnityEngine;

namespace Steerings
{

    public class Interpose : SteeringBehaviour
    {
        public GameObject m_TargetA;
        public GameObject m_TargetB;

        /*
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;

        */
        public override GameObject GetTarget()
        {
            return m_TargetA;
        }


        public override Vector3 GetLinearAcceleration()
        {
            return Interpose.GetLinearAcceleration(Context, m_TargetA, m_TargetB);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject targetA, GameObject targetB)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
            Vector3 l_VectorAB = targetB.transform.position - targetA.transform.position;
            Vector3 l_DesiredPointPosition = targetA.transform.position + l_VectorAB / 2;

            SURROGATE_TARGET.transform.position = l_DesiredPointPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET); 

        }

    }
}