using System;
using System.IO;
using System.Linq;
using System.Windows;
using CoreTweet;

namespace Shuugoo
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow
    {
        #region -- Private Fields --

        private const string FilePath = "option.json";
        private Tokens _twitterToken;

        #endregion

        #region -- Public Properties --

        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            "Options", typeof(Options), typeof(MainWindow), new PropertyMetadata(default(Options)));

        public Options Options
        {
            get { return (Options)GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }
        
        #endregion

        #region -- Public Constructors --

        public MainWindow()
        {
            InitializeComponent();

            // thanks for: http://grabacr.net/archives/480
            this.MouseLeftButtonDown += (sender, e) => this.DragMove();

            if (File.Exists(FilePath))
                this.Options = Options.Load(FilePath);
            else
            {
                this.Options = new Options();
                this.ShowSettingWindow();
            }
        }

        #endregion

        #region -- Private Methods --

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_ShowSetting_Click(object sender, RoutedEventArgs e)
        {
            this.ShowSettingWindow();
        }

        private void ShowSettingWindow()
        {
            var s = new Setting { Options = this.Options };
            s.ShowDialog();
            this.Options.Save(FilePath);
            this.CreateTwitterToken();
        }

        private void CreateTwitterToken()
        {
            this._twitterToken = Tokens.Create(this.Options.ConsumerKey, this.Options.ConsumerSecret, this.Options.AccessToken,
                    this.Options.AccessSecret);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Options.Save(FilePath);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            this.button.IsEnabled = false;
            this.label_status.Content = "Ready";

            if (this.Options.CurrentAdditionNumber > this.Options.MaximumAdditionNumber ||
                this.Options.CurrentAdditionNumber < 0)
                this.Options.CurrentAdditionNumber = 0;

            var tweet = this.Options.BaseString +
                        string.Join("", Enumerable.Repeat(this.Options.AdditionalString, this.Options.CurrentAdditionNumber));

            if (!this.Options.NoConfirmingDialog)
            {
                var result = MessageBox.Show($"「{tweet}」\r\n\r\nつぶやきますか？", "確認", MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                {
                    this.button.IsEnabled = true;
                    return;
                }
            }
            
            if (this._twitterToken == null)
                this.CreateTwitterToken();

            try
            {
                this.label_status.Content = "つぶやき中...";
                await this._twitterToken.Statuses.UpdateAsync(tweet);
                this.Options.CurrentAdditionNumber++;
                this.Options.Save(FilePath);
                this.label_status.Content = "つぶやき成功！";
            }
            catch (Exception ex)
            {
                this.label_status.Content = $"失敗: {ex.Message}";
            }

            this.button.IsEnabled = true;
        }

        #endregion

        
    }
}
