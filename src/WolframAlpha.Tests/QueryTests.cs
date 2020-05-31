using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Genbox.WolframAlpha.Enums;
using Genbox.WolframAlpha.Objects;
using Xunit;

namespace Genbox.WolframAlpha.Tests
{
    public class QueryTests : TestBase
    {
        [Fact]
        public async Task AssumptionsTest()
        {
            QueryRequest req = new QueryRequest("Pi");
            req.Assumptions.Add("*C.Pi-_*Movie-");

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.True(res.IsSuccess);

            Pod resultPod = res.Pods.Single(x => x.Title == "Input interpretation");
            Assert.Equal("Pi (movie)", resultPod.SubPods[0].Plaintext);
        }

        [Fact]
        public async Task AsyncTest()
        {
            QueryRequest req = new QueryRequest("weather in Copenhagen");
            req.UseAsync = true;
            req.PodTimeout = 1;

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.True(res.IsSuccess);

            Assert.Contains(res.Pods, x => x.AsyncUrl != null);

            await Client.GetAsyncPodsAsync(res).ConfigureAwait(false);

            Assert.True(res.Pods.All(x => x.AsyncUrl == null));
        }

        [Fact]
        public async Task DelimitersTest()
        {
            QueryResponse res = await Client.QueryAsync("sin(x").ConfigureAwait(false);

            Warning warning = Assert.Single(res.Warnings);
            Assert.Equal("An attempt was made to fix mismatched parentheses, brackets, or braces.", warning.Delimiters.Text);
        }

        [Fact]
        public async Task DidYouMeanTest()
        {
            QueryResponse resp = await Client.QueryAsync("This is a confusing query it in french").ConfigureAwait(false);
            Assert.False(resp.IsSuccess);
            Assert.False(resp.IsError);

            Assert.Equal(4, resp.DidYouMeans.Count);

            HashSet<Level> levels = new HashSet<Level>();
            levels.Add(Level.Low);
            levels.Add(Level.Medium);
            levels.Add(Level.High);

            Assert.NotEqual(0, resp.DidYouMeans[0].Score);
            Assert.Contains(resp.DidYouMeans[0].Level, levels);
            Assert.NotEmpty(resp.DidYouMeans[0].Value);

            Assert.NotEqual(0, resp.DidYouMeans[1].Score);
            Assert.Contains(resp.DidYouMeans[1].Level, levels);
            Assert.NotEmpty(resp.DidYouMeans[1].Value);

            Assert.NotEqual(0, resp.DidYouMeans[2].Score);
            Assert.Contains(resp.DidYouMeans[2].Level, levels);
            Assert.NotEmpty(resp.DidYouMeans[2].Value);

            Assert.NotEqual(0, resp.DidYouMeans[3].Score);
            Assert.Contains(resp.DidYouMeans[3].Level, levels);
            Assert.NotEmpty(resp.DidYouMeans[3].Value);
        }

        [Fact]
        public async Task ErrorTest()
        {
            Config.AppId = string.Empty;

            QueryResponse res = await Client.QueryAsync("does not matter").ConfigureAwait(false);
            Assert.False(res.IsSuccess);
            Assert.True(res.IsError);

            Assert.Equal(2, res.ErrorDetails.Code);
            Assert.Equal("Appid missing", res.ErrorDetails.Message);
        }

        [Fact]
        public async Task ExamplePageTest()
        {
            QueryRequest req = new QueryRequest("electromagnetic wave");

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.Equal("ElectricityAndMagnetism", res.ExamplePage.Category);
            Assert.NotNull(res.ExamplePage.Url);
        }

