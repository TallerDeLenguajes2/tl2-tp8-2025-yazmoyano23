using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp8.Models;


namespace tp8.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestoRepository presupuestoRepository;
    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestoRepository();
    }
    //A partir de aquí van todos los Action Methods (Get, Post,etc.)
    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = presupuestoRepository.GetAll();
        return View(presupuestos);
    }

    public IActionResult Details(int id)
    {
        var presupuesto = presupuestoRepository.GetById(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return View(presupuesto);

    }

  /*  public IActionResult Create()
    {
        
    }*/
}