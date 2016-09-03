using CrossOverStock.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Services
{
    public interface IStockService
    {
        IEnumerable<string> GetStockCodes();
        IEnumerable<StockCodeDto> GetCurrentUserStockCodes();
    }
}
