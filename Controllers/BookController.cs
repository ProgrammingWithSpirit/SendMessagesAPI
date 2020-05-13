using System;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace SendMessagesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBus _bus;

        public  BookController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async void PostAsync(Book book)
        {
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/quartz"));
            var destinationAddress = new Uri("rabbitmq://localhost/web-service-endpoint");
            var deliveryTime = DateTime.Now.AddSeconds(10);

            await sendEndpoint.ScheduleSend(destinationAddress, deliveryTime, book);
        }
    }
}