using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Api.Features.Categories.Commands;
using MiniCommerce.Api.Features.Categories.Queries;
using MiniCommerce.Api.Models.Requests;
using MiniCommerce.Api.Models.Categories;
using MiniCommerce.Application.Categories.Queries;

namespace MiniCommerce.Api.Controllers;

/// <summary>
/// Provides endpoints for category management operations
/// </summary>
[ApiController]
[Route("api/category")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="request">Category creation information</param>
    /// <returns>Created category details</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/category/create
    ///     {
    ///        "name": "Electronics",
    ///        "description": "Electronic devices and accessories"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Category successfully created</response>
    [Authorize(Roles = "Admin,CategoryManager")]
    [HttpPost("create")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CreateCategoryRequest request)
    {        
        var category = await _mediator.Send(new CreateCategoryCommand(request.Name, request.Description));
        return Ok(category);
    }

    /// <summary>
    /// Updates an existing category
    /// </summary>
    /// <param name="request">Category update information</param>
    /// <returns>Updated category details</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/category/update
    ///     {
    ///        "id": 1,
    ///        "name": "Electronics & Gadgets",
    ///        "description": "Updated category description"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Category successfully updated</response>
    [Authorize(Roles = "Admin,CategoryManager")]
    [HttpPut("update")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryResponse>> Update([FromBody] UpdateCategoryRequest request)
    {
        var category = await _mediator.Send(new UpdateCategoryCommand(request.Id, request.Name, request.Description));
        return Ok(category);
    }

    /// <summary>
    /// Deletes a category by ID
    /// </summary>
    /// <param name="id">Category ID to delete</param>
    /// <returns>Operation result</returns>
    /// <response code="200">Category successfully deleted</response>
    [Authorize(Roles = "Admin,CategoryManager")]
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return Ok();
    }

    /// <summary>
    /// Gets category details by ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>Category details</returns>
    /// <remarks>
    /// Sample response:
    /// 
    ///     {
    ///        "id": 1,
    ///        "name": "Electronics",
    ///        "description": "Electronic devices and accessories"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Category details successfully retrieved</response>
    [Authorize]
    [HttpGet("detail/{id}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryResponse>> GetCategory(int id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        return Ok(category);
    }

    /// <summary>
    /// Gets a list of all categories
    /// </summary>
    /// <returns>List of categories</returns>
    /// <remarks>
    /// Sample response:
    /// 
    ///     [
    ///        {
    ///           "id": 1,
    ///           "name": "Electronics",
    ///           "description": "Electronic devices"
    ///        },
    ///        {
    ///           "id": 2,
    ///           "name": "Books",
    ///           "description": "Physical and digital books"
    ///        }
    ///     ]
    /// 
    /// </remarks>
    /// <response code="200">Category list successfully retrieved</response>
    [Authorize]
    [HttpGet("list")]
    [ProducesResponseType(typeof(List<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryResponse>>> GetCategoryList()
    {
        var categories = await _mediator.Send(new GetCategoryListQuery());
        return Ok(categories);
    }
}
