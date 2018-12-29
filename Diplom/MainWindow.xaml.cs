using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TweetSharp;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using System.Data.Entity.Validation;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal TwitterService service;    // Полученный из диалогового окна класс для работы с моим Твиттером.
        int tweetsCount = 7;                // Количество отображаемых твитов.
        long maxId;                         // ID твита для постраничной загрузки твитов.
        User iAm = new User();              // Класс для загрузки данных при запуске.
        int magic = 3;

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Загружаю асинхронно и ОДНОВРЕМЕННО данные в приложение при запуске.
            GetAsyncMyPage();
        }

        // Асинхронная загрузка элементов домашней страницы.
        private async void GetAsyncMyPage()
        {
            var taskMe = Task.Run(() => GetIUser());
            User me = await taskMe;             
            User GetIUser()
            {
                // Получаю адрес собственной аватарки.
                iAm.Image = service.GetUserProfile(new GetUserProfileOptions()).ProfileImageUrl;
                // Получение моего ника.
                iAm.FIO = service.GetUserProfile(new GetUserProfileOptions()).Name;
                // Получение ника, который через @ пишется.
                iAm.DogName = "@" + service.GetUserProfile(new GetUserProfileOptions()).ScreenName;
                // Получение количества друзей.
                iAm.Follow = service.GetUserProfile(new GetUserProfileOptions()).FriendsCount;
                // Получение количества подписчиков.
                iAm.Followers = service.GetUserProfile(new GetUserProfileOptions()).FollowersCount;
                // Получение первых семи твитов со стены.
                // Непонятно почему, но иногда твитов для отображения семи нужно требовать 8, 9 или 10.
                // Поэтому { Count = tweetsCount + 1(или +2, или +3)}.
                iAm.ListOfTweets = GetTweets
                    (service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions() { Count = tweetsCount + magic }).ToList());
                return iAm;
            }

            // В элементах отображения показываю полученные выше свойства.
            lstBoxTweetText.ItemsSource = me.ListOfTweets;      
            imgMy.Source = new BitmapImage(new Uri(me.Image));
            txtBoxName.Text = me.FIO;
            txtBoxScreenName.Text = me.DogName;
            txtBoxUserFriends.Text = me.Follow.ToString();
            txtBoxFollowers.Text = me.Followers.ToString();

            //Получение списка десяти трендов Москвы. Неинтересно смотреть за трендами мира.
            trendsList.ItemsSource = service.ListLocalTrendsFor(new ListLocalTrendsForOptions() { Id = 1 })

            //trendsList.ItemsSource = service.ListLocalTrendsFor(new ListLocalTrendsForOptions() { Id = 2122265 })
            .Where(x => x.Name.StartsWith("#")).OrderBy(y => y.Name).Take(10).ToList();

            // Очищаю и заполняю таблицы с данными пользователя и трендами.
            using (var db = new DiplomContext())
            {
                // Очистка таблицы с данными пользователя.
                if (db.Users.Count() != 0)
                    db.Users.Remove(db.Users.First());

                // Очистка таблицы с трендами.
                var currentTags = db.Hashtags;
                Hashtag iTag;
                if (currentTags.Count() != 0)
                {
                    int numberTags = currentTags.First().Id;
                    for (int i = numberTags; i < currentTags.Count() + numberTags; i++)
                    {
                        iTag = currentTags.Where(x => x.Id == i).First();
                        currentTags.Remove(iTag);
                    }
                }
                db.SaveChanges();

                // Заполнение таблицы трендами.
                int j = 1;
                foreach (var x in service.ListLocalTrendsFor(new ListLocalTrendsForOptions() { Id = 2122265 }))
                {
                    db.Hashtags.Add(new Hashtag { Name = x.Name });
                    j++;
                }
                db.SaveChanges();

                // Заполнение таблицы данными пользователя.
                db.Users.Add
                    (new User { DogName = me.DogName, FIO = me.FIO, Followers = me.Followers, Follow = me.Follow, Image = me.Image });
                db.SaveChanges();
            }

        }

        // Метод для получения твитов.
        private List<Tweet> GetTweets(List<TwitterStatus> tweets)
        {            
            var listTweets = new List<Tweet>();

            // Создаю семь объектов класса Твит и заполняю ими List. 
            for (int i = 0; i < tweets.Count; i++)
            {                
                Tweet tw = new Tweet
                    (tweets[i].User.Name, tweets[i].Author.ScreenName, tweets[i].CreatedDate, tweets[i].Text, 
                    tweets[i].Author.ProfileImageUrl, tweets[i].Id);
                // Беру Id седьмого твита и перезаписываю maxId для постраничной загрузки.
                if (i == tweets.Count - 1)
                    maxId = tw.Identifier;
                listTweets.Add(tw);                
            }

            // Удаляю последний твит, так как он будет отображаться на следующей странице Твитов.
            if (tweets.Count != tweetsCount)
                listTweets.RemoveAt(tweets.Count - 1);

            // Заполняю таблицу с твитами в базе данных.
            using (var db = new DiplomContext())
            {
                // Очистка данных из таблицы с твитами.                
                var currentTweets = db.Tweets;
                Tweet iTweet;
                if (currentTweets.Count() != 0)
                {
                    int numberTweets = currentTweets.First().Id;
                    for (int i = numberTweets; i < currentTweets.Count() + numberTweets; i++)
                    {
                        iTweet = currentTweets.Where(x => x.Id == i).First();
                        currentTweets.Remove(iTweet);
                    }
                }
                db.SaveChanges();

                // Заполнение таблицы твитами.                
                for (int i = 0; i < listTweets.Count; i++)
                {
                    Tweet tw = listTweets.ElementAt(i);
                    db.Tweets.Add(new Tweet
                    {
                        Identifier = tw.Identifier,
                        Name = tw.Name,
                        DogName = tw.DogName,
                        ImageUrl = tw.ImageUrl,
                        DataInfo = tw.DataInfo,
                        Data = tw.Data,
                        Text = tw.Text
                    });
                }

                try
                {
                    db.SaveChanges();
                }
                // Здесь я отлавливал исключения при сохранении БД и читал описание ошибок. 
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        string x = "Object: " + validationError.Entry.Entity.ToString();

                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            string y = err.ErrorMessage;
                        }
                    }
                }
            }

            // Сохраняю аватарки только разных авторов твитов. Необязательно разных, т.к. аватарки "лёгкие", но уж сделал.
            var diffAuthors = listTweets.Select(x => x.DogName).Distinct();
            foreach (var x in diffAuthors)
            {
                Tweet y = (from tw in listTweets where tw.DogName == x select tw).First();
                using (var webClient = new WebClient())
                {
                    var url = y.ImageUrl;
                    webClient.DownloadFile(url, y.DogName.Remove(0, 1) + System.IO.Path.GetExtension(url));
                }
            }

            return listTweets;
        }         

        // При изменении размеров окна меняю размеры всех шрифтов, изменяю размер своей аватарки.
        private void mainWin_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double koefWidth, koefHeight;
            koefWidth = mainWin.Width / 1000;
            koefHeight = mainWin.Height / 808;

            elgImage.Center = new Point(63 * koefWidth, 47 * koefHeight);
            elgImage.RadiusX = 47 * koefWidth;
            elgImage.RadiusY = 47 * koefHeight;
            
            if (koefWidth >= koefHeight)
            {
                txtBoxName.FontSize = 20 * koefHeight;
                txtBoxScreenName.FontSize = 16 * koefHeight;
                txtBoxFollowers.FontSize = 16 * koefHeight;
                txtBoxUserFriends.FontSize = 16 * koefHeight;
                txtBlockUserFriends.FontSize = 13 * koefHeight;
                txtBlockActualThemes.FontSize = 18 * koefHeight;
                txtBlockSearch.FontSize = 16 * koefHeight;
                imgSearch.Margin = new Thickness(7 * koefHeight);
                imgHome.Margin = new Thickness(4 * koefHeight);
                txtBlockHome.FontSize = 14 * koefHeight;
                txtBlockLoad.FontSize = 13 * koefHeight;
            }
            else
            {
                txtBoxName.FontSize = 20 * koefWidth;
                txtBoxScreenName.FontSize = 16 * koefWidth;
                txtBoxFollowers.FontSize = 16 * koefWidth;
                txtBoxUserFriends.FontSize = 16 * koefWidth;
                txtBlockUserFriends.FontSize = 13 * koefWidth;
                txtBlockActualThemes.FontSize = 18 * koefWidth;
                txtBlockSearch.FontSize = 16 * koefWidth;
                imgSearch.Margin = new Thickness(7 * koefWidth);
                imgHome.Margin = new Thickness(4 * koefWidth);
                txtBlockHome.FontSize = 14 * koefWidth;
                txtBlockLoad.FontSize = 13 * koefWidth;
            }
        }        

        // Обработка нажатия на кнопку "Домой".
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            txtBoxSearchString.Text = "";
            GetAsyncMyPage();            
        }       

        // Обработка нажатия на кнопку "Поиск".
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxSearchString.Text != "")
            {
                switch (txtBoxSearchString.Text.First())
                {
                    case '#':
                        lstBoxTweetText.ItemsSource = GetTweets(service.Search
                            (new SearchOptions() { Count = tweetsCount, Q = txtBoxSearchString.Text }).Statuses.ToList());
                        break;
                    case '@':
                        lstBoxTweetText.ItemsSource = GetTweets(service.ListTweetsOnUserTimeline
                            (new ListTweetsOnUserTimelineOptions() { Count = tweetsCount, ScreenName = txtBoxSearchString.Text }).ToList());
                        break;
                    default:
                        break;
                }
            }
        }

        // Постраничная загрузка твитов.
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxSearchString.Text == "")
                lstBoxTweetText.ItemsSource = GetTweets
                    (service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions() { MaxId = maxId, Count = tweetsCount + magic }).ToList());
            // Не помню совершенно, зачем здесь else. Наверное, хотел обработать все нажатия на ссылки в одном событии.
            else
            {
                switch (txtBoxSearchString.Text.First())
                {
                    case '#':
                        lstBoxTweetText.ItemsSource = GetTweets(service.Search
                            (new SearchOptions() { MaxId = maxId, Count = tweetsCount + 1, Q = txtBoxSearchString.Text }).Statuses.ToList());
                        break;
                    case '@':
                        lstBoxTweetText.ItemsSource = GetTweets(service.ListTweetsOnUserTimeline
                            (new ListTweetsOnUserTimelineOptions() { MaxId = maxId, Count = tweetsCount + 1, ScreenName = txtBoxSearchString.Text }).ToList());
                        break;
                    default:
                        break;
                }
            }
        }

        // Нажатие на хештег.
        private void hlkHash_Click(object sender, RoutedEventArgs e)
        {            
            var link = (Hyperlink)sender;
            TextRange tr = new TextRange(link.ContentStart, link.ContentEnd);
            txtBoxSearchString.Text = tr.Text;
            lstBoxTweetText.ItemsSource = GetTweets(service.Search(new SearchOptions() { Q = txtBoxSearchString.Text, Count = tweetsCount }).Statuses.ToList());
            
        }

        // Нажатие на "собачье" имя автора твита.
        private void hlkDogName_Click(object sender, RoutedEventArgs e)
        {
            var dogLink= (Hyperlink)sender;
            TextRange tr = new TextRange(dogLink.ContentStart, dogLink.ContentEnd);
            txtBoxSearchString.Text = tr.Text;
            lstBoxTweetText.ItemsSource = GetTweets
                (service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = txtBoxSearchString.Text, Count = tweetsCount }).ToList());
        }
    }    
}
