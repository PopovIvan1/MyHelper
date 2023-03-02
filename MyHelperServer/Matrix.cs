namespace MyHelperServer
{
    public class Matrix
    {
        public int N { get; private set; }
        public int M { get; private set; }
        double[,] matrix;

        public Matrix(int n, int m)
        {
            N = n;
            M = m;
            matrix = generateRandomMatrix();
        }

        public double this[int i, int j]
        {
            get { return matrix[i, j]; } 
            set { matrix[i, j] = value; }
        }

        private double[,] generateRandomMatrix()
        {
            double[,] matrix = new double[N, M];
            Random random = new Random();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    matrix[i, j] = random.NextDouble() - 0.5;
            return matrix;
        }
    }
}
