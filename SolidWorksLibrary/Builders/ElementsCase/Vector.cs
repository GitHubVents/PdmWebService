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


        public static bool operator ==(Vector2 a, Vector2 b) {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b)) {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null)) {
                return false;
            }

            // Return true if the fields match:
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2 a, Vector2 b) {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b)) {
                return true;
            }            
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null)) {
                return false;
            }
            // Return true if the fields match:
            return a.X != b.X && a.Y != b.Y;
        }


        public static bool operator >(Vector2 a, Vector2 b) {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b)) {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null)) {
                return false;
            }

            // Return true if the fields match:
            return a.X > b.X && a.Y > b.Y;
        }

        public static bool operator <(Vector2 a, Vector2 b) {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b)) {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null)) {
                return false;
            }
            // Return true if the fields match:
            return a.X < b.X && a.Y < b.Y;
        }

        #region  
        public static bool operator >=(Vector2 a, Vector2 b) {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b)) {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null)) {
                return false;
            }

            // Return true if the fields match:
            return a.X >= b.X && a.Y >= b.Y;
        }

        public static bool operator <=(Vector2 a, Vector2 b) {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b)) {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null)) {
                return false;
            }
            // Return true if the fields match:
            return a.X <= b.X && a.Y <= b.Y;
        }

        public static Vector2 operator + (Vector2 a, Vector2 b) {
            if (a == null || b == null)
                throw new System.Exception("Failed to sum vectors, because one or both parameters equals to null");
            // Return true if the fields match:
            return new Vector2( a.X + b.X ,  a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b) {
            if (a == null || b == null)
                throw new System.Exception("Failed to get the difference, because one or both parameters equals to null");
            // Return true if the fields match:
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }
        #endregion
        public static Vector2 Zero { get { return new Vector2(0, 0); } }

        public override string ToString( )
        {
            return $"Vector2[x:{X},y:{Y}]";
        }
    }

        public class Vector3 : Vector2 
        {
            public double Z { get; set; }

            public Vector3(double x, double y,double z) :base(x,y)
            {
                this.Z = z;
            }
     
        public static Vector3 Zero { get { return new Vector3(0, 0,0); } }

        public override string ToString( )
        {
            return $"Vector2[x:{X}, y:{Y}, z:{Z}]";
        }
    }     
}
