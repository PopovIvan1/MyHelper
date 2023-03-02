namespace MyHelperServer
{
    public class Vector
    {
        public int Length { get; private set; }
        double[] values;

        public Vector(double[] values)
        {
            this.values = values;
            Length = values.Length;
        }

        public Vector(int length)
        {
            Length = length;
            values = new double[length];
        }

        public double this[int index]
        {
            get { return values[index]; }
            set { values[index] = value; }
        }
    }
}
