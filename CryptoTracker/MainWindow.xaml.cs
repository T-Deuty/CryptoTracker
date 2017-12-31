using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CryptoTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected static readonly Dictionary<string,string> nameDictionary = new Dictionary<string, string> {
            { "Bitcoin (BTC)" , "bitcoin" },
            { "Tron (TRX)" , "tron" },
            { "Ripple (XRP)" , "ripple" }
        };

        protected TextBlock selectedTextBlock;
        protected CoinMarketCapAPI apiObj;
        protected string selectedCurrency;
        protected double addedAmount, totalInvestmentValue;

        public MainWindow()
        {
            InitializeComponent();
            apiObj = new CoinMarketCapAPI();
            AddComboBox();
        }

        private void AddComboBox()
        {
            ComboBox comboBoxCryptoSelect = new ComboBox()
            {
                Margin = new Thickness(5, 5, 5, 0),
                FontSize = 14
            };

            comboBoxCryptoSelect.ItemsSource = nameDictionary.Keys;

            comboBoxCryptoSelect.SelectionChanged += ComboBoxCryptoSelect_SelectionChanged;

            stackPanelLeft.Children.Insert(stackPanelLeft.Children.IndexOf(textBlockCryptoSelect)+1, comboBoxCryptoSelect);
        }

        private void ComboBoxCryptoSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCurrency = e.AddedItems[0] as string;
            buttonAddAmount.IsEnabled = true;
        }

        private void ButtonAddAmount_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem = selectedCurrency;
            string ticker = nameDictionary[selectedItem];

            DisplayCurrencyStats(ticker, selectedItem);
        }

        private void DisplayCurrencyStats(string ticker, string displayName)
        {
            TickerJSONResult result = apiObj.RequestTicker(ticker);
            Label currencyLabel, usdLabel, btcLabel, userAmountLabel, userUSDValueLabel;
            CustomStackPanel stackPanelToUpdate;

            string
                stackPanelStr = ticker + "StackPanel",
                labelStr = ticker + "Label",
                labelContentStr = displayName + " Stats:",
                labelUSDStr = ticker + "USD",
                labelUSDContentStr = "USD Price: $" + result.price_usd,
                labelBTCStr = ticker + "BTC",
                labelBTCContentStr = "BTC Price: " + result.price_btc,
                labelUserAmountStr = ticker + "UserAmount",
                labelUserAmountContentStr = "Amount you own: ",
                labelUserUSDValueStr = ticker + "UserUSDVal",
                labelUserUSDValueContentStr = "Your estimated USD value: ";

            // add label if currency not already displayed
            stackPanelToUpdate = GetChildOfStackPanel(stackPanelRight, stackPanelStr) as CustomStackPanel;

            if (stackPanelToUpdate == null)
            {
                // create new StackPanel to hold labels
                stackPanelToUpdate = new CustomStackPanel
                {
                    Name = stackPanelStr,
                    Margin = new Thickness(5, 0, 5, 5)
                };

                // create and add currency label
                currencyLabel = new Label
                {
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Content = labelContentStr,
                    Name = labelStr
                };
                stackPanelToUpdate.Children.Add(currencyLabel);

                // create and add usdLabel
                usdLabel = new Label
                {
                    Name = labelUSDStr,
                    FontSize = 14,
                    Content = labelUSDContentStr
                };
                stackPanelToUpdate.Children.Add(usdLabel);

                // create and add btcLabel
                btcLabel = new Label
                {
                    Name = labelBTCStr,
                    FontSize = 14,
                    Content = labelBTCContentStr
                };
                stackPanelToUpdate.Children.Add(btcLabel);

                // create and add user holding amount label
                userAmountLabel = new Label
                {
                    Name = labelUserAmountStr,
                    FontSize = 14,
                    Content = labelUserAmountContentStr + addedAmount.ToString()
                };
                stackPanelToUpdate.Children.Add(userAmountLabel);

                // create and add userUSDValue label
                userUSDValueLabel = new Label
                {
                    Name = labelUserUSDValueStr,
                    FontSize = 14,
                    Content = UpdateUserUSDValueContentStr(ref stackPanelToUpdate, result.price_usd, labelUserUSDValueContentStr),
                    Foreground = Brushes.ForestGreen
                };
                stackPanelToUpdate.Children.Add(userUSDValueLabel);

                stackPanelRight.Children.Add(stackPanelToUpdate);
            } else
            {
                // update user value
                stackPanelToUpdate.UserValue = addedAmount * Convert.ToDouble(result.price_usd);

                // update price labels
                usdLabel = GetChildOfStackPanel(stackPanelToUpdate, labelUSDStr) as Label;
                usdLabel.Content = labelUSDContentStr;
                btcLabel = GetChildOfStackPanel(stackPanelToUpdate, labelBTCStr) as Label;
                btcLabel.Content = labelBTCContentStr;
                userUSDValueLabel = GetChildOfStackPanel(stackPanelToUpdate, labelUserUSDValueStr) as Label;
                userUSDValueLabel.Content = UpdateUserUSDValueContentStr(ref stackPanelToUpdate, result.price_usd, labelUserUSDValueContentStr);
                userAmountLabel = GetChildOfStackPanel(stackPanelToUpdate, labelUserAmountStr) as Label;
                userAmountLabel.Content = labelUserAmountContentStr + addedAmount.ToString();
            }

            // update total values
            UpdateTotalValue();
        }

        private void UpdateTotalValue()
        {
            totalInvestmentValue = 0.0;
            foreach (CustomStackPanel child in stackPanelRight.Children)
            {
                totalInvestmentValue += child.UserValue;
            }

            textBlockTotalValue.Text = "Total investment value: $" + totalInvestmentValue.ToString();
        }

        private FrameworkElement GetChildOfStackPanel(StackPanel parentPanel, string name)
        {
            foreach (FrameworkElement child in parentPanel.Children)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }

            return null;
        }

        private void TextBoxAddUserHolding_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxAddUserHolding.Text != null && textBoxAddUserHolding.Text != "")
            {
                addedAmount = Convert.ToDouble(textBoxAddUserHolding.Text);
            } else
            {
                addedAmount = 0.0;
            }
        }

        private string UpdateUserUSDValueContentStr(ref CustomStackPanel sP, string usdPriceStr, string contentStr)
        {
            // update user value
            sP.UserValue = addedAmount * Convert.ToDouble(usdPriceStr);

            if (addedAmount == 0.0)
            {
                contentStr += "N/A";
            } else
            {
                contentStr += "$" + sP.UserValue.ToString();
            }

            return contentStr;
        }
    }
}
