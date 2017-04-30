using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Process();           
        }

        public async void Process()
        {
            while (true)
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync("http://alexa-holo.cfapps.io").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // by calling .Result you are performing a synchronous call
                        var responseContent = response.Content;

                        // by calling .Result you are synchronously reading the result
                        string responseString = responseContent.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<UrlResult>(responseString);

                        if (result.modified)
                        {
                            Uri url = new Uri(result.url);
                            webView1.Navigate(url);
                        }
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        public class UrlResult
        {
            public Boolean modified { get; set; }
            public String url { get; set; }
        }
    }
}
