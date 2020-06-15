# Wolfram|Alpha

[![NuGet](https://img.shields.io/nuget/v/Genbox.WolframAlpha.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Genbox.WolframAlpha/)

### Features

* Support for the [full query API](https://products.wolframalpha.com/api/documentation/)
* Support for the [simple query API](https://products.wolframalpha.com/simple-api/documentation/)
* Support for the [validate query API](https://products.wolframalpha.com/api/documentation/#the-validatequery-function)
* Can recalculate/get async pods
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
    QueryResponse results = await client.QueryAsync("100 digits of pi").ConfigureAwait(false);

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