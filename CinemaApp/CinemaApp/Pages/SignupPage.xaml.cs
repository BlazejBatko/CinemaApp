using CinemaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CinemaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }

        private  async void ImgSignup_Tapped(object sender, EventArgs e)
        {
          var response = await ApiService.RegisterUser(EntName.Text, EntEmail.Text, EntPassword.Text);
            if (response)
            {
                await DisplayAlert("Hej", "Konto zostało stworzone pomyślnie", "Kontynuuj");
                await Navigation.PushModalAsync(new LoginPage());
            }
            else
            {
                await DisplayAlert(":(", "Niestety nie udało się utworzyć konta", "Anuluj");
            }
        }

        private async void LblLogin_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());

        }
    }
}