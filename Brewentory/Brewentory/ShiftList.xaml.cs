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
        private string action;
        private int employeeID;
        
        
        public ObservableCollection<BrewentoryModel> shiftCollection { get; set; }
		public ShiftList ()
		{
			InitializeComponent ();
            shiftCollection = new ObservableCollection<BrewentoryModel>();
            shiftCollection.CollectionChanged += ShiftCollection_CollectionChanged;

            // Toolbar items added here
            var CreateNewEmployee = new ToolbarItem()
            {
                Text = "Create New"
            };
            CreateNewEmployee.Clicked += CreateNewEmployee_Clicked;
            ToolbarItems.Add(CreateNewEmployee);

            var ModifyShifts = new ToolbarItem()
            {
                Text = "Modify shifts"
            };
            ModifyShifts.Clicked += ModifyShifts_Clicked;
            ToolbarItems.Add(ModifyShifts);
        }

        private async void CreateNewEmployee_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new TimesheetPopupView("", "Save", 0, shiftCollection));
        }

        private async void ModifyShifts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditShiftsView());
        }

        private void ShiftCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Getting here?? 
            
        }

        protected override async void OnAppearing()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("/api/shiftlist");
            shiftArray = JsonConvert.DeserializeObject<string[]>(json);
            var sortedArray = shiftArray.OrderBy(x => { var week = x.Split(","); return int.Parse(week[1]); });
            string[] sortedShiftArray = sortedArray.ToArray<string>();            
            shiftCollection.Clear(); // Clear the list of shifts before populating it again

            for (int i = 0; i < sortedShiftArray.Count(); i++)
            {
                string[] data = sortedShiftArray[i].Split(",");
                shiftCollection.Add(new BrewentoryModel { SheetID = int.Parse(data[0]), Week = int.Parse(data[1]), Name = data[2], Monday = data[3], Tuesday = data[4], Wednesday = data[5], Thursday = data[6], Friday = data[7]});            
            }
            
            shiftsList.ItemsSource = shiftCollection;
            
            base.OnAppearing();
        }
            
        private void ShiftsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }

        
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                
                var item = shiftsList.SelectedItem as BrewentoryModel;
                action = "Edit";
                if(item.SheetID == 0)
                {
                    await GetIdAfterChange(item.Week, item.Name);
                }
                else
                employeeID = item.SheetID;
                string selectedItem = item.Name.TrimStart();
                await Navigation.PushPopupAsync(new TimesheetPopupView(selectedItem, action, employeeID, shiftCollection));                
            }
            catch
            {
                await DisplayAlert("Ooops!", "Choose an item first.", "OK");
            }            
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = shiftsList.SelectedItem as BrewentoryModel;
                action = "Delete";
                employeeID = item.SheetID;
                string selectedItem = item.Name.TrimStart();
                await Navigation.PushPopupAsync(new TimesheetPopupView(selectedItem, action, employeeID, shiftCollection));
            }
            catch
            {
                await DisplayAlert("Oops!", "Choose an item to delete first!", "OK");
            }
            
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new TimesheetPopupView("", "Save", 0, shiftCollection));
        }

        private async void ShiftsList_BindingContextChanged(object sender, EventArgs e)
        {
            // See if we get here. Ever.
            await DisplayAlert("Ey!", "Binding context has changed!", "Ok");
        }

        private async void EditWeekButton_Clicked(object sender, EventArgs e)
        {
            action = "EditWeek";
            await Navigation.PushPopupAsync(new TimesheetPopupView("", action, 0, shiftCollection));
        }

        // Gets the newly created id because page is not reloaded
        private async Task GetIdAfterChange(int week, string name)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("/api/shiftlist");
            var all = JsonConvert.DeserializeObject<string[]>(json);
            for(int i = 0; i < all.Count(); i++)
            {
                string[] data = all[i].Split(",");
                if(int.Parse(data[1].TrimStart()) == week && data[2].TrimStart() == name)
                {
                    employeeID = int.Parse(data[0]);
                }
            }            
        }
    }
}