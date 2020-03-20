using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ViewModels;

namespace Business.Services.Interfaces {
    public interface IUserService {
        Task<UserDTO> Authenticate (LoginDTO loginDTO);
        // IEnumerable<UserDTO> GetAll ();
        // UserDTO GetById (int id);
    }
}