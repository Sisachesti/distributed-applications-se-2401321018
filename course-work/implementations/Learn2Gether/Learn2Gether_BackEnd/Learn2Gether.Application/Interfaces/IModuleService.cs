using Learn2Gether.Application.DTOs.Requests.Module;

namespace Learn2Gether.Application.Interfaces
{
    public interface IModuleService
    {
        Task<bool> AddModuleToCourseAsync(Guid courseId, AddNewModuleDTO newModule, Guid userId);
        Task<bool> UpdateModuleAsync(Guid moduleId, EditModuleDTO module, Guid userId);
        Task<bool> DeleteModuleAsync(Guid moduleId, Guid userId);
    }
}
