using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs.HoaDonsDTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;

namespace KeyBoard.Services.Implementations
{
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly IChiTietHoaDonRepository _repo;
        private readonly IMapper _mapper;

        public ChiTietHoaDonService(IChiTietHoaDonRepository repo , IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<bool> DeleteChiTietHoaDonAsync(int id)
        {
            var chiTietHoaDon = await _repo.GetByChiTietHoaDonId(id);
            if (chiTietHoaDon == null)
            {
                return false; // Không tìm thấy, trả về false
            }

            await _repo.DeleteChiTietAsync(id);
            return true;
        }
        public async Task<ChiTietHoaDonDTO> GetChiTietHoaDonsByChiTietHoaDonIdAsync(int maChiTietHD)
        {
           var chiTietHoaDon = await _repo.GetByChiTietHoaDonId(maChiTietHD);
            if (chiTietHoaDon == null)
            {
                throw new KeyNotFoundException("Không tìm thấy chi tiết hóa đơn.");
            }
            return _mapper.Map<ChiTietHoaDonDTO>(chiTietHoaDon);
        }

        public async Task<List<ChiTietHoaDonDTO>> GetChiTietHoaDonsByHoaDonIdAsync(int maHd)
        {
            var chiTietHoaDons = await _repo.GetByHoaDonIdAsync(maHd);
            if (chiTietHoaDons == null || !chiTietHoaDons.Any())
            {
                throw new KeyNotFoundException("Không tìm thấy chi tiết hóa đơn.");
            }
            return _mapper.Map<List<ChiTietHoaDonDTO>>(chiTietHoaDons);
        }

        public async Task<bool> UpdateChiTietHoaDonAsync(int id, int? newQuantity, decimal? donGia)
        {
            // Lấy chi tiết hóa đơn theo ID
            var chiTietHoaDon = await GetChiTietHoaDonsByChiTietHoaDonIdAsync(id);
            if (chiTietHoaDon == null)
            {
                throw new KeyNotFoundException("Không tìm thấy chi tiết hóa đơn.");
            }

            // Chỉ cập nhật nếu giá trị hợp lệ và khác giá trị cũ
            int newSoLuong = newQuantity.HasValue && newQuantity.Value > 0 ? newQuantity.Value : chiTietHoaDon.SoLuong;
            decimal newDonGia = donGia.HasValue && donGia.Value > 0 ? donGia.Value : chiTietHoaDon.DonGia;

            // Nếu không có gì thay đổi, không cần update
            if (newSoLuong == chiTietHoaDon.SoLuong && newDonGia == chiTietHoaDon.DonGia)
                return false;

            // Gửi dữ liệu xuống repository để cập nhật
            await _repo.UpdateChiTietAsync(id, newSoLuong, newDonGia);
            return true;
        }

    }
}
