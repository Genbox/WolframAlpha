using System.Configuration;
using System.Device.Location;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WolframAlphaNET;
using WolframAlphaNET.Objects;
using Unit = WolframAlphaNET.Unit;

namespace WolframAlphaNETTests
{
    [TestClass]
    public class WolframAlphaTest
    {
        private string _appId = ConfigurationManager.AppSettings["AppId"];

        [TestMethod]
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

        [TestMethod]
        public void SearchTest()
        {
            WolframAlpha wolframAlpha = new WolframAlpha(_appId);

            const string expectedPIApproximation = "3.1415926535897932384626433832795028841971693993751058...";

            QueryResult actual = wolframAlpha.Query("PI");
            Assert.IsNotNull(actual);

            //Get the interesting subpod
            string actualPIApproximation = QueryResultHelper.GetPrimaryPod(actual).SubPods.First().Plaintext;

            Assert.AreEqual(expectedPIApproximation, actualPIApproximation);
        }

        [TestMethod]
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

        [TestMethod]
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
            Assert.AreEqual(expectedOutput, positiveResults.Warnings.Translation.Trans);
            Assert.AreEqual(expectedLanguage, positiveResults.Warnings.Translation.Lang);
        }

        [TestMethod]
        public void ExcludePodIDsTest()
        {
        }

        [TestMethod]
        public void FormatsTest()
        {
        }

        [TestMethod]
        public void GPSLocationTest()
        {
        }

        [TestMethod]
        public void IgnoreCaseTest()
        {
        }

        [TestMethod]
        public void IncludePodIDsTest()
        {
        }

        [TestMethod]
        public void IpAddressTest()
        {
        }

        [TestMethod]
        public void LocationTest()
        {
        }

        [TestMethod]
        public void MagnificationTest()
        {
        }

        [TestMethod]
        public void MaxWidthTest()
        {
        }

        [TestMethod]
        public void OutputUnitTest()
        {
        }

        [TestMethod]
        public void ParseTimeoutTest()
        {
        }

        [TestMethod]
        public void PodIndexTest()
        {
        }

        [TestMethod]
        public void PodTimeoutTest()
        {
        }

        [TestMethod]
        public void PodTitlesTest()
        {
        }

        [TestMethod]
        public void ReInterpretTest()
        {
        }

        [TestMethod]
        public void ScanTimeoutTest()
        {
        }

        [TestMethod]
        public void ScannersTest()
        {
        }

        [TestMethod]
        public void UseAsyncTest()
        {
        }
    }
}