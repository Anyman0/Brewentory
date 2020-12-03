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
	public partial class LiveView : ContentPage
	{
        private string[] liveArray;
        private int productID;
        
        private ObservableCollection<BrewentoryModel> liveData;
		public LiveView ()
		{
			InitializeComponent ();            
            // Toolbar items
            var goToInventory = new ToolbarItem()
            {
                Text = "Inventory"
            };
            goToInventory.Clicked += GoToInventory_Clicked;
            ToolbarItems.Add(goToInventory);

            var goToTimesheet = new ToolbarItem()
            {
                Text = "Timesheet"
            };
            goToTimesheet.Clicked += GoToTimesheet_Clicked;
            ToolbarItems.Add(goToTimesheet);

            var goToCw = new ToolbarItem()
            {
                Text = "Completed"
            };
            goToCw.Clicked += GoToCw_Clicked;            
            ToolbarItems.Add(goToCw);

            var goToNotes = new ToolbarItem()
            {
                Text = "Notes"
            };
            goToNotes.Clicked += GoToNotes_Clicked;
            ToolbarItems.Add(goToNotes);            
		}

        private async void GoToNotes_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new NoteView());
        }

        private async void GoToCw_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompletedWorkView());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("api/live");
            liveArray = JsonConvert.DeserializeObject<string[]>(json);
            liveData = new ObservableCollection<BrewentoryModel>();
            for(int i = 0; i < liveArray.Count(); i++)
            {
                string[] data = liveArray[i].Split(",");
                productID = int.Parse(data[0]);
                liveData.Add(new BrewentoryModel { ProductID = int.Parse(data[0]), ProductLive = data[1], Batch = data[2], Pallets = int.Parse(data[3]), QuantityLive = int.Parse(data[4]), LiveStatus = bool.Parse(data[5]) });
            }
            if (liveData[0].LiveStatus) stackLayout.BackgroundColor = Color.Green;
            else if (!liveData[0].LiveStatus) stackLayout.BackgroundColor = Color.Red;
            liveList.ItemsSource = liveData;           
        }


        private async void GoToTimesheet_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShiftList());           
        }

        private async void GoToInventory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InventoryList());
        }

        private void LiveList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void AddToCompletedButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var action = "AddToCW";
                await Navigation.PushPopupAsync(new LivePopupView(action, productID, liveData, stackLayout));
            }
            catch
            {
                await DisplayAlert("Oops", "Something went wrong", "Close");
            }
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var action = "Edit";
                await Navigation.PushPopupAsync(new LivePopupView(action, productID, liveData, stackLayout));
            }
            catch
            {
                await DisplayAlert("Oops", "Something went wrong", "Close");
            }
        }

        
    }
}