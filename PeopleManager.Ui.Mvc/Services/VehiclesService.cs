using Microsoft.EntityFrameworkCore;
using PeopleManager.Ui.Mvc.Core;
using PeopleManager.Ui.Mvc.Models;

namespace PeopleManager.Ui.Mvc.Services
{
    public class VehiclesService 
    {
        private readonly PeopleManagerDbContext _dbContext;

        public VehiclesService(PeopleManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        
        }

        public IList<Vehicle> Find()
        {
        
            return _dbContext.Vehicles.
                Include(x => x.ResponsiblePerson).ToList(); 

        }

        public IList<Person> Persons()
        {

            return _dbContext.People.
                ToList();

        }


        public Vehicle Get(int id) 
        {
            return _dbContext.Vehicles.Find(id);

        }

        //Create
        public Vehicle? Create(Vehicle vechicle)
        {
            _dbContext.Add(vechicle);
            _dbContext.SaveChanges();

            return vechicle;
        }

        //Update
        public Vehicle? Update(int id, Vehicle vehicle)
        {
            var dbVehicle = _dbContext.Vehicles.Find(id);
            if (dbVehicle is null)
            {
                return null;
            }

            dbVehicle.LicensePlate = vehicle.LicensePlate;
            dbVehicle.Brand = vehicle.Brand;
            dbVehicle.Type = vehicle.Type;
           

            _dbContext.SaveChanges();

            return dbVehicle;
        }

        //Delete
        public void Delete(int id)
        {
            var vehicle = new Vehicle
            {
                Id = id,
                LicensePlate = string.Empty,
                Brand = string.Empty,
                Type = string.Empty
            };
            _dbContext.Vehicles.Attach(vehicle);

            _dbContext.Vehicles.Remove(vehicle);

            _dbContext.SaveChanges();
        }


    }
}
