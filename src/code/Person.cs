using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libme_scrapper.src.code;
class Person(string firstName, string lastName) {
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}