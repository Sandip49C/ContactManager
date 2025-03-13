using Microsoft.AspNetCore.Mvc;
using ContactManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContactManager.Services;
using Microsoft.Extensions.Logging;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactContext _context;
        private readonly NotificationService _notificationService;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(ContactContext context, NotificationService notificationService, ILogger<ContactsController> logger)
        {
            _context = context;
            _notificationService = notificationService;
            _logger = logger;
        }

        // GET: /Contacts
        public async Task<IActionResult> Index(string searchString)
        {
            var contacts = string.IsNullOrEmpty(searchString)
                ? await _context.Contacts
                    .Include(c => c.Category)
                    .Include(c => c.Phones)
                    .Include(c => c.Emails)
                    .ToListAsync()
                : await _context.Contacts
                    .Include(c => c.Category)
                    .Include(c => c.Phones)
                    .Include(c => c.Emails)
                    .Where(c => c.Name.Contains(searchString) ||
                               c.Phones.Any(p => p.PhoneNumber.Contains(searchString)) ||
                               c.Emails.Any(e => e.EmailAddress.Contains(searchString)))
                    .ToListAsync();

            _notificationService.AddNotification("Welcome to the contact list!", "Info");
            ViewData["Notifications"] = _notificationService.GetNotifications();
            return View(contacts);
        }

        // GET: /Contacts/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Loading Create view. Categories count: {@Count}", _context.Categories.Count());
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(new Contact());
        }

        // POST: /Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId")] Contact contact, string phoneNumber, string emailAddress)
        {
            // Check for duplicate phone number
            if (!string.IsNullOrEmpty(phoneNumber) && await _context.Phones.AnyAsync(p => p.PhoneNumber == phoneNumber))
            {
                ModelState.AddModelError("phoneNumber", "This phone number is already in use.");
            }

            // Check for duplicate email address
            if (!string.IsNullOrEmpty(emailAddress) && await _context.Emails.AnyAsync(e => e.EmailAddress == emailAddress))
            {
                ModelState.AddModelError("emailAddress", "This email address is already in use.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Contacts.Add(contact);
                    await _context.SaveChangesAsync();

                    // Add single phone if provided
                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        var phone = new Phone
                        {
                            ContactId = contact.Id,
                            PhoneNumber = phoneNumber
                        };
                        _context.Phones.Add(phone);
                    }

                    // Add single email if provided
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        var email = new Email
                        {
                            ContactId = contact.Id,
                            EmailAddress = emailAddress
                        };
                        _context.Emails.Add(email);
                    }

                    await _context.SaveChangesAsync();
                    _notificationService.AddNotification($"Contact {contact.Name} created successfully!", "Success");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database update failed during contact creation.");
                    ModelState.AddModelError("", "An error occurred while saving the contact. The phone number or email may already be in use.");
                }
            }

            _logger.LogWarning("ModelState invalid in Create: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", contact.CategoryId);
            return View(contact);
        }

        // GET: /Contacts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _context.Contacts
                .Include(c => c.Category)
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", contact.CategoryId);
            ViewData["Notifications"] = _notificationService.GetNotifications();
            return View(contact);
        }

        // POST: /Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId")] Contact contact, string phoneNumber, string emailAddress)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            // Check for duplicate phone number (excluding the current contact)
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var existingPhone = await _context.Phones
                    .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber && p.ContactId != id);
                if (existingPhone != null)
                {
                    ModelState.AddModelError("phoneNumber", "This phone number is already in use by another contact.");
                }
            }

            // Check for duplicate email address (excluding the current contact)
            if (!string.IsNullOrEmpty(emailAddress))
            {
                var existingEmail = await _context.Emails
                    .FirstOrDefaultAsync(e => e.EmailAddress == emailAddress && e.ContactId != id);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("emailAddress", "This email address is already in use by another contact.");
                }
            }

            _logger.LogInformation("Edit POST called for ID: {Id}, ModelState: {ModelState}", id, ModelState.IsValid);
            if (ModelState.IsValid)
            {
                try
                {
                    var existingContact = await _context.Contacts
                        .Include(c => c.Phones)
                        .Include(c => c.Emails)
                        .FirstOrDefaultAsync(c => c.Id == id);
                    if (existingContact != null)
                    {
                        existingContact.Name = contact.Name;
                        existingContact.CategoryId = contact.CategoryId;

                        // Clear existing phones and emails
                        _context.Phones.RemoveRange(existingContact.Phones);
                        _context.Emails.RemoveRange(existingContact.Emails);

                        // Add single phone if provided
                        if (!string.IsNullOrEmpty(phoneNumber))
                        {
                            var phone = new Phone
                            {
                                ContactId = id,
                                PhoneNumber = phoneNumber
                            };
                            _context.Phones.Add(phone);
                        }

                        // Add single email if provided
                        if (!string.IsNullOrEmpty(emailAddress))
                        {
                            var email = new Email
                            {
                                ContactId = id,
                                EmailAddress = emailAddress
                            };
                            _context.Emails.Add(email);
                        }

                        await _context.SaveChangesAsync();
                        _notificationService.AddNotification($"Contact {contact.Name} updated successfully!", "Success");
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database update failed for contact ID: {Id}", id);
                    ModelState.AddModelError("", "An error occurred while saving the contact. The phone number or email may already be in use.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error during edit for contact ID: {Id}", id);
                    ModelState.AddModelError("", "An unexpected error occurred.");
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid for contact ID: {Id}", id);
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Model error: {Error}", error.ErrorMessage);
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", contact.CategoryId);
            ViewData["Notifications"] = _notificationService.GetNotifications();
            return View(contact);
        }

        // GET: /Contacts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts
                .Include(c => c.Category)
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: /Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact != null)
            {
                _context.Phones.RemoveRange(contact.Phones);
                _context.Emails.RemoveRange(contact.Emails);
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                _notificationService.AddNotification($"Contact {contact.Name} deleted successfully!", "Success");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}