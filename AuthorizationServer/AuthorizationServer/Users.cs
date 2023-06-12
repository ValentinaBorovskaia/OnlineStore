namespace AuthorizationServer
{

    public class Users
    {
        public static List<User> ListUsers = new List<User>
        {
            new User 
            { 
                Id = 1, 
                Name = "Alice", 
                Email="Alice@com", 
                Password="1234", 
                Role = Roles.Manager 
            },
            new User
            {
                Id = 2,
                Name = "Bob",
                Email="Bob@com",
                Password="1234",
                Role = Roles.Buyer
            }
        };
    }
}
