using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp8.Models;

using tp8.ViewModels;


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
    public IActionResult Create(ProductoViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Productos nuevoProducto = new Productos
        {
            Descripcion = vm.Descripcion,
            Precio = vm.Precio
        };


        productoRepository.InsertarProducto(nuevoProducto);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var prod = productoRepository.GetByID(id);
        if (prod == null)
        return NotFound();

        ProductoViewModel productoVM = new ProductoViewModel
        {
            IdProducto = prod.IdProducto,
            Descripcion = prod.Descripcion,
            Precio = prod.Precio
        };

        return View(productoVM);
    }

    [HttpPost]
    public IActionResult Edit(int id, ProductoViewModel productoVM)
    {
        if (id != productoVM.IdProducto) return NotFound();
        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }

        Productos productoAEditar = new Productos
        {
            IdProducto = productoVM.IdProducto,
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };
        productoRepository.ModificarProducto(productoAEditar);
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
