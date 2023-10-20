using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Extensions;
using Play.Common.Repositories;

namespace Play.Catalog.Service.Controllers

{   // api/items
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IRepository<Item> _itemsRepository;

        public ItemsController(ILogger<ItemsController> logger,
                               IRepository<Item> itemsRepository)
        {
            _logger = logger;
            _itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync())
                        .Select(item => item.AsDTO());
            return items;
        }
        // GET /items/{:id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDTO();
        }
        // POST /items/
        [HttpPost]
        public async Task<ActionResult<ItemDTO>> PostAsync(CreateItemDTO createItemDTO)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDTO.Name,
                Description = createItemDTO.Description,
                Price = createItemDTO.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync),
            new
            {
                id = item.Id
            }, item);
        }
        // PUT /items/{:id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDTO updateItemDTO)
        {
            var existingItem = await _itemsRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDTO.Name;
            existingItem.Description = updateItemDTO.Description;
            existingItem.Price = updateItemDTO.Price;

            await _itemsRepository.UpdateAsync(existingItem);
            return Ok();
        }
        // DELETE /items/{:id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await _itemsRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            await _itemsRepository.RemoveAsync(id);
            return Ok();
        }
    }
}