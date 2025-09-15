using System.Security.Claims;
using FitnessBookingApp.Infrastructure;
using FitnessBookingApp.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FitnessBookingApp.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? Email(this ClaimsPrincipal user)
        {
            // 1) Pokud někdy přidáš claims, vezmeme z ClaimTypes.Email
            var claimEmail = user.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(claimEmail)) return claimEmail;

            // 2) Fallback přes Session -> DataService
            var http = ServiceLocator.Http?.HttpContext;
            var username = http?.Session.GetString("User");
            if (string.IsNullOrWhiteSpace(username)) return null;

            var data = ServiceLocator.Get<DataService>();
            var found = data.GetUsers().FirstOrDefault(u => u.Username == username);
            return found?.Email;
        }

        public static string? PhoneNumber(this ClaimsPrincipal user)
        {
            // 1) Pokud někdy přidáš claims, vezmeme z MobilePhone nebo "phone_number"
            var claimPhone = user.FindFirst(ClaimTypes.MobilePhone)?.Value
                             ?? user.FindFirst("phone_number")?.Value;
            if (!string.IsNullOrWhiteSpace(claimPhone)) return claimPhone;

            // 2) Fallback přes Session -> DataService
            var http = ServiceLocator.Http?.HttpContext;
            var username = http?.Session.GetString("User");
            if (string.IsNullOrWhiteSpace(username)) return null;

            var data = ServiceLocator.Get<DataService>();
            var found = data.GetUsers().FirstOrDefault(u => u.Username == username);
            return found?.PhoneNumber;
        }
    }
}