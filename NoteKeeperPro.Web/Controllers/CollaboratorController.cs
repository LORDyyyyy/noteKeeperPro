using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteKeeperPro.Application.Dtos.Collaborators;
using NoteKeeperPro.Application.Services.Collaborators;
using NoteKeeperPro.Web.ViewModels.Collaborators;

namespace NoteKeeperPro.Web.Controllers
{
    [Authorize]

    #region Services
    public class CollaboratorController : Controller
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly ILogger<CollaboratorController> _logger;
        private readonly IWebHostEnvironment _env;

        public CollaboratorController(ICollaboratorService collaboratorService, ILogger<CollaboratorController> logger, IWebHostEnvironment env)
        {
            _collaboratorService = collaboratorService;
            _logger = logger;
            _env = env;
        } 
        #endregion

        #region Index Action

        // Index Action - Get all collaborators
        [HttpGet]
        public IActionResult Index()
        {
            var collaborators = _collaboratorService.GetAllCollaborators();
            return View(collaborators);
        }

        #endregion

        #region Create Action

        // Create Action (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create Action (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CollaboratorViewModel collaboratorVM)
        {
            if (!ModelState.IsValid)
                return View(collaboratorVM);

            try
            {
                var result = _collaboratorService.CreateCollaborator(new CollaboratorToCreateDto()
                {
                    NoteId = collaboratorVM.NoteId,
                    UserId = collaboratorVM.UserId,
                });

                if (result > 0)
                {
                    TempData["Message"] = "New Collaborator Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Collaborator Cannot be Created");
                    return View(collaboratorVM);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["Message"] = _env.IsDevelopment() ? ex.Message : "An error occurred";
                return View(collaboratorVM);
            }
        }

        #endregion

        #region Details Action

        // Details Action - View specific collaborator
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var collaborator = _collaboratorService.GetCollaboratorById(id.Value);
            if (collaborator == null)
                return NotFound();

            return View(collaborator);
        }

        #endregion

        #region Edit Action

        // Edit Action (GET)
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var collaborator = _collaboratorService.GetCollaboratorById(id.Value);
            if (collaborator == null)
                return NotFound();

            return View(new CollaboratorViewModel
            {
                NoteId = collaborator.NoteId,
                UserId = collaborator.UserId,
            });
        }

        // Edit Action (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CollaboratorViewModel collaboratorVM)
        {
            if (!ModelState.IsValid)
                return View(collaboratorVM);

            try
            {
                var result = _collaboratorService.UpdateCollaborator(new CollaboratorToUpdateDto()
                {
                    Id = id,
                });

                if (result > 0)
                {
                    TempData["Message"] = "Collaborator Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Collaborator Cannot be Updated");
                    return View(collaboratorVM);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["Message"] = _env.IsDevelopment() ? ex.Message : "An error occurred";
                return View(collaboratorVM);
            }
        }

        #endregion

        #region Delete Action

        // Delete Action (GET)
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var collaborator = _collaboratorService.GetCollaboratorById(id.Value);
            if (collaborator == null)
                return NotFound();

            return View(collaborator);
        }

        // Delete Action (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _collaboratorService.DeleteCollaborator(id);
                if (result)
                {
                    TempData["Message"] = "Collaborator Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Message"] = "An error occurred while deleting";
                return View(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["Message"] = _env.IsDevelopment() ? ex.Message : "An error occurred";
                return View(nameof(Index));
            }
        }

        #endregion
    }
}
