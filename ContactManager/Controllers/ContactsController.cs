using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactManager.Models;
using ContactManager.Services;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactContext _context;
        private readonly NotificationService _notificationService;

        public ContactsController(ContactContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var contacts = from c in _context.Contacts select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(c => c.Name.Contains(searchString)
                    || c.Phone.Contains(searchString)
                    || c.Email.Contains(searchString));
                if (!contacts.Any())
                {
                    ViewData["SearchResult"] = "No results found.";
                }
            }

            return View(await contacts.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Contacts.AnyAsync(c => c.Phone == contact.Phone && c.Id != contact.Id))
                {
                    _notificationService.AddNotification("A contact with this phone number already exists.", "error");
                    return View(contact);
                }
                if (await _context.Contacts.AnyAsync(c => c.Email == contact.Email && c.Id != contact.Id))
                {
                    _notificationService.AddNotification("A contact with this email already exists.", "error");
                    return View(contact);
                }

                _context.Add(contact);
                await _context.SaveChangesAsync();
                _notificationService.AddNotification($"Contact '{contact.Name}' created successfully.", "success");
                return RedirectToAction(nameof(Index));
            }
            _notificationService.AddNotification("There was an error processing your request.", "error");
            return View(contact);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Email")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingContact = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (existingContact != null)
                {
                    if (await _context.Contacts.AnyAsync(c => c.Phone == contact.Phone && c.Id != contact.Id))
                    {
                        _notificationService.AddNotification("A contact with this phone number already exists.", "error");
                        return View(contact);
                    }
                    if (await _context.Contacts.AnyAsync(c => c.Email == contact.Email && c.Id != contact.Id))
                    {
                        _notificationService.AddNotification("A contact with this email already exists.", "error");
                        return View(contact);
                    }
                }

                try
                {
                    var entityToUpdate = await _context.Contacts.FindAsync(id);
                    if (entityToUpdate != null)
                    {
                        entityToUpdate.Name = contact.Name;
                        entityToUpdate.Phone = contact.Phone;
                        entityToUpdate.Email = contact.Email;
                        await _context.SaveChangesAsync();
                        _notificationService.AddNotification($"Contact '{contact.Name}' updated successfully.", "success");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        _notificationService.AddNotification("Contact not found.", "error");
                        return NotFound();
                    }
                    else
                    {
                        _notificationService.AddNotification("An error occurred while updating the contact.", "error");
                        throw;
                    }
                }
            }
            _notificationService.AddNotification("There was an error processing your request.", "error");
            return View(contact);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                _notificationService.AddNotification($"Contact '{contact.Name}' deleted successfully.", "success");
            }
            else
            {
                _notificationService.AddNotification("Contact not found.", "error");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}