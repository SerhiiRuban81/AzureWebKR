using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureWebKR.Models;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;

namespace AzureWebKR.Controllers
{
    public class AppartmentsController : Controller
    {
        private readonly BookingContext _context;
        private readonly BlobServiceClient _blobService;
        public AppartmentsController(BookingContext context, BlobServiceClient blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Appartments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Appartments.ToListAsync());
        }

        // GET: Appartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appartment = await _context.Appartments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appartment == null)
            {
                return NotFound();
            }

            return View(appartment);
        }

        // GET: Appartments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppartmentViewModel appartment)
        {
            if (appartment.Photo!=null)
            {
                BlobContainerClient containerClient = _blobService.GetBlobContainerClient("home");
                Stream stream = appartment.Photo.OpenReadStream();
                string fileName = $"{Guid.NewGuid().ToString()}{appartment.Photo.FileName}";
                BlobContentInfo blobContent = await containerClient.UploadBlobAsync(fileName, stream);
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                string url = blobClient.Uri.AbsoluteUri;
                Appartment newAppart = new Appartment();
                newAppart.Title = appartment.Title;
                newAppart.Description = appartment.Description;
                newAppart.Owner = appartment.Owner;
                newAppart.Price = appartment.Price;
                newAppart.Photo = fileName;
                newAppart.PhotoPath = url;
                _context.Add(newAppart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appartment);
        }

        // GET: Appartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appartment = await _context.Appartments.FindAsync(id);
            if (appartment == null)
            {
                return NotFound();
            }
            return View(appartment);
        }

        // POST: Appartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,Owner,Photo,PhotoPath")] Appartment appartment)
        {
            if (id != appartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppartmentExists(appartment.Id))
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
            return View(appartment);
        }

        // GET: Appartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appartment = await _context.Appartments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appartment == null)
            {
                return NotFound();
            }

            return View(appartment);
        }

        // POST: Appartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appartment = await _context.Appartments.FindAsync(id);
            _context.Appartments.Remove(appartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppartmentExists(int id)
        {
            return _context.Appartments.Any(e => e.Id == id);
        }
    }
}
