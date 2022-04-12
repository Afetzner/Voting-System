using System.Collections.Generic;

namespace VotingSystem.Controller
{
    public interface DbAccessor
    {
        private static readonly string ConnectionString = DbConnecter.ConnectionString;

        /// <summary>
        /// Adds an entry to the DB
        /// </summary>
        /// <param name="args">dictionary entries being added 'field':'parameter'</param>
        /// <returns>id of new entry</returns>
        public int AddEntry(Dictionary<string, object> args);

        /// <summary>
        /// Removes an entry from the DB
        /// </summary>
        /// <param name="id">id of entry to be removed</param>
        public void RemoveEntry(int id);

        /// <summary>
        /// Gets the id of an entry matching input args
        /// </summary>
        /// <param name="args">dictionary of fields to match. 'field':'parameter'</param>
        /// <returns>id of matching entry, or -1 if none</returns>
        public int GetEntryId(Dictionary<string, object> args);

        /// <summary>
        /// Gets all the fields of an entry with a matching id
        /// </summary>
        /// <param name="id">id of entry to match</param>
        /// <returns>Dictionary of returned entry 'field':'parameter'</returns>
        public Dictionary<string, object> GetEntryInfo(int id);
    }
}