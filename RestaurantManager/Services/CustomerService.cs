using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;
using ServiceStack.Script;

namespace RestaurantManager.Services
{
    public class CustomerService : ICustomerService
    {
        // Implementation of customer-related business logic will go here in the future.

        private readonly ILogger<CustomerService> _logger;
        private readonly RestaurantManagerContext _context;


        public CustomerService( RestaurantManagerContext context , ILogger<CustomerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            {
                var customers = await _context.Customer.ToListAsync();
                _logger.LogInformation("Retrieved all customers from the database.");
                return customers;
            }
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            {
                var customers = await _context.Customer.FromSqlRaw<Customer>("spGetCustomerById {0}", id).ToListAsync();
                var customer = customers.FirstOrDefault();

                if (customer != null)
                {
                    _logger.LogInformation($"Customer with ID {id} retrieved.");
                }
                else
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                }
                return customer;
            }
        }





        public async Task AddCustomer(Customer customer)
        {
            {

                _context.Customer.Add(customer);
                _logger.LogInformation($"Customer {customer.Name} added to the list.");
                await _context.SaveChangesAsync();
                
            }
        }
        public async Task EditCustomer(Customer customer)
        {
            {
                _context.Customer.Update(customer);
                _logger.LogInformation($"Customer {customer.Name} updated.");
                await _context.SaveChangesAsync();
            }
           
        }
        public async Task DeleteCustomer(int id)
        {
            {
                var customer = await _context.Customer.FindAsync(id);
                if (customer != null)
                {
                    _context.Customer.Remove(customer);
                    _logger.LogInformation($"Customer with ID {id} deleted.");
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Customer with ID {id} not found for deletion.");
                }
            }
        }


    }
}