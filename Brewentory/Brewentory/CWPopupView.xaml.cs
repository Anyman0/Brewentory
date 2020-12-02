using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Brewentory.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Brewentory
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CWPopupView
	{
        private string actionName;
        private int workId;
        private ObservableCollection<BrewentoryModel> collection;
		public CWPopupView (string action, int id, ObservableCollection<BrewentoryModel> model)
		{
			InitializeComponent ();
            actionName = action;
            workId = id;
            collection = model;
		}

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            BrewentoryModel data = new BrewentoryModel();
            try
            {
                if(actionName == "Edit" || actionName == "Delete")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = actionName,
                        completedWorkID = workId,
                        Date = dateEntry.Text,
                        cwProduct = productEntry.Text.ToUpper(),
                        cwBatch = batchEntry.Text.ToUpper(),
                        cwPallets = int.Parse(palletsEntry.Text),
                        cwQuantity = int.Parse(quantityEntry.Text),
                        StartShift = startShiftEntry.Text,
                        EndShift = endShiftEntry.Text,
                        Loss = int.Parse(lossEntry.Text)
                    };

                    for(int i = 0; i < collection.Count(); i++)
                    {
                        if(collection[i].completedWorkID == workId)
                        {
                            if(actionName == "Edit")
                            {
                                collection.Remove(collection[i]);
                                collection.Insert(i, data);
                                break;
                            }
                            else if(actionName == "Delete")
                            {
                                collection.Remove(collection[i]);
                                break;
                            }                           
                        }
                    }
                }
                else if(actionName == "DeleteAll")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = actionName,
                    };
                    collection.Clear();
                }

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string input = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");
                HttpResponseMessage msg = await client.PostAsync("/api/completedworkapi", content);
                string reply = await msg.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(reply);
                if(success)
                {
                    await DisplayAlert("Saved!", "Your changes have been saved.", "OK");
                    await Navigation.PopPopupAsync();
                }
                else
                {
                    await DisplayAlert("Failed!", "Couldn't save changes.", "Close");
                }
            }
            catch(Exception ex)
            {
                string msg = ex.GetType().Name + ": " + ex.Message;
                productEntry.Text = msg;
            }
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if(actionName == "DeleteAll")
                {
                    dateEntry.IsVisible = false;
                    productEntry.IsVisible = false;
                    batchEntry.IsVisible = false;
                    palletsEntry.IsVisible = false;
                    quantityEntry.IsVisible = false;
                    startShiftEntry.IsVisible = false;
                    endShiftEntry.IsVisible = false;
                    lossEntry.IsVisible = false;
                    CWLabel.Text = "Delete all works?";
                    saveButton.Text = "Yup. Delete!";
                }

                else if(actionName == "Delete")
                {
                    CWLabel.Text = "Delete Completed Work?";
                    saveButton.Text = "Delete";
                }
               
                if(actionName == "Delete" || actionName == "Edit")
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                    string json = await client.GetStringAsync("api/completedworkapi?cwid=" + workId);
                    BrewentoryModel chosenModel = JsonConvert.DeserializeObject<BrewentoryModel>(json);
                    dateEntry.Text = chosenModel.Date;
                    productEntry.Text = chosenModel.cwProduct;
                    batchEntry.Text = chosenModel.cwBatch;
                    palletsEntry.Text = chosenModel.cwPallets.ToString();
                    quantityEntry.Text = chosenModel.cwQuantity.ToString();
                    startShiftEntry.Text = chosenModel.StartShift;
                    endShiftEntry.Text = chosenModel.EndShift;
                    lossEntry.Text = chosenModel.Loss.ToString();
                }

                
            }
            catch(Exception ex)
            {
                string msg = ex.GetType().Name + ": " + ex.Message;
                productEntry.Text = msg;
            }
        }
    }
}