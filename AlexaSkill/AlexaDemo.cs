using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET;
using Alexa.NET.Response;
using System.Collections.Generic;
using System.Xml;
using Microsoft.SyndicationFeed.Rss;
using System.Linq;
using System;
using Alexa.NET.LocaleSpeech;
using System.Globalization;

namespace DemoSample
{
    public static class AlexaDemo
    {
        [FunctionName("Alexa")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            string language = skillRequest.Request.Locale;

            bool isValid = await ValidateRequest(req, log, skillRequest);
            if (!isValid)
            {
                return new BadRequestResult();
            }

            var requestType = skillRequest.GetRequestType();
            var locale = SetupLanguages(skillRequest);

            SkillResponse response = null;

            if (requestType == typeof(LaunchRequest))
            {
                var message = await locale.Get("Welcome", null);
                response = ResponseBuilder.Tell(message);
                response.Response.ShouldEndSession = false;
            }
            else if (requestType == typeof(IntentRequest))
            {
                var intentRequest = skillRequest.Request as IntentRequest;
                if(intentRequest.Intent.Name == "TurnOnPerfectLight")
                {
                    response = SetTheFieldState(response, intentRequest, FieldStateEnum.Match);
                }
                else if (intentRequest.Intent.Name == "TurnOffPerfectLight")
                {
                    response = SetTheFieldState(response, intentRequest, FieldStateEnum.off);
                }
                else if (intentRequest.Intent.Name == "GetSiteIntent")
                {
                    var allSite = WebService.GetSites();
                    var siteName = allSite.Select(x => x.Name).ToList();
                    var message = "Currentaly connected site are" + string.Join(",", siteName.ToArray());
                    response = ResponseBuilder.Tell(message);
                }
                else if (intentRequest.Intent.Name == "GetFieldIntent")
                {
                    var slotValue = intentRequest.Intent.Slots.Select(x => x.Value).FirstOrDefault().Value;
                    var allFields = WebService.GetSites().Where(x => x.Name == slotValue).Select(x => x.Fields).ToList();
                    var fields = allFields.Select(x => x.Select(y => y.Name)).FirstOrDefault();
                    var message = "Currentaly connected field are" + string.Join(",", fields.ToArray());
                }
                else if (intentRequest.Intent.Name == "AMAZON.CancelIntent")
                {
                    var message = await locale.Get("Cancel", null);
                    response = ResponseBuilder.Tell(message);
                }
                else if (intentRequest.Intent.Name == "AMAZON.HelpIntent")
                {
                    var message = await locale.Get("Help", null);
                    response = ResponseBuilder.Tell(message);
                    response.Response.ShouldEndSession = false;
                }
                else if (intentRequest.Intent.Name == "AMAZON.StopIntent")
                {
                    var message = await locale.Get("Stop", null);
                    response = ResponseBuilder.Tell(message);
                }        
            }
            else if (requestType == typeof(SessionEndedRequest))
            {
                log.LogInformation("Session ended");
                response = ResponseBuilder.Empty();
                response.Response.ShouldEndSession = true;
            }


            return new OkObjectResult(response);
        }

        private static SkillResponse SetTheFieldState(SkillResponse response, IntentRequest intentRequest, FieldStateEnum fieldState)
        {
            if (intentRequest.Intent.Slots.TryGetValue("site", out var siteSlot) && intentRequest.Intent.Slots.TryGetValue("field", out var fieldSlot))
            {
                string message;
                if (!string.IsNullOrEmpty(siteSlot.Value) && !string.IsNullOrEmpty(fieldSlot.Value))
                {
                    var siteName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(siteSlot.Value.ToLower());
                    var fieldName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fieldSlot.Value.ToLower());
                    var allFields = WebService.GetSites().FirstOrDefault(x => x.Name == siteName)?.Fields;
                    var selectedfield = allFields.Where(x => x.Name == fieldName).Select(y => y.Id).FirstOrDefault();
                    var result = WebService.SetFieldState(selectedfield, fieldState);
                    if (result.IsSuccessful)
                    {
                         message = $"Current Status of {fieldSlot.Value} is {fieldState.ToString()} state";
                       
                    }
                    else
                    {
                        message =result.Content;

                    }
                    response = ResponseBuilder.Tell(message);
                }
            }

            return response;
        }

        private static async Task<List<string>> ParseFeed(string url)
        {
            List<string> news = new List<string>();
            using (var xmlReader = XmlReader.Create(url, new XmlReaderSettings { Async = true }))
            {
                RssFeedReader feedReader = new RssFeedReader(xmlReader);
                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == Microsoft.SyndicationFeed.SyndicationElementType.Item)
                    {
                        var item = await feedReader.ReadItem();
                        news.Add(item.Title);
                    }
                }
            }

            return news;
        }

        private static async Task<bool> ValidateRequest(HttpRequest request, ILogger log, SkillRequest skillRequest)
        {
            request.Headers.TryGetValue("SignatureCertChainUrl", out var signatureChainUrl);
            if (string.IsNullOrWhiteSpace(signatureChainUrl))
            {
                log.LogError("Validation failed. Empty SignatureCertChainUrl header");
                return false;
            }

            Uri certUrl;
            try
            {
                certUrl = new Uri(signatureChainUrl);
            }
            catch
            {
                log.LogError($"Validation failed. SignatureChainUrl not valid: {signatureChainUrl}");
                return false;
            }

            request.Headers.TryGetValue("Signature", out var signature);
            if (string.IsNullOrWhiteSpace(signature))
            {
                log.LogError("Validation failed - Empty Signature header");
                return false;
            }

            request.Body.Position = 0;
            var body = await request.ReadAsStringAsync();
            request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
            {
                log.LogError("Validation failed - the JSON is empty");
                return false;
            }

            bool isTimestampValid = RequestVerification.RequestTimestampWithinTolerance(skillRequest);
            bool valid = await RequestVerification.Verify(signature, certUrl, body);

            if (!valid || !isTimestampValid)
            {
                log.LogError("Validation failed - RequestVerification failed");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static ILocaleSpeech SetupLanguages(SkillRequest skillRequest)
        {
            var store = new DictionaryLocaleSpeechStore();
            store.AddLanguage("en", new Dictionary<string, object>
            {
                { "Welcome", "Welcome to the Sample skill!" },             
                { "Stop", "Goodbye!" }
            });         


            var localeSpeechFactory = new LocaleSpeechFactory(store);
            var locale = localeSpeechFactory.Create(skillRequest);

            return locale;
        }

        public enum FieldStateEnum { Match, off};
    }
}
