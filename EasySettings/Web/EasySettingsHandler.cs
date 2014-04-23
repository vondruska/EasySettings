namespace EasySettings.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Routing;
    using System.Web.Script.Serialization;
    using System.Web.SessionState;

    using Extensions;

    public class EasySettingsHandler : IRouteHandler, IHttpHandler, IRequiresSessionState
    {
        const string TokenCookieName = "easysettings-token";
        
        public bool IsReusable
        {
            get { return true; }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        public void ProcessRequest(HttpContext context)
        {
            var outputStream = context.Response.Output;

            var settingsClass = new SettingsClassHelper(context);


            switch (context.Request.HttpMethod)
            {
                case "POST":
                    outputStream.Write(ProcessPost(context.Request.InputStream, settingsClass, context.Request.Cookies));
                    break;
                default:
                    outputStream.Write(ProcessGet(settingsClass, context.Response.Cookies));
                    break;
            }
        }

        internal string ProcessGet(SettingsClassHelper helper, HttpCookieCollection responseCookies)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var fileName = "{0}.Page.html".FormatWith(GetType().Namespace);

            string fileContent;
            using (var stream = assembly.GetManifestResourceStream(fileName))
            {
                if (stream == null)
                    throw new Exception("The UI can't be found.");

                using (var reader = new StreamReader(stream))
                    fileContent = reader.ReadToEnd();
            }

            var model = new ViewModel
                {
                    Settings = InflateSettingsViewModel(helper), 
                    Token = CreateCsrfToken(responseCookies)
                };

            foreach (var item in Configuration.SettingsProvider.GetAllValues())
            {
                model.Settings.First(x => x.Name == item.Key).Value = item.Value;
            }

            var outputJson = new JavaScriptSerializer().Serialize(model);

            fileContent = fileContent.Replace("{data}", outputJson);

            return fileContent;
        }

        internal string ProcessPost(Stream inputStream, SettingsClassHelper helper, HttpCookieCollection requestCookies)
        {
            string jsonString;

            var jsonSerializer = new JavaScriptSerializer();
            inputStream.Position = 0;
            using (var stream = new StreamReader(inputStream))
            {
                jsonString = stream.ReadToEnd();
            }

            var newJson = jsonSerializer.Deserialize<ViewModel>(jsonString);

            if (!ValidateCsrfToken(requestCookies, newJson.Token))
                throw new HttpRequestValidationException("Token was missing, or invalid");

            foreach (var item in newJson.Settings)
            {
                if (helper.IsValidValue(item.Name, item.Value))
                {
                    Configuration.SettingsProvider.SaveSetting(item.Name, item.Value);
                }

                //TODO: create return json array to tell which properties didn't save with invalid values
            }

            return "";
        }

        private bool ValidateCsrfToken(HttpCookieCollection cookies, string submittedToken)
        {
            if (cookies[TokenCookieName] == null) return false;

            return cookies[TokenCookieName].Value == submittedToken;
        }

        private string CreateCsrfToken(HttpCookieCollection cookies)
        {
            var guid = Guid.NewGuid().ToString("N");
            var csrfCookie = new HttpCookie(TokenCookieName)
            {
                Value = guid,
                HttpOnly = true
            };
            cookies.Add(csrfCookie);

            return guid;
        }

        internal IEnumerable<SettingViewModel> InflateSettingsViewModel(SettingsClassHelper helper)
        {
            return
                helper.GetProperties().Select(
                            x =>
                                new SettingViewModel
                                    {
                                        Name = x.Name,
                                        Value = (x.GetValue(helper.TheClass) ?? "").ToString(),
                                        Description = x.GetDescription(),
                                        Type = DetermineSettingType(x),
                                        PossibleValues = DetermineSettingType(x) == SettingType.Enum ? Enum.GetNames(x.PropertyType) : null
                                    })
                        .ToList();
        }

        private static SettingType DetermineSettingType(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsEnum) return SettingType.Enum;
            if (propertyInfo.PropertyType == typeof(bool)) return SettingType.Boolean;
            
            return SettingType.String;
        }
    }
}
