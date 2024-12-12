﻿namespace MiniCommerce.Api.Models.User
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
