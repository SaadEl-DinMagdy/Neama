using Neama.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.OrderService
{
    public class OrderResult
    {
        public bool IsSuccess { get; set; }
        public Order? Order { get; set; }
        public string? ErrorMessage { get; set; }


        public static OrderResult Success(Order order) => new OrderResult { IsSuccess = true, Order = order };
        public static OrderResult Failure(string message) => new OrderResult { IsSuccess = false, ErrorMessage = message };
    }
}
