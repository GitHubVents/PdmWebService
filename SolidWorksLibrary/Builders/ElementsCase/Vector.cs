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

        public static Vector2 Zero { get { return new Vector2(0, 0); } }
    }

        public class Vector3 : Vector2 
        {
            public double Z { get; set; }

            public Vector3(double x, double y,double z) :base(x,y)
            {
                this.Z = z;
            }

        public static Vector3 Zero { get { return new Vector2(0, 0); } }
    }     
}
