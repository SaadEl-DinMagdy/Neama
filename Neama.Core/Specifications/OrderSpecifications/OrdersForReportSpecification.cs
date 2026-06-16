using Neama.Core.Entities.Order_Aggregate;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.OrderSpecifications
{
    public class OrdersForReportSpecification : BaseSpecifications<Order>
    {
        public OrdersForReportSpecification(ReportTimeFilter? filter, int? specificYear = null)
        {
            Includes.Add(o => o.Items);
            var now = DateTimeOffset.UtcNow;

            if (specificYear.HasValue && specificYear.Value > 2000)
            {
                var startOfYear = new DateTimeOffset(specificYear.Value, 1, 1, 0, 0, 0, TimeSpan.Zero);
                var endOfYear = startOfYear.AddYears(1);
                Criteria = (o => o.OrderDate >= startOfYear && o.OrderDate < endOfYear);
            }
            else
            {
                switch (filter)
                {
                    case ReportTimeFilter.Today:
                        var startOfDay = now.Date;
                        var endOfDay = startOfDay.AddDays(1);
                        Criteria = (o => o.OrderDate >= startOfDay && o.OrderDate < endOfDay);
                        break;

                    case ReportTimeFilter.ThisMonth:
                        var startOfMonth = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset);
                        var endOfMonth = startOfMonth.AddMonths(1);
                        Criteria = (o => o.OrderDate >= startOfMonth && o.OrderDate < endOfMonth);
                        break;

                    case ReportTimeFilter.ThisYear:
                        var startOfThisYear = new DateTimeOffset(now.Year, 1, 1, 0, 0, 0, now.Offset);
                        var endOfThisYear = startOfThisYear.AddYears(1);
                        Criteria = (o => o.OrderDate >= startOfThisYear && o.OrderDate < endOfThisYear);
                        break;

                    case ReportTimeFilter.AllTime:
                    default:
                        break;
                }
            }
        }
        public OrdersForReportSpecification(ReportTimeFilter? filter, int? partnerId = null, int? branchId = null, int? specificYear = null)
        {
            Includes.Add(o => o.Items);

            if (partnerId.HasValue)
                Criteria = (o => o.PartnerId == partnerId.Value);

            if (branchId.HasValue)
                Criteria = (o => o.BranchId == branchId.Value);


            var now = DateTimeOffset.UtcNow;

            if (specificYear.HasValue && specificYear.Value > 2000)
            {
                var startOfYear = new DateTimeOffset(specificYear.Value, 1, 1, 0, 0, 0, TimeSpan.Zero);
                var endOfYear = startOfYear.AddYears(1);
                Criteria = (o => o.OrderDate >= startOfYear && o.OrderDate < endOfYear);
            }
            else
            {
                switch (filter)
                {
                    case ReportTimeFilter.Today:
                        var startOfDay = now.Date;
                        var endOfDay = startOfDay.AddDays(1);
                        Criteria = (o => o.OrderDate >= startOfDay && o.OrderDate < endOfDay);
                        break;
                    case ReportTimeFilter.ThisMonth:
                        var startOfMonth = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset);
                        var endOfMonth = startOfMonth.AddMonths(1);
                        Criteria = (o => o.OrderDate >= startOfMonth && o.OrderDate < endOfMonth);
                        break;
                    case ReportTimeFilter.ThisYear:
                        var startOfThisYear = new DateTimeOffset(now.Year, 1, 1, 0, 0, 0, now.Offset);
                        var endOfThisYear = startOfThisYear.AddYears(1);
                        Criteria = (o => o.OrderDate >= startOfThisYear && o.OrderDate < endOfThisYear);
                        break;
                }
            }
        }
    }
}
