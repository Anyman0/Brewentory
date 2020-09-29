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
	public partial class ShiftList : ContentPage
	{
        private string[] shiftArray;
        private ObservableCollection<BrewentoryModel> shiftCollection { get; set; }
		public ShiftList ()
		{
			InitializeComponent ();

            // Toolbar items
            var GoToNewShiftPopup = new ToolbarItem()
            {
                Text = "New employee",
            };
            GoToNewShiftPopup.Clicked += GoToNewShiftPopup_Clicked;
            ToolbarItems.Add(GoToNewShiftPopup);
        }

        private async void GoToNewShiftPopup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new NewShiftPopupView());
        }

        protected override async void OnAppearing()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("/api/Shifts");
            shiftArray = JsonConvert.DeserializeObject<string[]>(json);
            shiftCollection = new ObservableCollection<BrewentoryModel>();

            for (int i = 0; i < shiftArray.Count(); i++)
            {
                string[] data = shiftArray[i].Split(",");
                shiftCollection.Add(new BrewentoryModel { Week = int.Parse(data[0]), Name = data[1], Monday = data[2], Tuesday = data[3], Wednesday = data[4], Thursday = data[5], Friday = data[6] });
            }

            shiftsList.ItemsSource = shiftCollection;

            base.OnAppearing();
        }

        private void ShiftsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}