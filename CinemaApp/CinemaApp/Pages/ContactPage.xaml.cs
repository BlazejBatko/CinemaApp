using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CinemaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactPage : ContentPage
    {
        public ContactPage()
        {
            InitializeComponent();
        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void BtnCall_Clicked(object sender, EventArgs e)
        {
            
            //try
            //{
            //    PhoneDialer.Open("345-112-116");
            //}

            //catch (FeatureNotSupportedException ex)
            //{
            //    DisplayAlert("", ex.ToString(), "");
            //}
            //catch (Exception ex)
            //{
            //    DisplayAlert("", ex.ToString(), "");
            //}
        }
    }
}