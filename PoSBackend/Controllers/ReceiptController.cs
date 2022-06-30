using Microsoft.AspNetCore.Mvc;
using PoSBackend.Repositories;
using PoSBackend.ViewModels;

namespace PoSBackend.Controllers
{
    /*[ApiController]*/
    [Route("[controller]/[action]")]
    public class ReceiptController : ControllerBase
    {
        readonly ReceiptRepository repository;

        public ReceiptController(ReceiptRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult FindAll()
        {
            /*var result = repository.FindAll();

            var response = new Response<List<ReceiptViewModel>>
            {
                Code = 0,
                Message = "Success",
                Data = result,
            };

            return Ok(response);*/

            var response = repository.FindAll();

            return Ok(response);
        }

        [HttpGet]
        public IActionResult FindByDateRange([FromQuery] searchByDateParam param)
        {
            var response = repository.FindByDateRange(param.start, param.end);

            return Ok(response);
        }

        [HttpGet]
        public IActionResult FindOne(int id)
        {
            /*var result = repository.FindOne(id);

            var response = new Response<ReceiptDetailViewModel>();

            if (result == null)
            {
                response = new Response<ReceiptDetailViewModel>{ 
                   Code = 1,
                   Message = "Error",
                   Data = default(ReceiptDetailViewModel),
                };
            }
            else
            {
                response = new Response<ReceiptDetailViewModel>
                {
                    Code = 0,
                    Message = "Success",
                    Data = result,
                };
            }

            return Ok(response);*/

            var response = repository.FindOne(id);

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Receipt receipt)
        {
            /*var result = repository.Create(receipt);
            var response = new Response<string>();

            if (result != "Succeeded")
            {
                response = new Response<string>
                {
                    Code = 1,
                    Message = "Error",
                    Data = result
                };
            }
            else
            {
                response = new Response<string>
                {
                    Code = 0,
                    Message = "Success",
                    Data = result
                };
            }


            return Ok(response);*/

            var response = repository.Create(receipt);

            return Ok(response);
        }
    }
}

public class searchByDateParam
{
    public string start { get; set; } = string.Empty;
    public string end { get; set; } = string.Empty;
}
