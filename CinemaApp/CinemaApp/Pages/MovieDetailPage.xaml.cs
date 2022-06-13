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
    public partial class MovieDetailPage : ContentPage
    {
        private MovieDetail movie;
        public MovieDetailPage(int movieId)
        {
            InitializeComponent();
            GetMovieDetail(movieId);
        }

        //Przekazywanie Id filmu w celu wczytania danych na stronie MovieDetails
        private async void GetMovieDetail(int movieId)
        {
            movie = await ApiService.GetMovieDetail(movieId);
            LblMovieName.Text = movie.Name;
            LblGenre.Text = movie.Genre;
            LblRating.Text = movie.Rating.ToString();
            LblLanguage.Text = movie.Language;
            LblDuration.Text = movie.Duration;
            LblPlayingDate.Text = movie.PlayingDate;
            LblPlayingTime.Text = movie.PlayingTime;
            LblTicketPrice.Text = movie.TicketPrice.ToString();
            LblMovieDescription.Text = movie.Description;
            ImgMovie.Source = movie.FullImageUrl;
            ImgMovieCover.Source = movie.FullImageUrl;

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void TapVideo_Tapped(object sender, EventArgs e)
        {
            // Navigation.PushModalAsync(new VideoPlayerPage(movie.TrailorUrl));
            var uri = new Uri(movie.TrailorUrl);
            await Browser.OpenAsync(uri);
        }

        private void ImgBookTicket_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ReservationPage(movie));
        }
    }
}