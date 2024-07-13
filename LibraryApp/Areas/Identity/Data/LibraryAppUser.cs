using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the LibraryAppUser class
public class LibraryAppUser : IdentityUser
{
    [StringLength(100)]
    public string Firstname { get; set; }

    [StringLength(100)]
    public string Lastname { get; set; }
}

