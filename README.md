![mlops.neticon](https://img.shields.io/nuget/v/ML.NET.Templates.svg)

## ML.NET.Templates
ML.NET.Templates is a collection of `dotnet new` templates for ML.NET.

## How to use
To install the `dotnet new` templates, run the following
```
dotnet new --install ML.NET.Templates::0.1.1-beta
```

## Templates
`mlnetVersion` is optional and defaults to 1.5.1

### mlnet-training
The template bootstraps a .NET Core console app that can be used to train a `ML.NET` model.
```
dotnet new mlnet-training --mlnetVersion 1.5.1
```

### mlnet-web-embedded
The template bootstraps an ASP.NET Core Web App with an embedded `ML.NET` model and Docker support
```
dotnet new mlnet-web-embedded --mlnetVersion 1.5.1
```

## Ideas
Add `dotnet new` template for the following scenarios

- ML.NET Console App with MLOps.NET support
- ASP.NET Core with model accessed from URI
- Azure Function with model accessed from URI
- Custom scenarios, e.g. image classification, object detection, ONNX etc.

Have an idea? Feel free to open an issue!
