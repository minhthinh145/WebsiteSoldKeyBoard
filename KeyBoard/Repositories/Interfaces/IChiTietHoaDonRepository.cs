﻿using KeyBoard.Data;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IChiTietHoaDonRepository
    {
        Task<List<BillDetails>> GetByHoaDonIdAsync(int hoaDonId);
        Task UpdateChiTietAsync(int chiTietId, int soLuong, decimal donGia);
        Task DeleteChiTietAsync(int chiTietId);
        Task<BillDetails> GetByChiTietHoaDonId(int chiTietId);
    }
}
