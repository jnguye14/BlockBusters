#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Common.Shapes; // for triangles
#endregion

namespace Common
{
    public class Physics
    {
        // test collision between two spheres
        public static bool TestSphereSphere(Transform a, Transform b)
        {
            // if the distance between their centers is less than the sum of their radii
            // return true (i.e. is colliding)
            // else return false (i.e. is not colliding)
            return ((a.Position - b.Position).Length() < (a.Scale + b.Scale).Y);
        }

        // test collision between a sphere and a triangle
        public static bool TestSphereTriangle(Transform sphere, Triangle triangle)
        {
            return TestSphereTriangle(sphere, triangle.a, triangle.b, triangle.c);
        }

        // test collision between a sphere and a triangle given as three vectors (i.e. lines)
        public static bool TestSphereTriangle(Transform sphere, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            return TestSphereTriangle(sphere, a, b, c, normal);
        }

        public static bool TestSphereTriangle(Transform sphere, Vector3 a, Vector3 b, Vector3 c, Vector3 normal)
        {
            // Find the separation distance between the sphere center and the triangle plane
            float separation = Vector3.Dot(sphere.Position - a, normal);

            // If the separation value is out of bounds, quit
            if ((separation > sphere.Scale.Y) || (separation < 0))
            {
                return false;
            }
            else // Otherwise, there is collision with the plane. Find the nearest point
            {
                Vector3 pointOnPlane = sphere.Position - normal * separation;

                // Find the barycentric coordinates
                float area1 = Vector3.Dot(Vector3.Cross(b - a, pointOnPlane - a), normal);
                float area2 = Vector3.Dot(Vector3.Cross(c - b, pointOnPlane - b), normal);
                float area3 = Vector3.Dot(Vector3.Cross(a - c, pointOnPlane - c), normal);
                return !((area1 < 0) || (area2 < 0) || (area3 < 0));
            }
        }

        // Newton's third law of motion: for every action, there is an equal and opposite reaction
        public static void ResolveCollision(Rigidbody a, Rigidbody b)
        {
            // Find the direction of collision, all changes only occur in this direction
            Vector3 direction = a.Position - b.Position;
            direction.Normalize();
            
            // Determine the relative velocity in the direction of collision
            // Use a dot product to get the "projection", then multiply by the direction
            // Note the - sign since we are working with respect to a
            Vector3 relativeVelocity = a.Velocity - b.Velocity;
            Vector3 relativeVelocityPerp = -Vector3.Dot(relativeVelocity, direction) * direction;
            
            // Finally the impulse Force acting on the object is given thus
            // Formula = V_n / (m1 + m2) * m1m2 = V_n / (1/m1 + 1/m2)
            // Note: system fails if m1 or m2 = 0; but we don't allow that (see Rigidbody)
            Vector3 impulse = relativeVelocityPerp / ((1 / a.Mass) + (1 / b.Mass));
            
            // Finally add the impulse to both objects (third law of motion)
            // the 2 comes from (1 + e1e2) where e1 and e2 are the coefficients of restitution
            a.Impulse += 2 * impulse;
            b.Impulse -= 2 * impulse;
            
            //float epsilon = (a.CoRestitution * b.CoRestitution);
            //a.Impulse += (-(1 + epsilon) * (Vector3(collidingNormal)) / (a.Mass + b.Mass)) * (a.Mass * b.Mass) * impule;
            //b.Impulse -= (-(1 + epsilon) * (Vector3(collidingNormal)) / (a.Mass + b.Mass)) * (a.Mass * b.Mass) * impulse;

            // F = ((-(1 + e)*V_n)/(m1 + m2))*(m1*m2)
        }
    }
}
