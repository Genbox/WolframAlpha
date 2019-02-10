using System.Globalization;
using System.Linq;
using System.Threading;
using GeoCoordinatePortable;
using NUnit.Framework;
using WolframAlphaNET;
using WolframAlphaNET.Enums;
using WolframAlphaNET.Misc;
using WolframAlphaNET.Objects;
using Unit = WolframAlphaNET.Enums.Unit;

namespace WolframAlphaNETTests
{
    [TestFixture]
    public class WolframAlphaTest
    {
        private string _appId = "INSERT APPID HERE";

        [SetUp]
        public void Init()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Test]
        public void WolframAlphaConstructorTest()
        {
            WolframAlpha wolfram = new WolframAlpha(_appId);
            Assert.IsNotNull(wolfram.Assumptions);
            Assert.IsNotNull(wolfram.ExcludePodIDs);
            Assert.IsNotNull(wolfram.Formats);
            Assert.IsNotNull(wolfram.IncludePodIDs);
            Assert.IsNotNull(wolfram.PodTitles);
            Assert.IsNotNull(wolfram.PodIndex);
            Assert.IsNotNull(wolfram.Scanners);
        }

        [Test]
        public void SearchTest()
        {
            WolframAlpha wolframAlpha = new WolframAlpha(_appId);

            const string expectedPIApproximation = "3.141592653589793238462643383279502884197169399375105820974...";

            QueryResult actual = wolframAlpha.Query("PI");
            Assert.IsNotNull(actual);

            //Get the interesting subpod
            string actualPIApproximation = QueryResultHelper.GetPrimaryPod(actual).SubPods.First().Plaintext;

            Assert.AreEqual(expectedPIApproximation, actualPIApproximation);
        }

        [Test]
        public void ValidateQueryTest()
        {
            WolframAlpha wolframAlpha = new WolframAlpha(_appId);
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

            ValidateQueryResult results;
            const bool expected = true;
            bool actual = wolframAlpha.ValidateQuery("PI", out results);

            Assert.IsNotNull(results);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EnableTranslateTest()
        {
            //First try without translation
            WolframAlpha wolframAlpha = new WolframAlpha(_appId);
            wolframAlpha.EnableTranslate = false;
            QueryResult negativeResults = wolframAlpha.Query("uno dos tres");
            Assert.IsNull(negativeResults.Warnings);

            //Then try with translation
            wolframAlpha.EnableTranslate = true;
            QueryResult positiveResults = wolframAlpha.Query("uno dos tres");
            const string expectedOutput = "one two three";
            const string expectedLanguage = "Spanish";

            Assert.IsNotNull(positiveResults.Warnings.Translation);
            Assert.AreEqual(expectedOutput, positiveResults.Warnings.Translation.TranslatedText);
            Assert.AreEqual(expectedLanguage, positiveResults.Warnings.Translation.Language);
        }

        [Test]
        public void ExcludePodIDsTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void FormatsTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void GPSLocationTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void IgnoreCaseTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void IncludePodIDsTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void IpAddressTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void LocationTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void MagnificationTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void MaxWidthTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void OutputUnitTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void ParseTimeoutTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void PodIndexTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void PodTimeoutTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void PodTitlesTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void ReInterpretTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void ScanTimeoutTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void ScannersTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UseAsyncTest()
        {
            Assert.Inconclusive();
        }
    }
}