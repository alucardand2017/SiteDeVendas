using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Services;
using SalesWebMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Services.Exeptions;

namespace SalesWebMVC.Controllers
{
    public class Sellers1Controller : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public Sellers1Controller(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var department = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = department };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //o framework instancia alem do seller, o department, já que colocamos um private com DepartmentId
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); //redireciona a ação para o index acima
        }
        public IActionResult Delete(int? id) //tela de confirmação
        {
            if (id == null)
                return NotFound();
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
                return NotFound();
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
                return NotFound();
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
                return NotFound();
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if(id != seller.Id)
            {
                return BadRequest();
            }
           
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException )
            {

                return NotFound();
            }
            catch(DbConcurrencyException e)
            {
                return BadRequest();
            }

        }

    }
}
