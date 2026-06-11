using Microsoft.AspNetCore.Mvc;
using Organi.Data.UnitOfWork;
using Organi.Models.Entities;

namespace Organi.Controllers
{
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ContactController(IUnitOfWork uow) { _uow = uow; }

        public IActionResult Index()
        {
            ViewData["Page"] = "Contact";
            ViewData["Title"] = "İletişim";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string fullName, string email, string message)
        {
            if (!string.IsNullOrWhiteSpace(fullName) && !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(message))
            {
                await _uow.ContactMessages.AddAsync(new ContactMessage
                {
                    FullName = fullName,
                    Email = email,
                    Message = message
                });
                await _uow.SaveChangesAsync();
                TempData["Success"] = "Mesajınız alındı, en kısa sürede dönüş yapacağız!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
