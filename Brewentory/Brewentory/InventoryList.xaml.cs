using Brewentory.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
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
	public partial class InventoryList : ContentPage
	{

        public ObservableCollection<BrewentoryModel> inventory { get; set; }
        private ObservableCollection<BrewentoryModel> currentInventory { get; set; }       
        private string[] inventoryArray;
        private string action;
        private int locationID;
        private Color color;

        public InventoryList ()
		{
            
			InitializeComponent ();                                        
		}

       
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("api/inventory/");
            inventoryArray = JsonConvert.DeserializeObject<string[]>(json);
                        
            inventory = new ObservableCollection<BrewentoryModel>();           
            currentInventory = new ObservableCollection<BrewentoryModel>();

            for (int i = 0; i < inventoryArray.Count(); i++)
            {
               
                string[] data = inventoryArray[i].Split(",");               
                inventory.Add(new BrewentoryModel { LocationID = int.Parse(data[0]), Location = data[1], Product = data[2], Quantity = data[3]});
            }

            lstView.ItemsSource = inventory;
            
        }
        
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = lstView.SelectedItem as BrewentoryModel;
                action = "Edit";
                locationID = item.LocationID;               
                string selectedItem = item.Product.TrimStart();
                await Navigation.PushPopupAsync(new InventoryPopupView(CreateButton, action, locationID, inventory));                 
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
                var item = lstView.SelectedItem as BrewentoryModel;
                action = "Delete";
                locationID = item.LocationID;
                string selectedItem = item.Product.TrimStart();
                lstView.SelectedItem = null;
                await Navigation.PushPopupAsync(new InventoryPopupView(CreateButton, action, locationID, inventory));
            }
            catch
            {
                await DisplayAlert("Oops!", "Choose an item first", "OK");
            }
            
        }
     
        private void SearchButton_Clicked(object sender, EventArgs e)
        {

            currentInventory.Clear();
            
            if (searchEntry.Text != "")
            {
                for (int i = 0; i < inventoryArray.Count(); i++)
                {
                    string[] data = inventoryArray[i].Split(",");
                    if (data[1].TrimStart().Contains(searchEntry.Text.ToUpper()) || data[2].TrimStart().Contains(searchEntry.Text.ToUpper()) || data[3].TrimStart().Contains(searchEntry.Text.ToUpper()))
                    {
                        currentInventory.Add(new BrewentoryModel { LocationID = int.Parse(data[0]), Location = data[1], Product = data[2], Quantity = data[3] });
                    }
                }
                lstView.ItemsSource = null;
                lstView.ItemsSource = currentInventory;
            }

            else
            {                
                lstView.ItemsSource = null;
                lstView.ItemsSource = inventory;
            }
           
                      
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            /*var index = e.SelectedItemIndex;
            var item = e.SelectedItem as BrewentoryModel;
            //item.EditIcon = "EditIcon.png";
            //item.DeleteIcon = "DeleteIcon.png";
            item.BtnVisibility = true;           
            //inventory.Insert(index, item);                      
            //inventory.RemoveAt(index + 1);            
            lstView.ItemsSource = inventory;*/            
        }

        
               

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            if(CreateButton.Text == "Create New")
            {
                color = CreateButton.BackgroundColor;
                await Navigation.PushPopupAsync(new InventoryPopupView(CreateButton, "Save", 0, inventory));
            }
            else if(CreateButton.Text == "Refresh")
            {
                RefreshView();
            }
        }

        private async void RefreshView()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("api/inventory/");
            inventoryArray = JsonConvert.DeserializeObject<string[]>(json);

            inventory = new ObservableCollection<BrewentoryModel>();            

            for (int i = 0; i < inventoryArray.Count(); i++)
            {

                string[] data = inventoryArray[i].Split(",");
                inventory.Add(new BrewentoryModel { LocationID = int.Parse(data[0]), Location = data[1], Product = data[2], Quantity = data[3] });
            }

            lstView.ItemsSource = inventory;
            CreateButton.Text = "Create New";
            CreateButton.BackgroundColor = color;           
        }
        
    }
}