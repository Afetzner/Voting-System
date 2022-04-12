using System;
using System.Collections.Generic;

namespace VotingSystem.Controller
{
    public interface IDbAccessor
    {
        private static readonly string ConnectionString = DbConnecter.ConnectionString;

        /// <summary>
        /// Adds an entry to the DB
        /// </summary>
        /// <param name="entry">object being added</param>
        /// <returns>id of new entry</returns>
        public int AddEntry(object entry);

        /// <summary>
        /// Removes an entry from the DB
        /// </summary>
        /// <param name="id">id of entry to be removed</param>
        public void RemoveEntry(int id);

        /// <summary>
        /// Gets the id of an entry matching input args
        /// </summary>
        /// <param name="match">object to match</param>
        /// <returns>id of matching entry, or -1 if none</returns>
        public int GetEntryId(Object match);

        /// <summary>
        /// Gets all the fields of an entry with a matching id
        /// </summary>
        /// <param name="id">id of entry to match</param>
        /// <returns>object created out of returned entry</returns>
        public Object GetEntryInfo(int id);
    }
}