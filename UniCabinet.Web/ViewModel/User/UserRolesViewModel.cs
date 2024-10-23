﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace UniCabinet.Web.ViewModel.User
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public List<string> SelectedRoles { get; set; }
        public List<SelectListItem> AvailableRoles { get; set; }
        public string FullName { get; set; }
    }
}
