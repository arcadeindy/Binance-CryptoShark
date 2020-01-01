using CryptoShark.Hunting.Data;
using CryptoShark.Hunting.VolumeHunting;
using CryptoShark.Utility;
using Newtonsoft.Json.Linq;
using Quantum.Framework.GenericProperties.Controls.GenericPropertyListControl;
using Quantum.Framework.GenericProperties.Controls.GenericPropertyListControl.Enum;
using Quantum.Framework.GenericProperties.Data;
using System;
using System.IO;
using System.Windows.Forms;

namespace CryptoShark.Forms
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            // general properties
            genericPropertyListControl.Properties = SettingsHelper.GetGeneralProperties();
            genericPropertyListControl.RefreshItems();

            // hunting properties
            var huntingTypes = HuntingTypeManager.Instance.HuntingTypes;
            foreach (var huntingType in huntingTypes)
            {
                var properties = huntingType.GetProperties();
                var jArrayProperties = SettingsHelper.GetJArrayHuntingProperties(huntingType.TypeName);
                if (jArrayProperties != null)
                    GenericPropertySerializer.DeserializePropertiesFromArray(properties, jArrayProperties);

                var tabPage = new TabPage()
                {
                    Text = huntingType.DisplayName,
                    Tag = huntingType
                };

                var propertyControl = new GenericPropertyListControl
                {
                    Dock = DockStyle.Fill,
                    Properties = properties
                };
                propertyControl.Options.ViewMode = ViewMode.CategoryList;

                tabPage.Controls.Add(propertyControl);
                tabControl.TabPages.Add(tabPage);

                propertyControl.RefreshItems();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var jObjectSettings = new JObject();

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage.Tag is HuntingType huntingType)
                {
                    GenericPropertyCollection properties = null;
                    foreach (Control control in tabPage.Controls)
                    {
                        if (control is GenericPropertyListControl propertyControl)
                            properties = propertyControl.Properties;
                    }

                    if (properties != null)
                        jObjectSettings[huntingType.TypeName + Constants.HUNTING_PROPERTIES_POSTFIX] = GenericPropertySerializer.SerializePropertiesToArray(properties);
                }
            }

            jObjectSettings[Constants.GENERAL_PROPERTIES] = GenericPropertySerializer.SerializePropertiesToArray(genericPropertyListControl.Properties);

            SettingsHelper.SaveSettings(jObjectSettings);

            DialogResult = DialogResult.OK;
        }
    }
}
