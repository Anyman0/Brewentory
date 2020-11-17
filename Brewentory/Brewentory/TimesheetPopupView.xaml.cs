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
using Rg.Plugins.Popup.Extensions;
using System.Collections.ObjectModel;

namespace Brewentory
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimesheetPopupView
	{
        private string actionName;
        private bool reload;
        private int employeeId;
        private int workAround;
        private List<string> shiftTimes;
        private List<string> picks;
        private ObservableCollection<BrewentoryModel> shiftModel;
        private List<BrewentoryModel> modelList;
        
		public TimesheetPopupView (string selectedItem, string action, int empID, ObservableCollection<BrewentoryModel> Omodel)
		{
            actionName = action;
            employeeId = empID;
            shiftModel = Omodel;           
            modelList = new List<BrewentoryModel>();
            shiftTimes = new List<string>();
            picks = new List<string>();
            GetFullShiftList();
                      
			InitializeComponent ();            
		}

        private async void SaveShiftButton_Clicked(object sender, EventArgs e)
        {
            BrewentoryModel data = new BrewentoryModel();
            try
            {
                if(actionName == "Save")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = "Create",
                        EmployeeID = employeeId,
                        Week = int.Parse(WeekEntry.Text),
                        Name = NameEntry.Text,                       
                        ShiftName = ShiftPicker.SelectedItem?.ToString(),
                    };
                    for(int s = 0; s < picks.Count; s++)
                    {
                        if(picks[s] == data.ShiftName)
                        {
                            data.Monday = shiftTimes[s];
                            data.Tuesday = shiftTimes[s];
                            data.Wednesday = shiftTimes[s];
                            data.Thursday = shiftTimes[s];
                            data.Friday = shiftTimes[s];
                        }
                    }                  
                    shiftModel.Add(data);
                }

                // Adding the right amount in controller now. But in here.. Need to get all from timesheet and add the copy of the right week to shiftModel to display it right.
                else if(actionName == "EditWeek")
                {
                    int givenWeek = int.Parse(WeekEntry.Text);
                    int firstWk = shiftModel[0].Week;
                    bool deleting = false;
                    modelList.Clear();
                    workAround = 0;
                    foreach(var item in shiftModel)
                    {
                        if(item.Week == givenWeek)
                        {                            
                            if(workAround == 0)
                            {
                                modelList.Clear();
                                workAround++;
                            }
                            data = new BrewentoryModel()
                            {
                                Operation = "DeleteWeek",
                                Week = givenWeek
                            };                            
                            modelList.Add(item);
                            deleting = true;
                        }
                        else if(item.Week != givenWeek && !deleting)
                        {
                            data = new BrewentoryModel()
                            {
                                Operation = "AddWeek",
                                Week = givenWeek
                            };
                            if (item.Week == firstWk)
                            {
                                var itemCopy = item;
                                itemCopy.Week = givenWeek;                               
                                modelList.Add(itemCopy);
                            }
                        }
                    }                                    
                    foreach(var item in modelList)
                    {
                        if(data.Operation == "AddWeek")
                        {
                            reload = true;
                            shiftModel.Add(item);
                        }
                        else if(data.Operation == "DeleteWeek")
                        {
                            reload = false;
                            shiftModel.Remove(item);
                        }
                    }
                }

                else if(actionName == "Edit")
                {                    
                    data = new BrewentoryModel()
                    {
                        Operation = "Edit",
                        EmployeeID = employeeId,
                        Week = int.Parse(WeekEntry.Text),
                        Name = NameEntry.Text,
                        Monday = MondayEntry.Text,
                        Tuesday = TuesdayEntry.Text,
                        Wednesday = WednesdayEntry.Text,
                        Thursday = ThursdayEntry.Text,
                        Friday = FridayEntry.Text,                        
                        ShiftName = ShiftPicker.SelectedItem?.ToString()
                    };

                    for(int s = 0; s < picks.Count; s++)
                    {
                        if(picks[s] == data.ShiftName)
                        {
                            data.Monday = shiftTimes[s];
                            data.Tuesday = shiftTimes[s];
                            data.Wednesday = shiftTimes[s];
                            data.Thursday = shiftTimes[s];
                            data.Friday = shiftTimes[s];
                        }
                    }
                    
                }
                else if(actionName == "Delete")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = "Delete",
                        EmployeeID = employeeId,
                        Week = int.Parse(WeekEntry.Text),
                        Name = NameEntry.Text,
                        Monday = MondayEntry.Text,
                        Tuesday = TuesdayEntry.Text,
                        Wednesday = WednesdayEntry.Text,
                        Thursday = ThursdayEntry.Text,
                        Friday = FridayEntry.Text,                       
                    };                                       
                }

                // <<<<<< Below method works but may not be the best solution. Return to this? >>>>>>>
                if(actionName == "Edit" || actionName == "Delete")
                {
                    for (int i = 0; i < shiftModel.Count; i++)
                    {
                        if (shiftModel[i].EmployeeID == employeeId)
                        {
                            if (actionName == "Edit")
                            {
                                shiftModel.Remove(shiftModel[i]);
                                shiftModel.Insert(i, data);
                                break;
                            }
                            else if (actionName == "Delete")
                            {
                                shiftModel.Remove(shiftModel[i]);
                                break;
                            }

                        }
                    }
                }
                


                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string input = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

                HttpResponseMessage msg = await client.PostAsync("/api/ShiftList", content);
                string reply = await msg.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(reply);

                if(success)
                {
                    await DisplayAlert("Saved!", "Changes saved.", "OK");                    
                }
                else
                {
                    await DisplayAlert("Save failed!", "Sorry, couldn't save changes.", "Close");                    
                }

            }
            catch
            {
                await DisplayAlert("Oops!", "Couldn't get data from DB", "Close");
            }

            if (!reload)
                await Navigation.PopPopupAsync();
            else if (reload)
            {
                Navigation.PopPopupAsync();
                await Navigation.PopAsync();
            }
                                                       
        }

        private async void GetFullShiftList()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("/api/shiftlist?id=4");
            List<string[]> fullModel = JsonConvert.DeserializeObject<List<string[]>>(json);
            foreach(var item in fullModel)
            {
                foreach(var s in item)
                {
                    shiftTimes.Add(s);
                }
            }            
        }
        

        private async void GetPickerData()
        {
            // Need to get /api/shiftdata (or something) to get all shifts and populate picker itemsource with that data.
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("/api/ShiftList?id=5");
            List<string[]> pickerDataModel = JsonConvert.DeserializeObject<List<string[]>>(json);
            //List<string> picks = new List<string>();            
            foreach (var item in pickerDataModel)
            {
                foreach (var pick in item)
                {
                    picks.Add(pick);
                }
            }
            ShiftPicker.ItemsSource = picks;
        }       

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(actionName != "Save" && actionName != "EditWeek")
            {
                
                 HttpClient client = new HttpClient();
                 client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                 string json = await client.GetStringAsync("/api/shiftlist?employeeID=" + employeeId);
                 BrewentoryModel chosenEmployeeModel = JsonConvert.DeserializeObject<BrewentoryModel>(json);
                 WeekEntry.Text = chosenEmployeeModel.Week.ToString();
                 NameEntry.Text = chosenEmployeeModel.Name;
                 MondayEntry.Text = chosenEmployeeModel.Monday;
                 TuesdayEntry.Text = chosenEmployeeModel.Tuesday;
                 WednesdayEntry.Text = chosenEmployeeModel.Wednesday;
                 ThursdayEntry.Text = chosenEmployeeModel.Thursday;
                 FridayEntry.Text = chosenEmployeeModel.Friday;
                 GetPickerData();
                 if (actionName == "Edit") Header.Text = "Edit";
                             
            }
            else if(actionName == "Save")
            {
                MondayEntry.IsVisible = false;
                TuesdayEntry.IsVisible = false;
                WednesdayEntry.IsVisible = false;
                ThursdayEntry.IsVisible = false;
                FridayEntry.IsVisible = false;
                GetPickerData();
            }
            else if(actionName == "EditWeek")
            {
                NameEntry.IsVisible = false;
                MondayEntry.IsVisible = false;
                TuesdayEntry.IsVisible = false;
                WednesdayEntry.IsVisible = false;
                ThursdayEntry.IsVisible = false;
                FridayEntry.IsVisible = false;
                ShiftPicker.IsVisible = false;
                Header.Text = "Add / Delete Week";
            }
            if (actionName == "Delete")
            {
                SaveShiftButton.Text = "Delete";
            }
            else if (actionName == "Edit")
            {
                SaveShiftButton.Text = "Save changes";
            }
        }
        
       
    }
}