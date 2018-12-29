using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom
{
    class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DogName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FIO { get; set; }

        [Required]
        public int Followers { get; set; }

        [Required]
        public int Follow { get; set; }

        [Required]        
        public string Image { get; set; }

        [NotMapped]
        public List<Tweet> ListOfTweets { get; set; }

        public User() { }

        public User(string imageIrl, string dogName, string fio, int followers, int follow, List<Tweet> tweets)
        {
            Image = imageIrl;
            DogName = dogName;
            FIO = fio;
            Followers = followers;
            Follow = follow;
            ListOfTweets = tweets;
        }
    }
}
