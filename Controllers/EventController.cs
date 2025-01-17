using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagement.Data;
using EventManagement.Models;
using EventManagement.Models.Repositories;
using EventManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly ApplicationDbContext _context;

        public EventController(IEventRepository eventRepository, ApplicationDbContext context)
        {
            _eventRepository = eventRepository;
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            var events = await _eventRepository.GetAllAsync();
            return View(events);
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _eventRepository.GetByIdAsync(id.Value);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.AddAsync(@event);
                TempData["Success"] = "L'événement a été créé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Event/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var @event = await _eventRepository.GetByIdAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var allParticipants = await _eventRepository.GetAllParticipantsAsync();
            var currentParticipantIds = @event.Registrations.Select(r => r.ParticipantId).ToList();

            var viewModel = new EventEditViewModel
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                Date = @event.Date,
                Location = @event.Location,
                MaxParticipants = @event.MaxParticipants,
                Price = @event.Price,
                CurrentParticipants = @event.Registrations
                    .Select(r => new EventParticipantViewModel
                    {
                        Id = r.ParticipantId,
                        Name = r.Participant.Name,
                        Email = r.Participant.Email,
                        RegistrationDate = r.RegistrationDate
                    })
                    .ToList(),
                AvailableParticipants = allParticipants
                    .Where(p => !currentParticipantIds.Contains(p.Id))
                    .Select(p => new EventParticipantViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email
                    })
                    .ToList(),
                SelectedParticipantIds = currentParticipantIds
            };

            return View(viewModel);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, EventEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var @event = await _eventRepository.GetByIdAsync(id);
                if (@event == null)
                {
                    return NotFound();
                }

                // Update event properties
                @event.Name = viewModel.Name;
                @event.Description = viewModel.Description;
                @event.Date = viewModel.Date;
                @event.Location = viewModel.Location;
                @event.MaxParticipants = viewModel.MaxParticipants;
                @event.Price = viewModel.Price;

                    // Update participants
                    var currentParticipantIds = @event.Registrations.Select(r => r.ParticipantId).ToList();
                var selectedParticipantIds = viewModel.SelectedParticipantIds ?? new List<int>();

                // Remove participants who are no longer selected
                var toRemove = @event.Registrations
                    .Where(r => !selectedParticipantIds.Contains(r.ParticipantId))
                    .ToList();

                foreach (var registration in toRemove)
                {
                    @event.Registrations.Remove(registration);
                }

                // Add new participants
                var newParticipantIds = selectedParticipantIds
                    .Except(currentParticipantIds)
                    .ToList();

                foreach (var participantId in newParticipantIds)
                {
                    @event.Registrations.Add(new Registration
                    {
                        EventId = id,
                        ParticipantId = participantId,
                        RegistrationDate = DateTime.Now
                    });
                }

                await _eventRepository.UpdateAsync(@event);
                TempData["Success"] = "L'événement a été modifié avec succès.";
                return RedirectToAction(nameof(Details), new { id = @event.Id });
            }

            // If we reach here, something failed, reload data
            var allParticipants = await _eventRepository.GetAllParticipantsAsync();
            viewModel.AvailableParticipants = allParticipants
                .Where(p => !viewModel.CurrentParticipants.Any(cp => cp.Id == p.Id))
                .Select(p => new EventParticipantViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email
                })
                .ToList();

            return View(viewModel);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventRepository.DeleteAsync(id);
            TempData["Success"] = "L'événement a été supprimé avec succès.";
            return RedirectToAction(nameof(Index));
        }
    }
}
