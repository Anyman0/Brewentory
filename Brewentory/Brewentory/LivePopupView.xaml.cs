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
	public partial class LivePopupView
	{
        private int prodID;
        private string actionName;
        private StackLayout stackLayout;
        private ObservableCollection<BrewentoryModel> collection;
		public LivePopupView (string action, int productID, ObservableCollection<BrewentoryModel> model, StackLayout stack)
		{
			InitializeComponent ();
            prodID = productID;
            actionName = action;
            collection = model;
            stackLayout = stack;
		}

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            BrewentoryModel data = new BrewentoryModel();
            try
            {
                if(actionName == "Edit")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = actionName,
                        ProductID = prodID,
                        ProductLive = productEntry.Text.ToUpper(),
                        Batch = batchEntry.Text.ToUpper(),
                        Pallets = int.Parse(palletsEntry.Text),
                        QuantityLive = int.Parse(quantityEntry.Text),
                        LiveStatus = statusSwitch.IsToggled
                    };

                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (collection[i].ProductID == prodID)
                        {
                            if (actionName == "Edit")
                            {
                                collection.Remove(collection[i]);
                                collection.Insert(i, data);
                                break;
                            }
                        }
                    }
                    if (data.LiveStatus) stackLayout.BackgroundColor = Color.Green;
                    else if (!data.LiveStatus) stackLayout.BackgroundColor = Color.Red;
                }

                else if(actionName == "AddToCW")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = actionName,                       
                        cwProduct = productEntry.Text.ToUpper(),
                        cwBatch = batchEntry.Text.ToUpper(),
                        cwPallets = int.Parse(palletsEntry.Text),
                        cwQuantity = int.Parse(quantityEntry.Text),
                        StartShift = startShiftEntry.Text,
                        EndShift = endShiftEntry.Text,
                        Loss = int.Parse(lossEntry.Text)
                    };
                }
                
                
                // <<<<<< Below method works but may not be the best solution. Return to this? >>>>>>>
                /*if (actionName == "Edit")
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (collection[i].ProductID == prodID)
                        {
                            if (actionName == "Edit")
                            {
                                collection.Remove(collection[i]);
                                collection.Insert(i, data);
                                break;
                            }                            
                        }
                    }
                }*/

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string input = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");
                HttpResponseMessage msg = await client.PostAsync("api/live", content);
                string reply = await msg.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(reply);

                if(success)
                {
                    await DisplayAlert("Saved!", "Your changes have been saved!", "Close");                  
                    await Navigation.PopPopupAsync();                    
                }
                else
                {
                    await DisplayAlert("Failed!", "Couldn't save changes!", "Close");
                }
            }
            catch(Exception ex)
            {
                string errMsg = ex.GetType().Name + ": " + ex.Message;
                productEntry.Text = errMsg;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {

                if (actionName == "Edit")
                {
                    startShiftEntry.IsVisible = false;
                    endShiftEntry.IsVisible = false;
                    lossEntry.IsVisible = false;
                    saveButton.Text = "Edit";
                }
                else if(actionName == "AddToCW")
                {
                    LiveLabel.Text = "Add to Completed";
                    saveButton.Text = "Add to Completed";
                    statusSwitch.IsVisible = false;
                    switchLabel.IsVisible = false;
                }
                    

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string json = await client.GetStringAsync("api/live?productID=" + prodID);
                BrewentoryModel chosenModel = JsonConvert.DeserializeObject<BrewentoryModel>(json);
                productEntry.Text = chosenModel.ProductLive;
                batchEntry.Text = chosenModel.Batch;
                palletsEntry.Text = chosenModel.Pallets.ToString();
                quantityEntry.Text = chosenModel.QuantityLive.ToString();
                statusSwitch.IsToggled = chosenModel.LiveStatus;                                                                        
            }
            catch (Exception ex)
            {
                string errMsg = ex.GetType().Name + ": " + ex.Message;
                productEntry.Text = errMsg;
            }
        }
    }
}