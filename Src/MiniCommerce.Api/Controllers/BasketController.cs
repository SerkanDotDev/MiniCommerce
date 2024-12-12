using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Api.Features.Basket.Commands;
using MiniCommerce.Api.Features.Basket.Queries;
using MiniCommerce.Api.Models.Basket;
using MiniCommerce.Api.Models.Requests;

namespace MiniCommerce.Api.Controllers;

/// <summary>
/// Provides endpoints for shopping basket management operations
/// </summary>
[ApiController]
[Route("api/basket")]
[Produces("application/json")]
public class BasketController : ControllerBase
{
    private readonly IMediator _mediator;

    public BasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new basket for the user
    /// </summary>
    /// <param name="request">User ID and initial products information</param>
    /// <returns>Created basket details</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/basket/create
    ///     {
    ///        "userId": "user123",
    ///        "products": [
    ///           {
    ///              "productId": 1,
    ///              "quantity": 2
    ///           }
    ///        ]
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Basket successfully created</response>
    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(typeof(BasketResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<BasketResponse>> CreateBasket([FromBody] CreateBasketRequest request)
    {
        var basket = await _mediator.Send(new CreateBasketCommand(request.UserId,request.Products));
        return Ok(basket);
    }

    /// <summary>
    /// Adds a product to the specified basket
    /// </summary>
    /// <param name="request">Basket ID, product ID and quantity information</param>
    /// <returns>Success status</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/basket/add-product
    ///     {
    ///        "basketId": "basket123",
    ///        "productId": 1,
    ///        "quantity": 2
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Product successfully added to basket</response>
    [Authorize]
    [HttpPut("add-product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddProductToBasket([FromBody] AddProductToBasketRequest request)
    {
        await _mediator.Send(new AddProductToBasketCommand(request.BasketId, request.ProductId, request.Quantity));
        return Ok();
    }

    /// <summary>
    /// Removes a product from the specified basket
    /// </summary>
    /// <param name="request">Basket ID, product ID and quantity information</param>
    /// <returns>Success status</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE /api/basket/remove-product
    ///     {
    ///        "basketId": "basket123",
    ///        "productId": 1,
    ///        "quantity": 2
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Product successfully removed from basket</response>
    [Authorize]
    [HttpDelete("remove-product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveProductFromBasket([FromBody] RemoveProductFromBasketRequest request)
    {
        await _mediator.Send(new RemoveProductFromBasketCommand(request.BasketId, request.ProductId, request.Quantity));
        return Ok();
    }

    /// <summary>
    /// Gets basket details by ID
    /// </summary>
    /// <param name="id">Basket ID</param>
    /// <returns>Basket details</returns>
    /// <remarks>
    /// Sample response:
    /// 
    ///     {
    ///        "id": "basket123",
    ///        "userId": "user123",
    ///        "items": [
    ///           {
    ///              "productId": 1,
    ///              "productName": "Laptop",
    ///              "quantity": 2,
    ///              "price": 999.99
    ///           }
    ///        ],
    ///        "totalPrice": 1999.98
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Basket details successfully retrieved</response>
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BasketResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<BasketResponse>> GetBasketDetail(int id)
    {
        var result = await _mediator.Send(new GetBasketDetailQuery(id));
        return Ok(result);
    }
}
