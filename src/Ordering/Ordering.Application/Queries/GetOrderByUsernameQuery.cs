using MediatR;
using System.Collections.Generic;

namespace Ordering.Application.Queries
{
    public class GetOrderByUsernameQuery : IRequest<IEnumerable<OrderResponse>>
    {
    }
}
