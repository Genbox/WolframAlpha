using System;
using WolframAlpha.Misc;
using WolframAlpha.Objects;

namespace WolframAlpha.Client
{
    public class Program
    {
        //Insert your App ID into the App.config file
        private static readonly string _appId = "INSERT APPID HERE";

        static void Main(string[] args)
        {
            //Create the Engine.
            WolframAlpha wolfram = new WolframAlpha(_appId);
            wolfram.ScanTimeout = 0.1f; //We set ScanTimeout really low to get a quick answer. See RecalculateResults() below.
            wolfram.UseTLS = true; //Use encryption

            //We search for something. Notice that we spelled it wrong.
            QueryResult results = wolfram.Query("Who is Danald Duck?");

            //This fetches the pods that did not complete. It is only here to show how to use it.
            //This returns the pods, but also adds them to the original QueryResults.
            results.RecalculateResults();

            //Here we output the Wolfram|Alpha results.
            if (results.Error != null)
                Console.WriteLine("Woops, where was an error: " + results.Error.Message);

            if (results.DidYouMean.HasElements())
            {
                foreach (DidYouMean didYouMean in results.DidYouMean)
                {
                    Console.WriteLine("Did you mean: " + didYouMean.Value);
                }
            }

            Console.WriteLine();

            //Results are split into "pods" that contain information. Those pods can also have subpods.
            Pod primaryPod = results.GetPrimaryPod();

            if (primaryPod != null)
            {
                Console.WriteLine(primaryPod.Title);
                if (primaryPod.SubPods.HasElements())
                {
                    foreach (SubPod subPod in primaryPod.SubPods)
                    {
                        Console.WriteLine(subPod.Title);
                        Console.WriteLine(subPod.Plaintext);
                    }
                }
            }

            if (results.Warnings != null)
            {
                if (results.Warnings.Translation != null)
                    Console.WriteLine("Translation: " + results.Warnings.Translation.Text);

                if (results.Warnings.SpellCheck != null)
                    Console.WriteLine("Spellcheck: " + results.Warnings.SpellCheck.Text);
            }

            Console.ReadLine();
        }
    }
}
