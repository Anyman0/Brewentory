using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System.Net.Http;
using Brewentory.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Brewentory
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewShiftPopupView
	{
		public NewShiftPopupView ()
		{
			InitializeComponent ();

             
		}

        private async void SaveShiftButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                BrewentoryModel model = new BrewentoryModel()
                {
                    Operation = "Create",
                    
                    Name = NameEntry.Text,
                    ShiftName = ShiftPicker.SelectedItem?.ToString(),
                };

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string input = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

                HttpResponseMessage msg = await client.PostAsync("/api/ShiftList", content);
                string reply = await msg.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(reply);

                if(success)
                {
                    await DisplayAlert("Saved!", "New employee has been added.", "OK");
                }
                else
                {
                    await DisplayAlert("Save failed!", "Sorry, couldn't save shift", "Close");
                }

            }
            catch
            {
                await DisplayAlert("Save failed!", "Couldn't get data from DB", "Close");
            }
        }



        private void GetPickerData()
        {
            // Need to get /api/shiftdata (or something) to get all shifts and populate picker itemsource with that data.
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("/api/ShiftList?id=5");
            List<string[]> pickerDataModel = JsonConvert.DeserializeObject<List<string[]>>(json);
            List<string> picks = new List<string>();
            foreach(var item in pickerDataModel)
            {
                foreach(var pick in item)
                {
                    picks.Add(pick);
                }
            }
            ShiftPicker.ItemsSource = picks;
        }
    }
}