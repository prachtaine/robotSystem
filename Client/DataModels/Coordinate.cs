using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DataModels
{
    public class Coordinate
    {
        public double leftX { get; set; }
        public double leftY { get; set; }
        public double leftZ { get; set; }
        public double rightX { get; set; }
        public double rightY { get; set; }
        public double rightZ { get; set; }
        
        public Coordinate()
        {

        }
        public Coordinate(double leftx, double lefty, double leftz, double rightx, double righty, double rightz)
        {
            this.leftX = leftx;
            this.leftY = lefty;
            this.leftZ = leftz;
            this.rightX = rightx;
            this.rightY = righty;
            this.rightZ = rightz;
        }
        
    }
}
