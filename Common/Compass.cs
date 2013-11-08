using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    // JNN: TODO: Make more dynamic, for example:
    // make it so North is always in one direction
    // or so compass can point to an object with a magnetic field
    public class Compass
    {
        public enum direction
        {
            NORTH, NORTHWEST, WEST, SOUTHWEST, SOUTH, SOUTHEAST, EAST, NORTHEAST, UNKNOWN
        };

        public String facingString;
        public direction facingDirection;
        public float facingAngle;

        // constructor
        public Compass()
        {
            facingString = "N";
            facingDirection = direction.NORTH;
            facingAngle = 0.0f;
        }

        public void turnLeft(float delta)
        {
            facingAngle += delta;
            facingAngle = (float)(facingAngle % (Math.PI * 2));
            setFacingDirection();
        }

        public void turnRight(float delta)
        {
            turnLeft(-delta); // laziness >.>
        }

        private void setFacingDirection()
        {
            int temp = (int)(facingAngle / (Math.PI / 4));
            switch (temp)
            {
            case 0: facingDirection = direction.NORTH; break;
            case 1: case -7: facingDirection = direction.NORTHWEST; break;
            case 2: case -6: facingDirection = direction.WEST; break;
            case 3: case -5: facingDirection = direction.SOUTHWEST; break;
            case 4: case -4: facingDirection = direction.SOUTH; break; ;
            case 5: case -3: facingDirection = direction.SOUTHEAST; break;
            case 6: case -2: facingDirection = direction.EAST; break;
            case 7: case -1: facingDirection = direction.NORTHEAST; break;
            default: facingDirection = direction.UNKNOWN; break;
            }
            setFacingString();
        }

        private void setFacingString()
        {
            switch (facingDirection)
            {
            case direction.NORTH: facingString = "N"; break;
            case direction.NORTHWEST: facingString = "NE"; break;
            case direction.WEST: facingString = "W"; break;
            case direction.SOUTHWEST: facingString = "SW"; break;
            case direction.SOUTH: facingString = "S"; break;
            case direction.SOUTHEAST: facingString = "SE"; break;
            case direction.EAST: facingString = "E"; break;
            case direction.NORTHEAST: facingString = "NE"; break;
            case direction.UNKNOWN: default: facingString = "Unknown"; break;
            }
        }
    }
}
