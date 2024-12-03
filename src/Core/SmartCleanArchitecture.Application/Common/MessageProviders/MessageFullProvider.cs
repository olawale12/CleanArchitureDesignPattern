using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartCleanArchitecture.Application.Common.Enums;
using SmartCleanArchitecture.Application.Common.Interfaces;


namespace SmartCleanArchitecture.Application.Common.MessageProviders
{
    public class MessageFullProvider : IMessageFullProvider
    {
        private readonly MessageFullSettings _settings;
        private readonly IDictionary<string, MessageFull> _fulls;
        private readonly ILanguageService _languageService;



        public MessageFullProvider(IOptions<MessageFullSettings> settingsProvider, ILanguageService languageService)
        {
            _settings = settingsProvider.Value;
            _fulls = new Dictionary<string, MessageFull>();
            _languageService = languageService;
            CreateLanguagePacks();
        }

        private void CreateLanguagePacks()
        {
            var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();

            try
            {

                string resourceFolderPath = $"{_settings.BaseLocation}";
                string[] filesLocation = Directory.GetFiles(resourceFolderPath);
                List<string> paths = new List<string>();
                foreach (var file in filesLocation)
                {
                    paths.Add(Path.Combine(Directory.GetCurrentDirectory(), file));
                }

                var mappings = new List<IDictionary<string, string>>();
                foreach (var path in paths)
                {
                    if (File.Exists(path))
                    {
                        var reader = File.OpenText(path);
                        var mapping = (Dictionary<string, string>)serializer.Deserialize(reader, typeof(Dictionary<string, string>));

                        mappings.Add(mapping);

                    }
                }


                var messages = new Dictionary<OtpPurpose, string>();

                if (!string.IsNullOrEmpty(_settings.PasswordResetMessage))
                {
                    messages[OtpPurpose.PASSWORD_RESET] = _settings.PasswordResetMessage;
                }
                if (!string.IsNullOrEmpty(_settings.OnboardingMessage))
                {
                    messages[OtpPurpose.SSO_ONBOARDING] = _settings.OnboardingMessage;
                }
                if (!string.IsNullOrEmpty(_settings.TwoFactorAuthentication))
                {
                    messages[OtpPurpose.TWO_FACTOR_AUTHENTICATION] = _settings.TwoFactorAuthentication;
                }

                // full.NotificationMessages = messages;
                // fullfr.NotificationMessages = messages;

                for (var i = 0; i < paths.Count; i++)
                {
                    var full = new MessageFull(_settings.DefaultMessage);
                    string lang = GetLastWordBeforeJson(paths[i]);
                    full.Mappings = mappings[i];
                    _fulls.Add(lang, full);

                }


            }
            catch (Exception e)
            {
                
            }
        }
        public MessageFull GetPack()
        {
            var languageCode = GetLanguageCode();
            return (_fulls.TryGetValue(languageCode, out var pack)) ? pack : null;
           
        }

        private string GetLanguageCode()
        {
            return _languageService.GetLanguageCode();
        }

        static string GetLastWordBeforeJson(string fileName)
        {
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            string[] parts = nameWithoutExtension.Split('-');
            return parts[^1];
        }
    }
}
