using AutoMapper;
using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _cutomerRepository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository cutomerRepository, IMapper mapper)
        {
            _cutomerRepository = cutomerRepository;
            _mapper = mapper;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Customer> GetCustomer([FromRoute] long id)
        {
            DataAccess.Models.Customer customer = _cutomerRepository.FirstOrDefault(customer => customer.Id == id);
            Customer customerResult = _mapper.Map<Customer>(customer);

            if (customer == null)
                return NotFound();

            return Ok(customerResult);
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<long> CreateCustomer([FromBody] Customer customer)
        {
            DataAccess.Models.Customer existCustomer = _cutomerRepository.FirstOrDefault(customerEntity => customerEntity.Id == customer.Id);
            if (existCustomer != null)
                return Conflict($"Customer with the id {customer.Id} already exist.");

            DataAccess.Models.Customer newCustomer = _mapper.Map<DataAccess.Models.Customer>(customer);
            _cutomerRepository.Add(newCustomer);
            _cutomerRepository.Save();

            return Ok(customer.Id);
        }
    }
}