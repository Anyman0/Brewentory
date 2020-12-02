using Brewentory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Brewentory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletedWorkView : ContentPage
    {
        private string[] cwArray;
        private int cwID;
        public CompletedWorkView()
        {
            InitializeComponent();
        }

        private void CompletedWorkList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("api/completedworkapi");
            cwArray = JsonConvert.DeserializeObject<string[]>(json);
            var cwData = new ObservableCollection<BrewentoryModel>();
            for (int i = 0; i < cwArray.Count(); i++)
            {
                string[] data = cwArray[i].Split(",");
                cwID = int.Parse(data[0].TrimStart());                                
                cwData.Add(new BrewentoryModel { completedWorkID = int.Parse(data[0]), Date = data[1], cwProduct = data[2], cwBatch = data[3], cwPallets = int.Parse(data[4]), cwQuantity = int.Parse(data[5]), StartShift = data[6], EndShift = data[7], Loss = int.Parse(data[8]) });
            }

            completedWorkList.ItemsSource = cwData;
        }
    }
}