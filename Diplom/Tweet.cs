using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom
{
    [Serializable]
    public class Tweet
    {
        private TimeSpan tweetTime;

        public int Id { get; set; }

        [Required]
        public long Identifier { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string DogName { get; set; }
        
        public string Text { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(50)]
        public string DataInfo { get; set; }
        
        public Tweet() { }

        public Tweet(string text)
        {
            Text = GetBeautifulText(text);
        }

        public Tweet(string name, string dogname, DateTime data, string text, string url, long id)
        {
            Name = name;
            DogName = "@" + dogname;
            tweetTime = DateTime.UtcNow - data;
            Data = data;
            DataInfo = GetInfoAboutCreationTime();
            Text = GetBeautifulText(text);
            ImageUrl = url;
            Identifier = id;
        }

        private string GetInfoAboutCreationTime()
        {
            if (tweetTime.Days < 1)
            {
                if (tweetTime.Minutes == 0)
                    return string.Format("{0} сек. назад", tweetTime.Seconds);
                if (tweetTime.Hours == 0)
                    return string.Format("{0} мин. назад", tweetTime.Minutes);
                if (tweetTime.Hours == 1)
                    return "час назад";
                if (tweetTime.Hours < 5)
                    return string.Format("{0} часа назад", tweetTime.Hours);
                else
                    return string.Format("{0} часов назад", tweetTime.Hours);
            }
            else if (tweetTime.Days == 1)
                return "сутки назад";
            else
                return string.Format("{0} суток назад", tweetTime.Days);
        }

        private string GetBeautifulText(string stroka)
        {
            var words = stroka.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.First() != 'h');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < words.Count(); i++)
            {
                sb.Append(words.ElementAt(i) + " ");
            }
            if (sb.Length == 0)
                return "";
            else
                return sb.Remove(sb.Length - 1, 1).ToString();
        }
    }
}
