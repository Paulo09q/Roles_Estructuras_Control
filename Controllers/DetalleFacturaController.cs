using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Roles_Estructuras_Control.Data;
using Roles_Estructuras_Control.Models;
using Roles_Estructuras_Control.Models.Dto;

namespace Roles_Estructuras_Control.Controllers
{
    public class DetalleFacturaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetalleFacturaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DetalleFactura
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Obtener los datos para la tabla
        public List<DtoDetalleFactura> Tabla()
        {
            var dtf = _context.DetalleFactura
                 .Include(d => d.FacturaModel)
                 .Include(d => d.ProductoModels)
                 .Include(d => d.StockModels);
            List<DtoDetalleFactura> dtofac = new List<DtoDetalleFactura>();

            foreach (var deta in dtf)
            {
                var dtoDetalle = new DtoDetalleFactura
                {
                    Cantidad = deta.Cantidad,
                    Id = deta.Id,
                    Nombre_Producto = deta.ProductoModels.NombreProducto,
                    Precio = deta.valor,
                    Total = deta.Cantidad * deta.valor
                };
                dtofac.Add(dtoDetalle);
            }

            return dtofac;
        }

        // GET: Obtener detalles de un registro específico por ID
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var detalleFactura = await _context.DetalleFactura.FindAsync(id);
            if (detalleFactura == null)
            {
                return Json(new { success = false, message = "No encontrado" });
            }

            var dto = new
            {
                id = detalleFactura.Id,
                facturaModelId = detalleFactura.FacturaModelId,
                stockModelsId = detalleFactura.StockModelsId,
                productoModelsId = detalleFactura.ProductoModelsId,
                cantidad = detalleFactura.Cantidad,
                valor = detalleFactura.valor
            };

            return Json(dto);
        }

        public List<ClientesModel> ListaClientes()
        {
            return _context.Clientes.ToList();
        }

        // GET: DetalleFactura/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFacturaModel = await _context.DetalleFactura
                .Include(d => d.FacturaModel)
                .Include(d => d.ProductoModels)
                .Include(d => d.StockModels)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleFacturaModel == null)
            {
                return NotFound();
            }

            return PartialView(detalleFacturaModel);
        }

        // GET: DetalleFactura/Create
        public IActionResult Create()
        {
            ViewData["FacturaModelId"] = new SelectList(_context.Facturas, "Id", "Id");
            ViewData["ProductoModelsId"] = new SelectList(_context.Productos, "Id", "NombreProducto");
            ViewData["StockModelsId"] = new SelectList(_context.Stocks, "Id", "Id");
            return View();
        }

        // POST: DetalleFactura/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cantidad,valor,ProductoModelsId,FacturaModelId,StockModelsId")] DetalleFacturaModel detalleFacturaModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleFacturaModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacturaModelId"] = new SelectList(_context.Facturas, "Id", "Id", detalleFacturaModel.FacturaModelId);
            ViewData["ProductoModelsId"] = new SelectList(_context.Productos, "Id", "NombreProducto", detalleFacturaModel.ProductoModelsId);
            ViewData["StockModelsId"] = new SelectList(_context.Stocks, "Id", "Id", detalleFacturaModel.StockModelsId);
            return View(detalleFacturaModel);
        }

        // GET: DetalleFactura/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFacturaModel = await _context.DetalleFactura.FindAsync(id);
            if (detalleFacturaModel == null)
            {
                return NotFound();
            }
            ViewData["FacturaModelId"] = new SelectList(_context.Facturas, "Id", "Id", detalleFacturaModel.FacturaModelId);
            ViewData["ProductoModelsId"] = new SelectList(_context.Productos, "Id", "NombreProducto", detalleFacturaModel.ProductoModelsId);
            ViewData["StockModelsId"] = new SelectList(_context.Stocks, "Id", "Id", detalleFacturaModel.StockModelsId);
            return View(detalleFacturaModel);
        }

        // POST: DetalleFactura/Edit/5 - Para formularios tradicionales
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cantidad,valor,ProductoModelsId,FacturaModelId,StockModelsId")] DetalleFacturaModel detalleFacturaModel)
        {
            if (id != detalleFacturaModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleFacturaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleFacturaModelExists(detalleFacturaModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacturaModelId"] = new SelectList(_context.Facturas, "Id", "Id", detalleFacturaModel.FacturaModelId);
            ViewData["ProductoModelsId"] = new SelectList(_context.Productos, "Id", "NombreProducto", detalleFacturaModel.ProductoModelsId);
            ViewData["StockModelsId"] = new SelectList(_context.Stocks, "Id", "Id", detalleFacturaModel.StockModelsId);
            return View(detalleFacturaModel);
        }

        // POST: DetalleFactura/EditJson - Para peticiones AJAX
        [HttpPost]
        public async Task<JsonResult> EditJson([FromBody] DetalleFacturaModel detalleFacturaModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Modelo inválido" });
            }

            try
            {
                _context.Update(detalleFacturaModel);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: DetalleFactura/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFacturaModel = await _context.DetalleFactura
                .Include(d => d.FacturaModel)
                .Include(d => d.ProductoModels)
                .Include(d => d.StockModels)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleFacturaModel == null)
            {
                return NotFound();
            }

            return View(detalleFacturaModel);
        }

        // POST: DetalleFactura/Delete/5 - Para formularios tradicionales
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleFacturaModel = await _context.DetalleFactura.FindAsync(id);
            if (detalleFacturaModel != null)
            {
                _context.DetalleFactura.Remove(detalleFacturaModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: DetalleFactura/DeleteJson/5 - Para peticiones AJAX
        [HttpPost]
        public async Task<JsonResult> DeleteJson(int id)
        {
            var detalleFacturaModel = await _context.DetalleFactura.FindAsync(id);
            if (detalleFacturaModel == null)
            {
                return Json(new { success = false, message = "No encontrado" });
            }

            try
            {
                _context.DetalleFactura.Remove(detalleFacturaModel);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool DetalleFacturaModelExists(int id)
        {
            return _context.DetalleFactura.Any(e => e.Id == id);
        }
    }
}