using Client.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public class HomePosition
    {
        double offsetLeftX, offsetLeftY, offsetLeftZ;
        double offsetRightX, offsetRightY, offsetRightZ;

        public bool InvertX { get; set; }
        public bool InvertY { get; set; }
        public bool InvertZ { get; set; }


        public HomePosition()
        {
        }

        public Coordinate getPosition(Coordinate xyz)
        {
            Coordinate returnCoordinate = new Coordinate();
            double inputLeftX = InvertX ? -xyz.leftX : xyz.leftX;
            double inputLeftY = InvertY ? -xyz.leftY : xyz.leftY;
            double inputLeftZ = InvertZ ? -xyz.leftZ : xyz.leftZ;

            double inputRightX = InvertX ? -xyz.rightX : xyz.rightX;
            double inputRightY = InvertY ? -xyz.rightY : xyz.rightY;
            double inputRightZ = InvertZ ? -xyz.rightZ : xyz.rightZ;
            if (InvertX)
            {
                returnCoordinate.leftX = inputLeftX - offsetLeftX;
                returnCoordinate.rightX = inputRightX - offsetRightX;
            }
            else
            {
                returnCoordinate.leftX = inputLeftX + offsetLeftX;
                returnCoordinate.rightX = inputRightX + offsetRightX;
            }
            if (InvertY)
            {
                returnCoordinate.leftY = inputLeftY - offsetLeftY;
                returnCoordinate.rightY = inputRightY - offsetRightY;
            }
            else
            {
                returnCoordinate.leftY = inputLeftY + offsetLeftY;
                returnCoordinate.rightY = inputRightY + offsetRightY;
            }
            if (InvertZ)
            {
                returnCoordinate.leftZ = inputLeftZ - offsetLeftZ;
                returnCoordinate.rightZ = inputRightZ - offsetRightZ;
            }
            else
            {
                returnCoordinate.leftZ = inputLeftZ + offsetLeftZ;
                returnCoordinate.rightZ = inputRightZ + offsetRightZ;
            }
            return returnCoordinate;
        }

        public void resetHome(Coordinate xyz)
        {
            offsetLeftX = HomeX - xyz.leftX;
            offsetLeftY = HomeY - xyz.leftY;
            offsetLeftZ = HomeZ - xyz.leftZ;

            offsetRightX = HomeX - xyz.rightX;
            offsetRightY = HomeY - xyz.rightY;
            offsetRightZ = HomeZ - xyz.rightZ;
        }

        private double homeZ = 0;

        public double HomeZ
        {
            get
            {
                return homeZ;
            }

            set
            {
                if (homeZ == value)
                {
                    return;
                }

                homeZ = value;
            }
        }

        private double homeY = 0;

        public double HomeY
        {
            get
            {
                return homeY;
            }

            set
            {
                if (homeY == value)
                {
                    return;
                }

                homeY = value;
            }
        }

        private double homeX = 0;

        public double HomeX
        {
            get
            {
                return homeX;
            }

            set
            {
                if (homeX == value)
                {
                    return;
                }

                homeX = value;
            }
        }
    }
}
