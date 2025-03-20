using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Roles_Estructuras_Control.Data;
using Roles_Estructuras_Control.Models;

namespace Roles_Estructuras_Control.Controllers
{
    public class FacturaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Factura
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Facturas.Include(f => f.ClientesModel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Factura/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaModel = await _context.Facturas
                .Include(f => f.ClientesModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaModel == null)
            {
                return NotFound();
            }

            return View(facturaModel);
        }

        // GET: Factura/Create
        public IActionResult Create()
        {
            ViewData["ClientesModelId"] = new SelectList(_context.Clientes, "Id", "Id");
            return View();
        }

        // POST: Factura/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaIngreso,NumeroFacrtura,ClientesModelId")] FacturaModel facturaModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facturaModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientesModelId"] = new SelectList(_context.Clientes, "Id", "Id", facturaModel.ClientesModelId);
            return View(facturaModel);
        }

        // GET: Factura/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaModel = await _context.Facturas.FindAsync(id);
            if (facturaModel == null)
            {
                return NotFound();
            }
            ViewData["ClientesModelId"] = new SelectList(_context.Clientes, "Id", "Id", facturaModel.ClientesModelId);
            return View(facturaModel);
        }

        // POST: Factura/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaIngreso,NumeroFacrtura,ClientesModelId")] FacturaModel facturaModel)
        {
            if (id != facturaModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facturaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaModelExists(facturaModel.Id))
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
            ViewData["ClientesModelId"] = new SelectList(_context.Clientes, "Id", "Id", facturaModel.ClientesModelId);
            return View(facturaModel);
        }

        // GET: Factura/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaModel = await _context.Facturas
                .Include(f => f.ClientesModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaModel == null)
            {
                return NotFound();
            }

            return View(facturaModel);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facturaModel = await _context.Facturas.FindAsync(id);
            if (facturaModel != null)
            {
                _context.Facturas.Remove(facturaModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturaModelExists(int id)
        {
            return _context.Facturas.Any(e => e.Id == id);
        }
    }
}
