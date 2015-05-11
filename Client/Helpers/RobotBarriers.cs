using Client.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public class RobotBarriers
    {
        static double upperLength = 68.58;
        static double foreLength = 96.393;
        static double minShoulderTheta = 0;
        static double maxShoulderTheta = 90;
        static double minElbowTheta = 0;
        static double maxElbowTheta = 100;
        public static Coordinate GetBoundaryCoordinates(Coordinate xyz)
        {
            double Lmax = upperLength + foreLength;
            double minShoulderRadians = minShoulderTheta * Math.PI / 180;
            double maxShoulderRadians = maxShoulderTheta * Math.PI / 180;
            double minElbowRadians = minElbowTheta * Math.PI / 180;
            double maxElbowRadians = maxElbowTheta * Math.PI / 180;
            double radius;
            double setScale;
            double c = upperLength * Math.Sin(maxShoulderRadians);
            double xShiftOne = upperLength * Math.Cos(maxShoulderRadians);
            double torusOneTheta = Math.Atan(xyz.leftY / xyz.leftZ);
            double yShiftOne = c * Math.Sin(torusOneTheta);
            double zShiftOne = c * Math.Cos(torusOneTheta);
            double torusTwoTheta = Math.Atan(xyz.leftY / xyz.leftX);
            double xShiftTwo = upperLength * Math.Cos(torusTwoTheta);
            double yShiftTwo = upperLength * Math.Sin(torusTwoTheta);
            double a = foreLength;
            double torusOneR, torusTwoR;
            double xMin = upperLength * Math.Sin(maxElbowRadians) - foreLength;
            double sphereOneLimit = Lmax * Math.Sin(maxShoulderRadians);
            double sphereThreeX = upperLength + Math.Cos(maxElbowRadians) * foreLength;
            double sphereThreeY = Math.Sin(maxElbowRadians) * foreLength;
            double sphereTwoR = Math.Sqrt(Math.Pow(sphereThreeX, 2) + Math.Pow(sphereThreeY, 2));
            double phi = Math.Tan(xyz.leftZ / xyz.leftX);

            // Setpoints start on input coordinates
            Coordinate returnCoordinate = new Coordinate();
            returnCoordinate.leftX = xyz.leftX;
            returnCoordinate.leftY = xyz.leftY;
            returnCoordinate.leftZ = xyz.leftZ;

            radius = Math.Sqrt(Math.Pow(returnCoordinate.leftX, 2) + Math.Pow(returnCoordinate.leftY, 2) + Math.Pow(returnCoordinate.leftZ, 2));
            torusOneR = Math.Sqrt(Math.Pow(c - Math.Sqrt(Math.Pow(returnCoordinate.leftY, 2) + Math.Pow(returnCoordinate.leftZ, 2)), 2) + Math.Pow((returnCoordinate.leftX - xShiftOne), 2));
            torusTwoR = Math.Sqrt(Math.Pow(upperLength - Math.Sqrt(Math.Pow(returnCoordinate.leftX, 2) + Math.Pow(returnCoordinate.leftY, 2)), 2) + Math.Pow(returnCoordinate.leftZ, 2));
            // Outside barriers
            if ((radius > Lmax) && (xyz.leftX <= sphereOneLimit))   // Sphere 1 
            {
                setScale = Lmax / radius;
                returnCoordinate.leftX = returnCoordinate.leftX * setScale;
                returnCoordinate.leftY = returnCoordinate.leftY * setScale;
                returnCoordinate.leftZ = returnCoordinate.leftZ * setScale;
            }
            if ((torusOneR > a) && (xyz.leftX > sphereOneLimit) && (radius > sphereTwoR))  // Torus 1
            {
                setScale = a / torusOneR;
                returnCoordinate.leftX = ((returnCoordinate.leftX + xShiftOne) * setScale) - xShiftOne;
                returnCoordinate.leftY = ((returnCoordinate.leftY - yShiftOne) * setScale) + yShiftOne;
                returnCoordinate.leftZ = ((returnCoordinate.leftZ - zShiftOne) * setScale) + zShiftOne;
            }
            //if ((torusTwoR < a) && (x < 0) && (x > -25))
            //    setX = 0;
            if ((torusTwoR < a) && (xyz.leftX <= -25))  // Torus 2
            {
                setScale = a / torusTwoR;
                returnCoordinate.leftX = ((returnCoordinate.leftX + xShiftTwo) * setScale) - xShiftTwo;
                returnCoordinate.leftY = ((returnCoordinate.leftY + yShiftTwo) * setScale) - yShiftTwo;
                returnCoordinate.leftZ = returnCoordinate.leftZ * setScale;
            }
            // Sphere 3 force inside outward
            radius = Math.Sqrt(Math.Pow(xyz.leftX, 2) + Math.Pow((xyz.leftY + upperLength), 2) + Math.Pow(xyz.leftZ, 2));
            if ((radius < foreLength) && (xyz.leftY < -35.56) && (xyz.leftX > -25))
            {
                setScale = foreLength / radius;
                returnCoordinate.leftX = xyz.leftX * setScale;
                returnCoordinate.leftY = ((xyz.leftY + upperLength) * setScale) - upperLength;
                returnCoordinate.leftZ = xyz.leftZ * setScale;
            }
            // Shpere 2, force inside outward
            radius = Math.Sqrt(Math.Pow(returnCoordinate.leftX, 2) + Math.Pow(returnCoordinate.leftY, 2) + Math.Pow(returnCoordinate.leftZ, 2));
            if ((radius < sphereTwoR) && (xyz.leftY >= -35.56) && (xyz.leftX > -25))
            {
                setScale = sphereTwoR / radius;
                if(returnCoordinate.leftX != 0.0)
                    returnCoordinate.leftX = xyz.leftX * setScale;
                if(returnCoordinate.leftY != 0.0)
                    returnCoordinate.leftY = xyz.leftY * setScale;
                if(returnCoordinate.leftZ != 0.0)
                    returnCoordinate.leftZ = xyz.leftZ * setScale;
            }
            // Ceiling
            if (returnCoordinate.leftY > 0)
                returnCoordinate.leftY = 0;
            // Back Wall
            if (returnCoordinate.leftZ < 0)
                returnCoordinate.leftZ = 0;

            return returnCoordinate;
        }
    }
}
