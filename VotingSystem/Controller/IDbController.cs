using System;
using System.Collections.Generic;

namespace VotingSystem.Controller
{
    public interface IDbController <T>
    {
        private static readonly string ConnectionString = DbConnecter.ConnectionString;

        /// <summary>
        /// Adds an entry to the DB. No need to manage connection
        /// </summary>
        /// <param name="entry">object being added</param>
        /// <returns>id of new entry</returns>
        public int AddEntry(T entry);

        /// <summary>
        /// Removes an entry (by id) from the DB. No need to manage connection
        /// </summary>
        /// <param name="id">id of entry to be removed</param>
        public void DeleteEntry(int id);

        /// <summary>
        /// Gets the id of an entry matching input object. No need to manage connection
        /// </summary>
        /// <param name="match">object to match</param>
        /// <returns>id of matching entry, or -1 if none</returns>
        public int GetId(T match);

        /// <summary>
        /// Gets all the fields of an entry with a matching id. No need to manage connection
        /// </summary>
        /// <param name="id">id of entry to match</param>
        /// <returns>object created out of returned entry</returns>
        public T GetInfo(int id);
    }
}