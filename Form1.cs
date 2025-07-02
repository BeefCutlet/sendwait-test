using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace WinForms_GettingStarted
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            webView.NavigationStarting += EnsureHttps;
            InitializeAsync();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            goButton.Left = this.ClientSize.Width - goButton.Width;
            addressBar.Width = goButton.Left - addressBar.Left;
        }

        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                webView.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
                args.Cancel = true;
            }
        }

        async void InitializeAsync()
        {
            try
            {
                var userDataFolder = Path.Combine(Path.GetTempPath(), $"wv2_{Guid.NewGuid()}");
                var env = await CoreWebView2Environment.CreateAsync(
                    null,
                    Path.Combine(Path.GetTempPath(), $"wv2_{Guid.NewGuid()}"),
                    null
                );
                await webView.EnsureCoreWebView2Async(env);
                webView.CoreWebView2.Navigate("https://www.shabbang.shop");
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebView2 초기화 오류: " + ex.Message);
            }
        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String uri = args.TryGetWebMessageAsString();
            addressBar.Text = uri;
            webView.CoreWebView2.PostWebMessageAsString(uri);
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            webView.Focus();
            System.Windows.Forms.SendKeys.SendWait("123456@U");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }
        
        private void btnSendKeys_Click(object sender, EventArgs e)
        {
            // 구글 검색창에 포커스를 주기 위해 잠시 대기
            System.Threading.Thread.Sleep(500);
            
            // SendKeys로 텍스트 전송
            System.Windows.Forms.SendKeys.SendWait("123456");
            
            // 추가로 Enter 키도 보내고 싶다면
            // System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        }

    }
}
