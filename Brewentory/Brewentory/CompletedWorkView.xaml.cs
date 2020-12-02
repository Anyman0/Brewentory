using Brewentory.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
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
        private string actionName;
        private ObservableCollection<BrewentoryModel> cwData;
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
            cwData = new ObservableCollection<BrewentoryModel>();
            for (int i = 0; i < cwArray.Count(); i++)
            {
                string[] data = cwArray[i].Split(",");                                               
                cwData.Add(new BrewentoryModel { completedWorkID = int.Parse(data[0]), Date = data[1], cwProduct = data[2], cwBatch = data[3], cwPallets = int.Parse(data[4]), cwQuantity = int.Parse(data[5]), StartShift = data[6], EndShift = data[7], Loss = int.Parse(data[8]) });
            }

            completedWorkList.ItemsSource = cwData;
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = completedWorkList.SelectedItem as BrewentoryModel;
                actionName = "Edit";
                cwID = item.completedWorkID;
                await Navigation.PushPopupAsync(new CWPopupView(actionName, cwID, cwData));
            }
            catch
            {
                await DisplayAlert("Oops!", "Choose an item first.", "OK");
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = completedWorkList.SelectedItem as BrewentoryModel;
                actionName = "Delete";
                cwID = item.completedWorkID;
                await Navigation.PushPopupAsync(new CWPopupView(actionName, cwID, cwData));
            }
            catch
            {
                await DisplayAlert("Oops!", "Choose an item first.", "OK");
            }
        }

        private async void DeleteAllButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                actionName = "DeleteAll";
                await Navigation.PushPopupAsync(new CWPopupView(actionName, 0, cwData));
            }
            catch
            {
                await DisplayAlert("Oops!", "Couldn't delete all.", "OK");
            }
        }
    }
}