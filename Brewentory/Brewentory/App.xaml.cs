using Brewentory.Data;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Brewentory
{
	public partial class App : Application
	{
        static LoginDatabase lDatabase;
        public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new LiveView());
		}

        public static LoginDatabase LDatabase
        {
            get
            {
                if (lDatabase == null)
                {
                    lDatabase = new LoginDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LoginData.db"));
                }

                return lDatabase;
            }
        }
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
