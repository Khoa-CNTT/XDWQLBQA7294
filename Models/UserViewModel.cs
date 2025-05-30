﻿namespace Eshopp.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public List<string> AllRoles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}
