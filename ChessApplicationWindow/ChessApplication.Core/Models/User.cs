using ChessApplication.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApplication.Core.Models
{
    public interface IUser
    {

    }
    public class User : IUser
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
