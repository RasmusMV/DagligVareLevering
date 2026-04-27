using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DagligVareLevering.Pages.Purchase
{
    public class DeliveryTimeModel : PageModel
    {
        private IService<Order> _orderService;
        public DeliveryTimeModel(IService<Order> orderService)
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

        public async Task OnGet(int weekOffset = 0)
        {
            WeekOffset = weekOffset;

            // Hent tider og dage til visning på siden
            TimeSlots = GetTimeSlots();
            WeekDays = GetWeekDays(weekOffset);

            int userId = 1; // midlertidigt indtil login er lavet

            // Hent den aktuelle ordre for brugeren
            CurrentOrder = (await _orderService.GetObjectsAsync())
                .FirstOrDefault(o => o.UserId == userId);
        }

        public async Task<IActionResult> OnPostSelectTime(int weekOffset)
        {
            // Genopbyg data så siden stadig kan vises korrekt efter post
            WeekOffset = weekOffset;
            TimeSlots = GetTimeSlots();
            WeekDays = GetWeekDays(weekOffset);

            int userId = 1; // midlertidigt indtil login er lavet

            // Hent den aktuelle ordre
            CurrentOrder = (await _orderService.GetObjectsAsync())
                .FirstOrDefault(o => o.UserId == userId);

            // Hvis der ikke findes en ordre, vis siden igen
            if (CurrentOrder == null)
            {
                ModelState.AddModelError(string.Empty, "No active order was found.");
                return Page();
            }

            // Tjek at det valgte interval findes i listen over gyldige intervaller
            if (!TimeSlots.Contains(SelectedTimeSlot))
            {
                ModelState.AddModelError(string.Empty, "Please select a valid delivery interval.");
                return Page();
            }

            // Splitter fx "10:00-12:00" op i to dele
            string[] splitTime = SelectedTimeSlot.Split('-');

            // Laver start- og sluttid om til TimeSpan
            TimeSpan startTime = TimeSpan.Parse(splitTime[0]);
            TimeSpan endTime = TimeSpan.Parse(splitTime[1]);

            // Gemmer intervallet på ordren
            CurrentOrder.ExpectedDeliveryDate = SelectedDate.Date;
            CurrentOrder.ExpectedDeliveryTime = SelectedDate.Date.Add(startTime);

            // Opdaterer ordren i databasen
            await _orderService.UpdateObjectAsync(CurrentOrder);

            // Reload siden med samme uge
            return RedirectToPage(new { weekOffset = weekOffset });
        }

        // Hjælpemetode, så vi ikke gentager de samme linjer i OnGet og OnPost
        private void LoadCalendarData(int weekOffset)
        {
            TimeSlots = GetTimeSlots();
            WeekDays = GetWeekDays(weekOffset);
        }

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
