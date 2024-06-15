using AutoMapper;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.VoucherDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.VoucherRepository;
using MinhTuan.Service.Core;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace MinhTuan.Service.Services.VoucherService
{
    public class VoucherService : Service<Voucher>, IVoucherService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVoucherRepository _voucherRepository;
        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper, IVoucherRepository voucherRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _voucherRepository = voucherRepository;
        }

        public bool CheckExitCode(string code, Guid id)
        {
            if (id == Guid.Empty)
            {
                return _voucherRepository.FindBy(x => x.Code == code && x.IsDelete != true).Any();
            }
            return _voucherRepository.FindBy(x => x.Code == code && x.IsDelete != true && x.Id != id).Any();

        }

        public ResponseWithDataDto<PagedList<VoucherDTO>> GetDataByPage(VoucherSearchDTO searchDTO)
        {
            try
            {
                var query = from entityTbl in _voucherRepository.GetQueryable()
                            where entityTbl.IsDelete != true //Xét xóa mềm
                            select new VoucherDTO
                            {
                                Id = entityTbl.Id,
                                Code = entityTbl.Code ?? string.Empty,
                                MinimumPurchaseAmount = entityTbl.MinimumPurchaseAmount,
                                DiscountAmount = entityTbl.DiscountAmount,
                                DiscountPercent = entityTbl.DiscountPercent,
                                MaxValue = entityTbl.MaxValue,
                                Type = entityTbl.Type,
                                TimeStart = entityTbl.TimeStart,
                                TimeEnd = entityTbl.TimeEnd,
                                Quantity = entityTbl.Quantity

                            };


                //if (searchDTO != null)
                //{
                //    if (!string.IsNullOrEmpty(searchDTO.Name_Filter))
                //    {
                //        var idSearch = searchDTO.Name_Filter.ToString().RemoveAccentsUnicode();
                //        var isNormal = searchDTO.Name_Filter.ToString().ToLower() != idSearch.ToLower();
                //        var list = _VoucherRepository.GetQueryable().Select(x => x.Name).ToList().Where(x => x.ToString().ToLower().RemoveAccentsUnicode().Contains(idSearch.ToLower()));
                //        query = query.Where(x => list.Contains(x.Name));

                //    }
                //    if (!string.IsNullOrEmpty(searchDTO.Code_Filter))
                //    {
                //        var idSearch = searchDTO.Code_Filter.ToString();
                //        var isNormal = searchDTO.Code_Filter.ToString().ToLower() != idSearch.ToLower();
                //        var list = _VoucherRepository.GetQueryable().Select(x => x.Code).ToList().Where(x => x.ToString().ToLower().Contains(idSearch.ToLower()));
                //        query = query.Where(x => list.Contains(x.Code));
                //    }
                //}
                if (!string.IsNullOrEmpty(searchDTO.sortQuery))
                {
                    query = query.OrderBy(searchDTO.sortQuery);
                }
                else
                {
                    query = query.OrderBy(x => x.Code);
                }
                var result = PagedList<VoucherDTO>.Create(query, searchDTO);
                return new ResponseWithDataDto<PagedList<VoucherDTO>>()
                {
                    Data = result,

                    Message = "Lấy thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseWithDataDto<PagedList<VoucherDTO>>()
                {
                    Data = null,

                    Message = ex.Message

                };
            }
        }

       
    }
}
