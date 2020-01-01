using CryptoShark.Utility.Enum;
using Newtonsoft.Json.Linq;
using Quantum.Framework.GenericProperties.Data;
using Quantum.Framework.GenericProperties.Enum;
using System.IO;

namespace CryptoShark.Utility
{
    public class SettingsHelper
    {
        #region Singleton Members

        private static SettingsHelper instance;
        public static SettingsHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsHelper();
                    instance.genericProperties = GetGeneralProperties();
                }
                return instance;
            }
        }

        private GenericPropertyCollection genericProperties;

        #endregion

        private static JObject LoadSettings()
        {
            if (File.Exists(Constants.SETTINGS_FILENAME))
            {
                var contents = File.ReadAllText(Constants.SETTINGS_FILENAME);
                return JObject.Parse(contents);
            }
            else
                return null;
        }

        public static void SaveSettings(JObject jObjectSettings)
        {
            File.WriteAllText(Constants.SETTINGS_FILENAME, jObjectSettings.ToString());
        }

        public static JArray GetJArrayHuntingProperties(string typeName)
        {
            var jObjectSettings = LoadSettings();
            if (jObjectSettings != null && jObjectSettings[typeName + Constants.HUNTING_PROPERTIES_POSTFIX] != null)
                return (JArray)jObjectSettings[typeName + Constants.HUNTING_PROPERTIES_POSTFIX];
            else
                return null;
        }

        private static JArray GetJArrayGeneralProperties()
        {
            var jObjectSettings = LoadSettings();
            if (jObjectSettings != null && jObjectSettings[Constants.GENERAL_PROPERTIES] != null)
                return (JArray)jObjectSettings[Constants.GENERAL_PROPERTIES];
            else
                return null;
        }

        public static GenericPropertyCollection GetGeneralProperties()
        {
            var properties = new GenericPropertyCollection()
            {
                new GenericProperty()
                {
                    Browsable = true,
                    Name = SettingName.API_KEY,
                    DisplayName = "Api Key",
                    DefaultValue = string.Empty,
                    Type = GenericPropertyType.String
                },
                new GenericProperty()
                {
                    Browsable = true,
                    Name = SettingName.SECRET_KEY,
                    DisplayName = "Secret Key",
                    DefaultValue = string.Empty,
                    Type = GenericPropertyType.String
                },
                new GenericProperty()
                {
                    Browsable = true,
                    Name = SettingName.TRADING_ENABLED,
                    DisplayName = "Trading Enable",
                    DefaultValue = false,
                    Type = GenericPropertyType.Boolean
                }
            };

            var jArrayProperties = GetJArrayGeneralProperties();
            if (jArrayProperties != null)
                GenericPropertySerializer.DeserializePropertiesFromArray(properties, jArrayProperties);

            return properties;
        }

        public T GetSetting<T>(string name, T defaultValue)
        {
            var properties = GetGeneralProperties();
            if (properties.Contains(name))
                return properties.Get(name, defaultValue);
            else
                return defaultValue;
        }

        public void Invalidate()
        {
            genericProperties = GetGeneralProperties();
        }
    }
}