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
	public partial class EditShiftsView : ContentPage
	{
        public ObservableCollection<BrewentoryModel> shiftCollection { get; set; }
		public EditShiftsView ()
		{
			InitializeComponent ();
            shiftCollection = new ObservableCollection<BrewentoryModel>();           
		}

        private void AllShiftsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedShift = allShiftsList.SelectedItem as BrewentoryModel;
                int shiftID = selectedShift.ShiftID;
                string action = "Edit";
                await Navigation.PushPopupAsync(new ModifyShiftsPopupView(shiftID, action, shiftCollection));
            }
            catch
            {
                await DisplayAlert("Oops.", "Choose an item to edit first!", "OK");
            }
            
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedShift = allShiftsList.SelectedItem as BrewentoryModel;
                int shiftID = selectedShift.ShiftID;
                string action = "Delete";
                await Navigation.PushPopupAsync(new ModifyShiftsPopupView(shiftID, action, shiftCollection));
            }
            catch
            {
                await DisplayAlert("Oops.", "Choose an item to delete first.", "OK");
            }
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new ModifyShiftsPopupView(0, "Save", shiftCollection));
        }

        protected override async void OnAppearing()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("/api/shiftlist?shiftList=");
            string[] shiftArray = JsonConvert.DeserializeObject<string[]>(json);

            for (int i = 0; i < shiftArray.Count(); i++)
            {
                string[] data = shiftArray[i].Split(",");
                shiftCollection.Add(new BrewentoryModel {ShiftID = int.Parse(data[0]), ShiftName = data[1], ShiftTimes = data[2] });
            }

            allShiftsList.ItemsSource = shiftCollection;

            base.OnAppearing();
        }

    }
}