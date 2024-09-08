using Microsoft.Extensions.Options;
using SmartCleanArchitecture.Application.Common.Enums;
using SmartCleanArchitecture.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
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
        private readonly IDictionary<string, MessageFull> _fulls;

        public MessageFullProvider(IOptions<MessageFullSettings> settingsProvider)
        {
            _settings = settingsProvider.Value;
            _fulls = new Dictionary<string, MessageFull>();
            CreateLanguagePacks();
        }

        private void CreateLanguagePacks()
        {
            var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();

            try
            {
                var full = new MessageFull(_settings.DefaultMessage);
                var location = $"{_settings.BaseLocation}{Path.DirectorySeparatorChar}response-codes{FILE_EXTENSION}";
               var path = Path.Combine(Directory.GetCurrentDirectory(), location);
                if (!File.Exists(path))
                {
                   // _logger.LogWarning("No bundle file found");
                    return;
                }
                var reader = File.OpenText(path);

                full.Mappings = (Dictionary<string, string>)serializer.Deserialize(reader, typeof(Dictionary<string, string>));
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
                _fulls.Add("default", full);
            }
            catch (Exception e)
            {
              //  _logger.LogError(e, "Failed to load language pack");
              //Console.WriteLine(e.Message);
            }
        }
        public MessageFull GetPack()
        {
            return (_fulls.TryGetValue("default", out var pack)) ? pack : null;
        }
    }
}
