using CinemaApp.Models;
using CinemaApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CinemaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        //Nowa Kolekcja MoviesCollection
        public ObservableCollection<Movie> MoviesCollection;
        private int pageNumber = 0;
        public HomePage()
        {
            InitializeComponent();
            LblUserName.Text = Preferences.Get("userName", string.Empty); // Wyświetlanie nazwy użytkownika w menu
            MoviesCollection = new ObservableCollection<Movie>();
            GetMovies();
        }

        //Metoda GetMovies przekazujemy w niej numer strony oraz rozmiar
        private async void GetMovies()
        {
           pageNumber++; // paginacja
           var movies = await ApiService.GetAllMovies(pageNumber, 5);
           foreach (var movie in movies)
            {
                MoviesCollection.Add(movie);
            }
           CvMovies.ItemsSource = MoviesCollection;
        }

        private async void TapMenu_Tapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }
        private async void TapCloseMenu_Tapped(object sender, EventArgs e)
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }

        private void TapSearch_Tapped(object sender, EventArgs e)
        {

        }

        private void CvMovies_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            GetMovies();
        }
    }
}