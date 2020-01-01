using CryptoShark.Hunting.Data;
using Quantum.Framework.GenericProperties.Data;
using Quantum.Framework.GenericProperties.Enum;
using System.Collections.Generic;

namespace CryptoShark.Hunting.VolumeHunting
{
    public class VolumeHuntingType : HuntingType
    {
        public override string TypeName => "volumeHunting";
        public override string DisplayName => "Volume Hunting";

        public override Data.Hunting CreateInstance()
        {
            return new VolumeHunting()
            {
                Properties = GetProperties()
            };
        }

        public override GenericPropertyCollection GetProperties()
        {
            var customPropertiesDecimalPlaces = new Dictionary<string, object>()
            {
                ["decimalPlaces"] = 3
            };

            return new GenericPropertyCollection()
            {
                // hunting properties
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "volumeIncreaseThreshold",
                    DisplayName = "Volume Increase Threshold",
                    MaximumValue = decimal.MaxValue,
                    MinimumValue = 0m,
                    DefaultValue = 0m,
                    CustomProperties = customPropertiesDecimalPlaces,
                    Type = GenericPropertyType.Decimal
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "minimumVolume",
                    DisplayName = "Minimum Volume",
                    MaximumValue = decimal.MaxValue,
                    MinimumValue = 0m,
                    DefaultValue = 0m,
                    CustomProperties = customPropertiesDecimalPlaces,
                    Type = GenericPropertyType.Decimal
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "minimumPrice",
                    DisplayName = "Minimum Price",
                    MaximumValue = decimal.MaxValue,
                    MinimumValue = 0m,
                    DefaultValue = 0m,
                    CustomProperties = new Dictionary<string, object>()
                    {
                        ["decimalPlaces"] = 8
                    },
                    Type = GenericPropertyType.Decimal
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "minimumBidAskDifferencePercent",
                    DisplayName = "Minimum Bid-Ask Difference Percent",
                    MaximumValue = decimal.MaxValue,
                    MinimumValue = 0m,
                    DefaultValue = 0m,
                    CustomProperties = customPropertiesDecimalPlaces,
                    Type = GenericPropertyType.Decimal
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "checkIntervalMinutes",
                    DisplayName = "Check Interval Minutes",
                    MaximumValue = 2880,
                    MinimumValue = 1,
                    DefaultValue = 10,
                    Type = GenericPropertyType.Integer
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "getVolumeIntervalMinutes",
                    DisplayName = "Get Volume Interval Minutes",
                    MaximumValue = 2880,
                    MinimumValue = 1,
                    DefaultValue = 1,
                    Type = GenericPropertyType.Integer
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "priceChangeThreshold",
                    DisplayName = "Price Change Threshold",
                    MaximumValue = decimal.MaxValue,
                    MinimumValue = 0m,
                    DefaultValue = 0m,
                    Value = 0m,
                    CustomProperties = customPropertiesDecimalPlaces,
                    Type = GenericPropertyType.Decimal
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "priceDecreaseTriggerThreshold",
                    DisplayName = "Price Decrease Trigger Threshold",
                    MaximumValue = decimal.MaxValue,
                    MinimumValue = 0m,
                    DefaultValue = 0m,
                    Value = 0m,
                    CustomProperties = customPropertiesDecimalPlaces,
                    Type = GenericPropertyType.Decimal
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "hunting",
                    CategoryDisplayName = "Hunting",
                    Name = "priceDifferenceType",
                    DisplayName = "Price Difference Type",
                    DefaultValue = "lower",
                    EnumItems = new List<GenericPropertyEnumItem>()
                    {
                        new GenericPropertyEnumItem("Higher", "higher"),
                        new GenericPropertyEnumItem("Lower", "lower"),
                    },
                    Type = GenericPropertyType.Enumeration
                },
                // trading properties
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "trading",
                    CategoryDisplayName = "Trading",
                    Name = "mainFund",
                    DisplayName = "Main Fund",
                    DefaultValue = "BTC",
                    EnumItems = new List<GenericPropertyEnumItem>()
                    {
                        new GenericPropertyEnumItem("BTC", "BTC"),
                        new GenericPropertyEnumItem("ETH", "ETH"),
                        new GenericPropertyEnumItem("BNB", "BNB"),
                        new GenericPropertyEnumItem("USDT", "USDT"),
                    },
                    Type = GenericPropertyType.Enumeration
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "trading",
                    CategoryDisplayName = "Trading",
                    Name = "mainFundParts",
                    DisplayName = "Main Fund Parts",
                    MaximumValue = 20,
                    MinimumValue = 1,
                    DefaultValue = 1,
                    Type = GenericPropertyType.Integer
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "trading",
                    CategoryDisplayName = "Trading",
                    Name = "profitPercentage",
                    DisplayName = "Profit Percentage",
                    MaximumValue = decimal.MaxValue,
                    MinimumValue = 0m,
                    DefaultValue = 0.05m,
                    Value = 0m,
                    CustomProperties = customPropertiesDecimalPlaces,
                    Type = GenericPropertyType.Decimal
                },
                new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "trading",
                    CategoryDisplayName = "Trading",
                    Name = "stopLossPercentage",
                    DisplayName = "Stop Loss Percentage",
                    MaximumValue = decimal.MaxValue,
                    MinimumValue = 0m,
                    DefaultValue = 2m,
                    Value = 0m,
                    CustomProperties = customPropertiesDecimalPlaces,
                    Type = GenericPropertyType.Decimal
                },
                 new GenericProperty()
                {
                    Browsable = true,
                    CategoryName = "trading",
                    CategoryDisplayName = "Trading",
                    Name = "stopLossWaitMinutes",
                    DisplayName = "Stop Loss Wait Minutes",
                    MaximumValue = int.MaxValue,
                    MinimumValue = 0,
                    DefaultValue = 10,
                    Value = 0,
                    Type = GenericPropertyType.Integer
                },
            };
        }
    }
}
