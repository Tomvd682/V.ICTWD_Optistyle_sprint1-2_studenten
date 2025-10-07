using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactController> _logger;

        private const string Recipient = "info@optistyle.nl";

        public ReviewController(IEmailSender emailSender, ILogger<ContactController> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(new ReviewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(ReviewModel model, CancellationToken ct)
        {
            var subject = "Nieuwe klantbeoordeling via website";
            var body =
                $"Naam : {model.FullName}\n" +
                $"Beoordeling: {model.Rating}\n" +
                $"Toelichting: {model.Comment}\n" +
                $"Verzonden: {DateTimeOffset.Now:dd-MM-yyyy HH:mm:ss zzz}";

            try
            {
                await _emailSender.SendAsync(Recipient, subject, body, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij versturen review");
                ModelState.AddModelError(string.Empty, "Er ging iets mis bij het versturen. Probeer het later opnieuw.");
                return View("Index", model);
            }

            TempData["SuccessMessage"] = "Bedankt! Uw review is ontvangen door Optistyle. Na controle zal deze worden gepubliceerd op de website. Dit proces kan enkele dagen in beslag nemen.";
            return RedirectToAction(nameof(Thanks));
        }

        [HttpGet]
        public IActionResult Thanks() => View();
    }
}
