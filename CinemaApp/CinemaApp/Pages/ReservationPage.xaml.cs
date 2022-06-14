using CinemaApp.Models;
using CinemaApp.Services;
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
    public partial class ReservationPage : ContentPage
    {
        private int ticketPrice;
        private int movieId;
        public ReservationPage(MovieDetail movie)
        {
            InitializeComponent();
            LblMovieName.Text = movie.Name;
            LblGenre.Text = movie.Genre;
            LblRating.Text = movie.Rating.ToString();
            LblLanguage.Text = movie.Language;
            LblDuration.Text = movie.Duration;
            ImgMovie.Source = movie.FullImageUrl;
            SpanPrice.Text = movie.TicketPrice.ToString();
            SpanTotalPrice.Text = movie.TicketPrice.ToString();
            ticketPrice = movie.TicketPrice;
            movieId = movie.Id;
        }

        private void PickerQty_SelectedIndexChanged(object sender, EventArgs e)
        {
            var quantity = PickerQty.Items[PickerQty.SelectedIndex];
            SpanQty.Text = quantity;
            int TotalPrice =  Convert.ToInt16(SpanQty.Text) * ticketPrice;
            SpanTotalPrice.Text = TotalPrice.ToString();
        }

        private async void ImgReserve_Tapped(object sender, EventArgs e)
        {
            var reservation = new Reservation()
            {
                //INT cannot be cast to STRING
                UserId = Convert.ToInt16(Preferences.Get("userId", 1)),
                MovieId = movieId,
                Phone = EntPhone.Text,
                Qty = Convert.ToInt16(SpanQty.Text),
                Price = Convert.ToInt16(SpanTotalPrice.Text)
            };
            var response = await ApiService.ReserveMovieTicket(reservation);
            if (response)
            {
                await DisplayAlert(":)", "Pomyślnie zarezerwowano miejsca na wskazany seans", "Kontynuuj");
            }
            else
            {
                await DisplayAlert(":(", "Niestety nie udało się dokończyć rezerwacji", "Anuluj");
            }
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}