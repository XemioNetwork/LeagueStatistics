using Raven.Client.Document;
using Raven.Client.Listeners;

namespace LeagueStatistics.Server.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="DocumentStore"/> class.
    /// </summary>
    public static class DocumentStoreExtensions
    {
        /// <summary>
        /// Registers the specified <paramref name="listener"/> as all listeners it's implementing.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="listener">The listener.</param>
        public static void RegisterMultipleListeners(this DocumentStore store, object listener)
        {
            var conflictListener = listener as IDocumentConflictListener;
            var conversionListener = listener as IDocumentConversionListener;
            var deleteListener = listener as IDocumentDeleteListener;
            var queryListener = listener as IDocumentQueryListener;
            var storeListener = listener as IDocumentStoreListener;
            var extendedConversionListener = listener as IExtendedDocumentConversionListener;

            if (conflictListener != null)
                store.RegisterListener(conflictListener);

            if (conversionListener != null)
                store.RegisterListener(conversionListener);

            if (deleteListener != null)
                store.RegisterListener(deleteListener);

            if (queryListener != null)
                store.RegisterListener(queryListener);

            if (storeListener != null)
                store.RegisterListener(storeListener);

            if (extendedConversionListener != null)
                store.RegisterListener(extendedConversionListener);
        }
    }
}