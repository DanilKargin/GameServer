using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
	[DataContract]
	public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public List <Player> Players { get; set; }
        public void Update(User user)
        {
            if (user == null)
            {
                return;
            }
            Login = user.Login;
            PasswordHash = user.PasswordHash;
            Salt = user.Salt;
        }
        
    }
}
