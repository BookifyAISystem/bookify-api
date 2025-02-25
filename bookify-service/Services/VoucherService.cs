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
            return _mapper.Map<IEnumerable<GetVoucherDTO>>( voucherList );
        }
        public async Task<GetVoucherDTO?> GetByIdAsync(int id)
        {
            var voucher = await _voucherRepository.GetByIdAsync( id );
            return _mapper.Map<GetVoucherDTO>(voucher);
        }
        public async Task<bool> CreateVoucherAsync(AddVoucherDTO addVoucherDto)
        {
            var voucherToAdd = _mapper.Map<Voucher>(addVoucherDto);
            return await _voucherRepository.InsertAsync( voucherToAdd );
        }
        public async Task<bool> UpdateVoucherAsync(int id, UpdateVoucherDTO updateVoucherDto)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
                return false;

            _mapper.Map(updateVoucherDto, voucher);
            voucher.LastEdited = DateTime.UtcNow;

            return await _voucherRepository.UpdateAsync(voucher);
        }
        public async Task<bool> DeleteVoucherAsync(int id)
        {
            return await _voucherRepository.DeleteAsync(id);
        }
    }
}
