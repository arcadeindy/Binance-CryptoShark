using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Utility.SettingsManager.Data
{
    public class SettingsItem
    {
        public event EventHandler<EventArgs> OnChange;

        public string Name { get; set; }

        private object value_;

        public object Value
        {
            get { return value_; }
            set
            {
                value_ = value;

                OnChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public SettingsItem()
            : this(string.Empty)
        {

        }

        public SettingsItem(string name)
        {
            Name = name;
        }

        public int ToInt32()
        {
            return ToInteger(0);
        }

        public int ToInteger(int defaultValue)
        {
            return Value != null ? Convert.ToInt32(Value, CultureInfo.InvariantCulture) : defaultValue;
        }

        public override string ToString()
        {
            return ToString(string.Empty);
        }

        public string ToString(string defaultValue)
        {
            return Value != null ? Convert.ToString(Value, CultureInfo.InvariantCulture) : defaultValue;
        }

        public bool ToBoolean()
        {
            return ToBoolean(false);
        }

        public bool ToBoolean(bool defaultValue)
        {
            return Value != null ? Convert.ToBoolean(Value, CultureInfo.InvariantCulture) : defaultValue;
        }

        public double ToDouble()
        {
            return ToDouble(0);
        }

        public double ToDouble(double defaultValue)
        {
            return Value != null ? Convert.ToDouble(Value, CultureInfo.InvariantCulture) : defaultValue;
        }

        public object ToObject()
        {
            return ToObject(null);
        }

        public object ToObject(object defaultValue)
        {
            return Value != null ? Value : defaultValue;
        }

        public Guid ToGuid()
        {
            return ToGuid(Guid.Empty);
        }

        public Guid ToGuid(Guid defaultValue)
        {
            return Value != null ? new Guid(Convert.ToString(Value)) : defaultValue;
        }
    }

}
