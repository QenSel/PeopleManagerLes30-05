using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleManager.Ui.Mvc.Core;
using PeopleManager.Ui.Mvc.Models;
using PeopleManager.Ui.Mvc.Services;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehiclesService _VehicleServices;

        public VehiclesController(VehiclesService vehiclesService)
        {
            _VehicleServices = vehiclesService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var vehicles = _VehicleServices.Find();

            return View(vehicles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return CreateEditView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return CreateEditView("Create", vehicle);
            }

            _VehicleServices.Create(vehicle);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vehicle = _VehicleServices.Get(id);

            if (vehicle is null)
            {
                return RedirectToAction("Index");
            }

            return CreateEditView("Edit", vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, [FromForm]Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return CreateEditView("Edit", vehicle);
            }

            var dbVehicle = _VehicleServices.Update(id,vehicle);
            if (dbVehicle is null)
            {
                return RedirectToAction("Index");
            }

            dbVehicle.LicensePlate = vehicle.LicensePlate;
            dbVehicle.Brand = vehicle.Brand;
            dbVehicle.Type = vehicle.Type;
            dbVehicle.ResponsiblePersonId = vehicle.ResponsiblePersonId;

            

            return RedirectToAction("Index");
        }
        
        private IActionResult CreateEditView([AspMvcView]string viewName, Vehicle? vehicle = null)
        {
            var people = _VehicleServices.Persons()
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ToList();

            ViewBag.People = people;

            return View(viewName, vehicle);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var vehicle = _VehicleServices.Get(id) ;

            if (vehicle is null)
            {
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        [HttpPost("Vehicles/Delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            //var vehicle = _dbContext.Vehicles.Find(id);

            //if (vehicle is null)
            //{
            //    return RedirectToAction("Index");
            //}
            var vehicle = new Vehicle
            {
                Id = id,
                LicensePlate = string.Empty
            };
            _VehicleServices.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
