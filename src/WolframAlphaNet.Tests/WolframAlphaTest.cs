using System.Globalization;
using System.Linq;
using System.Threading;
using GeoCoordinatePortable;
using WolframAlphaNET.Enums;
using WolframAlphaNET.Misc;
using WolframAlphaNET.Objects;
using Xunit;
using Unit = WolframAlphaNET.Enums.Unit;

namespace WolframAlpha.Tests
{
    public class WolframAlphaTest
    {
        private readonly string _appId = "YOUR APPID HERE";

        public WolframAlphaTest()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Fact]
        public void WolframAlphaConstructorTest()
        {
            WolframAlphaNET.WolframAlpha wolfram = new WolframAlphaNET.WolframAlpha(_appId);
            Assert.NotNull(wolfram.Assumptions);
            Assert.NotNull(wolfram.ExcludePodIDs);
            Assert.NotNull(wolfram.Formats);
            Assert.NotNull(wolfram.IncludePodIDs);
            Assert.NotNull(wolfram.PodTitles);
            Assert.NotNull(wolfram.PodIndex);
            Assert.NotNull(wolfram.Scanners);
        }

        [Fact]
        public void SearchTest()
        {
            WolframAlphaNET.WolframAlpha wolframAlpha = new WolframAlphaNET.WolframAlpha(_appId);

            const string expectedPIApproximation = "3.141592653589793238462643383279502884197169399375105820974...";

            QueryResult actual = wolframAlpha.Query("PI");
            Assert.NotNull(actual);

            //Get the interesting subpod
            string actualPIApproximation = actual.GetPrimaryPod().SubPods.First().Plaintext;

            Assert.Equal(expectedPIApproximation, actualPIApproximation);
        }

        [Fact]
        public void ValidateQueryTest()
        {
            WolframAlphaNET.WolframAlpha wolframAlpha = new WolframAlphaNET.WolframAlpha(_appId);
            //We put in a lot of parameters
            wolframAlpha.EnableTranslate = true;
            wolframAlpha.MaxWidth = 200;
            wolframAlpha.OutputUnit = Unit.NonMetric;
            wolframAlpha.ExcludePodIDs.Add("NonExistingId");
            wolframAlpha.PodIndex.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            wolframAlpha.FormatTimeout = 5;
            wolframAlpha.Formats.Add(Format.MathML);
            wolframAlpha.Formats.Add(Format.Wav);
            wolframAlpha.Formats.Add(Format.Plaintext);
            wolframAlpha.GPSLocation = new GeoCoordinate(40.42, -3.70);
            wolframAlpha.IgnoreCase = true;
            wolframAlpha.Magnification = 2.0f;
            wolframAlpha.Width = 300;
            wolframAlpha.ParseTimeout = 5;
            wolframAlpha.ScanTimeout = 1;
            wolframAlpha.UseAsync = true;
            wolframAlpha.ReInterpret = true;
            wolframAlpha.PlotWidth = 200;
            wolframAlpha.PodTimeout = 5;

            const bool expected = true;
            bool actual = wolframAlpha.ValidateQuery("PI", out var results);

            Assert.NotNull(results);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EnableTranslateTest()
        {
            //First try without translation
            WolframAlphaNET.WolframAlpha wolframAlpha = new WolframAlphaNET.WolframAlpha(_appId);
            wolframAlpha.EnableTranslate = false;
            QueryResult negativeResults = wolframAlpha.Query("uno dos tres");
            Assert.Null(negativeResults.Warnings);

            //Then try with translation
            wolframAlpha.EnableTranslate = true;
            QueryResult positiveResults = wolframAlpha.Query("uno dos tres");
            const string expectedOutput = "one two three";
            const string expectedLanguage = "Spanish";

            Assert.NotNull(positiveResults.Warnings.Translation);
            Assert.Equal(expectedOutput, positiveResults.Warnings.Translation.TranslatedText);
            Assert.Equal(expectedLanguage, positiveResults.Warnings.Translation.Language);
        }
    }
}