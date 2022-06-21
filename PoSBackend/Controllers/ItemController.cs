using Microsoft.AspNetCore.Mvc;
using PoSBackend.Repositories;
using PoSBackend.ViewModels;

namespace PoSBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class ItemController : ControllerBase
    {

        readonly ItemRepository repository;

        public ItemController(ItemRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult FindAll()
        {
            /*var result = repository.FindAll();

            var response = new Response<List<ItemViewModel>>
            {
                Code = 0,
                Message = "Success",
                Data = result,
            };

            return Ok(response);*/

            var response = repository.FindAll();

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Item item)
        {
            /*var result = repository.Create(item);

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

            var response = repository.Create(item);

            return Ok(response);
        }

        [HttpPut]
        public IActionResult Update(int id, [FromBody] Item item)
        {
            /*var result = repository.Update(id, item);

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

            var response = repository.Update(id, item);

            return Ok(response);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            /*var result = repository.Delete(id);

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

            var response = repository.Delete(id);

            return Ok(response);
        }
    }
}
