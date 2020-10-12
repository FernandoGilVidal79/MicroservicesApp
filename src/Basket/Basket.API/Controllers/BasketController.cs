using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository, IMapper mapper) 
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public BasketController(IBasketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new BasketCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basket)
        {
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("username")]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            return Ok(await _repository.DeleteBasket(userName));
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get total price of the Basket
            // remove basket
            // send checkout event to rabbitMQ

            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            var basketRemoved = await _repository.DeleteBasket(basket.UserName);

            if (!basketRemoved)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        }
    }
}