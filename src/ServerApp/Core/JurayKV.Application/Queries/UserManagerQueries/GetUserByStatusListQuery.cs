using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.UserManagerQueries
{
    public sealed class GetUserByStatusListQuery : IRequest<UserListPagedDto>
    {

        public GetUserByStatusListQuery(AccountStatus accountStatus, DateTime? startDate, DateTime? endDate, int pageSize, int pageNumber, string? searchString, int sortOrder)
        {
            AccountStatus = accountStatus;
            PageSize = pageSize;
            PageNumber = pageNumber;
            SearchString = searchString;
            StartDate = startDate;
            EndDate = endDate;
            SortOrder = sortOrder;
        }

        public AccountStatus AccountStatus { get; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchString { get; set; }
        public int SortOrder { get; set; }
        private class GetUserByStatusListQueryHandler : IRequestHandler<GetUserByStatusListQuery, UserListPagedDto>
        {
            private readonly IUserManagerCacheRepository _userManager;

            public GetUserByStatusListQueryHandler(IUserManagerCacheRepository userManager)
            {
                _userManager = userManager;
            }

            public async Task<UserListPagedDto> Handle(GetUserByStatusListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                UserListPagedDto data = await _userManager.GetListByStatusAndDateAsync(request.AccountStatus, request.StartDate,
                    request.EndDate, request.PageSize, request.PageNumber, request.SearchString, request.SortOrder);

                return data;
            }
        }
    }
}
