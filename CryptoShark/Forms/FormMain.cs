using Binance;
using CryptoShark.Hunting.Data;
using CryptoShark.Utility;
using CryptoShark.Utility.Enum;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CryptoShark.Forms
{
    public partial class FormMain : EnhancedForm
    {
        #region Constants

        private const string WALLET_FORMAT = "{0} BTC / ${1} / {2} TL";
        private const int WALLET_REFRESH_INTERVAL = 5 * 60 * 1000; // in ms
        private const int ORDERS_REFRESH_INTERVAL = 10 * 60 * 1000; // in ms

        #endregion

        private Hunting.Data.Hunting currentHunting;
        private Timer timerWallet;
        private Timer timerOrders;

        private bool TradeEnabled => SettingsHelper.Instance.GetSetting(SettingName.TRADING_ENABLED, false);

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            comboBoxHuntingTypes.DataSource = HuntingTypeManager.Instance.HuntingTypes;

            FileLogger.Instance.OnFileLogged += Instance_OnFileLogged;

            InitializeVariables();

            timerWallet = new Timer();
            timerWallet.Interval = WALLET_REFRESH_INTERVAL;
            timerWallet.Tick += TimerWallet_Tick;
            timerWallet.Start();

            timerOrders = new Timer();
            timerOrders.Interval = ORDERS_REFRESH_INTERVAL;
            timerOrders.Tick += TimerOrders_Tick;
            timerOrders.Start();
        }


        private void Instance_OnFileLogged(object sender, Utility.Events.FileLoggedEventArgs e)
        {
            if (checkBoxLogActive.Checked)
            {
                richTextBox.Text += e.Text + "\n";

                richTextBox.SelectionStart = richTextBox.Text.Length;
                richTextBox.ScrollToCaret();
            }
        }

        #region UI Events

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (currentHunting != null)
            {
                currentHunting.StopHunting();
                currentHunting = null;

                buttonStartStop.Text = "Start Hunting";
            }
            else
            {
                if (comboBoxHuntingTypes.SelectedIndex != -1)
                {
                    var huntingType = (HuntingType)comboBoxHuntingTypes.SelectedItem;
                    currentHunting = huntingType.CreateInstance();
                    currentHunting.Initialize();
                    currentHunting.StartHunting();

                    RefreshWallet();
                    //RefreshOrders();

                    buttonStartStop.Text = "Stop Hunting";
                }
                else
                    MessageBox.Show("Hunting type is not selected!");
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBox.Text = string.Empty;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formSettings = new FormSettings();
            if (formSettings.ShowDialog(this) == DialogResult.OK)
            {
                SettingsHelper.Instance.Invalidate();

                if (currentHunting != null)
                {
                    if (ShowQuestion("Do you want to stop current hunting process?") == DialogResult.Yes)
                    {
                        currentHunting.StopHunting();
                        currentHunting = null;

                        buttonStartStop.Text = "Start Hunting";
                    }
                }
            }
        }

        private void dataGridViewOrders_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridViewOpenOrders.Columns.Count > 0)
            {
                dataGridViewOpenOrders.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridViewOpenOrders.Columns[0].Width = 48;
            }

            foreach (DataGridViewRow row in dataGridViewOpenOrders.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) == "Buy")
                    row.DefaultCellStyle.BackColor = Constants.Green;
                else
                    row.DefaultCellStyle.BackColor = Constants.Red;
            }
        }

        private void dataGridViewAllOrders_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridViewAllOrders.Columns.Count > 0)
            {
                dataGridViewAllOrders.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridViewAllOrders.Columns[0].Width = 48;
            }

            foreach (DataGridViewRow row in dataGridViewAllOrders.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) == "Buy")
                    row.DefaultCellStyle.BackColor = Constants.Green;
                else
                    row.DefaultCellStyle.BackColor = Constants.Red;
            }
        }

        #endregion

        #region Timer Events

        private void TimerOrders_Tick(object sender, EventArgs e)
        {
            if (TradeEnabled)
                RefreshOrders();
        }

        private void TimerWallet_Tick(object sender, EventArgs e)
        {
            if (TradeEnabled)
                RefreshWallet();
        }

        #endregion

        #region UI Operations

        private async void RefreshWallet()
        {
            if (currentHunting?.User != null)
            {
                try
                {
                    var account = await currentHunting.Api.GetAccountInfoAsync(currentHunting.User);
                    var prices = await currentHunting.Api.GetPricesAsync();
                    decimal totalBTC = 0;
                    foreach (var balance in account.Balances)
                    {
                        if (balance.Free > 0 || balance.Locked > 0)
                        {
                            if (balance.Asset == Asset.BTC.Symbol)
                                totalBTC += (balance.Free + balance.Locked);
                            else
                            {
                                if (balance.Asset == "USDT")
                                {
                                    var priceBTC = prices.Where(x => x.Symbol == "BTCUSDT").FirstOrDefault().Value;
                                    totalBTC += (balance.Free + balance.Locked) / priceBTC;
                                }
                                else
                                {
                                    var price = prices.Where(x => x.Symbol == balance.Asset + "BTC").FirstOrDefault();
                                    if (price != null)
                                        totalBTC += price.Value * (balance.Free + balance.Locked);
                                }
                            }
                        }
                    }

                    var btcUsdPrice = await currentHunting.Api.GetPriceAsync(Symbol.BTC_USDT);

                    var btcStringFormat = string.Format("{0:0.########}", totalBTC);
                    var usdStringFormat = string.Format("{0:0}", totalBTC * btcUsdPrice.Value);
                    var tlStringFormat = string.Format("{0:0}", totalBTC * btcUsdPrice.Value * (decimal)ServiceHelper.USDtoTRY());

                    toolStripStatusLabelWallet.Text = string.Format(WALLET_FORMAT, btcStringFormat, usdStringFormat, tlStringFormat);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async void RefreshOrders()
        {
            var symbols = await currentHunting.Api.GetSymbolsAsync();
            try
            {
                var openOrders = await BinanceApiHelper.Instance.GetOpenOrders();
                dataGridViewOpenOrders.DataSource = openOrders;
                if (openOrders != null)
                    dataGridViewOpenOrders.Columns["Date"].DefaultCellStyle.Format = "MM.dd.yyyy HH:mm:ss";

                /*
                var allOrders = await BinanceApiHelper.Instance.GetAllOrders();
                dataGridViewAllOrders.DataSource = allOrders;
                if (allOrders != null)
                    dataGridViewAllOrders.Columns["Date"].DefaultCellStyle.Format = "MM.dd.yyyy HH:mm:ss";
                */
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Utility Functions

        private void InitializeVariables()
        {

        }

        #endregion
    }
}
