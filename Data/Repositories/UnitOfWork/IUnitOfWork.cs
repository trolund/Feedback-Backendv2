using System;
using Data.Repositories.Interface;

namespace Data.Contexts {
    public interface IUnitOfWork : IDisposable {
        IMeetingRepository Meetings { get; }
        bool Save ();
    }
}