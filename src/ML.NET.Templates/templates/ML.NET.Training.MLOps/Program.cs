using Microsoft.ML;
using Microsoft.ML.Data;
using ML.NET.Training.Schema;
using System;
using System.Diagnostics;
using MLOps.NET;
using MLOps.NET.Entities.Impl;
using MLOps.NET.SQLite;
using MLOps.NET.Extensions;
using System.Threading.Tasks;

namespace ML.NET.Training
{
    class Program
    {
        private static readonly string DataPath = "";
        private static readonly string ModelName = "model.zip";
        private static readonly bool HasHeader = true;
        private static readonly char SeparatorChar = ',';

        static async Task Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            (IMLOpsContext mlOpsContext, Run run) = await CreateRun("MyExperiment");
            var mlContext = new MLContext(seed: 1);

            Console.WriteLine($"Loading data from {DataPath}");
            var data = mlContext.Data.LoadFromTextFile<ModelInput>(DataPath, hasHeader: HasHeader, separatorChar: SeparatorChar);

            Console.WriteLine("Logging data");
            await mlOpsContext.Data.LogDataAsync(run.RunId, data);

            Console.WriteLine("Splitting the data");
            var trainTestSplit = mlContext.Data.TrainTestSplit(data);

            Console.WriteLine("Transforming the data");
            IEstimator<ITransformer> dataProcessPipeline = null;

            Console.WriteLine("Training the model");
            IEstimator<ITransformer> trainer = null;
            EstimatorChain<ITransformer> trainingPipeline = dataProcessPipeline.Append(trainer);

            Console.WriteLine("Logging hyper-parameters");
            await mlOpsContext.Training.LogHyperParametersAsync(run.RunId, trainer);

            ITransformer model = trainingPipeline.Fit(trainTestSplit.TrainSet);

            Console.WriteLine("Evaluating the model's performance");
            //await mlOpsContext.Evaluation.LogMetricsAsync(run.RunId, metrics);

            stopWatch.Stop();
            Console.WriteLine($"Training finished in: {stopWatch.ElapsedMilliseconds} milliseconds");
            await mlOpsContext.LifeCycle.SetTrainingTimeAsync(run.RunId, stopWatch.Elapsed);

            Console.WriteLine($"Saving the model to {ModelName}");
            mlContext.Model.Save(model, trainTestSplit.TrainSet.Schema, ModelName);

            Console.WriteLine("Uploading model to model repository");
            await mlOpsContext.Model.UploadAsync(run.RunId, ModelName);
        }

        private static async Task<(IMLOpsContext, Run)> CreateRun(string experimentName)
        {
            var mlOpsContext = new MLOpsBuilder()
                .UseSQLite()
                .UseLocalFileModelRepository()
                .Build();

            var run = await mlOpsContext.LifeCycle.CreateRunAsync(experimentName);

            return (mlOpsContext, run);
        }
    }
}