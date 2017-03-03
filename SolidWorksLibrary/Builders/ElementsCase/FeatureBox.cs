using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase
{
    public class FeatureBox
    {
        public string ComponentName { get; set; }
        public string FileName { get; set; }
        public bool IsOptions { get; set; }

        public bool Equals(FeatureBox other)
        {

            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return FileName.Equals(other.FileName)  ;
        }

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null. 
            int hashProductName = FileName == null ? 0 : FileName.GetHashCode();

            //Get hash code for the Code field. 
           // int hashProductCode = Code.GetHashCode();

            //Calculate the hash code for the product. 
            return hashProductName  ;
        }
    }
}