        [Fact]
        public async Task FullQueryTest()
        {
            QueryRequest req = new QueryRequest("Bill Gates");
            req.IgnoreCase = true;

            req.GeoLocation = new GeoCoordinate(40, -19);

            QueryResponse resp = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.True(resp.IsSuccess);
            Assert.False(resp.IsError);
            Assert.Equal(12, resp.Pods.Count);
            Assert.Equal("Book,Movie,Person", resp.DataTypes);
            Assert.Empty(resp.TimedOut);
            Assert.Empty(resp.TimedOutPods);
            Assert.NotEqual(0, resp.Timing);
            Assert.NotEqual(0, resp.ParseTiming);
            Assert.False(resp.ParseTimedOut);
            Assert.Null(resp.RecalculateUrl);
            Assert.NotEmpty(resp.Id);
            Assert.NotNull(resp.Host);
            Assert.NotEqual(0, resp.Server);
            Assert.NotNull(resp.Related);
            Assert.Equal(Version.Parse("2.6"), resp.Version);

            Assert.Equal(3, resp.Sources.Count);

            Source firstSource = resp.Sources[0];
            Assert.NotEmpty(firstSource.Url.ToString());
            Assert.Equal("Book data", firstSource.Text);

            Source secondSource = resp.Sources[1];
            Assert.NotEmpty(secondSource.Url.ToString());
            Assert.Equal("Movie data", secondSource.Text);

            Source thirdSource = resp.Sources[2];
            Assert.NotEmpty(thirdSource.Url.ToString());
            Assert.Equal("People data", thirdSource.Text);

            Pod inputInter = resp.Pods[0];
            Assert.Equal("Input interpretation", inputInter.Title);
            Assert.Equal("Identity", inputInter.Scanner);
            Assert.Equal("Input", inputInter.Id);
            Assert.Equal(100, inputInter.Position);
            Assert.False(inputInter.IsError);
            Assert.False(inputInter.IsPrimary);

            SubPod inputInterSub = Assert.Single(inputInter.SubPods);
            Assert.Equal("Bill Gates (businessperson)", inputInterSub.Plaintext);
            Assert.NotEmpty(inputInterSub.Image.Src.ToString());
            Assert.Equal("Bill Gates (businessperson)", inputInterSub.Image.Alt);
            Assert.Equal("Bill Gates (businessperson)", inputInterSub.Image.Title);
            Assert.Equal(178, inputInterSub.Image.Width);
            Assert.Equal(18, inputInterSub.Image.Height);
            Assert.Equal("Default", inputInterSub.Image.Type);
            Assert.Equal("1,2,3,4,5,6,7,8,9,10,11,12", inputInterSub.Image.Themes);
            Assert.True(inputInterSub.Image.ColorInvertible);

            Pod basicInfo = resp.Pods[1];
            Assert.Equal("Basic information", basicInfo.Title);
            Assert.Equal("Data", basicInfo.Scanner);
            Assert.Equal("BasicInformation:PeopleData", basicInfo.Id);
            Assert.Equal(200, basicInfo.Position);
            Assert.False(basicInfo.IsError);
            Assert.False(basicInfo.IsPrimary);

            SubPod basicInfoSub = Assert.Single(basicInfo.SubPods);
            string microSource = Assert.Single(basicInfoSub.MicroSources);
            Assert.Equal("PeopleData", microSource);

            Expressiontype expressionType = Assert.Single(basicInfo.ExpressionTypes);
            Assert.Equal("Grid", expressionType.Name);

            Pod notableFacts = resp.Pods[4];
            SubPod notableFactSub = Assert.Single(notableFacts.SubPods);
            string dataSource = Assert.Single(notableFactSub.DataSources);

            Assert.Equal("TheWikimediaFoundationIncWikipedia", dataSource);

            Pod famRelation = resp.Pods[6];

            Assert.Equal(4, famRelation.ExpressionTypes.Count);
            Assert.Equal(4, famRelation.SubPods.Count);

            State state = Assert.Single(famRelation.States);
            Assert.Equal("Show full dates", state.Name);
            Assert.Equal("FamilialRelationships:PeopleData__Show full dates", state.Input);

            Pod notableFilms = resp.Pods[7];
            Definition definition = Assert.Single(notableFilms.Definitions);
            Assert.Equal("Appeared in", definition.Word);
            Assert.Equal("Includes films where an individual appeared as him or herself, or in historical or archival footage.", definition.Description);

            SubPod notableFilmsSub = Assert.Single(notableFilms.SubPods);
            Assert.Equal(2, notableFilmsSub.MicroSources.Count);

            Pod wikipediaSummary = resp.Pods[10];
            SubPod wikipediaSummarySub = Assert.Single(wikipediaSummary.SubPods);
            Info info = Assert.Single(wikipediaSummarySub.Infos);
            Link link = Assert.Single(info.Links);

            Assert.Equal("http://en.wikipedia.org/wiki?curid=3747", link.Url.ToString());
            Assert.Equal("Full entry", link.Text);
        }

