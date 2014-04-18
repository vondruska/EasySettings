namespace EasySettings.Web
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.Routing;
    using System.Web.Script.Serialization;
    using System.Web.SessionState;

    using Extensions;

    public class EasySettingsHandler : IRouteHandler, IHttpHandler, IRequiresSessionState
    {
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

            if (context.Request.HttpMethod == "POST")
            {
                var jsonString = String.Empty;

                var jsonSerializer = new JavaScriptSerializer();
                HttpContext.Current.Request.InputStream.Position = 0;
                using (var inputStream = new StreamReader(HttpContext.Current.Request.InputStream))
                {
                    jsonString = inputStream.ReadToEnd();
                }

                var newJson = jsonSerializer.Deserialize<ViewModel>(jsonString);

                if (!ValidateCsrfToken(context, newJson.Token))
                    throw new HttpRequestValidationException("Token was missing, or invalid");

                foreach (var item in newJson.Settings)
                {
                    if (settingsClass.IsValidValue(item.Name, item.Value))
                        Configuration.PersistantSettingsProvider.SaveSetting(item.Name, item.Value);

                    //TODO: create return json array to tell which properties didn't save
                }
            }
            else
            {
                var assembly = Assembly.GetExecutingAssembly();

                var fileName = "{0}.Page.html".FormatWith(GetType().Namespace);

                string fileContent;
                using (var stream = assembly.GetManifestResourceStream(fileName))
                {
                    if (stream == null)
                    {
                        throw new Exception(
                            "The HTML file can't be found.".FormatWith(fileName));
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }

                var model = new ViewModel { Settings = InflateSettingsViewModel(settingsClass), Token = CreateCsrfToken(context) };

                foreach (var item in Configuration.PersistantSettingsProvider.GetAllValues())
                {
                    model.Settings.First(x => x.Name == item.Key).Value = item.Value;
                }

                var outputJson = new JavaScriptSerializer().Serialize(model);

                fileContent = fileContent.Replace("{data}", outputJson);
                outputStream.Write(fileContent);
            }
        }

        private bool ValidateCsrfToken(HttpContext context, string submittedToken)
        {
            if (context.Request.Cookies["easysettings-token"] == null) return false;

            return context.Request.Cookies["easysettings-token"].Value == submittedToken;
        }

        private string CreateCsrfToken(HttpContext context)
        {
            var guid = Guid.NewGuid().ToString("N");
            var csrfCookie = new HttpCookie("easysettings-token")
            {
                Value = guid,
                HttpOnly = true
            };
            context.Response.Cookies.Add(csrfCookie);

            return guid;
        }

        protected IEnumerable<SettingViewModel> InflateSettingsViewModel(SettingsClassHelper helper)
        {
            return
                helper.GetProperties().Select(
                            x =>
                                new SettingViewModel
                                    {
                                        Name = x.Name,
                                        Value = x.GetValue(helper.TheClass).ToString(),
                                        Description = x.GetDescription(),
                                        Type = DetermineSettingType(x),
                                        PossibleValues = DetermineSettingType(x) == SettingType.Enum ? Enum.GetNames(x.PropertyType) : null
                                    })
                        .ToList();
        }

        protected SettingType DetermineSettingType(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsEnum) return SettingType.Enum;
            if (propertyInfo.PropertyType == typeof(bool)) return SettingType.Boolean;
            
            return SettingType.String;
        }

        
    }
}
