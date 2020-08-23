using Microsoft.ML;
using Microsoft.ML.Data;
using ML.NET.Training.Schema;
using System;
using System.Diagnostics;

namespace ML.NET.Training
{
    class Program
    {
        private static readonly string DataPath = "";
        private static readonly string ModelName = "model.zip";
        private static readonly bool HasHeader = true;
        private static readonly char SeparatorChar = ',';

        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var mlContext = new MLContext(seed: 1);

            Console.WriteLine($"Loading data from {DataPath}");
            var data = mlContext.Data.LoadFromTextFile<ModelInput>(DataPath, hasHeader: HasHeader, separatorChar: SeparatorChar);

            Console.WriteLine("Splitting the data");
            var trainTestSplit = mlContext.Data.TrainTestSplit(data);

            Console.WriteLine("Transforming the data");
            IEstimator<ITransformer> dataProcessPipeline = null;

            Console.WriteLine("Training the model");
            IEstimator<ITransformer> trainer = null;
            EstimatorChain<ITransformer> trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer model = trainingPipeline.Fit(trainTestSplit.TrainSet);

            Console.WriteLine("Evaluating the model's performance");
            //Depends on Trainer

            stopWatch.Stop();
            Console.WriteLine($"Training finished in: {stopWatch.ElapsedMilliseconds} milliseconds");

            Console.WriteLine($"Saving the model to {ModelName}");
            mlContext.Model.Save(model, trainTestSplit.TrainSet.Schema, ModelName);
        }
    }
}
