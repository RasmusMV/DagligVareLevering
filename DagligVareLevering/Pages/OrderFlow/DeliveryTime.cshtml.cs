
using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class DeliveryTimeModel : PageModel
    {
        private readonly IOrderService _orderService;
        public DeliveryTimeModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Liste med ugens 7 dage, som skal vises i tabellen
        public List<DateTime> WeekDays { get; set; } = new List<DateTime>();

        // Liste med gyldige leveringstidsintervaller
        public List<string> TimeSlots { get; set; } = new List<string>();

        // Bruges til at gå frem og tilbage mellem uger
        public int WeekOffset { get; set; }

        // Den ordre som kunden arbejder med
        public Order? CurrentOrder { get; set; }

        // Den dato kunden klikker på i skemaet
        [BindProperty]
        public DateTime SelectedDate { get; set; }

        // Det tidsinterval kunden klikker på, fx "10:00-12:00"
        [BindProperty]
        public string SelectedTimeSlot { get; set; }

        // Henter kalenderdata og den aktuelle ordre, når siden indlæses
        public async Task<IActionResult> OnGet(int weekOffset = 0)
        {
            WeekOffset = weekOffset;
            // Gør dage og tidsintervaller klar til at blive vist i kalenderen
            TimeSlots = GetTimeSlots();
            WeekDays = GetWeekDays(weekOffset);

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Henter den nyeste ordre for brugeren, som vi skal gemme leveringstidspunktet på senere
            CurrentOrder = await _orderService.GetLatestUserOrderAsync(userId.Value);

            if (CurrentOrder == null)
            {
                // Sender brugeren tilbage til kurven, hvis der ikke findes en aktiv ordre
                TempData["StatusMessage"] = "Du skal have varer i kurven, før du kan vælge leveringstid.";
                return RedirectToPage("/Cart");
            }

            return Page();
        }

        // Kører når kunden vælger et leveringstidspunkt i tabellen
        public async Task<IActionResult> OnPostSelectTime(int weekOffset)
        {
            WeekOffset = weekOffset;
            // Genopbygger kalenderdata, så siden stadig kan vises korrekt ved fejl
            TimeSlots = GetTimeSlots();
            WeekDays = GetWeekDays(weekOffset);

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Henteer den nyeste ordre for brugeren
            CurrentOrder = await _orderService.GetLatestUserOrderAsync(userId.Value);

            if (CurrentOrder == null)
            {
                // Sender brugeren tilbage til kurven, hvis orderen ikke fines
                TempData["StatusMessage"] = "Du skal have varer i kurven, før du kan vælge leveringstid.";
                return RedirectToPage("/Cart");
            }

            if (!TimeSlots.Contains(SelectedTimeSlot))
            {
                // Sikrer at kunden kun kan vælge et tidsinterval fra listen
                ModelState.AddModelError(string.Empty, "Vælg venligst et gyldigt leveringstidspunkt.");
                return Page();
            }

            if (IsSlotUnavailable(SelectedDate, SelectedTimeSlot))
            {
                // Forhindrer valg af datoer og tidspunkter, der allerede er passeret
                ModelState.AddModelError(string.Empty, "Du kan ikke vælge et leveringstidspunkt, der allerede er passeret.");
                return Page();
            }

            string[] splitTime = SelectedTimeSlot.Split('-');
            TimeSpan startTime = TimeSpan.Parse(splitTime[0]);

            // Gemmer valgt dato og starttidspunkt på orderen
            CurrentOrder.ExpectedDeliveryTime = SelectedDate.Date.Add(startTime);

            // Opdaterer orderen i databasen
            await _orderService.UpdateObjectAsync(CurrentOrder);

            return RedirectToPage("/OrderFlow/OrderSummary");
        }

        // Tjekker om et leveringstidspunkt skal deaktiveres i tabellen
        public bool IsSlotUnavailable(DateTime date, string timeSlot)
        {
            if (date.Date < DateTime.Today)
            {
                return true;
            }

            string[] splitTime = timeSlot.Split('-');
            TimeSpan startTime = TimeSpan.Parse(splitTime[0]);

            if (date.Date == DateTime.Today && startTime <= DateTime.Now.TimeOfDay)
            {
                return true;
            }

            return false;
        }

        // Tjekker om et tidspunkt er det tidspunkt, brugeren allerede har valgt
        public bool IsSelectedSlot(DateTime date, string timeSlot)
        {
            if (CurrentOrder == null)
            {
                return false;
            }

            string[] splitTime = timeSlot.Split('-');
            TimeSpan startTime = TimeSpan.Parse(splitTime[0]);

            return CurrentOrder.ExpectedDeliveryTime.Date == date.Date
                && CurrentOrder.ExpectedDeliveryTime.TimeOfDay == startTime;
        }

        // Finder mandag i den valgte uge og retunerer ugens 7 dage
        private List<DateTime> GetWeekDays(int weekOffset)
        {
            DateTime today = DateTime.Today;

            // Finder mandag i den aktuelle uge
            int diff = today.DayOfWeek == DayOfWeek.Sunday
                ? -6
                : DayOfWeek.Monday - today.DayOfWeek;

            DateTime monday = today.AddDays(diff).Date;

            // Flytter frem eller tilbage i uger
            monday = monday.AddDays(weekOffset * 7);

            List<DateTime> days = new List<DateTime>();

            // Tilføjer 7 dage: mandag til søndag
            for (int i = 0; i < 7; i++)
            {
                days.Add(monday.AddDays(i));
            }

            return days;
        }

        // Returnerer en liste med faste leveringstidspunkter, som skal vises i tabellen
        private List<string> GetTimeSlots()
        {
            return new List<string>
            {
                "08:00-10:00",
                "10:00-12:00",
                "12:00-14:00",
                "14:00-16:00",
                "16:00-18:00",
                "18:00-20:00"
            };
        }
    }
}
