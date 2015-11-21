using System;

namespace PersistenceService
{
    public interface ITransactionService
    {
        /// <summary>
        /// Führt Code in einer Transaktion aus
        /// </summary>
        /// <param name="action"></param>
        void ExecuteInTransaction(Action action);

        /// <summary>
        /// Führt Code in einer Transaktion aus
        /// </summary>
        /// <param name="func"></param>
        T ExecuteInTransaction<T>(Func<T> func);

    }
}