using Microsoft.AspNetCore.Mvc;
using ZombieParty.Models;
using ZombieParty.Models.Data;

namespace ZombieParty.Controllers
{
    public class HuntingLogController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public HuntingLogController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }

        public IActionResult Index()
        {
            List<HuntingLog> huntingLogs = _baseDonnees.HuntingLogs.OrderBy(h => h.Title).ToList();

            return View(huntingLogs);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new HuntingLog());
            }
            else
            {
                return View(_baseDonnees.HuntingLogs.Find(id));
            }
        }

        public IActionResult Detail(int id)
        {
            HuntingLog? huntingLog = _baseDonnees.HuntingLogs.Find(id);
            if (huntingLog == null)
            {
                return NotFound();
            }

            return View(huntingLog);
        }

        public IActionResult Delete(int id)
        {
            HuntingLog? huntingLog = _baseDonnees.HuntingLogs.Find(id);
            if (huntingLog == null)
            {
                return NotFound();
            }

            return View(huntingLog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            HuntingLog? huntingLog = _baseDonnees.HuntingLogs.Find(id);
            if (huntingLog == null)
            {
                return NotFound();
            }

            _baseDonnees.HuntingLogs.Remove(huntingLog);
            _baseDonnees.SaveChanges();
            TempData["Success"] = $"Hunting log {huntingLog.Title} deleted";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(HuntingLog huntingLog)
        {
            if (ModelState.IsValid)
            {
                if (huntingLog.Id == 0)
                {
                    _baseDonnees.HuntingLogs.Add(huntingLog);
                    TempData["Success"] = $"{huntingLog.Title} hunting log added";
                }
                else
                {
                    _baseDonnees.HuntingLogs.Update(huntingLog);
                    TempData["Success"] = $"{huntingLog.Title} hunting log updated";
                }
                _baseDonnees.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(huntingLog);
        }
    }
}