        [Fact]
        public async Task FullQueryTest2()
        {
            QueryRequest req = new QueryRequest("Pi");

            QueryResponse resp = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.True(resp.IsSuccess);
            Assert.False(resp.IsError);

            Pod inputPod = resp.Pods.Single(x => x.Title == "Input");
            SubPod inputSubPod = Assert.Single(inputPod.SubPods);

            //Check if we can represent symbols correctly
            Assert.Equal("π", inputSubPod.Plaintext);

            Pod altRepresent = resp.Pods.Single(x => x.Title == "Alternative representations");
            Assert.Equal(4, altRepresent.Infos.Count);

            State state = Assert.Single(altRepresent.States);
            Assert.Equal("More", state.Name);
            Assert.Equal("AlternativeRepresentations:MathematicalFunctionIdentityData__More", state.Input);
        }

        [Fact]
        public async Task FutureTopicTest()
        {
            QueryResponse res = await Client.QueryAsync("Microsoft Windows").ConfigureAwait(false);
            Assert.Equal("Operating Systems", res.FutureTopic.Topic);
            Assert.Equal("Development of this topic is under investigation...", res.FutureTopic.Message);
        }

        [Fact]
        public async Task GeneralizationsTest()
        {
            QueryRequest req = new QueryRequest("price of copernicium");

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.Equal("copernicium", res.Generalization.Topic);
            Assert.Equal("General results for:", res.Generalization.Description);
            Assert.NotNull(res.Generalization.Url);
        }

        [Fact]
        public async Task IncludeExcludePodTest()
        {
            QueryRequest req = new QueryRequest("pi");

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.NotEmpty(res.Pods);

            int orgPodCount = res.Pods.Count;

            Pod decApprox = res.Pods.Single(x => x.Title == "Decimal approximation");
            req.IncludePodIds.Add(decApprox.Id);

            res = await Client.QueryAsync(req).ConfigureAwait(false);

            Assert.Single(res.Pods);

            req.IncludePodIds.Clear();
            req.ExcludePodIds.Add(decApprox.Id);

            res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.Equal(orgPodCount - 1, res.Pods.Count);
        }

        [Fact]
        public async Task LanguageMessageTest()
        {
            QueryResponse res = await Client.QueryAsync("wo noch nie").ConfigureAwait(false);
            Assert.Equal("Wolfram|Alpha does not yet support German.", res.LanguageMessage.English);
            Assert.Equal("Wolfram|Alpha versteht noch kein Deutsch.", res.LanguageMessage.Other);

            Tip tip = Assert.Single(res.Tips);
            Assert.Equal("Try spelling out abbreviations", tip.Text);
        }

        [Fact]
        public async Task PodIndexTest()
        {
            QueryRequest req = new QueryRequest("pi");
            req.PodIndex.Add(1);
            req.PodIndex.Add(2);

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.NotEmpty(res.Pods);

            //There is usually 8, we only asked for 2.
            Assert.Equal(2, res.Pods.Count);
        }

        [Fact]
        public async Task PodStatesTest()
        {
            QueryRequest req = new QueryRequest("pi");

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.NotEmpty(res.Pods);

            //Get the Decimal approximation pod
            Pod decApprox = res.Pods.Single(x => x.Title == "Decimal approximation");
            int length = decApprox.SubPods[0].Plaintext.Length;
            req.PodStates.Add(decApprox.States[0].Input);

            res = await Client.QueryAsync(req).ConfigureAwait(false);
            decApprox = res.Pods.Single(x => x.Title == "Decimal approximation");
            int length2 = decApprox.SubPods[0].Plaintext.Length;

            Assert.True(length2 > length);
        }

        [Fact]
        public async Task ReinterpretTest()
        {
            QueryRequest req = new QueryRequest("cantalope duck");
            req.Reinterpret = true;

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.True(res.IsSuccess);
            Assert.False(res.IsError);

            Warning warning = Assert.Single(res.Warnings);

            Assert.Equal("Using closest Wolfram|Alpha interpretation:", warning.Reinterpret.Text);
            Assert.Equal("cantalope", warning.Reinterpret.New);
            Assert.NotEqual(0, warning.Reinterpret.Score);
            Assert.Equal(Level.Medium, warning.Reinterpret.Level);

            Alternative alternative = Assert.Single(warning.Reinterpret.Alternatives);

            Assert.NotEqual(0, alternative.Score);
            Assert.Equal(Level.Low, alternative.Level);
            Assert.Equal("duck", alternative.Value);
        }

