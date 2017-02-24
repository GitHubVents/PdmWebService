namespace SolidWorksLibrary.Builders.ElementsCase
{
    
        public class Vector2
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Vector2(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        public class Vector3 : Vector2 
        {
            public double Z { get; set; }

            public Vector3(double x, double y,double z) :base(x,y)
            {
                this.Z = z;
            }
        }
     
}
