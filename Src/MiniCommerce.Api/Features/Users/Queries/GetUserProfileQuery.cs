using MediatR;
using MiniCommerce.Api.Common.Exceptions;
using MiniCommerce.Domain.Repositories;

namespace MiniCommerce.Api.Features.Users.Queries;

public class GetUserProfileQuery : IRequest<GetUserProfileResponse>
{

    public GetUserProfileQuery(int userId)
    {
        UserId = userId;
    }

    public int UserId { get; set; }


    public class Handler : IRequestHandler<GetUserProfileQuery, GetUserProfileResponse>
    {
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                 throw new NotFoundException($"User not found.");

            return new GetUserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.Name.FirstName,
                LastName = user.Name.LastName,
                ProfilePicture = user.ProfilePicture
            };
        }
    }
}
