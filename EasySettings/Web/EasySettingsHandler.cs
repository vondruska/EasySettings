﻿namespace EasySettings.Web
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

    class EasySettingsHandler : IRouteHandler, IHttpHandler, IReadOnlySessionState
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

            if (context.Request.HttpMethod == "POST")
            {
                string jsonString = String.Empty;

                var jsonSerializer = new JavaScriptSerializer();
                HttpContext.Current.Request.InputStream.Position = 0;
                using (StreamReader inputStream = new StreamReader(HttpContext.Current.Request.InputStream))
                {
                    jsonString = inputStream.ReadToEnd();
                }

                var newJson = jsonSerializer.Deserialize<ViewModel>(jsonString);

                if (!ValidateCsrfToken(context, newJson.Token))
                    throw new HttpRequestValidationException("Token was missing, or invalid");

                var settingsClass = GetSettingsClass(context);
                
                foreach (var item in newJson.Settings)
                {
                    var property = settingsClass.GetType().GetProperty(item.Name);
                    if (property == null || !property.CanWrite) continue;

                    var foo = TypeDescriptor.GetConverter(property.PropertyType);
                    var casted = foo.ConvertFromInvariantString(item.Value);

                    property.SetValue(settingsClass, casted);

                    Configuration.PersistantSettingsProvider.SaveSetting(item.Name, item.Value);
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
                            "The file \"{0}\" cannot be found as an embedded resource.".FormatWith(fileName));
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }

                var model = new ViewModel { Settings = GetSettings(context), Token = CreateCsrfToken(context) };

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
            HttpCookie csrfCookie = new HttpCookie("easysettings-token")
            {
                Value = guid,
                HttpOnly = true
            };
            context.Response.Cookies.Add(csrfCookie);

            return guid;
        }

        private IEnumerable<SettingViewModel> GetSettings(HttpContext context)
        {
            var theClass = GetSettingsClass(context);
            return theClass.GetType().GetProperties().Select(x => new SettingViewModel { Name = x.Name, Value = x.GetValue(theClass).ToString(), Description = GetDescription(x) }).ToList();
        }

        private object GetSettingsClass(HttpContext context)
        {
            var type = context.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            var assembly = type == null ? null : type.Assembly;

            if (assembly == null) return null;


            return (from t in assembly.GetTypes()
                            where t.BaseType == (typeof(BaseEasySettings)) && t.GetConstructor(Type.EmptyTypes) != null
                            select (BaseEasySettings)Activator.CreateInstance(t)).Single();
        }

        private string GetDescription(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
            if (attribute == null) return "";
            else return attribute.Description;
        }
    }
}
