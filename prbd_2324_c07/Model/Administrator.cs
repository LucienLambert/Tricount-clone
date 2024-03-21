using PRBD_Framework;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_c07.Model;

public class Administrator : User {

    public Administrator() {
        Role = Role.Administrator;
    }

    public Administrator(string mail, string password, string fullname) : base(mail, password, fullname) {
        Role = Role.Administrator;
    }
}

