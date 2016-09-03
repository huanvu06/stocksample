using CrossOverStock.Data;
using CrossOverStock.Data.Domain.Repository.Contract;
using CrossOverStock.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Services.Implement
{
    public class StockService: IStockService
    {
        readonly IStockCodeRepository _stockCodeRepository;
        readonly IUserStockCodeRepository _userStockCodeRepository;
        readonly ICurrentUser _currentUser;

        public StockService(IStockCodeRepository stockCodeRepository, IUserStockCodeRepository userStockCodeRepository, ICurrentUser currentUser)
        {
            _stockCodeRepository = stockCodeRepository;
            _userStockCodeRepository = userStockCodeRepository;
            _currentUser = currentUser;
        }

        public IEnumerable<string> GetStockCodes()
        {
            return _stockCodeRepository.Get().Select(x => x.Code).AsEnumerable();
        }

        public IEnumerable<StockCodeDto> GetCurrentUserStockCodes()
        {
            var data = _userStockCodeRepository.Get(x => x.FK_UserId == _currentUser.UserId)
                            .Join(_stockCodeRepository.Get(), u => u.FK_StockCodeId, c => c.Id, (u, c) => new StockCodeDto { Id = c.Id, Code = c.Code });
            return data.AsEnumerable();
        }


    }
}
