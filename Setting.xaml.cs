using System;
using System.Windows;
using System.Windows.Controls;
using CoreTweet;

namespace Shuugoo
{
    /// <summary>
    /// Setting.xaml の相互作用ロジック
    /// </summary>
    public partial class Setting
    {
        #region -- Public Properties --

        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            "Options", typeof(Options), typeof(Setting), new PropertyMetadata(default(Options)));

        public Options Options
        {
            get { return (Options)GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        #endregion

        #region -- Public Constructors --

        public Setting()
        {
            InitializeComponent();
        }

        #endregion

        #region -- Private Methods --

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            this.label_confirm.Content = "接続中...";
            this.button_confirm.IsEnabled = false;

            try
            {
                var token = Tokens.Create(this.textBox_ck.Text, this.textBox_cs.Text, this.textBox_ak.Text,
                    this.textBox_as.Text);
                var res = await token.Account.VerifyCredentialsAsync();

                this.label_confirm.Content = $"接続成功: {res.Name} (@{res.ScreenName})";
                this.button_confirm.IsEnabled = true;
            }
            catch (Exception ex)
            {
                this.label_confirm.Content = $"接続失敗: {ex.Message}";
                this.button_confirm.IsEnabled = true;
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var src = (TextBox)e.Source;
            src.Text = src.Text.Trim();
        }

        #endregion
    }
}
