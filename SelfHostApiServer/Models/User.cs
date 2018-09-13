using System;

namespace SelfHostApiServer.Models
{
    public class User : IEquatable<User>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        // INFO THAT USER SAVED ABOUT ORDER

		public string PhoneNum { get; set; }
		public string Street { get; set; }
		public string House { get; set; }
		public string Entrance { get; set; }
		public string Apartment { get; set; }

		// ------------------------------------------------------------
              
		//constructor by default
        public User(){}

        /// <summary>
        /// initializes a new instance with loaded OrderInfo
        /// </summary>
        /// <param name="userID">User identifier.</param>
        /// <param name="UserName">User name.</param>
        /// <param name="Password">Password.</param>
        /// <param name="PhoneNum">Phone number.</param>
        /// <param name="Street">Street.</param>
        /// <param name="House">House.</param>
        /// <param name="Entrance">Entrance.</param>
        /// <param name="Apartment">Apartment.</param>
		public User(int userID, string UserName, string Password, string PhoneNum, string Street, string House, string Entrance, string Apartment)
        {
            this.Id = userID;
            this.UserName = UserName;
            this.Password = Password;
			this.PhoneNum = PhoneNum;
			this.Street = Street;
			this.House = House;
			this.Entrance = Entrance;
			this.Apartment = Apartment;
        }

        // constructor for authorization
        public User(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }

        // constructor with identity
        public User(int userID, string UserName, string Password)
        {
            this.Id = userID;
            this.UserName = UserName;
            this.Password = Password;
        }

        public bool CheckAuthorizationInfo()
        {
            if (UserName.Equals("") || Password.Equals(""))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Compares Users by their {username\password}
        /// </summary>
        /// <param name="other"> another User to be compared with this intanse </param>
        /// <returns><c>true</c> if this User has the same {username,password} as other
        /// <see cref="T:SelfHostApiServer.Models.User"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(User other)
        {
            if (other == null)
                return false;

                // compared username + password
            if (this.UserName == other.UserName && this.Password == other.Password)
                return true;
            else
                return false;
        }

        /// <summary>
        /// returns the info about User (id\username\password)
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:SelfHostApiServer.Models.User"/>.</returns>
		public override string ToString()
		{
            return $"----USER--------\n" +
                   $"name={UserName}\n" +
                   $"password={Password}\n"+
                   $"-------------------";
		}
	}
}
