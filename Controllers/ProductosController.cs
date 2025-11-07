using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp8.Models;


namespace tp8.Controllers;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;
    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }
    //A partir de aquí van todos los Action Methods (Get, Post,etc.)
    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = productoRepository.GetAll();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create() //muestra formulario vacio
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Productos nuevoProducto)
    {
        if (ModelState.IsValid)
        {
            // Si el modelo es válido, lo guardamos
            productoRepository.InsertarProducto(nuevoProducto);
            return RedirectToAction("Index");
        }

        // Si no es válido, volvemos a mostrar el formulario con los errores
        return View(nuevoProducto);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var prod = productoRepository.GetByID(id);
        return View(prod);


    }

    [HttpPost]
    public IActionResult Edit(Productos producto)
    {
        productoRepository.ModificarProducto(producto);
        return RedirectToAction("Index");

    }

    [HttpGet]
    public IActionResult Delete(int id) //Cuando elijo borrar voy a esta vista
    {
        var prod = productoRepository.GetByID(id); // recibo el producto y lo muestro para preguntar si realmente quiere eliminarlo
        return View(prod);
    }

    [HttpPost]
    public IActionResult Delete(Productos producto)
    {
        productoRepository.Eliminar(producto);
        return RedirectToAction("Index");
    }

}