        [Fact]
        public async Task ScannerFilterTest()
        {
            QueryRequest req = new QueryRequest("pi");
            req.Scanners.Add("Numeric");

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);

            //There are 2 scanners with numeric.
            Assert.Equal(2, res.Pods.Count);
        }

        [Fact]
        public async Task SpellCheckTest()
        {
            QueryResponse res = await Client.QueryAsync("wolframalpa").ConfigureAwait(false);
            Assert.True(res.IsSuccess);
            Assert.False(res.IsError);

            Warning warning = Assert.Single(res.Warnings);

            Assert.Equal("wolframalpa", warning.SpellCheck.Word);
            Assert.Equal("wolframalpha", warning.SpellCheck.Suggestion);
            Assert.Equal("Interpreting \"wolframalpa\" as \"wolframalpha\"", warning.SpellCheck.Text);
        }

        [Fact]
        public async Task TimeoutTest()
        {
            QueryRequest req = new QueryRequest("pi");
            req.ScanTimeout = 1;
            req.TotalTimeout = 5;
            req.ParseTimeout = 7;
            req.PodTimeout = 8;
            req.FormatTimeout = 9;

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.True(res.IsSuccess);

            //Because ScanTimeout is 1, we should get a recalculate url
            Assert.NotNull(res.RecalculateUrl);

            await Client.RecalculateAsync(res).ConfigureAwait(false);

            Assert.Null(res.RecalculateUrl);
        }

        [Fact]
        public async Task TranslationTest()
        {
            QueryRequest req = new QueryRequest("Bonjour Monde");
            req.Translation = true;

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.True(res.IsSuccess);
            Assert.False(res.IsError);

            Assumption assumption = Assert.Single(res.Assumptions);

            Assert.Equal(AssumptionType.Clash, assumption.Type);
            Assert.Equal("hello world", assumption.Word);
            Assert.Equal("Assuming \"${word}\" is ${desc1}. Use as ${desc2} instead", assumption.Template);

            Assert.Equal(3, assumption.Values.Count);

            Assert.Equal("Miscellaneous", assumption.Values[0].Name);
            Assert.Equal("a phrase", assumption.Values[0].Description);
            Assert.Equal("*C.hello+world-_*Miscellaneous-", assumption.Values[0].Input);

            Assert.Equal("Movie", assumption.Values[1].Name);
            Assert.Equal("a movie", assumption.Values[1].Description);
            Assert.Equal("*C.hello+world-_*Movie-", assumption.Values[1].Input);

            Assert.Equal("MusicWork", assumption.Values[2].Name);
            Assert.Equal("a music work", assumption.Values[2].Description);
            Assert.Equal("*C.hello+world-_*MusicWork-", assumption.Values[2].Input);

            Warning warning = Assert.Single(res.Warnings);
            Assert.Equal("Bonjour Monde", warning.Translation.Phrase);
            Assert.Equal("hello world", warning.Translation.TranslatedText);
            Assert.Equal("French", warning.Translation.Language);
            Assert.Equal("Translating from French to \"hello world\"", warning.Translation.Text);
        }

        [Fact]
        public async Task UnitsTest()
        {
            QueryRequest req = new QueryRequest("distance from New York to Tokyo");
            req.OutputUnit = Unit.Metric;

            QueryResponse res = await Client.QueryAsync(req).ConfigureAwait(false);
            Assert.True(res.IsSuccess);

            Pod resultPod = res.Pods.Single(x => x.Title == "Result");
            Assert.Equal("10879 km (kilometers)", resultPod.SubPods[0].Plaintext);

            req.OutputUnit = Unit.NonMetric;
            res = await Client.QueryAsync(req).ConfigureAwait(false);

            resultPod = res.Pods.Single(x => x.Title == "Result");
            Assert.Equal("6760 miles", resultPod.SubPods[0].Plaintext);
        }
    }
}