using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ViewModels;

namespace Business.Services.Interfaces {
    public interface IUserService {
        Task<UserDTO> Authenticate (LoginDTO loginDTO);
        Task signout ();
        Task<UserDTO> UserRegistration (UserRegistrationDTO Entity);
        Task<bool> ConfirmationUser (string email, string emailToken);

        Task<ICollection<UserAdminDTO>> UpdateUserAdmin (IEnumerable<UserAdminDTO> usersToUpdate);
        // IEnumerable<UserDTO> GetAll ();
        // UserDTO GetById (int id);
    }
}