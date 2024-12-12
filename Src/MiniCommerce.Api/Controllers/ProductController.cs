using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Api.Features.Products.Commands;
using MiniCommerce.Api.Features.Products.Queries;
using MiniCommerce.Api.Models.Requests;
using MiniCommerce.Api.Models.Products;

namespace MiniCommerce.Api.Controllers;

/// <summary>
/// Provides endpoints for product management operations
/// </summary>
[ApiController]
[Route("api/product")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="request">Product creation information</param>
    /// <returns>Created product details</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/product/create
    ///     {
    ///        "name": "Sample Product",
    ///        "description": "Product description",
    ///        "price": 99.99,
    ///        "categoryId": 1,
    ///        "image": [binary file]
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Product successfully created</response>
    [Authorize(Roles = "Admin,ProductManager")]
    [HttpPost("create")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ProductResponse>> Create([FromForm] CreateProductRequest request)
    {
        var product = await _mediator.Send(new CreateProductCommand(request.Name, request.Description, request.Price, request.CategoryId, request.Image));
        return Ok(product);
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="request">Product update information</param>
    /// <returns>Updated product details</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/product/update
    ///     {
    ///        "id": 1,
    ///        "name": "Updated Product",
    ///        "description": "Updated description",
    ///        "price": 149.99,
    ///        "categoryId": 2,
    ///        "image": [binary file]
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Product successfully updated</response>
    /// <response code="400">Invalid request or validation error</response>
    /// <response code="401">Unauthorized access</response>
    /// <response code="403">Insufficient permissions</response>
    /// <response code="404">Product not found</response>
    [Authorize(Roles = "Admin,ProductManager")]
    [HttpPut("update")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    [ProducesResponseType(StatusCodes.Status403Forbidden)]

    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ProductResponse>> Update([FromForm] UpdateProductRequest request)
    {
        var product = await _mediator.Send(new UpdateProductCommand(request.Id, request.Name, request.Description, request.Price, request.CategoryId, request.Image));
        return Ok(product);
    }

    /// <summary>
    /// Deletes a product by ID
    /// </summary>
    /// <param name="id">Product ID to delete</param>
    /// <returns>Operation result</returns>
    /// <response code="200">Product successfully deleted</response>
    /// <response code="401">Unauthorized access</response>
    /// <response code="403">Insufficient permissions</response>
    /// <response code="404">Product not found</response>
    [Authorize(Roles = "Admin,ProductManager")]
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]

    [ProducesResponseType(StatusCodes.Status403Forbidden)]

    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return Ok();
    }

    /// <summary>
    /// Gets product details by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    /// <remarks>
    /// Sample response:
    /// 
    ///     {
    ///        "id": 1,
    ///        "name": "Sample Product",
    ///        "description": "Product description",
    ///        "price": 99.99,
    ///        "categoryId": 1,
    ///        "imageUrl": "https://example.com/products/sample.jpg"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Product details successfully retrieved</response>
    /// <response code="401">Unauthorized access</response>
    /// <response code="404">Product not found</response>
    [Authorize]
    [HttpGet("detail/{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]


    public async Task<ActionResult<ProductResponse>> GetProductDetail(int id)
    {
        var product = await _mediator.Send(new GetProductDetailQuery(id));
        return Ok(product);
    }

    /// <summary>
    /// Gets a paginated list of products
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 10)</param>
    /// <param name="categoryId">Optional category filter</param>
    /// <returns>List of products</returns>
    /// <remarks>
    /// Sample response:
    /// 
    ///     {
    ///        "items": [
    ///           {
    ///              "id": 1,
    ///              "name": "Product 1",
    ///              "price": 99.99,
    ///              "categoryId": 1
    ///           }
    ///        ],
    ///        "totalCount": 100,
    ///        "pageSize": 10,
    ///        "currentPage": 1,
    ///        "totalPages": 10
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Product list successfully retrieved</response>
    /// <response code="401">Unauthorized access</response>
    [Authorize]
    [HttpGet("list")]
    [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]

    public async Task<ActionResult<List<ProductResponse>>> GetProductList(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? categoryId = null)
    {
        var products = await _mediator.Send(new GetProductListQuery(pageNumber, pageSize, categoryId));
        return Ok(products);
    }
}
