using Brewentory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Brewentory
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        private string person;
		public LoginPage ()
		{
			InitializeComponent ();
		}

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(UsernameEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                string logData = UsernameEntry.Text + " " + PasswordEntry.Text;

                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                    string json = await client.GetStringAsync("api/loginapi?LogData="+ logData);
                    bool state = JsonConvert.DeserializeObject<bool>(json);

                    if(state && !rememberSwitch.IsToggled)
                    {
                        HeaderText.Text = "Welcome " + UsernameEntry.Text;
                        await Navigation.PushAsync(new LiveView());
                        await Navigation.PopToRootAsync();
                    }
                    else if(state && rememberSwitch.IsToggled)
                    {
                        HeaderText.Text = "Welcome " + UsernameEntry.Text;
                        await App.LDatabase.SaveItemAsync(new LoginModel
                        {
                            Username = UsernameEntry.Text,
                            Password = PasswordEntry.Text
                        });
                        await Navigation.PushAsync(new LiveView());
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await DisplayAlert("Login failed.", "Username or Password is incorrect.", "OK");
                    }
                }
                catch
                {
                    await DisplayAlert("Login failed.", "Couldnt retrieve data from DB", "OK..");
                }                           
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var personLogged = await App.LDatabase.GetData();
                foreach (var item in personLogged) person = item.Username;
                if(person != null)
                HeaderText.Text = "Welcome " + person + "!";
            }
            catch
            {

            }
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            LoginModel data = new LoginModel();
            await App.LDatabase.DeleteItemAsync(data);
            HeaderText.Text = "Login";
        }
    }
}