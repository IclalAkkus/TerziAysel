using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TerzimAysel.Models;
using Microsoft.EntityFrameworkCore;
using TerzimAysel.Data;

namespace TerzimAysel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // Her kategori için yorumları ayrı ayrı getir
        ViewBag.DisGiyimYorumlar = _context.Yorumlar
            .Include(y => y.Kullanici)
            .Where(y => y.kategoriler == "dis-giyim")
            .OrderByDescending(y => y.Tarih)
            .ToList();

        ViewBag.AltGiyimYorumlar = _context.Yorumlar
            .Include(y => y.Kullanici)
            .Where(y => y.kategoriler == "alt-giyim")
            .OrderByDescending(y => y.Tarih)
            .ToList();

        ViewBag.ElbiseYorumlar = _context.Yorumlar
            .Include(y => y.Kullanici)
            .Where(y => y.kategoriler == "elbise")
            .OrderByDescending(y => y.Tarih)
            .ToList();

        return View();
    }

    // Diğer action'lar...
}
