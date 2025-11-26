using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using tp8.Models;
using tp8.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList


namespace tp8.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestoRepository presupuestoRepository;
    private ProductoRepository productoRepository;
    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestoRepository();
        productoRepository = new ProductoRepository();
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

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {

        if (presupuestoVM.FechaCreacion > DateTime.Now)
        {
            ModelState.AddModelError("FechaCreacion", "La fecha no puede ser posterior a la actual.");
        }

        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }
        Presupuestos nuevoPresupuesto = new Presupuestos
        {
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };

        presupuestoRepository.InsertarPresupuesto(nuevoPresupuesto);

        return RedirectToAction("Index");
    }

    public IActionResult AgregarProducto(int id)
    {
        List<Productos> productos = productoRepository.GetAll();

        AgregarProductoViewModel model = new AgregarProductoViewModel
        {
            IdPresupuesto = id,
            ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")

        };
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // LÓGICA CRÍTICA DE RECARGA: Si falla la validación,
            // debemos recargar el SelectList porque se pierde en el POST.
            Console.WriteLine("ModelState inválido");
            foreach (var error in ModelState)
            {
                Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }
            List<Productos> productos = productoRepository.GetAll();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
            return View(model);
        }

        // 2. Si es VÁLIDO: Llamamos al repositorio para guardar la relación
        presupuestoRepository. AgregarDetalleProducto(model.IdPresupuesto, model.IdProducto, model.Cantidad);
        return RedirectToAction(nameof(Details), new { id = model.IdPresupuesto }); //Presupuesto/Details/5
    }

    [HttpGet]
    public IActionResult Delete(int id) //Cuando elijo borrar voy a esta vista
    {
        Presupuestos presupuesto = presupuestoRepository.GetById(id);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Delete(Presupuestos presupuesto)
    {
        presupuestoRepository.Eliminar(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id) 
    {
        Presupuestos pres = presupuestoRepository.GetById(id);
        if (pres == null)
            return NotFound();

        PresupuestoViewModel presupuestoVM = new PresupuestoViewModel
        {
            IdPresupuesto = pres.IdPresupuesto,
            NombreDestinatario = pres.NombreDestinatario,
            FechaCreacion = pres.FechaCreacion
        };

        return View(presupuestoVM);
    }
    
    [HttpPost]
    public IActionResult Edit(int id, PresupuestoViewModel presupuestoVM)
    {
        if (id != presupuestoVM.IdPresupuesto) return NotFound();
        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }

        Presupuestos presAEditar = new Presupuestos
        {
            IdPresupuesto = presupuestoVM.IdPresupuesto,
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };
        presupuestoRepository.ModificarPresupuesto(presAEditar);
        return RedirectToAction("Index");

    }

    public IActionResult QuitarProducto(int idPresupuesto, int idProducto)
    {
        presupuestoRepository.EliminarProductoDeDetalle(idPresupuesto, idProducto);

        return RedirectToAction("Details", new { id = idPresupuesto });
    }
}