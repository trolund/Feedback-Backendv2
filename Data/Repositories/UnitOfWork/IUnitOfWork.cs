
using System;
using Feedback.Data.Repositories;

namespace Feedback.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IMeetingRepository Meetings { get; }
        bool Save();
    }
}