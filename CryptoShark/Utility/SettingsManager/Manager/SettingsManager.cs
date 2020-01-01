using CryptoShark.Utility.SettingsManager.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Utility.SettingsManager.Manager
{
    public class SettingsManager
    {
        #region Singleton Members
        private static SettingsManager instance;
        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SettingsManager();
                return instance;
            }
        }
        #endregion

        public List<SettingsItem> Items { get; set; }

        public bool AutoSave { get; set; }
        public string FileName { get; set; }

        public bool IsAutoSaveSuspended { get; set; }

        public void SuspendAutoSave()
        {
            IsAutoSaveSuspended = true;
        }

        public void ResumeAutoSave()
        {
            IsAutoSaveSuspended = false;
        }

        public SettingsManager()
        {
            Items = new List<SettingsItem>();

            FileName = string.Empty;
        }

        public void Save()
        {
            Save(FileName);
        }

        public void Save(string fileName)
        {
            FileName = fileName;

            var jArraySettings = new JArray();

            foreach (var item in Items)
            {
                if (item.Value != null)
                {
                    var jObjectSettingsItem = new JObject();
                    jObjectSettingsItem["name"] = item.Name;

                    if (item.Value is string)
                    {
                        jObjectSettingsItem["type"] = "string";
                        jObjectSettingsItem["value"] = Convert.ToString(item.Value);
                    }
                    else if (item.Value is int)
                    {
                        jObjectSettingsItem["type"] = "integer";
                        jObjectSettingsItem["value"] = Convert.ToInt32(item.Value);
                    }
                    else if (item.Value is decimal)
                    {
                        jObjectSettingsItem["type"] = "double";
                        jObjectSettingsItem["value"] = Convert.ToDouble(item.Value);
                    }
                    else if (item.Value is bool)
                    {
                        jObjectSettingsItem["type"] = "boolean";
                        jObjectSettingsItem["value"] = Convert.ToBoolean(item.Value);
                    }
                    else if (item.Value is Guid)
                    {
                        jObjectSettingsItem["type"] = "guid";
                        jObjectSettingsItem["value"] = ((Guid)item.Value).ToString();
                    }
                    else if (item.Value is Point point)
                    {
                        jObjectSettingsItem["type"] = "point";
                        jObjectSettingsItem["value"] = new JObject()
                        {
                            ["x"] = point.X,
                            ["y"] = point.Y
                        };
                    }
                    else if (item.Value is Size size)
                    {
                        jObjectSettingsItem["type"] = "size";
                        jObjectSettingsItem["value"] = new JObject()
                        {
                            ["width"] = size.Width,
                            ["height"] = size.Height
                        };
                    }
                    else if (item.Value is DateTime dateTime)
                    {
                        jObjectSettingsItem["type"] = "datetime";
                        var dateTimeUtc = dateTime.ToUniversalTime();
                        jObjectSettingsItem["value"] = dateTimeUtc.ToString("o");
                    }
                    else if (item.Value is Version version)
                    {
                        jObjectSettingsItem["type"] = "version";
                        jObjectSettingsItem["value"] = version.ToString(3);
                    }
                    else if (item.Value is List<string> stringList)
                    {
                        jObjectSettingsItem["type"] = "list:String";

                        var jArrayStringList = new JArray();
                        foreach (var str in stringList)
                            jArrayStringList.Add(str);

                        jObjectSettingsItem["value"] = jArrayStringList;
                    }

                    jArraySettings.Add(jObjectSettingsItem);
                }
            }

            File.WriteAllText(fileName, jArraySettings.ToString(), Encoding.UTF8);
        }

        public void Load()
        {
            Load(FileName);
        }

        public void Load(string fileName)
        {
            FileName = fileName;

            if (File.Exists(fileName))
            {
                var contents = File.ReadAllText(fileName, Encoding.UTF8);
                var jObjectSettings = JArray.Parse(contents);


                Items.Clear();

                SuspendAutoSave();

                foreach (JObject jObjectSettingsItem in jObjectSettings)
                {
                    try
                    {
                        var name = JObjectHelper.GetString(jObjectSettingsItem, "name", string.Empty);
                        var type = JObjectHelper.GetString(jObjectSettingsItem, "type", string.Empty);

                        object value = null;

                        if (type == "string")
                            value = JObjectHelper.GetString(jObjectSettingsItem, "value", string.Empty);
                        else if (type == "integer")
                            value = JObjectHelper.GetInt32(jObjectSettingsItem, "value", 0);
                        else if (type == "double")
                            value = JObjectHelper.GetDecimal(jObjectSettingsItem, "value", 0);
                        else if (type == "boolean")
                            value = JObjectHelper.GetBoolean(jObjectSettingsItem, "value", false);
                        else if (type == "guid")
                            value = JObjectHelper.GetGuid(jObjectSettingsItem, "value", Guid.Empty);
                        else if (type == "point")
                        {
                            var x = JObjectHelper.GetInt32(jObjectSettingsItem, "x", 0);
                            var y = JObjectHelper.GetInt32(jObjectSettingsItem, "y", 0);
                            value = new Point(x, y);
                        }
                        else if (type == "size")
                        {
                            var width = JObjectHelper.GetInt32(jObjectSettingsItem, "width", 0);
                            var height = JObjectHelper.GetInt32(jObjectSettingsItem, "height", 0);
                            value = new Size(width, height);
                        }
                        else if (type == "datetime")
                        {
                            var valueStr = JObjectHelper.GetString(jObjectSettingsItem, "value", DateTime.UtcNow.ToString("o"));
                            value = DateTime.Parse(valueStr);
                        }
                        else if (type == "version")
                        {
                            value = new Version(JObjectHelper.GetString(jObjectSettingsItem, "value"));
                        }
                        else if (type == "list:String")
                        {
                            value = new List<string>();
                            var jArrayStrings = (JArray)jObjectSettingsItem["value"];
                            foreach (string str in jArrayStrings)
                            {
                                ((List<string>)value).Add(str);
                            }
                        }

                        var settingsItem = new SettingsItem(name);
                        settingsItem.OnChange += settingsItem_OnChange;
                        settingsItem.Value = value;
                        Items.Add(settingsItem);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                ResumeAutoSave();
            }
        }

        public void ClearItems()
        {
            Items.Clear();
        }

        public SettingsItem GetItem(string name)
        {
            foreach (var item in Items)
            {
                if (item.Name == name)
                    return item;
            }

            return null;
        }

        public void AddItem(string name, object value)
        {
            var settingsItem = new SettingsItem(name)
            {
                Value = value
            };

            settingsItem.OnChange += settingsItem_OnChange;

            Items.Add(settingsItem);
        }

        void settingsItem_OnChange(object sender, EventArgs e)
        {
            if (AutoSave && !IsAutoSaveSuspended)
            {
                var settingsManagerDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"VFabrika");
                if (!Directory.Exists(settingsManagerDirectoryPath))
                    Directory.CreateDirectory(settingsManagerDirectoryPath);

                Save();
            }
        }

        public bool RemoveItem(string name)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (item.Name == name)
                {
                    item.OnChange -= settingsItem_OnChange;
                    Items.RemoveAt(i);

                    return true;
                }
            }

            return false;
        }

        public bool ContainsItem(string name)
        {
            foreach (var item in Items)
            {
                if (item.Name.Equals(name))
                {
                    if (item.Value != null)
                        return true;
                }
            }
            return false;
        }

        public T Get<T>(string name)
        {
            return Get<T>(name, default(T));
        }

        public T Get<T>(string name, T defaultValue)
        {
            foreach (var item in Items)
                if (item.Name == name)
                    return item.Value != null ? (T)item.Value : defaultValue;

            return defaultValue;
        }

        public void Set(string name, object value)
        {
            var item = GetItem(name);
            if (item == null)
            {
                item = new SettingsItem();
                item.OnChange += settingsItem_OnChange;
                Items.Add(item);
                item.Name = name;
                item.Value = value;
            }
            else
                item.Value = value;
        }
    }

}
