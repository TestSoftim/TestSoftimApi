using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSoftimApi.Data;
using TestSoftimApi.Models;

namespace TestSoftimApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        CustomersDbContext _context;
        public CustomersController(CustomersDbContext context)
        {
            _context = context;
        }

        //POST
        // api/Customers

        /// <summary>
        /// Returns a list of customers by date from-to
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">Returns a list of all customers</response>
        /// <response code="400">Bad request</response> 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomersModel>>> ReadCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        //POST
        // api/Customers

        /// <summary>
        /// Returns a list of customers by date from-to
        /// </summary>
        /// <remarks>
        /// Sample request: Object FromToModel
        ///
        ///     {
        ///        "from": "2020-11-30T18:38:31.874Z",
        ///        "to": "2020-11-30T18:38:31.874Z"
        ///     }
        ///     
        /// or
        /// 
        ///     {
        ///        "from": "2020-11-30T18:38:31.874Z"
        ///     }
        ///     
        /// or
        /// 
        ///     {
        ///        "to": "2020-11-30T18:38:31.874Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="condition">DateTime from and to format json</param>
        /// <returns>No content</returns>
        /// <response code="200">Returns a list of customers</response>
        /// <response code="400">Bad request</response> 
        [HttpPost("FromTo")]
        public async Task<ActionResult<IEnumerable<CustomersModel>>> ReadCustomersAsync(FromToModel condition)
        {
            return await FindContext(condition);
        }

        //POST
        // api/Customers

        /// <summary>
        /// Insert new customer into system
        /// </summary>
        /// <remarks>
        /// Sample request: Object CustomersForInsertModel
        ///
        ///     {
        ///        "age": "customer age",
        ///        "wasSatisfied": "if the customer was satisfied",
        ///        "sex": "customer's gender"
        ///     }
        ///
        /// </remarks>
        /// <param name="customer">New customer</param>
        /// <returns>No content</returns>
        /// <response code="204">A response as creation of customers</response>
        /// <response code="400">Bad request</response>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomersForInsertModel>> InputCustomerAsync(CustomersForInsertModel customer)
        {
            try
            {
                CustomersModel newCustomer = GetNewCustomer(customer);
                _context.Add(newCustomer);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        //POST
        // api/Customers

        /// <summary>
        /// Adding more new customers
        /// </summary>
        /// <remarks>
        /// Sample request: Object List of CustomersForInsertModel
        ///
        /// [
        ///     {
        ///        "age": "customer age",
        ///        "wasSatisfied": "if the customer was satisfied",
        ///        "sex": "customer's gender"
        ///     },
        ///     {
        ///        "age": "customer age",
        ///        "wasSatisfied": "if the customer was satisfied",
        ///        "sex": "customer's gender"
        ///     }
        /// ]
        ///
        /// </remarks>
        /// <param name="customers">New customer</param>
        /// <returns>No content</returns>
        /// <response code="204">A response as creation of customers</response>
        /// <response code="400">Bad request</response>  
        [HttpPost("InputCustomers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomersForInsertModel>> InputCustomersAsync(List<CustomersForInsertModel> customers)
        {
            try
            {
                foreach(CustomersForInsertModel customer in customers)
                {
                    CustomersModel newCustomer = GetNewCustomer(customer);
                    _context.Add(newCustomer);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        private CustomersModel GetNewCustomer(CustomersForInsertModel newCustomer)
        {
            CustomersModel customer = new CustomersModel();
            customer.Age = newCustomer.Age;
            customer.VisitDateTime = DateTime.Now;
            customer.WasSatisfied = newCustomer.WasSatisfied;
            customer.Sex = newCustomer.Sex;
            return customer;
        }

        private async Task<ActionResult<IEnumerable<CustomersModel>>> FindContext(FromToModel condition)
        {
            if(condition.From != null && condition.To != null)
            {
                return await _context.Customers.Where(obj => obj.VisitDateTime >= condition.From && obj.VisitDateTime <= condition.To).ToListAsync();
            }
            else if(condition.From != null)
            {
                return await _context.Customers.Where(obj => obj.VisitDateTime >= condition.From).ToListAsync();
            }
            else if(condition.To != null)
            {
                return await _context.Customers.Where(obj => obj.VisitDateTime <= condition.To).ToListAsync();
            }

            return await _context.Customers.ToListAsync();
        }

        
    }
}
