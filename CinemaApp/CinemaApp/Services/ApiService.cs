using CinemaApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnixTimeStamp;
using Xamarin.Essentials;

namespace CinemaApp.Services
{
    //statyczna klasa w celu mozliwosci dostepu do zawartych funkcji w innych czesciach projektu
    public static class ApiService
    {
        public static async Task<bool> RegisterUser(string name, string email, string password)
        {
            var register = new Register()
            {
                Name = name,
                Email = email,
                Password = password
            };
            //Serializacja obiektow na JSON w celu przesyłania do serwera
            var HttpClient = new HttpClient();
            var SerializedJson = JsonConvert.SerializeObject(register);
            var content = new StringContent(SerializedJson, Encoding.UTF8, "application/json");
            //asynchroniczne wysyłanie żadania do enpointu odpowiedzialnego za rejestracje użytkowników
            //Fragment URL pochodzacy z klasy AppSettings w celu poprawy utrzymania kodu
            var response = await HttpClient.PostAsync(AppSettings.ApiUrl + "api/users/register", content);
            //zwracanie wartosci bool w zaleznosci od uzyskanego status code
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async static Task<bool> Login(string email, string password) 
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };
            //Serializacja obiektow na JSON w celu przesyłania do serwera
            var HttpClient = new HttpClient();
            var SerializedJson = JsonConvert.SerializeObject(login);
            var content = new StringContent(SerializedJson, Encoding.UTF8, "application/json");
            //asynchroniczne wysyłanie żadania do enpointu odpowiedzialnego za rejestracje użytkowników
            //Fragment URL pochodzacy z klasy AppSettings w celu poprawy utrzymania kodu
            var response = await HttpClient.PostAsync(AppSettings.ApiUrl + "api/users/login", content);
            //zwracanie wartosci bool w zaleznosci od uzyskanego status code
            if (!response.IsSuccessStatusCode) return false;
            var jsonResult = await response.Content.ReadAsStringAsync();
            //Deserializacja w oparciu o własności klasy Token
            var result = JsonConvert.DeserializeObject<Token>(jsonResult);
            //zapisywanie wartosci access tokenu w preferences
            Preferences.Set("accessToken", result.access_token);
            Preferences.Set("userId", result.user_id);
            Preferences.Set("userName", result.user_Name);
            Preferences.Set("tokenExpirationTime", result.expiration_Time);
            Preferences.Set("currentTime", UnixTime.GetCurrentTime());
            return true;
        }

        public static async Task<List<Movie>> GetAllMovies(int pageNumber, int pageSize)
        {
            //sprawdzenie czy token jest dalej aktualny, jezeli nie to wywolywana jest funkcja wystawiajaca nowy token
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            //pobieranie accessTokenu zapisanego wczesniej w Preferences
            //Wysyłanie headera zawierającego token w celu pobrania listy filmów
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + String.Format("api/movies/AllMovies?sort=asc&pageNumber={0}&pageSize={1}", pageNumber, pageSize));
            //Konwertowanie danych JSON na liste opartą o klasę Movie
            return JsonConvert.DeserializeObject<List<Movie>>(response);
        }

        public static async Task<MovieDetail> GetMovieDetail(int movieId)
        {
            //sprawdzenie czy token jest dalej aktualny, jezeli nie to wywolywana jest funkcja wystawiajaca nowy token
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            //pobieranie accessTokenu zapisanego wczesniej w Preferences
            //Wysyłanie headera zawierającego token w celu pobrania szczegółów dotyczących filmu
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + String.Format("api/movies/MovieDetail/{0}", movieId));
            //Konwertowanie danych JSON na liste opartą o klasę MovieDetail
            return JsonConvert.DeserializeObject<MovieDetail>(response);
        }

        public static async Task<List<FindMovie>> FindMovies(string movieName)
        {
            //sprawdzenie czy token jest dalej aktualny, jezeli nie to wywolywana jest funkcja wystawiajaca nowy token
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            //pobieranie accessTokenu zapisanego wczesniej w Preferences
            //Wysyłanie headera zawierającego token w celu pobrania listy filmów zaczynających się wskazanym ciągiem znaków
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + String.Format("api/movies/FindMovies?movieName=" + movieName));
            //Konwertowanie danych JSON na liste opartą o klasę FindMovie
            return JsonConvert.DeserializeObject<List<FindMovie>>(response);
        }

        public static async Task<bool> ReserveMovieTicket(Reservation reservation)
        {

            //sprawdzenie czy token jest dalej aktualny, jezeli nie to wywolywana jest funkcja wystawiajaca nowy token
            await TokenValidator.CheckTokenValidity();
            //Serializacja obiektow na JSON w celu przesyłania do serwera
            var HttpClient = new HttpClient();
            var SerializedJson = JsonConvert.SerializeObject(reservation);
            var content = new StringContent(SerializedJson, Encoding.UTF8, "application/json");
            //Header zawierający access token
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await HttpClient.PostAsync(AppSettings.ApiUrl + "api/reservations", content);
            //zwracanie wartosci bool w zaleznosci od uzyskanego status code
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

    }

    public static class TokenValidator
    {
        public static async Task CheckTokenValidity()
        {
           var expirationTime = Preferences.Get("tokenExpirationTime", 0);
            Preferences.Set("currentTime", UnixTime.GetCurrentTime());
            var currentTime = Preferences.Get("currentTime", 0);
            if (expirationTime < currentTime)
            {
               var email = Preferences.Get("email", string.Empty);
               var password = Preferences.Get("password", string.Empty);

               await ApiService.Login(email, password);

            }

        }
    }
}
