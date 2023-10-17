using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;

namespace Play.Catalog.Service.Controllers

{   // api/items
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(ILogger<ItemsController> logger)
        {
            _logger = logger;
        }

        // TODO: Replace with database services
        private static readonly List<ItemDTO> items = new(){
            new ItemDTO(Id: Guid.NewGuid(),Name: "Item 1", Description: "Description 1",Price: 5,CreatedDate: DateTimeOffset.UtcNow),
            new ItemDTO(Id: Guid.NewGuid(),Name: "Item 2", Description: "Description 2",Price: 5,CreatedDate: DateTimeOffset.UtcNow),
            new ItemDTO(Id: Guid.NewGuid(),Name: "Item 3", Description: "Description 3",Price: 5,CreatedDate: DateTimeOffset.UtcNow),
            new ItemDTO(Id: Guid.NewGuid(),Name: "Item 4", Description: "Description 4",Price: 5,CreatedDate: DateTimeOffset.UtcNow),
        };
        [HttpGet]
        public IEnumerable<ItemDTO> Get()
        {
            return items;
        }
        // GET /items/{:id}
        [HttpGet("{id}")]
        public ActionResult<ItemDTO> GetById(Guid id)
        {
            var item = items.Where(item => item.Id == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        // POST /items/{:id}
        [HttpPost("{id}")]
        public IActionResult Post(CreateItemDTO createItemDTO)
        {
            var item = new ItemDTO(Id: Guid.NewGuid(), Name: createItemDTO.Name, Description: createItemDTO.Description, Price: createItemDTO.Price, CreatedDate: DateTimeOffset.UtcNow);
            if (item == null)
            {
                return NotFound();
            }
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
        // PUT /items/{:id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDTO updateItemDTO)
        {
            var existingItem = items.Where(item => item.Id == id).FirstOrDefault();
            if (existingItem == null)
            {
                return NotFound();
            }
            var updated = existingItem with
            {
                Name = updateItemDTO.Name,
                Description = updateItemDTO.Description,
                Price = updateItemDTO.Price,
            };
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updated;
            return NoContent();
        }
        // DELETE /items/{:id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(item => item.Id == id);
            if (index < 0)
            {
                return NotFound();
            }
            items.RemoveAt(index);
            return NoContent();
        }
    }
}