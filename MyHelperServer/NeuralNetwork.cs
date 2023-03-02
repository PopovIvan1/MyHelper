namespace MyHelperServer
{
    public class NeuralNetwork
    {
        struct Layer
        {
            public Vector x, z, df;
        }

        Matrix[] weights;
        int layersCount;
        Layer[] L;
        Vector[] deltas;

        public NeuralNetwork(int[] sizes)
        {
            layersCount = sizes.Length - 1;
            weights = new Matrix[layersCount];
            L = new Layer[layersCount];
            deltas= new Vector[layersCount];
            for (int i = 0; i < layersCount; i++)
            {
                weights[i] = new Matrix(sizes[i + 1], sizes[i]);
                L[i].x = new Vector(sizes[i]);
                L[i].z = new Vector(sizes[i + 1]);
                L[i].df = new Vector(sizes[i + 1]);
                deltas[i] = new Vector(sizes[i + 1]);
            }
        }

        public void Train(Vector[] x, Vector[] y, double alpha, double eps, int epochs)
        {
            int epoch = 0;
            double error;
            do
            {
                error = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    forward(x[i]);
                    backward(y[i], ref error);
                    updateWeights(alpha);
                }
                epoch++;
            } while (epoch < epochs && error > eps);
        }

        private void forward(Vector input)
        {
            for (int i = 0; i < input.Length; i++)
                L[0].x[i] = input[i];
            for (int i = 1; i < layersCount; i++)
            {
                for (int j = 0; j < L[i - 1].z.Length; j++)
                    L[i].x[j] = L[i - 1].z[j];
                for (int j = 0; j < weights[i].N; j++)
                {
                    double y = 0;
                    for (int k = 0; k < weights[i].M; k++)
                        y += weights[i][j, k] * L[i].x[k];
                    L[i].z[j] = 1 / (1 + Math.Exp(-y));
                    L[i].df[j] = L[i].z[j] * (1 - L[i].z[j]);
                }
            }
        }

        private void backward(Vector output, ref double error)
        {
            error = 0;
            for (int i = 0; i < output.Length; i++)
            {
                double e = L[layersCount - 1].z[i] - output[i];
                deltas[layersCount - 1][i] = e * L[layersCount - 1].df[i];
                error += e * e / 2;
            }
            for (int k = layersCount - 1; k > 0; k--)
            {
                for (int i = 0; i < weights[k].M; i++)
                {
                    deltas[k - 1][i] = 0;
                    for (int j = 0; j < weights[k].N; j++)
                        deltas[k - 1][i] += weights[k][j, i] * deltas[k][j];
                    deltas[k - 1][i] *= L[k - 1].df[i];
                }
            }
        }

        private void updateWeights(double alpha)
        {
            for (int k = 0; k < layersCount; k++)
                for (int i = 0; i < weights[k].N; i++)
                    for (int j = 0; j < weights[k].M; j++)
                        weights[k][i, j] -= alpha * deltas[k][i] * L[k].x[j];
        }
    }
}