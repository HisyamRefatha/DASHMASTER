using AutoMapper;
using DASHMASTER.CORE.Helper;
using DASHMASTER.CORE.Service.Product.Object;
using DASHMASTER.DATA;
using DASHMASTER.DATA.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vleko.DAL.Interface;
using Vleko.Result;

namespace DASHMASTER.CORE.Service.Product.Query
{
    #region request
    public class ListProductRequest : ListRequest, IListRequest<ListProductRequest>, IRequest<ListResponse<ProductResponse>>
    {
    }
    #endregion
    internal class ListProductHandler : IRequestHandler<ListProductRequest, ListResponse<ProductResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;

        public ListProductHandler(ILogger<ListProductHandler> logger, IMapper mapper, IUnitOfWork<ApplicationDBContext> context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ListResponse<ProductResponse>> Handle(ListProductRequest request, CancellationToken cancellationToken)
        {
            ListResponse<ProductResponse> result = new();
            try
            {
                var query = _context.Entity<MstProduct>().AsQueryable();

                #region filter
                Expression<Func<MstProduct, object>> column_short = null;
                List<Expression<Func<MstProduct, bool>>> where = new List<Expression<Func<MstProduct, bool>>>();
                if (request.Filter != null && request.Filter.Count > 0)
                {
                    foreach (var filter in request.Filter)
                    {
                        var obj = ListExpression(filter.Search, filter.Field, true);
                        if (obj.where != null)
                            where.Add(obj.where);
                    }
                }
                if (where != null && where.Count > 0)
                {
                    foreach (var w in where)
                    {
                        query = query.Where(w);
                    }
                }
                if (request.Sort != null)
                {
                    column_short = ListExpression(request.Sort.Field, request.Sort.Field, false).order!;
                    if (column_short != null)
                        query = request.Sort.Type == SortTypeEnum.ASC ? query.OrderBy(column_short) : query.OrderByDescending(column_short);
                    else
                        query = query.OrderBy(d => d.Id);
                }
                #endregion

                var query_count = query;
                if (request.Start.HasValue && request.Length.HasValue && request.Length > 0)
                    query = query.Skip((request.Start.Value - 1) * request.Length.Value).Take(request.Length.Value);
                var data_list = await query.ToListAsync();

                result.List = _mapper.Map<List<ProductResponse>>(data_list);
                result.Filtered = data_list.Count();
                result.Count = await query_count.CountAsync();
                result.OK();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed Get List Category {ex}", request);
                result.Error("Failed Get List Category", ex.Message);
            }
            return result;
        }

        #region list utility
        private (Expression<Func<MstProduct, bool>> where, Expression<Func<MstProduct, object>> order) ListExpression(string search, string field, bool is_where)
        {
            Expression<Func<MstProduct, object>> result_order = null;
            Expression<Func<MstProduct, bool>> result_where = null;
            if (!string.IsNullOrWhiteSpace(search) && !string.IsNullOrWhiteSpace(field))
            {
                switch (field)
                {
                    case "Id":
                        if (is_where)
                        {
                            result_where = (d => d.Id == d.Id);
                        }
                        else
                            result_order = (d => d.Id);
                        break;
                }
            }
            return (result_where, result_order);
        }
        #endregion
    }
}
