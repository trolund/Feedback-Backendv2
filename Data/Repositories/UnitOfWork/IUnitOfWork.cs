using System;
using Data.Contexts.Repositories;

namespace Data.Contexts {
    public interface IUnitOfWork : IDisposable {
        IMeetingRepository Meetings { get; }
        bool Save ();
    }
}