using KeyBoard.Data;
using KeyBoard.DTOs.AuthenDTOs;
using Microsoft.AspNetCore.Identity;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Tìm kiếm người dùng dựa trên địa chỉ email.
        /// </summary>
        /// <param name="email">Địa chỉ email của người dùng.</param>
        /// <returns>
        /// Trả về đối tượng <see cref="ApplicationUser"/> nếu tìm thấy, ngược lại trả về <c>null</c>.
        /// </returns>
        Task<ApplicationUser?> FindByEmailAsync(string email);

        /// <summary>
        /// Kiểm tra tính hợp lệ của mật khẩu người dùng.
        /// </summary>
        /// <param name="user">Đối tượng người dùng cần kiểm tra.</param>
        /// <param name="password">Mật khẩu cần xác thực.</param>
        /// <returns>
        /// <c>true</c> nếu mật khẩu hợp lệ, ngược lại trả về <c>false</c>.
        /// </returns>
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// Lấy danh sách vai trò của một người dùng.
        /// </summary>
        /// <param name="user">Đối tượng người dùng.</param>
        /// <returns>
        /// Danh sách các vai trò của người dùng.
        /// </returns>
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);

        /// <summary>
        /// Tạo một người dùng mới trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="user">Đối tượng người dùng cần tạo.</param>
        /// <param name="password">Mật khẩu của người dùng.</param>
        /// <returns>
        /// Đối tượng <see cref="IdentityResult"/> chứa thông tin về kết quả tạo người dùng.
        /// </returns>
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

        /// <summary>
        /// Kiểm tra xem một vai trò có tồn tại trong hệ thống không.
        /// </summary>
        /// <param name="roleName">Tên vai trò cần kiểm tra.</param>
        /// <returns>
        /// <c>true</c> nếu vai trò tồn tại, ngược lại trả về <c>false</c>.
        /// </returns>
        Task<bool> RoleExistsAsync(string roleName);

        /// <summary>
        /// Tạo một vai trò mới trong hệ thống.
        /// </summary>
        /// <param name="roleName">Tên vai trò cần tạo.</param>
        /// <returns>
        /// Một <see cref="Task"/> đại diện cho quá trình thực hiện.
        /// </returns>
        Task CreateRoleAsync(string roleName);

        /// <summary>
        /// Gán một vai trò cho người dùng.
        /// </summary>
        /// <param name="user">Đối tượng người dùng.</param>
        /// <param name="role">Tên vai trò cần gán.</param>
        /// <returns>
        /// Một <see cref="Task"/> đại diện cho quá trình thực hiện.
        /// </returns>
        Task AddToRoleAsync(ApplicationUser user, string role);
    }
}
