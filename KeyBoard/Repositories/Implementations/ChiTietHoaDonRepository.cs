using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class ChiTietHoaDonRepository : IChiTietHoaDonRepository
    {
        private readonly ApplicationDbContext _context;

        public ChiTietHoaDonRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteChiTietAsync(int chiTietId)
        {
            var chitiet = await _context.ChiTietHoaDons.FindAsync(chiTietId);
            if (chitiet == null) throw new Exception("Chi tiết hóa đơn không tồn tại");
            _context.ChiTietHoaDons.Remove(chitiet);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChiTietHoaDon>> GetByHoaDonIdAsync(int hoaDonId)
        {
            return await _context.ChiTietHoaDons
                .Where(ct => ct.MaHd == hoaDonId)
                .ToListAsync();
        }

        public async Task UpdateChiTietAsync(int chiTietId, int soLuong, decimal donGia)
        {
            var chitiet = await _context.ChiTietHoaDons.FindAsync(chiTietId);
            if(chitiet == null) throw new Exception("Chi tiết hóa đơn không tồn tại");
            chitiet.SoLuong = soLuong;
            chitiet.DonGia = donGia;
            await _context.SaveChangesAsync();
        }
    }
}
