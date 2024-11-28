using Microsoft.Extensions.Options;
using SmartCleanArchitecture.Application.Common.Enums;
using SmartCleanArchitecture.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.MessageProviders
{
    public class MessageFullProvider : IMessageFullProvider
    {
        private readonly MessageFullSettings _settings;
        private const string FILE_EXTENSION = ".json";
        private readonly IDictionary<string, MessageFull> _fullsEn;
        private readonly IDictionary<string, MessageFull> _fullsFr;
        public MessageFullProvider(IOptions<MessageFullSettings> settingsProvider)
        {
            _settings = settingsProvider.Value;
            _fullsEn = new Dictionary<string, MessageFull>();
            _fullsFr = new Dictionary<string, MessageFull>();
            CreateLanguagePacks();
        }

        private void CreateLanguagePacks()
        {
            var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();

            try
            {
                var full = new MessageFull(_settings.DefaultMessage);
                var fullfr = new MessageFull(_settings.DefaultMessage);
                var location = $"{_settings.BaseLocation}{Path.DirectorySeparatorChar}response-codes{FILE_EXTENSION}";
                var locationFr = $"{_settings.BaseLocation}{Path.DirectorySeparatorChar}response-codes-fr{FILE_EXTENSION}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), location);
                var pathfr = Path.Combine(Directory.GetCurrentDirectory(), locationFr);
                if (!File.Exists(path))
                {
                    // _logger.LogWarning("No bundle file found");
                    return;
                }
                var reader = File.OpenText(path);

                full.Mappings = (Dictionary<string, string>)serializer.Deserialize(reader, typeof(Dictionary<string, string>));
                StreamReader? readerfr;
                if (File.Exists(path))
                {
                    readerfr = File.OpenText(path);
                    fullfr.Mappings = (Dictionary<string, string>)serializer.Deserialize(readerfr, typeof(Dictionary<string, string>));
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

                full.NotificationMessages = messages;
                fullfr.NotificationMessages = messages;
                _fullsEn.Add("default", full);
                _fullsFr.Add("default", fullfr);
            }
            catch (Exception e)
            {
                //  _logger.LogError(e, "Failed to load language pack");
                //Console.WriteLine(e.Message);
            }
        }
        public MessageFull GetPack(string languageCode = "en")
        {
            if (languageCode == "en")
            {
                return (_fullsEn.TryGetValue("default", out var pack)) ? pack : null;
            }
            else
            {
                return (_fullsFr.TryGetValue("default", out var pack)) ? pack : null;
            }

        }
    }
}
