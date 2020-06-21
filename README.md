# Wolfram|Alpha

[![NuGet](https://img.shields.io/nuget/v/Genbox.WolframAlpha.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Genbox.WolframAlpha/)

### Features

* Support for the [Full Results API](https://products.wolframalpha.com/api/documentation/)
* Support for the [Simple Results API](https://products.wolframalpha.com/simple-api/documentation/)
* Support for the [Short Answers API](https://products.wolframalpha.com/short-answers-api/documentation/)
* Support for the [Spoken Results API](https://products.wolframalpha.com/spoken-results-api/documentation/)
* Full support for [async queries](https://products.wolframalpha.com/api/documentation/#podtimeout-async)
* Uses object pooling to minimize memory usage
* Dependency injection friendly

### How do I get an AppId?

First you need to get a Wolfram|Alpha AppId from their website.

1. Go to https://developer.wolframalpha.com/portal/signup.html and create an account
   if you don't already have one.
2. Go to https://developer.wolframalpha.com/portal/myapps/index.html and click "Get an AppID"
3. Just follow their wizard and then you will have an AppID in the format: XXXXXX-XXXXXXXXXX

### Example

```csharp
static async Task Main(string[] args)
{
    //Create the client.
    WolframAlphaClient client = new WolframAlphaClient("YOUR APPID HERE");

    //We start a new query.
    FullResultResponse results = await client.FullResultAsync("100 digits of pi");

    //Results are split into "pods" that contain information.
    foreach (Pod pod in results.Pods)
    {
        Console.WriteLine(pod.Title + ":");

        foreach (SubPod subPod in pod.SubPods)
        {
            if (string.IsNullOrEmpty(subPod.Plaintext))
                Console.WriteLine("<Cannot output in console>");
            else
                Console.WriteLine(subPod.Plaintext);
        }

        Console.WriteLine();
    }
}
```