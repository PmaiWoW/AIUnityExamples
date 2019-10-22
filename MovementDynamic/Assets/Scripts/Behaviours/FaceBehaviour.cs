/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *
 * Author: Nuno Fachada
 * */
using UnityEngine;

public class FaceBehaviour : AlignBehaviour
{

    // Face behaviour
    public override SteeringOutput GetSteering(GameObject target)
    {
        // Initialize linear and angular forces to zero
        SteeringOutput sout = new SteeringOutput(Vector2.zero, 0);

        // Do I have a target?
        if (target != null)
        {
            // Determine the direction to our target
            Vector2 direction =
                target.transform.position - transform.position;

            // Continue only if there is a distance between us and the target
            if (direction.magnitude > 0)
            {
                // Determine the orientation for our temporary target, which
                // should be as if it was looking away from me
                float angle = Vec2Deg(direction);

                // Create our temporary target
                GameObject tempTarget = CreateTarget(Vector3.zero, angle);

                // Use align superclass to determine the torque to return based
                // on the temp. target orientation (i.e. looking away from me)
                sout = base.GetSteering(tempTarget);

                // Destroy the temporary copy of our target
                Destroy(tempTarget);
            }
        }
        // Output the steering
        return sout;
    }

}
