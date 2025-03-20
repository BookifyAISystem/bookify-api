using AutoMapper;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_data.Repository;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public VoucherService ( IVoucherRepository voucherRepository, IMapper mapper )
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetVoucherDTO>> GetAllAsync()
        {
            var voucherList = await _voucherRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetVoucherDTO>>( voucherList);
        }
        public async Task<GetVoucherDTO?> GetByIdAsync(int id)
        {
            var voucher = await _voucherRepository.GetByIdAsync( id );
            return _mapper.Map<GetVoucherDTO>(voucher);
        }
        public async Task<bool> CreateVoucherAsync(AddVoucherDTO addVoucherDto)
        {
            if (addVoucherDto.Discount <= 0 ||
                addVoucherDto.Discount > 100 ||
            addVoucherDto.MinAmount < 0 ||
            addVoucherDto.MaxDiscount < 0 ||
            addVoucherDto.Quantity < 0)
            {
                throw new ArgumentException("Invalid voucher");
            }
            var voucherToAdd = _mapper.Map<Voucher>(addVoucherDto);
            voucherToAdd.CreatedDate = DateTime.UtcNow;
            voucherToAdd.LastEdited = DateTime.UtcNow;
            voucherToAdd.Status = 1;
            return await _voucherRepository.InsertAsync( voucherToAdd );
        }
        public async Task<bool> UpdateVoucherAsync(int id, UpdateVoucherDTO updateVoucherDto)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
                return false;
            if (updateVoucherDto.Discount <= 0 ||
                updateVoucherDto.Discount > 100 ||
            updateVoucherDto.MinAmount < 0 ||
            updateVoucherDto.MaxDiscount < 0 ||
            updateVoucherDto.Quantity < 0 ||
            (updateVoucherDto.Status != 1 &&
            updateVoucherDto.Status != 0))
            {
                throw new ArgumentException("Invalid voucher");
            }
            _mapper.Map(updateVoucherDto, voucher);
            voucher.LastEdited = DateTime.UtcNow;

            return await _voucherRepository.UpdateAsync(voucher);
        }

        public async Task<bool> UpdateVoucherStatusAsync(int id, int newStatus)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
            {
                throw new Exception($"Not found with ID = {voucher}");
            }

            if (voucher.Status != 0 && voucher.Status != 1)
            {
                throw new ArgumentException("Invalid Status");
            }
            voucher.Status = newStatus;
            voucher.LastEdited = DateTime.UtcNow;
            return await _voucherRepository.UpdateAsync(voucher);
        }

        public async Task<bool> DeleteVoucherAsync(int id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
            {
                throw new Exception($"Not found with ID = {voucher}");
            }

            voucher.Status = 0;
            voucher.LastEdited = DateTime.UtcNow;
            return await _voucherRepository.UpdateAsync(voucher);
        }

    }
}
