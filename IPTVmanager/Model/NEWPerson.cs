﻿namespace IPTVman.Model
{
    /// <summary>
    /// A POCO class is one that does not need any special interfaces or inheritance
    /// In WPF MVVM terms, a POCO class is one that does not Fire PropertyChanged events
    /// </summary>

    class NEWperson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
