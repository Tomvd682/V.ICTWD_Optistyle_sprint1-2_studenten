using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactController> _logger;

        private const string Recipient = "info@optistyle.nl";

        public ContactController(IEmailSender emailSender, ILogger<ContactController> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new CallMeRequestModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CallMeRequest(CallMeRequestModel model, CancellationToken ct)
        {
            // Honeypot check
            if (!string.IsNullOrEmpty(model.MiddleName))
            {
                _logger.LogWarning("Spam geblokkeerd: honeypot veld ingevuld.");
                return BadRequest("Ongeldige aanvraag");
            }

            if (!ModelState.IsValid)
                return View("Index", model);

            var subject = "Terugbelverzoek via website";
            var body =
                $"Naam : {model.Name}\n" +
                $"Telefoon: {model.Phone}\n" +
                $"Verzonden: {DateTimeOffset.Now:dd-MM-yyyy HH:mm:ss zzz}";

            try
            {
                await _emailSender.SendAsync(Recipient, subject, body, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij versturen terugbelverzoek");
                ModelState.AddModelError(string.Empty, "Er ging iets mis bij het versturen. Probeer het later opnieuw.");
                return View("Index", model);
            }

            TempData["SuccessMessage"] = "Bedankt! Uw terugbelverzoek is ontvangen door Optistyle.";
            return RedirectToAction(nameof(Thanks));
        }

        [HttpGet]
        public IActionResult Thanks() => View();

        [HttpGet]
        public IActionResult OefenformulierThanks() => View();

        public IActionResult Oefenformulier()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Oefenformulier")]
        public IActionResult OefenformulierPost()
        {
            return RedirectToAction(nameof(OefenformulierThanks));
        }
    }
}
