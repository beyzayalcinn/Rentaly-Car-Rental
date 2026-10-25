using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProcessController : Controller
    {
        private readonly IProcessService _processService;

        public ProcessController(IProcessService processService)
        {
            _processService = processService;
        }

        public async Task<IActionResult> ProcessList()
        {
            var values = await _processService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateProcess()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProcess(Process process)
        {
            await _processService.TInsertAsync(process);
            return RedirectToAction("ProcessList");
        }

        public async Task<IActionResult> DeleteProcess(int id)
        {
            await _processService.TDeleteAsync(id);
            return RedirectToAction("ProcessList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProcess(int id)
        {
            var value = await _processService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProcess(Process process)
        {
            await _processService.TUpdateAsync(process);
            return RedirectToAction("ProcessList");
        }
    }
}