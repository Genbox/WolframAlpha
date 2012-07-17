# Wolfram|Alpha.Net - A full implementation of the 2.0 API

### Features

* Based on RestSharp (http://restsharp.org) to deserialize the Wolfram|Alpha XML into objects
* Handles Assumptions, different formats, warnings, tips, did you means, timings and more.

### Tutorial

First you need to get a Wolfram|Alpha AppID from their website.

1. Go to https://developer.wolframalpha.com/portal/signup.html and create an account if you don't already have one.
2. Go to http://developer.wolframalpha.com/portal/myapps/index.html and click "Get an AppID"
3. Just follow their wizard and then you will have an AppID in the format: XXXXXX-XXXXXXXXXX
4. Paste the AppID into the WolframAlphaNETClient/App.config file where it says "INSERT APPID HERE".
5. Press F5 to run the client

### Examples

Here is the simplest form of getting data from Wolfram|Alpha:

```csharp
static void Main(string[] args)
{
	//First create the main class:
	WolframAlpha wolfram = new WolframAlpha("APPID HERE");

	//Then you simply query Wolfram|Alpha like this
	//Note that the spelling error will be correct by Wolfram|Alpha
	QueryResult results = wolfram.Query("Who is Danald Duck?");

	//The QueryResult object contains the parsed XML from Wolfram|Alpha. Lets look at it.
	//The results from wolfram is split into "pods". We just print them.
	if (results != null)
	{
		foreach (Pod pod in results.Pods)
		{
			Console.WriteLine(pod.Title);
			if (pod.SubPods != null)
			{
				foreach (SubPod subPod in pod.SubPods)
				{
					Console.WriteLine(subPod.Title);
					Console.WriteLine(subPod.Plaintext);
				}
			}
		}
	}
}

```

For more examples, take a look at the WolframAlpha.NET Client and WolframAlpha.NET Tests projects.