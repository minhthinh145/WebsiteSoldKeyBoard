using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly ApplicationDbContext _context;

        public HoaDonRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<Bill?> GetHoaDonByIdAsync(int maHd)
        {
            return await _context.HoaDons
                .Include(hd => hd.ChiTietHoaDons)
                .Include(hd => hd.MaTrangThaiNavigation)
                .FirstOrDefaultAsync(hd => hd.MaHd == maHd);
        }

        public async Task<IEnumerable<Bill>> GetHoaDonsByUserAsync(string userId)
        {
                return await _context.HoaDons
            .Where(hd => hd.UserId == userId)
            .Include(hd => hd.ChiTietHoaDons)
            .Include(hd => hd.MaTrangThaiNavigation) 
            .OrderByDescending(hd => hd.NgayDat) 
            .ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatusAsync(int maHd, int trangThai, DateTime ngayGiao)
        {
            var existingHoaDon = await _context.HoaDons.FindAsync(maHd);
            if (existingHoaDon == null) return false; 

            existingHoaDon.MaTrangThai = trangThai;
            existingHoaDon.NgayGiao = ngayGiao;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHoaDonByIdAsync(int maHD)
        {
            var existingHoaDon = await _context.HoaDons
                                          .Include(h => h.ChiTietHoaDons) // Load danh sách chi tiết hóa đơn
                                          .FirstOrDefaultAsync(h => h.MaHd == maHD);

            if (existingHoaDon == null)
                return false;

            // Xóa tất cả các chi tiết hóa đơn trước
            _context.ChiTietHoaDons.RemoveRange(existingHoaDon.ChiTietHoaDons);

            // Xóa hóa đơn
            _context.HoaDons.Remove(existingHoaDon);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Order?> GetOrderWithDetailsAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) 
                .Include(o => o.User) 
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<int> AddHoaDonAsync(Bill hoadon)
        {
            _context.HoaDons.Add(hoadon);
            await _context.SaveChangesAsync();
            return hoadon.MaHd;
        }
    }
}
