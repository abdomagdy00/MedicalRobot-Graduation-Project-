

using Application.DTOs;

namespace Application.Interfaces
{
    public interface IMedicineDrawerService
    {
        Task<string> OpenDrawerByFaceIdAsync(int faceId);

        // التحكم اليدوي من الموبايل
        Task<bool> ToggleDrawerAsync(int drawerId, bool open);

        // الحصول على حالة الأدراج كلها (عشان لو الأدمن عاوز يشوف إيه المفتوح)
        Task<IEnumerable<MedicineDrawerDto>> GetAllDrawersStatusAsync();
    }
}
