using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AwardController : Controller
    {
        private readonly IAwardService _awardService;

        public AwardController(IAwardService awardService)
        {
            _awardService = awardService;
        }

        public async Task<IActionResult> AwardList()
        {
            var values = await _awardService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateAward()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAward(Award award)
        {
            await _awardService.TInsertAsync(award);
            return RedirectToAction("AwardList");
        }

        public async Task<IActionResult> DeleteAward(int id)
        {
            await _awardService.TDeleteAsync(id);
            return RedirectToAction("AwardList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAward(int id)
        {
            var value = await _awardService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAward(Award award)
        {
            await _awardService.TUpdateAsync(award);
            return RedirectToAction("AwardList");
        }
    }
}