﻿/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *
 * Author: Nuno Fachada 
 * */
using UnityEngine;

public class CollisionAvoidanceBehaviour : SteeringBehaviour
{

    // Collision radius
    public float radius;

    // Collision avoidance behaviour
    public override SteeringOutput GetSteering(GameObject target)
    {
        // Initialize linear and angular forces to zero
        Vector2 linear = Vector2.zero;
        float angular = 0f;

        // 1. Find the target that's closest to collision

        // Get all possible targets
        DynamicAgent[] targets = FindObjectsOfType<DynamicAgent>();

        // First collision time
        float shortestTime = float.PositiveInfinity;

        // Store the target that collidens and auxiliar data
        DynamicAgent firstTarget = null;
        float firstMinSep = 0, firstDistance = 0;
        Vector2 firstRelPos = Vector2.zero, firstRelVel = Vector2.zero;

        // Loop through each target
        foreach (DynamicAgent currTarget in targets)
        {
            // Calculate the time to collision
            Vector2 relPos =
                transform.position - currTarget.transform.position;
            Vector2 relVel = currTarget.Rb.velocity - rb.velocity;
            float relSpeed = relVel.magnitude;
            float timeToCollision =
                Vector2.Dot(relPos, relVel) / (relSpeed * relSpeed);

            // Check if it is goind to be a collision at all
            float distance = relPos.magnitude;
            float minSep = distance - relSpeed * timeToCollision;
            if (minSep > 2 * radius) continue;

            // Check if it is the shortest
            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                // Store the time, target and other data
                shortestTime = timeToCollision;
                firstTarget = currTarget;
                firstMinSep = minSep;
                firstDistance = distance;
                firstRelPos = relPos;
                firstRelVel = relVel;
            }
        }

        // 2. Calculate the steering

        // Was a target found?
        if (firstTarget != null)
        {
            Vector2 relPos;

            // If we're goind to hit exactly or if we're already colliding,
            // then do the steering based on current position
            if (firstMinSep <= 0 || firstDistance < 2 * radius)
                relPos = transform.position - firstTarget.transform.position;
            // Otherwise determine the future relative position
            else
                relPos = firstRelPos + firstRelVel * shortestTime;
            // Avoid the target
            linear = relPos.normalized * agent.maxAccel;

        }

        // Output the steering
        return new SteeringOutput(linear, angular);
    }

}
